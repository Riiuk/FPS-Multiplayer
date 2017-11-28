using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour {

    [Header("Atributos")]
    [Tooltip("Variable donde almacenaremos la fuerza con la que lanzamos la granada")]
    public float fuerzaDisparo = 40f;

    [Header("Prefabs")]
    [Tooltip("Prefab de la granada que lanzaremos")]
    public GameObject prefabGranada;


	// Update is called once per frame
	void Update () {
        //Si pulsamos el boton izquierdo del raton, lanzamos la granada
        if (Input.GetMouseButtonDown(0)) {
            LanzarGranada();
        }
	}

    /// <summary>
    /// Función que usaremos para lanzar la granada
    /// </summary>
    void LanzarGranada() {
        // Instanciamos el prefab de granada como un nuevo GameObject
        GameObject granada = Instantiate(prefabGranada, transform.position, transform.rotation);
        // Conseguimos el componente RigidBody de esta granada
        Rigidbody rb = granada.GetComponent<Rigidbody>();
        // Aplicamos una fuerza en al direccion donde mira el lanzagranadas
        rb.AddForce(transform.forward * fuerzaDisparo, ForceMode.VelocityChange);
    }
}
