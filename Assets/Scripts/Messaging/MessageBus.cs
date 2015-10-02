using UnityEngine;
using System;
using System.Collections.Generic;

public class MessageBus
{
    // TODO allow multiple MessageBus types by allowing different
    //      messages enums to be configured.
	private MessageType[] MessageTypes;
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
        //if (MessageMap[messageType].Count > 1)
        //{
        //    Debug.Log(messageType + " Has more than 1 listener object!");
        //}
    }

	public void TriggerMessage(Message message) {
		MessageType messageType = message.MessageType;
		if (MessageMap.ContainsKey(messageType)) {
			List<IMessageHandler> Listeners = MessageMap [messageType];
            //if (Listeners.Count > 1)
            //{
            //    Debug.Log(messageType + " has " + Listeners.Count + " Listeners");
            //}
			Listeners.ForEach (delegate(IMessageHandler handlerObject) {
                //Debug.Log("Triggering : " + messageType + " on " + handlerObject.GetType());
                handlerObject.HandleMessage(message);
            });
		}
	}
}

