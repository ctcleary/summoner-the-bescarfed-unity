﻿using UnityEngine;
using System.Collections;

public class MovementModule : NPCModule, INPCModule {
	
	public MovementProperties movementProperties;

	private float moveSpeed;
	private Vector2 maxVelocity;
	private Vector2 movementAdjustment;
	private bool isImmovable = false;
	
	public Facing facing;
	private int facingFactor = 1;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		Reset ();
		UseFacing ();
		if (GetFacing () == Facing.LEFT) {
			facingFactor = -1;
		}
	}

    // Implement NPCModule abstracts
    protected override void Listen()
    {

    }

    public override void HandleMessage(Message message)
    {

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

		newVelocity = AdjustMovement(newVelocity);

		GetComponent<Rigidbody2D>().velocity = newVelocity;
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
