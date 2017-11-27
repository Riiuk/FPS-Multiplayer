using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetSkillRougeFlame : NetworkBehaviour
{

    [Header("Modificadores habilidad")]
    // Tiempo que stuneara la habildiad
    public float damageMultiplier = 100f;

    public float shootRate = 1f;
    // Duración del sprint
    [Header("Atributos comúnes")]
    public float duration = 5f;
    // Tiempo de enfriamiento de la habilidad
    public float coolDown = 30f;
    [Header("Unity Settings")]
    // Imagen que indicará el tiempo restante de CoolDown
    public Image coolDownImage;
    // Sistema de partículas que será mostrado al usar la skill
    public GameObject skillEffect;
    public GameObject spearEffect;

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
        // Aplicamos el godmode
        StartCoroutine("GodMode");
        // Activamos las habilidades de arma
        StartCoroutine("FireSpear");
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
        skillEffect.SetActive(true);
        spearEffect.SetActive(true);
        yield return new WaitForSeconds(duration);
        skillEffect.SetActive(false);
        spearEffect.SetActive(false);
    }

    /// <summary>
    /// Activa y desactiva el GodMode
    /// </summary>
    /// <returns></returns>
    IEnumerator GodMode()
    {
        netPlayerHealth.godMode = true;
        yield return new WaitForSeconds(duration);
        netPlayerHealth.godMode = false;
    }

    /// <summary>
    /// Activa y desactiva el daño en el arma
    /// </summary>
    /// <returns></returns>
    IEnumerator FireSpear()
    {
        // Variable temporal que almacena la cadencia de disparo default
        float _shootDelay = netWeaponShoot.shootDelay;
        // Variable temporal para almacenar el daño default del disparo
        float _damage = netWeaponShoot.damage;

        // Ponemos el daño y el delay de disparo como hemos configurado arriba
        netWeaponShoot.damage = damageMultiplier;
        netWeaponShoot.shootDelay = shootRate;
        // Esperamos el tiempo establecido que dura la habilidad
        yield return new WaitForSeconds(duration);
        // Volvemos a poner en default el daño y el delay de disparo
        netWeaponShoot.damage = _damage;
        netWeaponShoot.shootDelay = _shootDelay;
    }
}
