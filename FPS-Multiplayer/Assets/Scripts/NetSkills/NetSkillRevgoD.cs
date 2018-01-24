using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetSkillRevgoD : NetworkBehaviour
{

    [Header("Habilidades Revgo.D")]
    // Cantidad de salud curada
    public float healthAmount = 150f;

    public Image crosshair;
    public GameObject scopeOverlay;
    public Camera cam;
    public GameObject rifle;
    public MeshRenderer rightHand;
    public MeshRenderer leftHand;

    // Duración del sprint
    public float duration = 1f;
    // Tiempo de enfriamiento de la habilidad
    public float coolDown = 30f;
    // Imagen que indicará el tiempo restante de CoolDown
    public Image coolDownImage;
    // Sistema de partículas que será mostrado al usar la skill
    public ParticleSystem skillEffect;

    public Animator scopeAnim;

    public bool scoped = false;

    // Contador interno de tiempo de enfriamiento
    private float coolDownTimer;
    
    private NetPlayerHealth netPlayerHealth;
    
    void Start()
    {
        netPlayerHealth = GetComponent<NetPlayerHealth>();
        scoped = false;
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
        
        if (Input.GetMouseButtonDown(1))
        {
            scoped = !scoped;
            scopeAnim.SetBool("Scope", scoped);
            if (scoped)
            {
                ScopeIn();
                StartCoroutine("OnScope");
            } else
            {
                ScopeOut();
            }
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
        HealSkill();
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

    void HealSkill() {
        netPlayerHealth.TakeHealth(healthAmount);
    }

    void ScopeIn()
    {
        scoped = true;
        crosshair.enabled = false;
        
    }

    void ScopeOut()
    {
        scoped = false;
        crosshair.enabled = true;
        scopeOverlay.SetActive(false);
        rifle.SetActive(true);
        rightHand.enabled = true;
        leftHand.enabled = true;
        cam.fieldOfView = 60f;
    }

    IEnumerator OnScope()
    {
        yield return new WaitForSeconds(.15f);
        rightHand.enabled = false;
        leftHand.enabled = false;
        rifle.SetActive(false);
        scopeOverlay.SetActive(true);
        cam.fieldOfView = 15f;
    }
}
