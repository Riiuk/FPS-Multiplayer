using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{

    public float respawnDelay = 20f;

    public GameObject powerUpObj;
    public Transform spawnPoint;

    // Use this for initialization
    void Start () {
        Instantiate(powerUpObj, spawnPoint);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public IEnumerator RespawnPowerUp()
    {
        yield return new WaitForSeconds(respawnDelay);
        Instantiate(powerUpObj, spawnPoint);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PowerUp")
        {
            
        } else
        {
            StartCoroutine("RespawnPowerUp");
        }
    }
}
