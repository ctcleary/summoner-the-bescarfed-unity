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
public class VisionTrigger : MonoBehaviour {

	private VisionModule visionModule;

	public void SetVisionModule(VisionModule visionModule)
	{
		this.visionModule = visionModule;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		visionModule.HandleOnTriggerEnter2D(other);
	}
}
