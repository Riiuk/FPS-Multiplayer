using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

	public GameObject prefavVersionRompible;	// Referencia al objeto rompible

	/// <summary>
    /// Función que llamamos cuando hacemos click sobre el juego
    /// </summary>
	public void Destruir ()
	{
		// Instanciamos el objeto rompible, en la posición y rotación de la antigua
		Instantiate(prefavVersionRompible, transform.position, transform.rotation);
		// Destruimos el objeto primario
		Destroy(gameObject);
	}

}
