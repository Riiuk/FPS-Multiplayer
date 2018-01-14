using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetShoot : NetworkBehaviour {

	public bool fullAuto = false;

	public bool canShoot = false;

    public bool grenadeLauncher = false;
    public GameObject grenadePrefab;
    public Transform grenadeHolder;
    public float fuerzaDisparo = 100f;

	 // Referencia del player que será utilizada como origen del RayCast de disparo
	 // Usaremos el transform del objeto Head, ya que la cámara será desactivada
	public Transform firePosition;

	public Text fullAutoDisplay; 

	// Use this for initialization
	void Start () {
		// Si se trata del jugador local, le permitimos interactuar
		if (isLocalPlayer) {
			canShoot = true;
		}	
	}
	
	// Update is called once per frame
	void Update () {
		// Si no puedo disparar, salgo de la función
		if (!canShoot) {
			return;
		}

		// Disparo tiro a tiro
		if (!fullAuto && Input.GetButtonDown("Fire1")) {
			// Llamada al comando que realizará la llamada al disparo
			CmdShoot(firePosition.forward);
		}

		// Disparo en FullAuto
		if (fullAuto && Input.GetButton("Fire1")) {
			CmdShoot (firePosition.forward);
		}

		// Recarga
		if (Input.GetButtonDown("Reload")) {
			CmdReload ();
		}
		// FullAuto
		if (Input.GetButtonDown("FullAuto")) {
			fullAuto = !fullAuto;

			if (!fullAuto) {
				fullAutoDisplay.text = "FullAuto Off";
				Debug.LogWarning ("FullAuto Desactivado");
			} else {
				Debug.LogWarning ("FullAuto Activado");
				fullAutoDisplay.text = "FullAuto On";
			}
		}

	}
	// los comandos son peticions de ejecución de código en el servidor
	[Command] 
	void CmdShoot(Vector3 direction) {
		RpcShoot (direction);
	}

	// Le decimos al servidor que vamos a recargar
	[Command]
	void CmdReload() {
		RpcReload ();
	}

	// Los ClientRpc hacen que las instancias de los clientes ejecuten acciones
	[ClientRpc]
	void RpcShoot (Vector3 direction) {
        if (!grenadeLauncher)
        {
            BroadcastMessage("CallShoot", direction);
        } else
        {
            BroadcastMessage("CallShoot", direction);
        }
		
	}
	// Le decimos al cliente que recargamos
	[ClientRpc]
	void RpcReload() {
		BroadcastMessage ("CallReload");
	}

}
