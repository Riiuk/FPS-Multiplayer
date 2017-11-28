using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    [Header("Tiempo atras de explosión")]
    [Tooltip("Delay que aplicamos antes de llamar a la explosión")]
    public float delayDestruccion = 3f;

    [Header("Explosión")]
    public float radio = 5f;
    public float fuerzaExplosion = 700f;

    [Header("Prefabs")]
    [Tooltip("Prefab del ParticleSystem")]
    public GameObject prefabEfectos;


    float tiempoAtras;          // Varible que usamos para almacenar el contador de tiempo
    bool haExplotado = false;   // Variable que usamos para avisar de que la granada ya ha explotado

	// Use this for initialization
	void Start () {

        tiempoAtras = delayDestruccion;
	}
	
	// Update is called once per frame
	void Update () {

        tiempoAtras -= Time.deltaTime;

        if (tiempoAtras <= 0f && !haExplotado) {

            Explotar();
        }
	}

    void Explotar() {
        haExplotado = true;

        Instantiate(prefabEfectos, transform.position, transform.rotation);

        Collider[] collidersDestruir =  Physics.OverlapSphere(transform.position, radio);

        foreach (Collider objetoEnRango in collidersDestruir) {

            Destructible destruible = objetoEnRango.GetComponent<Destructible>();

            if (destruible != null) {

                destruible.Destruir();
            }
        }

        Collider[] collidersMover = Physics.OverlapSphere(transform.position, radio);

        foreach (Collider objetoEnRango in collidersMover) {

            Rigidbody rb = objetoEnRango.GetComponent<Rigidbody>();

            if (rb != null) {

                rb.AddExplosionForce(fuerzaExplosion, transform.position, radio);
            }
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Función para dibujar Gizmos al tener seleccionado el objeto
    /// </summary>
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);    
    }
}
