using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlobalKeyListener : MonoBehaviour {

    private bool isPaused = false;
    //private GUIStyle pausedStyle = new GUIStyle();

    public Text pauseText;
    //private string pauseTextString;

    // Use this for initialization
    void Start ()
    {
        //pausedStyle.fontSize = 42;
        //pausedStyle.fontStyle = FontStyle.Bold;
        //pausedStyle.alignment = TextAnchor.MiddleCenter;
        //pausedStyle.normal.textColor = Color.white;

        pauseText.text = "";
    }

    void OnGUI()
    {
        //if (isPaused && pauseText.text == "")
        //{
        //    pauseText.text = "PAUSED";
        //}
        //else
        //{
        //    pauseText.text = "";
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
        }

        if (isPaused && Time.timeScale != 0)
        {
            Time.timeScale = 0;
            pauseText.text = "PAUSED";
        } else if (!isPaused && Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pauseText.text = "";
        }
    }
}
