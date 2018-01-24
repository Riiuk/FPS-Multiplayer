using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int killsToWin = 2;
    public InputField killsToWinField;

	public GameObject gameMenu;
    public GameObject finalMenu;

	public string playerName;
	// Referencia al campo de texto donde mostraremos las puntuaciones
	public Text scoreText;
    public Text scoreKillsText;
    public Text scoreDeadsText;

    public Text killsToWinText;

    public Text finalMenssage;

	public bool pause;

	// Texto que será mostrado en el score
	private string score;
    private string scoreKills;
    private string scoreDeads;

    private string killsToWinString;

	public static GameManager GM;

	void Awake() {
		if (GM == null) {
			GM = GetComponent<GameManager> ();
		}
	}

    void Start()
    {
        finalMenssage.text = "";
        finalMenu.SetActive(false);    
    }

    void Update()
    {
        WinMatch();
    }

    /// <summary>
    /// Muestra/oculta el menú de juego
    /// </summary>
    /// <param name="show">If set to <c>true</c> show.</param>
    public void GameMenu(bool show) {
		
		if (show) {
			UpdateScore ();
		}
		pause = show;
		gameMenu.SetActive (show);
	}

	/// <summary>
	/// Actualiza el score revisando la puntuación de cada jugador en la partida
	/// </summary>
	public void UpdateScore() {

        /*
		score = "Player \t\t\t\t Kills \t Deads \n";

		// Recorro todos los jugadores en la escena, para generar el texto de score,
		// que mostrará el nombre, kills y muertes de cada jugador
		foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
			Score tempScore = player.GetComponent<Score> ();
			score += tempScore.playerName + "\t\t\t\t" + tempScore.kills + "\t" + tempScore.deads + "\n";
		}

		scoreText.text = score;
        */

        score = "";
        scoreKills = "";
        scoreDeads = "";

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Score tempScore = player.GetComponent<Score>();
            score += tempScore.playerName + "\n";
            scoreKills += tempScore.kills + "\n";
            scoreDeads += tempScore.deads + "\n";
        }

        scoreText.text = score;
        scoreKillsText.text = scoreKills;
        scoreDeadsText.text = scoreDeads;
    }

    public void WinMatch()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Score tempScore = player.GetComponent<Score>();
            if (tempScore.kills >= killsToWin)
            {
                Cursor.lockState = CursorLockMode.None;
                finalMenssage.text = "EL JUGADOR " + tempScore.playerName + " HA GANADO LA RONDA. PERO HA MUERTO " + tempScore.deads + " VECES.";
                finalMenu.SetActive(true);
                StartCoroutine("FinishMatch");
            }
        }
    }

    IEnumerator FinishMatch()
    {
        GameObject sceneFader = GameObject.FindGameObjectWithTag("SceneFader");
        yield return new WaitForSeconds(5f);
        sceneFader.GetComponent<SceneFader>().FadeTo("Menu");
    }

    public void SetKillsToWin()
    {
        killsToWin = int.Parse (killsToWinField.text);
    }

	public void ExitGame() {
		Application.Quit ();
	}

}
