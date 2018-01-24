using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;         // Nombre que tendrá el sonido para llamarlo

    public AudioClip clip;      // Clip de sonido

    [Range(0f, 1f)]
    public float volume;        // Volumen preestablecido del sonido

    [Range(.1f, 3f)]
    public float pitch;         // Pitch preestablecido del sonido

    public bool loop;           // Variable tipo bool para controlar si el sonido tiene que loopearse

    [HideInInspector]
    public AudioSource source;  // Componente público creado con el AudioManager por cada clip que se encuentre
}
