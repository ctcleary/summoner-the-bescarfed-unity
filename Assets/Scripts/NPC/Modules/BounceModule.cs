using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BounceModule : NPCModule, INPCModule {

	private bool goUp;
    private Rigidbody2D rigidbodyRef;

    private float minX = 2;
    private float maxX = 6;
    private float minY = 1;
    private float maxY = 3;


    private float bounceFactor = 1.5f;

    protected override Dictionary<MessageType, Action<Message>> GetSupportedMessageMap()
    {
        return new Dictionary<MessageType, Action<Message>>()
        {
            { MessageType.Collided, HandleCollided }
        };
    }

    public override void Start()
	{
		base.Start ();
        rigidbodyRef = GetComponent<Rigidbody2D>();
	}

    private void HandleCollided(Message message)
    {
        GameObject other = message.GameObjectValue;
        if (other.CompareTag(transform.tag))
        {
            BounceAgainst(other);
        }
    }

    public void BounceAgainst(GameObject other)
	{
		Vector3 bounceDirection = transform.position - other.transform.position;
		Vector2 newVelocity = new Vector2 (bounceDirection.x, bounceDirection.y);
		float xRand = UnityEngine.Random.Range(minX, maxX);
		newVelocity.x += (newVelocity.x >= 0) ? xRand : -xRand;

		float yRand = UnityEngine.Random.Range(minY, maxY);
		ShouldItGoUp (bounceDirection);
		newVelocity.y += (goUp == true) ? -yRand : yRand;
        
        // TODO, buggy?
        rigidbodyRef.velocity = rigidbodyRef.velocity + (newVelocity * bounceFactor);
    }

	private void ShouldItGoUp(Vector3 bounceDirection)
	{
		if (bounceDirection.y != 0) {
			goUp = (bounceDirection.y < 0);

		} else {
            // Coinflip if it's at 0.
            float rnd = UnityEngine.Random.Range(0.0f, 1.0f);
            goUp = (rnd > 0.5f);
        }
	}
	
	// Implement Abstract
	public override void Reset()
	{
		
	}

}
