using UnityEngine;
using System.Collections;

/**
 * This is attached to a VisionCollider prefab
 * in order to separate the vision collider from the
 * Entity's actual hitbox. This script/prefab's only
 * duty is to inform the VisionModule attached to the
 * parent NPC when a TriggerEnter event occurs, indicating
 * that the Entity is now seeing something new.
 */
public class VisionColliderController : MonoBehaviour {

	private VisionModule visionModule;
	private Vector2 visionScale;

	public void SetVisionModule(VisionModule visionModule)
	{
		this.visionModule = (VisionModule)visionModule;
		visionScale = this.visionModule.GetVisionScale ();
		
		transform.localScale = new Vector3 (visionScale.x, visionScale.y, transform.localScale.z);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		visionModule.HandleOnTriggerEnter2D(other);
	}
}
