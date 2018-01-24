using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        GameObject networkManager = GameObject.FindGameObjectWithTag("Network");

        if (networkManager != null)
        {
            Destroy(networkManager);
        }
	}
}
