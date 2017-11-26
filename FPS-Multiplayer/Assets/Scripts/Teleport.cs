using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public Transform teleportPoint;     // Variable tipo Transform que almacena la posición de salida del Teleport
    
    /// <summary>
    /// Método de tipo OnTrigger que almacena la información del objeto con el que colisiona
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        // Si el objeto colisionado tiene el tag de "Player"
        if (other.CompareTag("Player"))
        {
            // Cambia su posición a la establecida con el punto de teleport
            other.transform.position = teleportPoint.position;
        }
    }
}
