 using UnityEngine;
using System.Collections;

public class ScoreKeeper : Singleton<ScoreKeeper> {

	public static int villageLives = 20;
	public static int playerScore = 0;

	public static int winScore = 15;

	private static bool hasLost = false;
	private static bool hasWon = false;
	private static string endMsg;
	
//	private static float aspect;
//	private static float orthoSize;

	void Awake() {
		_instance = this; // Access via .instance
	}

//	void Start () {
//		Camera mainCam = Camera.main;
//		aspect = mainCam.aspect;
//		orthoSize = mainCam.orthographicSize;
//	}

	void OnGUI() {
		// TEMP
		GUI.Label (new Rect (0, 0, 100, 25), "Lives: " + villageLives);
		GUI.Label (new Rect (100, 0, 100, 25), "Score: " + playerScore);

		if (hasWon || hasLost) {
			GUI.Label (new Rect (0, 30, 200, 25), endMsg);  
			GUI.Label (new Rect (0, 45, 200, 25), "(Play will continue.)");  
		}
	}
	
	public static void addScore(int toAdd) {
		playerScore += toAdd;
		if (playerScore >= winScore && !hasLost) {
			hasWon = true;
			endMsg = "You won, bro. Kudos.";
//			Debug.Log ("You won, bro.");
		}
	}

	public static void loseLives(int lives = 1) {
		villageLives -= lives;
		if (lives <= 0 && !hasWon) {
			hasLost = true;
			endMsg = "You lost. It's okay though.";
//			Debug.Log ("You lost, bro.");
		}
	}
}
