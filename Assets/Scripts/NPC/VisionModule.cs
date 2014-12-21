using UnityEngine;
using System.Collections;

public class VisionModule : MonoBehaviour, INPCModule {
	
	private NPC npcController;

	// Use this for initialization
	void Start () {
		npcController = GetComponent<NPC>();
	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}
	
	// Called by the "VisionTrigger"
	public void HandleOnTriggerEnter2D(Collider2D other)
	{
		npcController.HandleOnVisionEnter(other);
	}
	
	// INPCModule
	public void Reset()
	{
		
	}
}
