using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour, IMessageHandler {

    public AudioClip playMusic;
    public AudioClip gameOverMusic;

    private MessageBus GameMessageBus = GlobalMessageBus.Instance;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        if (playMusic != null) {
            audioSource.clip = playMusic;
            audioSource.Play();
        }

        GameMessageBus.AddMessageListener(MessageType.Lost, this);

    }

    public void HandleMessage(Message message)
    {
        if (message.MessageType == MessageType.Lost)
        {
            HandleGameOverScreen();
        }

    }

    // Update is called once per frame
    private void HandleGameOverScreen()
    {
        if (gameObject != null && gameOverMusic != null)
        {
            audioSource.clip = gameOverMusic;
            audioSource.Play();
        }
    }
}
