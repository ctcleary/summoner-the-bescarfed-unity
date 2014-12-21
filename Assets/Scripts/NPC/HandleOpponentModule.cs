using UnityEngine;
using System.Collections;

public class HandleOpponentModule : MonoBehaviour, INPCModule {
	
	private NPC npcController;

	private string opponentTag;
	private Facing facing;

	private Transform pursuitTarget;
	private Transform fleeTarget;

	// Use this for initialization
	void Start ()
	{
		npcController = GetComponent<NPC>();
//		opponentTag = npcController.OpponentTag;
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
		if (pursuitTarget != null) {
			adjustment += (pursuitTarget.position - transform.position);
		}
		if (fleeTarget != null) {
//			adjustment -= (transform.position - fleeTarget.transform);
		}

		return new Vector2(adjustment.x, adjustment.y).normalized;
	}

	public void HandleSawOpponent(Collider2D other)
	{
		if (other.CompareTag(opponentTag)) {
			if (pursuitTarget == null) {
				pursuitTarget = other.transform;
			} else {
				pursuitTarget = WhichIsCloser(other.transform, pursuitTarget);
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
