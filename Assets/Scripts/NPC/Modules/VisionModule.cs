using UnityEngine;
using System.Collections;

public class VisionModule : NPCModule {

	public float yScale = 1.0f;
	public float xScale = 1.0f;

	private NPC npcController;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
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

	public Vector2 GetVisionScale()
	{
		return new Vector2(xScale, yScale);
	}
	
	// INPCModule
	public override void Reset()
	{
		
	}
}
