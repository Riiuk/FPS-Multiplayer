using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CursorLock : NetworkBehaviour {

	NetFPSController netFPSController;
	NetShoot netShoot;

	// Use this for initialization
	void Start () {
		// Inicialmente bloqueamos el cursor al cargar el objeto que tenga asignado este script
		Cursor.lockState = CursorLockMode.Locked;

		netFPSController = GetComponent<NetFPSController> ();
		netShoot = GetComponent<NetShoot> ();
		}
		
	// Update is called once per frame
	void Update () {

		// Si detectamos la pulsación de la tecla Esc, liberamos el cursor
		if (Input.GetButtonDown ("Cancel")) {
			Cursor.lockState = CursorLockMode.None;
			// Activamos el menú de juego
			GameManager.GM.GameMenu (true);

			GameManager.GM.UpdateScore ();

			netFPSController.enabled = false;
			netShoot.enabled = false;
		}

		// Bloqueamos el raton al levantar la pulsacion de la tecla izquierda del ratón
		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			Cursor.lockState = CursorLockMode.Locked;
			// Desactivamos el menú de juego
			GameManager.GM.GameMenu (false);

			netFPSController.enabled = true;
			netShoot.enabled = true;
		}
	}
}
