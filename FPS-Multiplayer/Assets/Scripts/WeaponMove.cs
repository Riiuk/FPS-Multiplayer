using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMove : MonoBehaviour {

	// Multiplicador del desplazamiento aplicado al ratón
	public float amount = 0.02f;
	// Desplazamiento máximo permitido por ciclo, para que el arma no se pierda en giros rápidos de ratón
	public float max = 0.06f;
	// Suavizado del lerp
	public float smooth = 6f;

	// Variable para guardar la posición inicial del arma
	Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		initialPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		// El movimiento en el eje X e Y será opuesto al movimiento del ratón, asi que recupero el valor invertido
		float movementX = -Input.GetAxis ("Mouse X") * amount;
		float movementY = -Input.GetAxis ("Mouse Y") * amount;

		// Hacemos un Clamp del movimiento, para evitar que supere el límite de desplazamiento establecido
		movementX = Mathf.Clamp (movementX, -max, max);
		movementY = Mathf.Clamp (movementY, -max, max);

		// Calculamos el vector con la posición final que ocupará el arma en el desplazamiento
		Vector3 finalPosition = new Vector3 (movementX, movementY, 0);

		// Realizamos el movimiento del arma desde el origen hasta el destino, con un lerp que suaviza el movimiento
		transform.localPosition = Vector3.Lerp (transform.localPosition,
												finalPosition + initialPosition,
												Time.deltaTime * smooth);
	}
}
