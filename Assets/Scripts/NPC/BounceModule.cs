using UnityEngine;
using System.Collections;

public class BounceModule : MonoBehaviour, INPCModule {

	private bool goUp;

	public void BounceAgainst(Collider2D other)
	{
		Vector3 bounceDirection = transform.position - other.transform.position;
		Vector2 newVelocity = new Vector2 (bounceDirection.x, bounceDirection.y);
		float xRand = Random.Range(2, 6);
		newVelocity.x += (newVelocity.x >= 0) ? xRand : -xRand;

		float yRand = Random.Range(1, 3);
		ShouldItGoUp (bounceDirection);
		newVelocity.y += (goUp == true) ? -yRand : yRand;
		GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + newVelocity;
	}

	private void ShouldItGoUp(Vector3 bounceDirection)
	{
		if (bounceDirection.y != 0) {
			goUp = (bounceDirection.y < 0);

		} else {
			// Coinflip if it's at 0.
			goUp = (Random.Range (0, 2) > 0);
		}
	}
	
	// INPCModule
	public void Reset()
	{
		
	}

}
