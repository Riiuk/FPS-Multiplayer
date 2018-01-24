using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// Heredará de NetworkBehaviour
public class NetSkillLADYMet : NetworkBehaviour
{

   
    // Tiempo de enfriamiento de la habilidad
    public float coolDown = 30f;
    public float spawnDelay = 0.5f;
    public int cantidadMisiles = 5;
    // Imagen que indicará el tiempo restante de CoolDown
    public Image coolDownImage;
    // Sistema de partículas que será mostrado al usar la skill
    public ParticleSystem skillEffect;

    public GameObject missile;
    public Transform misileSpawnPoint;
    // Contador interno de tiempo de enfriamiento
    private float coolDownTimer;


    private NetPlayerHealth netPlayerHealth;

    void Start()
    {
        netPlayerHealth = GetComponent<NetPlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        // Si no es el jugador local, salimos dle update sin hacer nada
        if (!isLocalPlayer)
        {
            return;
        }
        // Si el jugador pulsa el botón Hability1 y si el temporizador ha terminado
        if (Input.GetButtonDown("Hability1") && coolDownTimer <= 0 && netPlayerHealth.stunned <= 0)
        {
            CmdUseSkill();
        }
        // Si el contador es mayor que 0 realizamos las operaciones de dibujado del CooldownImage
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
            // Mostramos visualmente el tiempo restante para que la habilidad esté disponible
            coolDownImage.fillAmount = coolDownTimer / coolDown;
        } else
        {
            coolDownImage.fillAmount = 0;
        }
    }

    IEnumerator MissileLauncher()
    {
        for (int i = 0; i < cantidadMisiles; i++)
        {
            Instantiate(missile, misileSpawnPoint.position, misileSpawnPoint.rotation);
            yield return new WaitForSeconds(spawnDelay);
        }
        
    }

    [Command]
    /// <summary>
    /// Comando ejecutado en el servidor que será el encargado de verificar que los jugadores han sido stuneados
    /// </summary>
    public void CmdUseSkill()
    {
        //MissileLauncher();

        StartCoroutine("MissileLauncher");

        // Ponemos el contador de tiempo de coolDown al valor definido
        coolDownTimer = coolDown;
        // Activamos los efectos de la habilidad
        SkillEffect();
        // Solicitamos que se realicen las acciones necesarias en las instancias
        RpcUseSkill();

    }

    [ClientRpc]
    /// <summary>
    /// Activamos el efecto de skill y el reseteo del cooldown al resto de instancias
    /// </summary>
    public void RpcUseSkill()
    {
        // Mostramos efectos de la habildiad
        SkillEffect();
        // Reseteamos el temporizador
        coolDownTimer = coolDown;
    }

    /// <summary>
    /// Activa los efectos asociados a la habilidad
    /// </summary>
    public void SkillEffect()
    {
        // Activamos el sistema de partículas
        skillEffect.Play();
    }
}

