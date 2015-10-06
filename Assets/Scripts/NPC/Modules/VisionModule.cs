using UnityEngine;

public class VisionModule : NPCModule {

	public float yScale = 1.0f;
	public float xScale = 1.0f;

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
