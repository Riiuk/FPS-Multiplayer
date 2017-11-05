using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

// Nuestra clase heredará de NetworkManager,
// ya que queremos hacer una versión personalizada de este
public class NetworkManagerCustom : NetworkManager {

	// Personaje elegido
	public int chosenCharacter = 0;
	// Array con los posibles personajes a elegir
	public GameObject[] characters;

	// Clase que utilizamos para enviar al servidor
	// cual es el personaje que el jugador ha elegido al conectarse
	public class NetworkMessage : MessageBase {
		public int chosenClass;
	}

	public override void OnServerAddPlayer(NetworkConnection conn,
											short playerControllerId,
											NetworkReader extraMessageReader) {
		// Recuperamos el mensaje enviado con la sulicitud de cración de jugador
		NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage> ();
		// Recuperamos la ID del perosnaje seleccionado
		int selectedClass = message.chosenClass;

		Debug.Log ("Se ha seleccionado el personaje " + selectedClass);

		// Creamos una variable para almacenar el GameObject del player que será instanciado
		GameObject player;
		// Recuperamos el transfomr del spawnpoint donde será generado
		Transform startPos = GetStartPosition ();

		// Si hay SpawnPoints, instanciamos el player en el spawnpoint recuperado
		if (startPos != null) {
			player = Instantiate (characters [selectedClass], startPos.position, startPos.rotation) as GameObject;
		} else {
			player = Instantiate (characters [selectedClass], Vector3.zero, Quaternion.identity) as GameObject;
		}
		// Finalmente añadimos el player seleccionado a la partida
		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
	}

	public override void OnClientConnect(NetworkConnection conn){
		NetworkMessage temp = new NetworkMessage ();
		temp.chosenClass = chosenCharacter;
		// Enviamos el mensaje a la conexión seleccionada
		ClientScene.AddPlayer (conn, 0, temp);
	}


}
