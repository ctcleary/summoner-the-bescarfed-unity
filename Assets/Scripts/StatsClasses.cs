using UnityEngine;
using System.Collections;

[System.Serializable]
public class MovementProperties
{
	public float moveSpeed;
	public Vector2 maxVelocity;
}

[System.Serializable]
public enum Facing
{
	LEFT, 
	RIGHT 
}

[System.Serializable]
public enum HandleOpponentBehavior
{
	FIGHT,
	FLEE
}
