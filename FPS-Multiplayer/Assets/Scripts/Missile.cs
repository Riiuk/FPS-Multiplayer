using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Missile : MonoBehaviour
{
    [Header("Explosión")]
    [Tooltip("Radio de la explosión")]
    public float radio = 5f;
    [Tooltip("Velocidad del misil")]
    public float speed = 10f;
    [Tooltip("Fuerza de la explosión")]
    public float fuerzaExplosion = 700f;
    [Tooltip("Daño del misil")]
    public float daño = 50f;

    [Header("Prefabs")]
    [Tooltip("Prefab del Sistema de Particulas para la explosion")]
    public GameObject prefabExplosionEffect;
    // Variable que usamos para avisar de que la granada ya ha explotado
    bool haExplotado = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    void Explotar()
    {
        haExplotado = true;

        Instantiate(prefabExplosionEffect, transform.position, transform.rotation);

        Collider[] collidersDestruir = Physics.OverlapSphere(transform.position, radio);

        foreach (Collider objetoEnRango in collidersDestruir)
        {

            Destructible destruible = objetoEnRango.GetComponent<Destructible>();

            if (destruible != null)
            {

                destruible.Destruir();
            }
        }

        Collider[] collidersMover = Physics.OverlapSphere(transform.position, radio);

        foreach (Collider objetoEnRango in collidersMover)
        {

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
        Explotar();
    }

    /// <summary>
    /// Función para dibujar Gizmos al tener seleccionado el objeto
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
