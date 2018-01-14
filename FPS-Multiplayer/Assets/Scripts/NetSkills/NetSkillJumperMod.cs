using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetSkillJumperMod : NetworkBehaviour
{
    // Tiempo que stuneara la habildiad
    public float jumpMultipler = 3f;
    // Duración del sprint
    public float duration = 5f;
    // Tiempo de enfriamiento de la habilidad
    public float coolDown = 30f;
    // Imagen que indicará el tiempo restante de CoolDown
    public Image coolDownImage;
    // Sistema de partículas que será mostrado al usar la skill
    public ParticleSystem skillEffect;

    // Contador interno de tiempo de enfriamiento
    private float coolDownTimer;


    private NetFPSController netFPSController;
    private NetWeaponShoot netWeaponShoot;
    private NetPlayerHealth netPlayerHealth;


    void Start()
    {
        netFPSController = GetComponent<NetFPSController>();
        netWeaponShoot = GetComponentInChildren<NetWeaponShoot>();
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
        if (Input.GetButtonDown("Hability1") && coolDownTimer <= 0)
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

    [Command]
    /// <summary>
    /// Comando ejecutado en el servidor que será el encargado de verificar que los jugadores han sido stuneados
    /// </summary>
    public void CmdUseSkill()
    {
        // Llamamos a la corrutina que se encarga de aumentarnos la duración del sprint y resetearla
        StartCoroutine("RunMultipler");
        // Aplicamos el stunt al arma
        StartCoroutine("ApplyStunt");
        // Aplicamos el godmode
        StartCoroutine("GodMode");
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
        StartCoroutine("SkillEffect");
        // Reseteamos el temporizador
        coolDownTimer = coolDown;
    }

    /// <summary>
    /// Activa los efectos asociados a la habilidad
    /// </summary>
    public IEnumerator SkillEffect()
    {
        // Activamos el sistema de partículas
        skillEffect.Play();
        yield return new WaitForSeconds(duration);
        skillEffect.Stop();
    }

    IEnumerator RunMultipler()
    {
        netFPSController.jumpForce *= jumpMultipler;
        yield return new WaitForSeconds(duration);
        netFPSController.jumpForce /= jumpMultipler;
    }

    IEnumerator ApplyStunt()
    {
        netWeaponShoot.shootStunt = true;
        yield return new WaitForSeconds(duration);
        netWeaponShoot.shootStunt = false;
    }

    IEnumerator GodMode()
    {
        netPlayerHealth.godMode = true;
        yield return new WaitForSeconds(duration);
        netPlayerHealth.godMode = false;
    }

}
