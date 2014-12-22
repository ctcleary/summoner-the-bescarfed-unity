using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {

	private Texture2D black;
	private Texture2D green;
	private Texture2D red;

	// Use this for initialization
	void Start () {
		black = new Texture2D (1, 1);
		black.SetPixel (1, 1, Color.black);
		black.Apply ();
	}

	void OnGUI()
	{
		
		GUI.DrawTexture (new Rect (transform.position.x, transform.position.y, 300, 300), black);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
