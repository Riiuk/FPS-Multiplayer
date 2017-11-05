using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class Score : NetworkBehaviour {

	[SyncVar]
	public int kills = 0;
	[SyncVar]
	public int deads = 0;
	[SyncVar]
	public string playerName;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			CmdChangeName (GameManager.GM.playerName);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Command]
	/// <summary>
	/// Cambia el nombre del jugador en el servidor
	/// </summary>
	/// <param name="name">Name.</param>
	void CmdChangeName (string name){
		// Esto cambiará el nombre SOLO en el servidor, en el resto de clientes
		// recuperará este nombre ya que la variable playerName es una SyncVar
		// sincronizada en red
		playerName = name;
	}

}
