using UnityEngine;
using System;
using System.Collections.Generic;

public class MessageBus
{
	private EntityMessage[] EntityMessages;
	private Dictionary<EntityMessage, List<Action<Message>>> MessageMap;

	public MessageBus ()
	{
		MessageMap = new Dictionary<EntityMessage, List<Action<Message>>> ();
	}

	public void AddMessageListener (EntityMessage messageType, IMessageHandler handlerObject)
	{
		if (!MessageMap.ContainsKey (messageType)) {
			MessageMap[messageType] = new List<Action<Message>>();
		}

		List<Action<Message>> HandlerList = MessageMap [messageType];
		HandlerList.Add (handlerObject.HandleMessage);
	}

	public void TriggerMessage(Message message) {
		EntityMessage messageType = message.MessageType;
		if (MessageMap.ContainsKey(messageType)) {
			List<Action<Message>> Listeners = MessageMap [messageType];
			Listeners.ForEach (delegate(Action<Message> action) {
				action (message);
			});
		}
	}
}

