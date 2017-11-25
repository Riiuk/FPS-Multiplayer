using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{

    public float respawnDelay = 20f;
    float delay;

    public GameObject powerUpObj;
    public Transform spawnPoint;

    public bool status;

    // Use this for initialization
    void Start () {
        Instantiate(powerUpObj, spawnPoint);
        status = true;
        delay = respawnDelay;
    }
	
	// Update is called once per frame
	void Update () {

        if (!status)
        {
            delay -= Time.deltaTime;
        }

        if (delay <= 0f)
        {
            Instantiate(powerUpObj, spawnPoint);
            status = true;
            delay = respawnDelay;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.tag == "PowerUp")
        {
            status = true;
        } else
        {
            status = false;
        }
       
    }
}
