using UnityEngine;
static class MessageBuilder
{
    public static Message BuildMessage(MessageType messageType)
    {
        Message message = new Message();
        message.MessageType = messageType;
        return message;
    }

    public static Message BuildBoolMessage(MessageType messageType, bool boolValue)
    {
        Message message = BuildMessage(messageType);
        message.BoolValue = boolValue;
        return message;
    }

    public static Message BuildIntMessage(MessageType messageType, int intValue)
    {
        Message message = BuildMessage(messageType);
        message.IntValue = intValue;
        return message;
    }

    public static Message BuildFloatMessage(MessageType messageType, float floatValue)
    {
        Message message = BuildMessage(messageType);
        message.FloatValue = floatValue;
        return message;
    }

    public static Message BuildGameObjectMessage(MessageType messageType, GameObject gameObject)
    {
        Message message = BuildMessage(messageType);
        message.GameObjectValue = gameObject;
        return message;
    }

    public static Message BuildFacedMessage(Facing facingValue)
    {
        Message message = BuildMessage(MessageType.Faced);
        message.FacingValue = facingValue;
        return message;
    }

    public static Message BuildNPCKindValueMessage(MessageType messageType, NPCKind npcKindValue)
    {
        Message message = BuildMessage(messageType);
        message.NPCKindValue = npcKindValue;
        return message;
    }
    
}

