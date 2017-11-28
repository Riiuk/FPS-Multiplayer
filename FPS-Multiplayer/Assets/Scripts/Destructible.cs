using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

	public GameObject prefavVersionRompible;	// Referencia al objeto rompible

	/// <summary>
    /// Funci�n que llamamos cuando hacemos click sobre el juego
    /// </summary>
	public void Destruir ()
	{
		// Instanciamos el objeto rompible, en la posici�n y rotaci�n de la antigua
		Instantiate(prefavVersionRompible, transform.position, transform.rotation);
		// Destruimos el objeto primario
		Destroy(gameObject);
	}

}
