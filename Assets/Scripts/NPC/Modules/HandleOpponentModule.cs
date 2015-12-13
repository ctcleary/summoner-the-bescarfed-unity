using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HandleOpponentModule : NPCModule, INPCModule {
    
	private Transform pursuitTarget;
	private Transform fleeTarget;

    protected override Dictionary<MessageType, Action<Message>> GetSupportedMessageMap()
    {
        return new Dictionary<MessageType, Action<Message>>()
        {
            { MessageType.OpponentsChange, HandleOpponentsChange },
            { MessageType.VisionEnter, HandleVisionEnter }//,
            //{ MessageType.Faced, HandleFaced }
        };
    }

    // Use this for initialization
    public override void Start ()
	{
		base.Start ();
	}

    private void HandleOpponentsChange(Message message)
    {
        OpponentTag = message.NPCKindValue.Tag;
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
        if (HasAnyTargets()) {
            DetermineLoseTargets();
        }
        if (HasAnyTargets()) {
             NPCMessageBus.TriggerMessage(
                MessageBuilder.BuildVector2Message(MessageType.MovementAdjustment, GetMovementDirection()));
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

	public void HandleVisionEnter(Message visionEnterMessage)
	{
        GameObject other = visionEnterMessage.GameObjectValue;
		if (other.CompareTag (OpponentTag)) {
            switch (this.Controller.GetBehavior())
            {
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

            TargetAcquired();
        }
	}

	private void DetermineLoseTargets() {
		if (HasPursuitTarget() && IsBehindMe (pursuitTarget)) {
			pursuitTarget = null;
            TargetLost();
		}
		if (HasFleeTarget() && IsBehindMe(fleeTarget)) {
            fleeTarget = null;
            TargetLost();
		}
	}

	private bool IsBehindMe(Transform other)
	{
        bool isBehindMe = false;
        if (this.Controller.facing == Facing.RIGHT) {
            isBehindMe = other.position.x < transform.position.x;
		} else {
            isBehindMe = other.position.x > transform.position.x;
        }
        return isBehindMe;
    }

    private void TargetAcquired()
    {
        NPCMessageBus.TriggerMessage(
            MessageBuilder.BuildMessage(MessageType.TargetAcquired));
    }

    private void TargetLost()
    {
        NPCMessageBus.TriggerMessage(
            MessageBuilder.BuildMessage(MessageType.TargetLost));
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
