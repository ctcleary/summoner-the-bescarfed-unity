using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashController : MonoBehaviour {
    
    public Text startText;
    public Text pressToRevealText;
    public Canvas controlsDetails;
    public AudioSource BGM;

    void Awake()
    {
        controlsDetails.enabled = false;
    }

    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 1;
        StartCoroutine(BlinkStartText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            SceneManager.UnloadScene("Level0");
            SceneManager.LoadScene("Level0");
        }

        if (Input.GetButton("C"))
        {
            controlsDetails.enabled = true;
            pressToRevealText.enabled = false;
        }

        if (Input.GetButton("Mute"))
        {
            if (BGM.enabled) {
                BGM.enabled = false;
            } else {
                BGM.enabled = true;
                BGM.Play();
            }
        }
    }

    private IEnumerator BlinkStartText()
    {
        while (true)
        {
            startText.enabled = true;
            yield return new WaitForSeconds(0.85f);
                startText.enabled = false;
                yield return new WaitForSeconds(0.2f);
        }
    }
}

