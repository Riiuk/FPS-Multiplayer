using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// Heredará de NetworkBehaviour
public class NetSkillAreaStunt : NetworkBehaviour {

	// Tiempo que stuneara la habildiad
	public float stuntTime = 5f;
	// Tiempo de enfriamiento de la habilidad
	public float coolDown = 30f;
	// Radio de impacto de la habilidad
	public float impactRadius = 5f;
	// para poder mostrar u ocultar el área de acción de la skill en el editor
	public bool drawGizmo = false;
	// Imagen que indicará el tiempo restante de CoolDown
	public Image coolDownImage;
	// Sistema de partículas que será mostrado al usar la skill
	public ParticleSystem skillEffect;

	// Contador interno de tiempo de enfriamiento
	private float coolDownTimer;


	private NetPlayerHealth netPlayerHealth;

	void Start() {
		netPlayerHealth = GetComponent<NetPlayerHealth> ();
	}

	// Update is called once per frame
	void Update () {
		// Si no es el jugador local, salimos dle update sin hacer nada
		if (!isLocalPlayer) {
			return;
		}
		// Si el jugador pulsa el botón Hability1 y si el temporizador ha terminado
		if (Input.GetButtonDown("Hability1") && coolDownTimer <= 0 && netPlayerHealth.stunned <= 0) {
			CmdUseSkill ();
		}
		// Si el contador es mayor que 0 realizamos las operaciones de dibujado del CooldownImage
		if (coolDownTimer > 0) {
			coolDownTimer -= Time.deltaTime;
			// Mostramos visualmente el tiempo restante para que la habilidad esté disponible
			coolDownImage.fillAmount = coolDownTimer / coolDown;
		} else {
			coolDownImage.fillAmount = 0;
		}
	}

	[Command]
	/// <summary>
	/// Comando ejecutado en el servidor que será el encargado de verificar que los jugadores han sido stuneados
	/// </summary>
	public void CmdUseSkill() {
		// recuperamos como parametro los objetos golpeados por el overlapSphere
		var colls = Physics.OverlapSphere (transform.position, impactRadius);

		foreach (Collider col in colls) {
			// Intentamos recuperar el componente NetPlayerHealth en el obhjeto colisionado por el overlapsphere
			NetPlayerHealth enemy = col.GetComponent<NetPlayerHealth> ();

			// Si el objeto impactado tiene el script de vida, significará que se trate de un jugador susceptible de ser stuneado
			if (enemy != null  && col.gameObject != gameObject) {
				// Si cumple las condiciones, lo stuneamos
				enemy.TakeStunt (stuntTime);
			}
		}
		// Ponemos el contador de tiempo de coolDown al valor definido
		coolDownTimer = coolDown;
		// Activamos los efectos de la habilidad
		SkillEffect ();
		// Solicitamos que se realicen las acciones necesarias en las instancias
		RpcUseSkill ();

	}

	[ClientRpc]
	/// <summary>
	/// Activamos el efecto de skill y el reseteo del cooldown al resto de instancias
	/// </summary>
	public void RpcUseSkill() {
		// Mostramos efectos de la habildiad
		SkillEffect ();
		// Reseteamos el temporizador
		coolDownTimer = coolDown;
	}

	/// <summary>
	/// Activa los efectos asociados a la habilidad
	/// </summary>
	public void SkillEffect(){
		// Activamos el sistema de partículas
		skillEffect.Play ();
	}

	void OnDrawGizmos() {
		// Si tenemos marcado en el inspector que se muestre el gizmo,
		// dibujamos la esfera representando la zona en la que se aplicará la habilidad
		if (drawGizmo) {
			Gizmos.DrawSphere (transform.position, impactRadius);
		}
	}

}
