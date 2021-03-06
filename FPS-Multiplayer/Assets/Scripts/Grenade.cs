﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Grenade : NetworkBehaviour {

    [Header("Tiempo atras de explosión")]
    [Tooltip("Delay que aplicamos antes de llamar a la explosión")]
    public float delayDestruccion = 3f;

    [Header("Explosión")]
    public float radio = 5f;
    public float fuerzaExplosion = 700f;
    public float daño = 50f;

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

            NetPlayerHealth enemy = objetoEnRango.GetComponent<NetPlayerHealth>();

            if (enemy != null)
            {
                
                // El método TakeDamage devuelve true, si somos nosotros los que lo hemos matado
                if (enemy.TakeDamage(daño))
                {
                    // Incrementamos el número de muertes
                    GetComponentInParent<Score>().kills++;
                }
            }
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !haExplotado)
        {
            Explotar();
        }    
    }

    /// <summary>
    /// Función para dibujar Gizmos al tener seleccionado el objeto
    /// </summary>
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);    
    }
}
