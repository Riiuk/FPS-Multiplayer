using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDestroy : MonoBehaviour {

    public float rotationSpeed = 1f;
    public float nobreakTime = 2f;
    bool unbreak;

    void Start()
    {
        unbreak = true;    
    }

    void Update()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));

        nobreakTime -= Time.deltaTime;

        if (nobreakTime <= 0f)
        {
            unbreak = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && !unbreak)
        {
            Destroy(gameObject);
        }
    }
}
