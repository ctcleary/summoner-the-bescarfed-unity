 using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : Singleton<ScoreKeeper> {

	public static int villageLives = 20;
	public static int playerScore = 0;

	//public static int winScore = 15;

	private static bool hasLost = false;
	private static bool hasWon = false;
	private static string endMsg;

    public Text livesText;
    public Text scoreText;
    
    void Awake() {
		_instance = this; // Access via .instance
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (hasLost)
        {
            Debug.Log("HAS LOST");
            Time.timeScale = 0;
        }
    }

    void OnGUI()
    {
        livesText.text = "Lives: " + villageLives;
        scoreText.text = "Score: " + playerScore;
    }
	
	public static void addScore(int toAdd) {
		playerScore += toAdd;
	}

	public static void loseLives(int lives = 1) {
		villageLives -= lives;
		if (lives <= 0) {
			hasLost = true;
			endMsg = "You lost. It's okay though.";
            Debug.Log("You lost, bro.");
        }
    }
}
