using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFade : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("Tiempo que tardará el boton en aparecer")]
    public float delay = 3f;
    [Header("Unity Settings")]
    [Tooltip("Boton para volver hacia atras")]
    public GameObject button;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        
        if (delay <= 0)
        {
            button.SetActive(true);
        }
	}
}
