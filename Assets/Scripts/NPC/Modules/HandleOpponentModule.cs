using UnityEngine;
using System.Collections;
using System;

public class HandleOpponentModule : NPCModule, INPCModule {

	public HandleOpponentBehavior behavior;
	private Facing facing = Facing.RIGHT;

	private Transform pursuitTarget;
	private Transform fleeTarget;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
	}

    // Implement NPCModule abstracts
    protected override void Listen()
    {
        NPCMessageBus.AddMessageListener(MessageType.OpponentsChange, (IMessageHandler)this);
        NPCMessageBus.AddMessageListener(MessageType.VisionEnter, (IMessageHandler)this);
        NPCMessageBus.AddMessageListener(MessageType.Faced, (IMessageHandler)this);
    }

    public override void HandleMessage(Message message)
    {
        switch (message.MessageType)
        {
            case MessageType.OpponentsChange:
                HandleOpponentsChangeMessage(message);
                break;
            case MessageType.Faced:
                HandleFacedMessage(message);
                break;
            case MessageType.VisionEnter:
                HandleVisionEnterMessage(message);
                break;
        }
    }

    private void HandleOpponentsChangeMessage(Message message)
    {
        OpponentTag = message.NPCKindValue.Tag;
    }

    private void HandleFacedMessage(Message message)
    {
        facing = message.FacingValue;
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
		//facing = npcController.GetFacing();

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
			adjustment -= (fleeTarget.position - transform.position);
		}

		adjustment.x = Mathf.Clamp (adjustment.x, -0.25f, 0.25f);
		Vector2 normalizedAdjustment = new Vector2 (adjustment.x, adjustment.y).normalized;
		return normalizedAdjustment;
	}

	public void HandleVisionEnterMessage(Message visionEnterMessage)
	{
        GameObject other = visionEnterMessage.GameObjectValue;
		if (other.CompareTag (OpponentTag)) {
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

	
	// Implement Abstract
	public override void Reset()
	{
		pursuitTarget = null;
		fleeTarget = null;
	}
}
