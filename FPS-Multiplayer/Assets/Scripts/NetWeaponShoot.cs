using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetWeaponShoot : MonoBehaviour {

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

    public bool shootStunt = false;
    public bool unlimitedAmmo = false;

    public bool GrenadeLauncher = false;
    public GameObject grenadePref;
    public Transform grenadeHolder;
    public float fuerzaDisparo = 40f;

    public float stuntDuration = 1f;
    // Tamaño del cargador
    public int magazineSize = 15;
	// Cargador actual
	public int magazine;

	public Text magazineText;

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


	// Usaremos el head como origen del RayCast
	Transform head;
	// Animator del arma
	Animator animator;
	// AudioSource del arma
	AudioSource audioSource;

	void OnEnable(){
		magazine = magazineSize;
		UpdateAmmoDisplay ();
	}

	// Use this for initialization
	void Start () {

		// RFecuperamos el transform del Head
		head = transform.parent.transform.parent.transform;
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
		// Iniciamos el cargador completo
		magazine = magazineSize;

		UpdateAmmoDisplay ();
	}
		
	/// <summary>
	/// Realiza el cálculo del RayCast para el disparo y activación de animación
	/// Será llamado mediante Message por el objeto padre Player. TOdas las armas
	/// deberían tener un método con el mismo nombre para poder ser disparadas.
	/// </summary>
	public void CallShoot(Vector3 direction)
    {
        if (!GrenadeLauncher)
        {
		// Si aún no podemos disparar, salimos del método
		if (Time.time < nextTimeToShoot)
        {
		return;
		}

		// Calculamos el momento en el que el jugador podrá volver a disparar
		nextTimeToShoot = Time.time + shootDelay;

		// Si no quedan balas, emitimos un sonido especial de aviso y salimos del método.
		if (magazine <= 0)
        {
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
		impact = Physics.Raycast (head.position + direction, direction, out hitInfo, range, shootableLayer);


		// A modo de testeo, podemos mostrar por consola el nombre del objeto impactado por el Raycast
		if (impact) {
			Debug.Log (hitInfo.transform.name);

			// Instanciamos el sistema de partículas en el punto de impacto
			GameObject impactParticleTemp = Instantiate (impactParticle, hitInfo.point, Quaternion.LookRotation (hitInfo.normal));
			// Destruimos las particulas tras 1 segundos.
			Destroy (impactParticleTemp, 1f);


			if (hitInfo.transform.gameObject.layer != LayerMask.NameToLayer("Player")) {
							
				// Instanciamos el Decal en el punto de impacto
				GameObject decalTemp = Instantiate (impactDecal, hitInfo.point + hitInfo.normal * 0.01f, Quaternion.LookRotation (-hitInfo.normal));
				// Hacmeos que el decal sea hijo del objeto impactado
				decalTemp.transform.parent = hitInfo.transform;
				// Destruimos el decal tras 20 segundos
				Destroy (decalTemp, 20f);
			}

			// Añadimos fuerza para mover objetos con RigidBody
			if (hitInfo.rigidbody != null) {
				hitInfo.rigidbody.AddForce (-hitInfo.normal * impactForce);
			}

			// Verificamos si el objeto impactado dispone de un componente NetPlayerHealth
			NetPlayerHealth enemy = hitInfo.transform.GetComponent<NetPlayerHealth> ();
			// Si dispone de el, significará que es otro jguador, así que le aplico el daño del arma
			if (enemy != null) {

                // Si el disparo debe stunear
                if (shootStunt)
                {
                    // Llamamos a la función del enemigo de ser stuneado con la duración preestablecida
                    enemy.TakeStunt(stuntDuration);
                }

                // El método TakeDamage devuelve true, si somos nosotros los que lo hemos matado
                if (enemy.TakeDamage (damage)) {
					// Incrementamos el número de muertes
					GetComponentInParent<Score> ().kills++;
				}
			}
		}

        if (!unlimitedAmmo)
        {
            // Tras realizar el disparo, reducimos la bala en el cargador
            magazine--;
            UpdateAmmoDisplay();
        }
        } else
        {
            // Si aún no podemos disparar, salimos del método
            if (Time.time < nextTimeToShoot)
            {
                return;
            }

            // Calculamos el momento en el que el jugador podrá volver a disparar
            nextTimeToShoot = Time.time + shootDelay;

            // Si no quedan balas, emitimos un sonido especial de aviso y salimos del método.
            if (magazine <= 0)
            {
                audioSource.PlayOneShot(noAmmoSound);
                return;
            }

            // Efecto de particulas del arma
            shootParticles.Play();
            // Efecto de sonido del arma
            audioSource.Play();
            // Animación de disparo
            animator.SetTrigger("Shoot");

            GameObject granada = Instantiate(grenadePref, transform.position, transform.rotation);
            Rigidbody rb = granada.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * fuerzaDisparo, ForceMode.VelocityChange);

            if (!unlimitedAmmo)
            {
                // Tras realizar el disparo, reducimos la bala en el cargador
                magazine--;
                UpdateAmmoDisplay();
            }
        }
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
		UpdateAmmoDisplay ();
	}

	public void UpdateAmmoDisplay() {
        if (!unlimitedAmmo)
        {
            magazineText.text = magazine.ToString() + "  " + "/" + "  " + magazineSize.ToString();
        } else
        {
            magazineText.text = "∞ / ∞";
        }
			
	}
}
