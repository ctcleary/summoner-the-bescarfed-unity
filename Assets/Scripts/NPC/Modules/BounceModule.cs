using UnityEngine;
using System.Collections;

public class BounceModule : NPCModule, INPCModule {

	private bool goUp;
    private Rigidbody2D rigidbodyRef;

    private float minX = 2;
    private float maxX = 6;
    private float minY = 1;
    private float maxY = 3;

    public override void Start()
	{
		base.Start ();
        rigidbodyRef = GetComponent<Rigidbody2D>();
	}

    // Implement NPCModule abstracts
    protected override void Listen()
    {

    }

    public override void HandleMessage(Message message)
    {

    }

    public void BounceAgainst(Collider2D other)
	{
		Vector3 bounceDirection = transform.position - other.transform.position;
		Vector2 newVelocity = new Vector2 (bounceDirection.x, bounceDirection.y);
		float xRand = Random.Range(minX, maxX);
		newVelocity.x += (newVelocity.x >= 0) ? xRand : -xRand;

		float yRand = Random.Range(minY, maxY);
		ShouldItGoUp (bounceDirection);
		newVelocity.y += (goUp == true) ? -yRand : yRand;
        rigidbodyRef.velocity = rigidbodyRef.velocity + newVelocity;
	}

	private void ShouldItGoUp(Vector3 bounceDirection)
	{
		if (bounceDirection.y != 0) {
			goUp = (bounceDirection.y < 0);

		} else {
            // Coinflip if it's at 0.
            float rnd = Random.Range(0.0f, 1.0f);
            goUp = (rnd > 0.5f);
        }
	}
	
	// Implement Abstract
	public override void Reset()
	{
		
	}

}
