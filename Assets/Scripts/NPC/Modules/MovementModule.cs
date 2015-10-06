using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MovementModule : NPCModule, INPCModule {
	
	public MovementProperties movementProperties;
    public Facing facing;

    private float moveSpeed;
	private Vector2 maxVelocity;
	private Vector2 movementAdjustment;
	private bool isImmovable = false;
	
	private int facingFactor = 1;

    protected override Dictionary<MessageType, Action<Message>> GetSupportedMessageMap()
    {
        return new Dictionary<MessageType, Action<Message>>()
        {
            { MessageType.MovementAdjustment, HandleMovementAdjustment },
            { MessageType.TargetLost, HandleTargetLost },
            { MessageType.FightEngaged, HandleFightEngaged },
            { MessageType.FightResolved, HandleFightResolved },
        };
    }

    // Use this for initialization
    public void Awake()
    {
        InitFacing();
       
    }
	public override void Start ()
	{
		base.Start ();
		Reset ();
        NPCMessageBus.TriggerMessage(MessageBuilder.BuildFacedMessage(this.facing));
    }

    private void HandleMovementAdjustment(Message message)
    {
        SetMovementAdjustment(message.Vector2Value);
    }

    private void HandleTargetLost(Message message)
    {
        SetMovementAdjustment(new Vector2(0, 0));
    }

    private void HandleFightEngaged(Message message)
    {
        IsImmovable = true;
    }

    private void HandleFightResolved(Message message)
    {
        IsImmovable = false;
    }


    // Update is called once per frame
    void Update () {
		if (isImmovable) {
			StopMovement ();

		} else {
			if (IsStopped()) {
				Reset ();
			}

			Move();
			EnforceBounds ();
			ClampVelocity ();
		}
	}
	
	// Implement Abstract
	public override void Reset()
	{
        // Set to private variables so we can reset based on `movementProperties`
        SetMovementAdjustment(new Vector2(0, 0));
        moveSpeed = movementProperties.moveSpeed;
		maxVelocity = movementProperties.maxVelocity;
	}

	private void Move()
	{
		Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
		newVelocity.x += (moveSpeed * 0.3f) * facingFactor;
		if (newVelocity.y != 0) {
			if (newVelocity.y > 0) {
				newVelocity.y -= GetComponent<Rigidbody2D>().drag * 0.02f;
				newVelocity.y = Mathf.Clamp(newVelocity.y, 0, 100);
			} else {
				newVelocity.y += GetComponent<Rigidbody2D>().drag * 0.02f;
				newVelocity.y = Mathf.Clamp(newVelocity.y, -100, 0);
			}
		}
        
        Vector2 adjustedVelocity = AdjustMovement(newVelocity);

		GetComponent<Rigidbody2D>().velocity = adjustedVelocity;
	}

	public void SetMovementAdjustment(Vector2 direction) {
		this.movementAdjustment = direction;
	}

	public Vector2 AdjustMovement(Vector2 currDeterminedVelocity)
	{
//		Vector2 adjusted = Vector2.MoveTowards(rigidbody2D.velocity, adjustment, 60f);
//		this.movementAdjustment = movementAdjustment;
		Vector2 adjusted = currDeterminedVelocity + movementAdjustment;
		return adjusted;
	}

	private void EnforceBounds ()
	{
		Camera mainCam = Camera.main;
		
		Vector3 camPosition = mainCam.transform.position;
		Vector3 spriteSize = GetComponent<Renderer>().bounds.size;
		
		float yDist = mainCam.orthographicSize;
		float yMax = camPosition.y + yDist - spriteSize.y / 2;
		float yMin = camPosition.y - yDist + spriteSize.y / 2;

		Vector3 newPosition = transform.position;
		if (newPosition.y > yMax || newPosition.y < yMin) {
			newPosition.y = Mathf.Clamp (newPosition.y, yMin, yMax);
		}
		
		transform.position = newPosition;
	}

	private void ClampVelocity()
	{
		Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
		if (facing == Facing.RIGHT) {
			newVelocity.x = Mathf.Clamp (newVelocity.x, 0, maxVelocity.x);
		} else {
			newVelocity.x = Mathf.Clamp (newVelocity.x, -maxVelocity.x, 0);
		}
		newVelocity.y = Mathf.Clamp (newVelocity.y, -maxVelocity.y, maxVelocity.y);

		GetComponent<Rigidbody2D>().velocity = newVelocity;
	}

	private void StopMovement()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2 (0, 0);
	}

	private bool IsStopped()
	{
		return GetComponent<Rigidbody2D>().velocity.x == 0 && GetComponent<Rigidbody2D>().velocity.y == 0;
	}

    private void InitFacing()
    {
        UseFacing();
        if (GetFacing() == Facing.LEFT)
        {
            facingFactor = -1;
        }
    }
    private void UseFacing()
	{
		if (facing == Facing.LEFT)
		{
			Vector3 newScale = transform.localScale;
			newScale.x = -1;
			transform.localScale = newScale;
		}
	}

	public Facing GetFacing()
	{
		return facing;
	}

	public bool IsImmovable {
		get { return isImmovable; }
		set { isImmovable = value; }
	}
}
