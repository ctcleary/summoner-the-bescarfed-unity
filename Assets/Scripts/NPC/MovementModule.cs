using UnityEngine;
using System.Collections;

public class MovementModule : MonoBehaviour, INPCModule {
	
	public MovementProperties movementProperties;

	private float moveSpeed;
	private Vector2 maxVelocity;
	private bool isImmovable = false;
	
	public Facing facing;

	// Use this for initialization
	void Start () {
		Reset ();
		UseFacing ();
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
			ClampVelocity ();
		}
	}

	public void Reset()
	{
		// Set to private variables so we can reset based on `movementProperties`
		moveSpeed = movementProperties.moveSpeed;
		maxVelocity = movementProperties.maxVelocity;
	}

	private void Move()
	{
		Vector2 newVelocity = rigidbody2D.velocity;
		newVelocity.x += moveSpeed * 0.3f;
		if (newVelocity.y != 0) {
			if (newVelocity.y > 0) {
				newVelocity.y -= rigidbody2D.drag * 0.02f;
				newVelocity.y = Mathf.Clamp(newVelocity.y, 0, 100);
			} else {
				newVelocity.y += rigidbody2D.drag * 0.02f;
				newVelocity.y = Mathf.Clamp(newVelocity.y, -100, 0);
			}
		}
		rigidbody2D.velocity = newVelocity;
	}

	private void ClampVelocity()
	{
		Vector2 newVelocity = rigidbody2D.velocity;
		newVelocity.x = Mathf.Clamp(newVelocity.x, -maxVelocity.x, maxVelocity.x);
		newVelocity.y = Mathf.Clamp(newVelocity.y, -maxVelocity.y, maxVelocity.y);
		rigidbody2D.velocity = newVelocity;
	}

	private void StopMovement()
	{
		rigidbody2D.velocity = new Vector2 (0, 0);
	}

	private bool IsStopped()
	{
		return rigidbody2D.velocity.x == 0 && rigidbody2D.velocity.y == 0;
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

	public bool IsImmovable {
		get { return isImmovable; }
		set { isImmovable = value; }
	}
}
