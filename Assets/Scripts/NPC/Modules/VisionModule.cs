using UnityEngine;
using System.Collections;

public class VisionModule : NPCModule {

	public float yScale = 1.0f;
	public float xScale = 1.0f;

	//private NPC npcController;

	
	public GameObject visionColliderPrefab;
	private GameObject visionCollider;
	private VisionColliderController visionColliderController;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		
		// Instantiate a VisionCollider and attach it here.
		visionCollider = Instantiate(visionColliderPrefab, transform.position, Quaternion.identity) as GameObject;
		visionCollider.transform.parent = transform;
		
		visionColliderController = visionCollider.GetComponent<VisionColliderController> ();
		visionColliderController.SetVisionModule (this);
	}

    // Implement NPCModule abstracts
    protected override void Listen()
    {

    }

    public override void HandleMessage(Message message)
    {

    }

    // Called by the "VisionTrigger"
    public void HandleOnTriggerEnter2D(Collider2D other)
	{
        TriggerVisionEnterMessage(other);
	}

    private void TriggerVisionEnterMessage(Collider2D other)
    {
        NPCMessageBus.TriggerMessage(
            MessageBuilder.BuildGameObjectMessage(
                MessageType.VisionEnter, other.gameObject));
    }

	public Vector2 GetVisionScale()
	{
		return new Vector2(xScale, yScale);
	}
	
	// INPCModule
	public override void Reset()
    {
        visionColliderController = null;
        visionCollider = null;
    }
}
