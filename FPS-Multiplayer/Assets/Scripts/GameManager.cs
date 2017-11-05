using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject gameMenu;

	public string playerName;
	// Referencia al campo de texto donde mostraremos las puntuaciones
	public Text scoreText;

	public bool pause;

	// Texto que será mostrado en el score
	private string score;

	public static GameManager GM;

	void Awake() {
		if (GM == null) {
			GM = GetComponent<GameManager> ();
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
		score = "Player \t\t\t\t Kills \t Deads \n";

		// Recorro todos los jugadores en la escena, para generar el texto de score,
		// que mostrará el nombre, kills y muertes de cada jugador
		foreach (var player in GameObject.FindGameObjectsWithTag("Player")) {
			Score tempScore = player.GetComponent<Score> ();
			score += tempScore.playerName + "\t\t\t\t" + tempScore.kills + "\t" + tempScore.deads + "\n";
		}

		scoreText.text = score;
	}

	public void ExitGame() {
		Application.Quit ();
	}

}
