using UnityEngine;
using System.Collections;

public class HandleOpponentModule : MonoBehaviour, INPCModule {

	public HandleOpponentBehavior behavior;

	private NPC npcController;

	private string opponentTag;
	private Facing facing;

	private Transform pursuitTarget;
	private Transform fleeTarget;

	// Use this for initialization
	void Start ()
	{
		npcController = GetComponent<NPC>();
		opponentTag = npcController.OpponentTag;
//		facing = npcController.GetFacing();
	}

	public bool HasAnyTargets()
	{
		return HasPursuitTarget() || HasFleeTarget();
	}
	
	private bool HasPursuitTarget()
	{
		return pursuitTarget != null;
	}
	private bool HasFleeTarget()
	{
		return fleeTarget != null;
	}
	
	// Update is called once per frame
	void Update ()
	{
		opponentTag = npcController.OpponentTag;
		facing = npcController.GetFacing();

		if (HasAnyTargets()) {
			DetermineLoseTargets();
		}
	}

	public Vector2 GetMovementDirection()
	{
		Vector3 adjustment = new Vector3(0,0,0);
		if (tag.Equals (NPCKind.ENEMY)) {
			Debug.Log (HasPursuitTarget () + " :: " + HasFleeTarget ());
		}
		if (pursuitTarget != null) {
			adjustment += (pursuitTarget.position - transform.position);
		}
		if (fleeTarget != null) {
			adjustment -= (fleeTarget.position - transform.position);
		}

		adjustment.x = Mathf.Clamp (adjustment.x, -0.25f, 0.25f);
		Vector2 normalizedAdjustment = new Vector2 (adjustment.x, adjustment.y).normalized;
		return normalizedAdjustment;
	}

	public void HandleSawOpponent(Collider2D other)
	{
		if (opponentTag == null) {
			// Hasn't been initialized yet.
			return;
		}
		if (other.CompareTag (opponentTag)) {
			switch (behavior) {
			case HandleOpponentBehavior.FIGHT:
				if (!HasPursuitTarget ()) {
					pursuitTarget = other.transform;
				} else {
					pursuitTarget = WhichIsCloser (other.transform, pursuitTarget);
				}
				break;
			case HandleOpponentBehavior.FLEE:
				if (!HasFleeTarget()) {
					fleeTarget = other.transform;
				} else {
					fleeTarget = WhichIsCloser (other.transform, fleeTarget);
				}
				break;
			default:
				break;
			}
		}
	}

	private void DetermineLoseTargets() {
		if (HasPursuitTarget() && IsBehindMe (pursuitTarget)) {
			pursuitTarget = null;
		}
		if (HasFleeTarget() && IsBehindMe(fleeTarget)) {
			fleeTarget = null;
		}
	}

	private bool IsBehindMe(Transform other)
	{
		if (facing == Facing.RIGHT) {
			return other.position.x < transform.position.x;
		} else {
			return other.position.x > transform.position.x;
		}
	}

	private Transform WhichIsCloser(Transform a, Transform b)
	{
		float distanceToA = Vector3.Distance(transform.position, a.position);
		float distanceToB = Vector3.Distance(transform.position, b.position);

		if (distanceToA < distanceToB) {
			return a;
		} else {
			return b;
		}
	}


	// INPCModule
	public void Reset()
	{
		pursuitTarget = null;
		fleeTarget = null;
	}
}
