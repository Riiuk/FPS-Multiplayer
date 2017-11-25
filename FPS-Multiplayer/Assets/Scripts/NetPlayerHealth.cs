using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Usamos la librería Networking
using UnityEngine.Networking;

using UnityEngine.UI;

public class NetPlayerHealth : NetworkBehaviour {

	public float maxHealth = 100f;
    public float maxArmor = 50f;

	NetPlayer player;
	public float health;
    public float armor;

	public Text healthText;
    public Text armorText;

	public Image damageImage;
	public Color damageColor = new Color (1f, 0f, 0f, 0.2f);
	public bool damaged = false;

	// Tiempo restante de stunt
	public float stunned;
	// Referencia al texto de stunt
	public Text stunnedText;
	// Referencia al FPSController, para desactivarlo cuando el jugador se encuentre stuneado
	NetFPSController fpsController;
	// Referencia al NetShoot, para desactivarlo cuando el jguador se encuentre stuneado
	NetShoot netShoot;

	// Referencia al componente Network Animator
	NetworkAnimator anim;


	void Awake () {
		player = GetComponent<NetPlayer> ();
		// Recuperamos la referencia del componente NetworkAnimator
		anim = GetComponent<NetworkAnimator> ();
		// Recuperamos la referencia del componente NetFPSController
		fpsController = GetComponent<NetFPSController> ();
		// Recuperamos la referencia del componente NetShoot
		netShoot = GetComponent<NetShoot> ();
	}

	void Update () {
		// Si se trata del jugador local
		if (isLocalPlayer) {
			// Si el valor de stunned es mayor que 0, significará que el jugador está stuneado
			if (stunned > 0) {
				// Muestro por pantalla la condicion de stunned y el tiempo restante
				stunnedText.text = "Stunned \n" + ((int)stunned).ToString ();
				// Desactivamos el FPSController
				fpsController.enabled = false;
				// Evitamos que el jugador pueda disparar
				netShoot.enabled = false;
				// Reducimos el tiempo restante de stunt
				stunned -= Time.deltaTime;
			} else {
				if (!GameManager.GM.pause) {
					stunnedText.text = "";
					fpsController.enabled = true;
					netShoot.enabled = true;	
				}
			}
		}

		if (damaged) {
			damageImage.color = damageColor;
		} else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, Time.deltaTime);
		}
		damaged = false;
	}

	// Use this for initialization
	void OnEnable () {
		damageImage.color = Color.clear;
		health = maxHealth;
        armor = maxArmor;
		healthText.text = health.ToString ();
        armorText.text = armor.ToString();
		anim.SetTrigger ("Alive");
		// Si se trata del jugador local
		// normalmente, esto se disparará en el momento de hacer respawn
		if (isLocalPlayer) {
			// Activamos el NetFPSController
			fpsController.enabled = true;
			// Activamos el NetShoot
			netShoot.enabled = true;
			// Vaciamos el texto de información de stunt
			stunnedText.text = "";
			// Reseteamos la condición de stuneado, para no hacer spawn stuneados
			stunned = 0;
		}
	}

    [Server]
    public void TakeHealth(float value)
    {
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        RpcTakeHealth(health);
    }

    [Server]
    public void TakeArmor(float value)
    {
        armor += value;
        if (armor > maxArmor)
        {
            armor = maxArmor;
        }
        RpcTakeArmor(armor);
    }

    [ClientRpc]
    void RpcTakeHealth(float actualHealth)
    {
        health = actualHealth;
        healthText.text = health.ToString();
    }

    [ClientRpc]
    void RpcTakeArmor(float actualArmor)
    {
        armor = actualArmor;
        armorText.text = armor.ToString();
    }

    // Solo el servidor ejecutará el siguiente método
    [Server]
	/// <summary>
	/// Aplica el daño recibido como parámetro, devolverá true si el jugador muere
	/// </summary>
	/// <returns><c>true</c>, if damage was taken, <c>false</c> otherwise.</returns>
	/// <param name="damage">Damage.</param>
	public bool TakeDamage(float damage) {
		// Consideramos inicialmente que el jugador no está muerto
		bool died = false;

		// Verificamos que el jugador no esté previamente muerto
		// Si está muerto, salimos del método
		if (health <= 0) {
			return died;
		}

        if (armor <= 0)
        {
            armor = 0f;
            // Si está vivo, le hacemos daño
            health -= damage;
        } else
        {
            armor -= damage;
        }
		
		
		// Guardamos el la variabl ebool si el jugador ha muerto tras el impacto
		died = health <= 0;
		// Si está muerto, activamos el trigger del animator para muerte
		if (died) {
			anim.SetTrigger ("Dead");
			// Incremento el número de deads en el servidor, lo que hará que el contador
			// se transmita al resto de instancias gracias al SyncVar
			GetComponent<Score> ().deads++;
		}
		// Si ha muerto por el impacto, informo a todas las instancias del jugador
		RpcTakeDamage (died, health, armor);
		// Devuelvo si el jugador ha muerto por el impacto
		return died;
	}

	[ClientRpc]
	/// <summary>
	/// Informamos al resto de instancias si el jugador ha muerto, dependiendo del parámetro
	/// </summary>
	/// <param name="died">If set to <c>true</c> died.</param>
	void RpcTakeDamage(bool died, float actualHealth, float actualArmor) {
		if (died) {
			// Ejecutamos el método die, que desactivará al jugador y preparará el respawn
			player.Die ();
			anim.SetTrigger ("Dead");
		} else {
			// Activamos la imagen de daño cuando se recibe daño
			damaged = true;
			health = actualHealth;
            armor = actualArmor;
			healthText.text = health.ToString ();
            armorText.text = armor.ToString();
        }
	}

	[Server]
	/// <summary>
	/// Ejecuta el stunt en el servidor
	/// </summary>
	/// <param name="stuntTime">Stunt time.</param>
	public void TakeStunt(float stuntTime){
		// Asignamos el tiempo recibido como parámetro, como tiempo que el jugador
		// estará stuneado
		stunned = stuntTime;
		RpcTakeStunt (stuntTime);
	}

	[ClientRpc]
	/// <summary>
	/// Aplica el efecto de stunt a todas las instancias de clientes
	/// </summary>
	/// <param name="stuntTime">Stunt time.</param>
	void RpcTakeStunt(float stuntTime){
		stunned = stuntTime;
	}
}