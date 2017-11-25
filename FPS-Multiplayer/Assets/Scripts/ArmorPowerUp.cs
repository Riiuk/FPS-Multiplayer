using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPowerUp : MonoBehaviour {

    public float value = 100f;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            NetPlayerHealth player = other.transform.GetComponent<NetPlayerHealth>();

            if (player != null)
            {
                player.TakeArmor(value);
                Debug.LogWarning("PLAYER CURADO: " + value + "HP");
            }
        }
    }
}
