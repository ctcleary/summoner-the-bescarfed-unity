using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MessageBus
{
    // TODO allow multiple MessageBus types by allowing different
    //      messages enums to be configured.

	private Dictionary<MessageType, List<IMessageHandler>> MessageMap;

	public MessageBus ()
	{
		MessageMap = new Dictionary<MessageType, List<IMessageHandler>> ();
	}

	public void AddMessageListener (MessageType messageType, IMessageHandler handlerObject)
	{
		if (!MessageMap.ContainsKey (messageType)) {
			MessageMap[messageType] = new List<IMessageHandler>();
		}
		MessageMap [messageType].Add (handlerObject);
    }

	public void TriggerMessage(Message message) {
		MessageType messageType = message.MessageType;
		if (MessageMap != null && MessageMap.ContainsKey(messageType)) {
			List<IMessageHandler> Listeners = MessageMap [messageType];
			Listeners.ForEach (delegate(IMessageHandler handlerObject) {
                handlerObject.HandleMessage(message);
            });
		}
	}

    public void Reset()
    {
        MessageMap = null;
    }
}

