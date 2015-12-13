using UnityEngine;
using System.Collections;

public class GlobalKeyListener : MonoBehaviour {

    private bool isPaused = false;
    private GUIStyle pausedStyle = new GUIStyle();

    // Use this for initialization
    void Start ()
    {
        pausedStyle.fontSize = 42;
        pausedStyle.fontStyle = FontStyle.Bold;
        pausedStyle.alignment = TextAnchor.MiddleCenter;
        pausedStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        // TEMP
        if (isPaused)
        {
            GUI.Label(new Rect(400, 250, 100, 100), "PAUSED", pausedStyle);
        }

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
        } else if (!isPaused && Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}
