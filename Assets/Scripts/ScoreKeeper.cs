 using UnityEngine;
using System.Collections;

public class ScoreKeeper : Singleton<ScoreKeeper> {

	public static int villageLives = 20;
	public static int playerScore = 0;

	public static int winScore = 10;

	void Awake() {
		_instance = this; // Access via .instance
	}

	void OnGUI() {
		// TEMP
		GUI.Label (new Rect (0, 0, 100, 25), "Lives: " + villageLives);
		GUI.Label (new Rect (100, 0, 100, 25), "Score: " + playerScore);
	}
	
	public static void addScore(int toAdd) {
		playerScore += toAdd;
		if (playerScore >= winScore) {
			Debug.Log ("You won, bro.");
		}
	}

	public static void loseLives(int lives = 1) {
		villageLives -= lives;
		if (lives <= 0) {
			Debug.Log ("You lost, bro.");
		}
	}
}
