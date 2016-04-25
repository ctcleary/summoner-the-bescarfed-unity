using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour, IMessageHandler {
    
    private MessageBus GameMessageBus = GlobalMessageBus.Instance;
    private Dictionary<MessageType, Action<Message>> SupportedMessageMap;

    private bool isPaused = false;
    public GameObject pauseText;

    private bool isEnded = false;
    public GameObject gameOver;
    public Text score;
    public Text time;

    public void Awake()
    {
        SupportedMessageMap = new Dictionary<MessageType, Action<Message>>()
        {
            { MessageType.Paused, HandlePaused },
            { MessageType.Lost, HandleLost }
        };
    }

    private void Listen()
    {
        IMessageHandler thisHandler = (IMessageHandler)this;
        foreach (MessageType mesageType in SupportedMessageMap.Keys)
        {
            GameMessageBus.AddMessageListener(mesageType, thisHandler);
        }
    }

    public void HandleMessage(Message message)
    {
        // Call this message type's handler function from the SupportedMessageMap
        SupportedMessageMap[message.MessageType](message);
    }

    // Use this for initialization
    void Start()
    {
        Listen();

        pauseText.SetActive(false);
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Esc"))
        {
            Application.Quit();
        }

        if (isEnded && Input.GetButtonDown("Submit"))
        {
            Time.timeScale = 1;
            // TODO reset all the things... score, player health, etc.
            // instead of being lazy and reloading splash menu
            SceneManager.LoadScene(0);
        }

        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            GameMessageBus.TriggerMessage(
                MessageBuilder.BuildPausedMessage(MessageType.Paused, isPaused));
        }
    }

    private void HandlePaused(Message message)
    {
        if (message.BoolValue)
        {
            Time.timeScale = 0;
            pauseText.SetActive(true);
        } else {

            Time.timeScale = 1;
            pauseText.SetActive(false);
        }
    }

    private void HandleLost(Message message)
    {
        Debug.Log("HandleLost");
        Time.timeScale = 0;

        //score.text = ScoreKeeper.getScore().ToString();
        //time.text = "";

        isEnded = true;
        gameOver.SetActive(isEnded);
    }

}
