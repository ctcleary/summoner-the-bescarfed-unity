using UnityEngine;
using System.Collections;

public class GlobalKeyListener : MonoBehaviour {

    bool isPaused = false; 

	// Use this for initialization
	void Start () {
	
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
