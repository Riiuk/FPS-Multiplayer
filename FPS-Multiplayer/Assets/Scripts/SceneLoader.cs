using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Nombre de la siguiente escena")]
    public string sceneName;
    [Tooltip("Tiempo que tardará la escena en cambiar")]
    public float delay;
    [Header("Unity Objects")]
    [Tooltip("Objeto tipo imagen del fade")]
    public Image imagenFade;
    [Header("Unity Settings")]
    [Tooltip("Curva de fade")]
    public AnimationCurve curva;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            FadeTo(sceneName);
        }
    }

    public void FadeTo(string escena)
    {
        StartCoroutine(FadeOut(escena));
    }

    IEnumerator FadeIn()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime;
            float _curva = curva.Evaluate(alpha);
            imagenFade.color = new Color(0f, 0f, 0f, _curva);
            yield return 0;
        }
    }

    IEnumerator FadeOut(string escena)
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            float _curva = curva.Evaluate(alpha);
            imagenFade.color = new Color(0f, 0f, 0f, _curva);
            yield return 0;
        }

        SceneManager.LoadScene(escena);
    }


}
