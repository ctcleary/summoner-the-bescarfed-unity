using UnityEngine;

public struct Message
{
	public MessageType MessageType;
    public bool BoolValue;
	public int IntValue;
	public float FloatValue;
	public Vector2 Vector2Value;
	public GameObject GameObjectValue;

    // TODO Make these a separate kind?
    public Facing FacingValue;
    public NPCKind NPCKindValue;
}

