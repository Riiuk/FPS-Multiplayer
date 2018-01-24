using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;

public class Network : MonoBehaviour {

	// Referencia al menú de inicio
	public GameObject startMenu;
	// Referencia al valor de la IP
	public Text ip;
	// Referencia al valor del puerto
	public Text port;
	// Referencia al campo de texto con el nombre del jugador
	public Text playerName;
	// Almacenaremos una variable que nos informará de si somos o no el servidor
	private bool isHost;
	// Llamamos al NetworkManager
	NetworkManager networkManager;

	// Use this for initialization
	void Start () {
		// Llamamos al componente NetworkManager, hubicado en el mismo GameObject que este script
		networkManager = GetComponent<NetworkManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Si no existe un acliente de conexión
		if (networkManager.client == null) {
			// Mostramos el menú de inicio
			startMenu.SetActive (true);
		}
	}

	/// <summary>
	/// Crea un servidor en el puerto seleccionado
	/// </summary>
	public void HostGame(){
		// Indicamos que seremos el servidor
		isHost = true;
		// Indicamos cual será el puerto a utilziar. (int.Parse(); devuelve un resultado en integer).
		networkManager.networkPort = int.Parse (port.text);
		// Desactivamos el menú de inicio
		startMenu.SetActive (false);
		// Llamamos al método StartHost, que crea un servidor y se conecta como cliente
		networkManager.StartHost ();
		// Indico al GameManager como se va a llamar el jugador
		GameManager.GM.playerName = playerName.text;

        GameManager.GM.SetKillsToWin();

        AudioManager.instance.Stop("MainTheme");
	}

	/// <summary>
	/// Se une a un servidor con la IP y puerto especificados
	/// </summary>
	public void JoinGame(){
		// Indicamos que no seremos el host
		isHost = false;
		// Indicamos la IP a la que nos vamos a conectar
		networkManager.networkAddress = ip.text;
		// Indicamos el puerto al que nos vamos a conectar
		networkManager.networkPort = int.Parse (port.text);
		// Desactivamos el menú de inicio
		startMenu.SetActive (false);
		// Iniciamos la conexión como clientes
		networkManager.StartClient ();
		// Indico al GameManager como se va a llamar el jugador
		GameManager.GM.playerName = playerName.text;

        AudioManager.instance.Stop("MainTheme");
    }

	/// <summary>
	/// Abandona el servidor si es cliente. Cierra el servidor si es host
	/// </summary>
	public void LeaveGame() {
		if (isHost) {
			// Cierra el servidor y el cliente
			networkManager.StopHost ();
		} else {
			// Cierra el cliente
			networkManager.StopClient ();
		}
		// Mostramos el menú de inicio
		startMenu.SetActive (true);
	}

	/// <summary>
	/// Asigna el ID del personaje seleccionado como elegido, en el NetworkManager
	/// </summary>
	/// <param name="player">Player.</param>
	public void PickPlayer(int player) {
		NetworkManager.singleton.GetComponent<NetworkManagerCustom> ().chosenCharacter = player;
	}

}
