﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShoot : MonoBehaviour {

	// Daño del arma
	public float damage = 10f;
	// Alcance del arma
	public float range = 100f;
	// Fuerza de impacto
	public float impactForce = 30f;
	// Tiempo de retardo entre disparos
	public float shootDelay = 0.5f;
	// Variable en la que guardamos cuando podremos realizar el siguiente disparo
	private float nextTimeToShoot = 0f;

	// Tamaño del cargador
	public int magazineSize = 15;
	// Cargador actual
	public int magazine;

	public Text actualMagazine;
	public Text maxMagazine;

	// Sonido cuando la munición se ha terminado
	public AudioClip noAmmoSound;

	// Partículas generadas en el cañón del arma
	public ParticleSystem shootParticles;
	// Particulas generadas en la zona de impacto
	public GameObject impactParticle;
	// Referencia al prefab con el decal del impacto
	public GameObject impactDecal;
	// Listado de layers disparables
	public LayerMask shootableLayer;


	// Cámara usada para raycast
	Camera fpsCamera;
	// Animator del arma
	Animator animator;
	// AudioSource del arma
	AudioSource audioSource;



	// Use this for initialization
	void Start () {

		// Recuperamos el componete Camera en cualquiera de los parents
		fpsCamera = GetComponentInParent<Camera> ();
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
		// Iniciamos el cargador completo
		magazine = magazineSize;
	}

	void Update () {
		actualMagazine.text = magazine.ToString ();
		maxMagazine.text = magazineSize.ToString ();

	}


	/// <summary>
	/// Realiza el cálculo del RayCast para el disparo y activación de animación
	/// Será llamado mediante Message por el objeto padre Player. TOdas las armas
	/// deberían tener un método con el mismo nombre para poder ser disparadas.
	/// </summary>
	public void CallShoot() {

		// Si aún no podemos disparar, salimos del método
		if (Time.time < nextTimeToShoot) {
			return;
		}

		// Calculamos el momento en el que el jugador podrá volver a disparar
		nextTimeToShoot = Time.time + shootDelay;

		// Si no quedan balas, emitimos un sonido especial de aviso y salimos del método.
		if (magazine <= 0) {
			audioSource.PlayOneShot (noAmmoSound);
			return;
		}

		// Efecto de particulas del arma
		shootParticles.Play ();
		// Efecto de sonido del arma
		audioSource.Play ();
		// Animación de disparo
		animator.SetTrigger ("Shoot");

		// Guardamos la información de si el RayCast ha golpeado algo o no 
		bool impact = false;
		// Información del objeto golpeado
		RaycastHit hitInfo;
		// Realizamos el Raycast desde la cámara
		impact = Physics.Raycast (fpsCamera.transform.position, fpsCamera.transform.forward, out hitInfo, range, shootableLayer);


		// A modo de testeo, podemos mostrar por consola el nombre del objeto impactado por el Raycast
		if (impact) {
			Debug.Log (hitInfo.transform.name);

			// Instanciamos el sistema de partículas en el punto de impacto
			GameObject impactParticleTemp = Instantiate (impactParticle, hitInfo.point, Quaternion.LookRotation (hitInfo.normal));
			// Destruimos las particulas tras 1 segundos.
			Destroy (impactParticleTemp, 1f);

			// Instanciamos el Decal en el punto de impacto
			GameObject decalTemp = Instantiate (impactDecal, hitInfo.point + hitInfo.normal * 0.01f, Quaternion.LookRotation (-hitInfo.normal));
			// Hacmeos que el decal sea hijo del objeto impactado
			decalTemp.transform.parent = hitInfo.transform;
			// Destruimos el decal tras 20 segundos
			Destroy (decalTemp, 20f);
		
			// Añadimos fuerza para mover objetos con RigidBody
			if (hitInfo.rigidbody != null) {
				hitInfo.rigidbody.AddForce (-hitInfo.normal * impactForce);
			}


		}

		// Tras realizar el disparo, reducimos la bala en el cargador
		magazine--;

	}


	/// <summary>
	/// Método llamado mediante Message, para iniciar el proceso de recarga del arma.
	/// </summary>
	public void CallReload() {
		// Si el cargador actual tiene menos balas que el tamaño máximo del cargador, permitimos la recarga
		if (magazine < magazineSize) {
			// Llamamos a la animaciñon que a su vez mediante un triffer realizará la recarga
			animator.SetTrigger ("Reload");
		}
	}

	/// <summary>
	/// Recarga el cargador
	/// </summary>
	public void Reload() {
		magazine = magazineSize;
	}

}
