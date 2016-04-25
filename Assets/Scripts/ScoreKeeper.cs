 using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : Singleton<ScoreKeeper> {

    private static MessageBus GameMessageBus = GlobalMessageBus.Instance;

    public static int villageLives = 20;
	public static int playerScore = 0;

    //private int currVillageLives = villageLives;
    //private int currPlayerScore = playerScore;

    public Text livesText;
    public Text scoreText;
    
    private static AudioSource audioSource;
    
    void Awake() {
        _instance = this; // Access via .instance
        villageLives = 20;
        playerScore = 0;
        audioSource = GetComponent<AudioSource>();
    }
    
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
        if (villageLives >= 1)
        {
            villageLives -= lives;
            if (audioSource) {
                audioSource.Play();
            }
        }

		if (villageLives == 0) {
            GameMessageBus.TriggerMessage(
                MessageBuilder.BuildMessage(MessageType.Lost));
        }
    }
}
