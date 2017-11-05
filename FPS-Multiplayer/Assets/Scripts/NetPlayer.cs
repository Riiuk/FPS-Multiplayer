using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Libreria para el uso del Network
using UnityEngine.Networking;
// Libreria para la creacion y uso de eventos
using UnityEngine.Events;

// Usamos el NetworkBehaviour en lugar del MonoBehaviour para hacer uso de funciones de red
public class NetPlayer : NetworkBehaviour {

	[SerializeField] UnityEvent onSharedEnable;

	[SerializeField] UnityEvent onLocalEnable;

	[SerializeField] UnityEvent onRemoteEnable;

	[SerializeField] UnityEvent onShareDisable;

	[SerializeField] UnityEvent onLocalDisable;

	[SerializeField] UnityEvent onRemoteDisable;

	// Tiempo que tardará el jugador en hacer respawn
	public float respawnTime = 5f;



	// Referencia a la cámara inicial de la escena
	GameObject mainCamera;


	// Use this for initialization
	void Start () {
		// Recuperación de la cámara inicial de la escena
		mainCamera = GameObject.Find ("StartCamera");

		EnablePlayer ();
	}

	/// <summary>
	/// Método par arealizar las acciones de activación del jugador, en función de si es local o remoto
	/// </summary>
	void EnablePlayer() {
		// Acciones conjuntas sea Local o Remote
		onSharedEnable.Invoke ();

		if (isLocalPlayer) {
			// Desactivamos la camara inicial
			mainCamera.SetActive (false);
			// Acciones a realizar exclusivas del player local
			onLocalEnable.Invoke ();
		} else {
			onRemoteEnable.Invoke ();
		}
	}

	void OnDisable() {
		// Si el player ha sido desactivado, volvemos a reactivar la cámara inicial
		mainCamera.SetActive (true);
	}

	/// <summary>
	/// Acciones de desactivación del jugador
	/// </summary>
	void DisablePlayer() {
		// Desactivamos los elementos comunes
		onShareDisable.Invoke ();

		if (isLocalPlayer) {
			// Activamos la cámara general
			mainCamera.SetActive (true);
			// Llamaremos al evento de desactivación local
			onLocalDisable.Invoke ();
		} else {
			// Llamamos al evento de desactivación remoto
			onRemoteDisable.Invoke ();
		}
	}

	/// <summary>
	/// Método al que llamaremos cuando el jugador muera
	/// </summary>
	public void Die() {
		// Llamamos al método con un tiempo de Delay
		DisablePlayer ();
		Invoke("Respawn", respawnTime);
	}

	/// <summary>
	/// Método que realiza la reactivación del jugador
	/// </summary>
	void Respawn(){
		
		if (isLocalPlayer) {
			// Pedimos al NetworkManager un nuevo punto de Spawn
			Transform spawn = NetworkManager.singleton.GetStartPosition();
			// Asignamos al Player la posición del punto de Spawn
			transform.position = spawn.position;
			// Asignamos al Player la rotación del punto de Spawn
			transform.rotation = spawn.rotation;
		}

		EnablePlayer ();

	}
}