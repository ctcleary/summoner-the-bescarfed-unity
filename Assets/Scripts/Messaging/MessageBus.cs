using UnityEngine;
using System;
using System.Collections.Generic;

public class MessageBus
{
    // TODO allow multiple MessageBus types by allowing different
    //      messages enums to be configured.
	private MessageType[] MessageTypes;
	private Dictionary<MessageType, List<Action<Message>>> MessageMap;

	public MessageBus ()
	{
		MessageMap = new Dictionary<MessageType, List<Action<Message>>> ();
	}

	public void AddMessageListener (MessageType messageType, IMessageHandler handlerObject)
	{
		if (!MessageMap.ContainsKey (messageType)) {
			MessageMap[messageType] = new List<Action<Message>>();
		}

		List<Action<Message>> HandlerList = MessageMap [messageType];
		HandlerList.Add (handlerObject.HandleMessage);
	}

	public void TriggerMessage(Message message) {
		MessageType messageType = message.MessageType;
		if (MessageMap.ContainsKey(messageType)) {
			List<Action<Message>> Listeners = MessageMap [messageType];
			Listeners.ForEach (delegate(Action<Message> action) {
				action (message);
			});
		}
	}
}

