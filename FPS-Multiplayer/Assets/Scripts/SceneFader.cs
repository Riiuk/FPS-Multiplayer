using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {

    [Header("Configuracion Delay")]
    [Tooltip("Elegimos si el fader es por tiempo o de elección")]
    public bool delayActivado;
    [Tooltip("Nombre de la siguiente escena (Dejar en blanco si no se activa esta opción)")]
    public string sceneName;
    [Tooltip("Segundos que tardará la escena en pasar (Dejar en blanco si no se activa esta opción)")]
    public float delay = 3f;
    
    [Header("Unity Objects")]
    [Tooltip("Objeto tipo imagen del fade")]
    public Image imagenFade;
    [Tooltip("Panel de seguridad para salir")]
    public GameObject exitPanel;
    [Header("Unity Settings")]
    [Tooltip("Curva de fade")]
    public AnimationCurve curva;

    void Start() {
        StartCoroutine(FadeIn());   
    }

    void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        
        if (delay <= 0 && delayActivado)
        {
            FadeTo(sceneName);
        }
    }

    public void FadeTo(string escena) {
        StartCoroutine(FadeOut(escena));
    }

    public void Salir()
    {
        exitPanel.SetActive(true);
    }

    public void CancelarSalir()
    {
        exitPanel.SetActive(false);
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }

    IEnumerator FadeIn() {
        float alpha = 1f;
        while (alpha > 0f) {
            alpha -= Time.deltaTime;
            float _curva = curva.Evaluate(alpha);
            imagenFade.color = new Color(0f, 0f, 0f, _curva);
            yield return 0;
        }
    }

    IEnumerator FadeOut(string escena) {
        float alpha = 0f;
        while (alpha < 1f) {
            alpha += Time.deltaTime;
            float _curva = curva.Evaluate(alpha);
            imagenFade.color = new Color(0f, 0f, 0f, _curva);
            yield return 0;
        }

        SceneManager.LoadScene(escena);
    }
}
