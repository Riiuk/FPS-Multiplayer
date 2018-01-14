using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {

    public float tiempoVivo = 10f;

	// Use this for initialization
	void Update () {
        tiempoVivo -= Time.deltaTime;

        if (tiempoVivo <= 0)
        {
            Destroy(this);
        }
    }
}
