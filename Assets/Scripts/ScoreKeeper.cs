 using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : Singleton<ScoreKeeper> {

    private static MessageBus GameMessageBus = GlobalMessageBus.Instance;

    public static int villageLives = 20;
	public static int playerScore = 0;

	//private static bool hasLost = false;

    public Text livesText;
    public Text scoreText;
    
    void Awake() {
        _instance = this; // Access via .instance
        //DontDestroyOnLoad(gameObject);
    }

    //void Update()
    //{
    //}

    void OnGUI()
    {
        livesText.text = "Lives: " + villageLives;
        scoreText.text = "Score: " + playerScore;
    }

    public static void addScore(int toAdd)
    {
        playerScore += toAdd;
    }
    public static int getScore()
    {
        return playerScore;
    }

    public static void loseLives(int lives = 1) {
		villageLives -= lives;
		if (villageLives <= 0) {
			//hasLost = true;
            GameMessageBus.TriggerMessage(
                MessageBuilder.BuildMessage(MessageType.Lost));
        }
    }
}
