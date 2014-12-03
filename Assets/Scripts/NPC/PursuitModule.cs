using UnityEngine;
using System.Collections;

public class PursuitModule : MonoBehaviour {

	private Collider2D visionCollider;
	private bool hasRequiredComponents = false;

	// Use this for initialization
	void Start ()
	{
		FindVisionColliderOrDisable ();
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void FindVisionColliderOrDisable()
	{
		Transform visionColliderContainer = transform.FindChild ("VisionCollider");
		if (visionColliderContainer != null) {
			visionCollider = visionColliderContainer.collider2D;

			if (visionCollider != null) {
				hasRequiredComponents = true;
			}
		}
		
		if (!hasRequiredComponents) {
			this.enabled = false;
		}
	}
}
