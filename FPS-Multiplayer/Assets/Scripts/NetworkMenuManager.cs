using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkMenuManager : MonoBehaviour
{
    [Header("GameObjects")]
    [Tooltip("Panel de personajes")]
    public GameObject panelPersonajes;
    [Tooltip("Panel de conexión")]
    public GameObject panelHost;

    void Start()
    {
        panelPersonajes.SetActive(true);
        panelHost.SetActive(false);
    }

    public void PersonajeToHost()
    {
        panelPersonajes.SetActive(false);
        panelHost.SetActive(true);
    }

    public void HostToPersonajes()
    {
        panelPersonajes.SetActive(true);
        panelHost.SetActive(false);
    }
}
