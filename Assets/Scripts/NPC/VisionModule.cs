using UnityEngine;
using System.Collections;

public class VisionModule : MonoBehaviour, INPCModule {
	
	protected string opponentTag;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Called by the "VisionTrigger"
	public void HandleOnTriggerEnter2D(Collider2D other)
	{
//		Debug.Log(Time.realtimeSinceStartup + " :: " + this.tag + " SAW A " + other.tag);
	}
	
	// INPCModule
	public void Reset()
	{
		
	}
}
