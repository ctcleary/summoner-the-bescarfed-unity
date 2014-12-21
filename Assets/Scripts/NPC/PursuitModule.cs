using UnityEngine;
using System.Collections;

public class PursuitModule : MonoBehaviour, INPCModule {

//	private Collider2D visionCollider;
//	private bool hasRequiredComponents = false;

	public Collider2D pursuitTarget;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (pursuitTarget != null) {
			Debug.Log ("Should pursue :: " + pursuitTarget.tag);
		}
	}

	// INPCModule
	public void Reset()
	{

	}
}
