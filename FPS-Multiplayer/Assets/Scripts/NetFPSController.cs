using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetFPSController : MonoBehaviour {

	// Velocidad de desplazamiento
	public float speed = 2f;

	// Relación del movimiento del ratón con el movimiento de la cámara
	public float sensitivity = 2f;

	// Referencia al componente Head
	public GameObject head;

	// Permite invertir el movimiento vertical de la cámara
	public bool invertView = false;

	// Fuerza con la que se puede realizar el salto
	public float jumpForce = 4f;

	// Velocidad a la que iremos corriendo
	public float runSpeed = 4f;
	// Velocidad normal del jugador
	public float regularSpeed = 2f;
	// Velocidad a la que iremos al ir agachado o despacio
	public float slowWalkSpeed = 1f;

	// variable para controlar si el jugador esta corriendo
	public bool running = false;
	// variable para controlar si el usuario ha saltado
	public bool jumped = false;
	// Almacenamos en una variable el estado de isGrounded previo
	bool previouslyGrounded = true;
	// variable para controlar si el usuario esta agachado
	public bool crouched = false;
	// variable para controlar si el jugador va despacio
	public bool slowing = false;

	// Distancia que recorrerá el player antes de reproducir el sonido de paso
	public float stepDistance = 1f;
	// Contador interno de distancia recorrida, para controlar cuando se ha recibido la distancia de un paso
	float stepDistanceCounter = 0f;

	public float minVerticalDistanceSound = 0.5f;
	// Contador vertical de caida
	float verticalDistanceCounter = 1f;

	// Modificador para disparar manual o FullAuto
	public bool fullAuto = false;

	// Fuerza con la que se empujará a los objetos que dispongan de RigidBody
	public float pushForce = 2f;

	// Clip de sonido para los pasos
	public AudioClip stepSound;
	// Clip de sonido para el salto
	public AudioClip jumpSound;
	// Clip de sonido para el aterrizaje
	public AudioClip landingSound;

	// Referencia al componente CharacterController
	CharacterController player;
	// Referencia al componente AudioSource
	AudioSource audioSource;

	// Variables donde almacenaremos de forma temporal, el valor de los ejes de desplazamiento
	float moveHorizontal;
	float moveVertical;

	// Variables donde almacenaremos de forma temporal el valor de la rotación de la cámara
	float rotationHorizontal;
	float rotationVertical = 0f;

	// Vector que indicará la dirección del desplazamiento
	Vector3 movement = new Vector3();

	// Variable que controlará la velocidad vertical
	float verticalV;

	// Use this for initialization
	void Start () {
		// Recuperamos la referencia al componente CharacterController
		player = GetComponent<CharacterController> ();

		audioSource = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		// Verificamos el movimiento del jugador
		Movement ();
		// Aplicamos la gravedad al player
		ApplyGravity ();

		if (Input.GetButtonDown("Jump")) {
			Jump ();
		}

		// Si pulasamos la tecla Run y estamos en el suelo, cambiamos el speed
		if (Input.GetButtonDown("Run") && player.isGrounded && !crouched) {
			Run ();
		} else if (Input.GetButtonUp("Run") && !crouched) {
			Walk ();
		}

		if (Input.GetButtonDown("SlowWalk") && player.isGrounded && !crouched) {
			SlowWalk ();
		} else if (Input.GetButtonUp("SlowWalk") && !crouched) {
			Walk ();
		}

		if (Input.GetButtonDown ("Crouch")) {
			// Invertimos el valor de la variable booleana que no sindica si el jugador está agachado
			crouched = !crouched;

			if (crouched) {
				speed = slowWalkSpeed;
			} else {
				speed = regularSpeed;
			}

			Crouch ();
		}
	}

	/// <summary>
	/// Método que realiza el movimiento del jugador y rotación de la cámara
	/// </summary>
	void Movement() {

		// Recuperamos el desplazamiento del ratón par arealziar la rotación del jugador
		rotationHorizontal = Input.GetAxis ("Mouse X") * sensitivity;
		rotationVertical += Input.GetAxis ("Mouse Y") * sensitivity * (invertView?1:-1);

		// Limitamos la rotación para que solo pueda tener valores entre -90 y 90
		rotationVertical = Mathf.Clamp (rotationVertical, -90, 90);

		// Para la rotación horizontal, rotamos el objeto player
		transform.Rotate (0, rotationHorizontal, 0);

		// Para la rotación vertical, rotamos el objeto head
		//head.transform.Rotate (rotationVertical, 0, 0);

		// Convertimos el desplazamiento vertical del ratón, en la rotación de la cámara
		head.transform.localEulerAngles = new Vector3 (rotationVertical, 0, 0);

		// Guardamos el valor del eje horizontal
		moveHorizontal = Input.GetAxisRaw ("Horizontal");
		// Guardamos el valor del eje vertical
		moveVertical = Input.GetAxisRaw ("Vertical");

		// Asignamos el balor del Vector de movimiento
		movement.Set (moveHorizontal, 0, moveVertical);

		// Orientamos el Vector de movimiento aplicándole la misma rotación que la del objeto player
		movement = transform.rotation * movement;

		// Normalizo el Vector de desplazamiento
		movement = movement.normalized;

		if (previouslyGrounded) {
			// Contamos la distancia desplazada
			stepDistanceCounter += movement.magnitude * speed * Time.deltaTime;	
		}

		// Si la distancia registrada es mayor o igual a un paso
		if (stepDistanceCounter >= stepDistance && !slowing) {
			// Reiniciamos el contador
			stepDistanceCounter = 0f;
			// reproducimos el sonido de un paso
			audioSource.PlayOneShot (stepSound);
		}


		// Le indicamos al CharacterController, que realizaremos un desplazamiento
		player.Move (movement * speed * Time.deltaTime);

	}

	/// <summary>
	/// Método que realaizará la simulación de la gravedad
	/// </summary>
	void ApplyGravity() {

		if (!previouslyGrounded) {
			verticalDistanceCounter += verticalV * Time.deltaTime;
		} else {
			verticalDistanceCounter = 0f;
		}

		// Aplicamos la fuerza vertical
		player.Move (transform.up * verticalV * Time.deltaTime);

		if (player.isGrounded) {
			if (!jumped) {
				verticalV = Physics.gravity.y;

				if (verticalDistanceCounter <= -minVerticalDistanceSound) {
					audioSource.PlayOneShot (landingSound);
				}
			}
		} else {
			// Si no se encuentra tocando el suelo, iremos incrementando la velocidad vertical
			verticalV += Physics.gravity.y * Time.deltaTime;

			// Limitamos el valor de desplazamiento vertical
			verticalV = Mathf.Clamp (verticalV, -50f, 50f);

			jumped = false;
		}

		previouslyGrounded = player.isGrounded;
	}

	/// <summary>
	/// Método que realiza la acción de saltar
	/// </summary>
	void Jump() {

		if (player.isGrounded) {
			verticalV = jumpForce;
			jumped = true;
			audioSource.PlayOneShot (jumpSound);
			if (crouched) {
				speed = regularSpeed;
				crouched = false;
				Crouch ();
			}
		}

	}

	/// <summary>
	/// Método para correr
	/// </summary>
	void Run() {
		speed = runSpeed;
		running = true;
		slowing = false;
		crouched = false;
	}

	/// <summary>
	/// Método para agacharse
	/// </summary>
	void Crouch() {
		player.height = (crouched?(player.height/2) : (player.height * 2));
	}

	/// <summary>
	/// Método para andar
	/// </summary>
	void Walk() {
		speed = regularSpeed;
		running = false;
		slowing = false;
	}

	/// <summary>
	/// Método para andar despacio
	/// </summary>
	void SlowWalk() {
		speed = slowWalkSpeed;
		crouched = false;
		slowing = true;
	}

	/// <summary>
	/// Evento en el que controlaremos las colisiones del CharacterController contra objetos
	/// </summary>
	/// <param name="hit">Hit.</param>
	void OnControllerColliderHit(ControllerColliderHit hit) {
		// Intentamos recuperar el RigidBody asociado al Collider colisionado
		Rigidbody body = hit.collider.attachedRigidbody;

		// Si no hay RigidBody o el objeto está marcado como Kinematic, salimos del método
		if (body == null || body.isKinematic) {
			return;
		}

		// Genero el Vector de empuje basado en el valor de la dirección de movimiento del controller
		// en el momento de la colisión
		Vector3 pushDir = new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);

		// Aplico la fuerza indicada al RigidBody
		body.velocity = pushDir * pushForce;
	}
}
