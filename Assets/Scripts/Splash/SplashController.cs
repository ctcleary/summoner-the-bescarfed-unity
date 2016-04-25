using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashController : MonoBehaviour {
    
    public Text startText;

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
            SceneManager.LoadScene("Level0");
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

