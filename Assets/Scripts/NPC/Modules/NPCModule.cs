using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class NPCModule : MonoBehaviour, IMessageHandler
{
	private NPC NPCGameObject;
	private MessageBus messageBus;
    private string opponentTag;

    private Dictionary<MessageType, Action<Message>> SupportedMessageMap;

    public abstract void Reset();

    public virtual void Start()
	{
		NPCGameObject = GetComponentInParent<NPC>();
        this.OpponentTag = NPCGameObject.OpponentTag;
        this.NPCMessageBus = NPCGameObject.MessageBus;

        SupportedMessageMap = GetSupportedMessageMap();
        Listen();
	}

    public MessageBus NPCMessageBus
    {
        get { return messageBus; }
        private set { this.messageBus = value; }
    }

    public string OpponentTag
    {
        get { return opponentTag; }
        protected set { this.opponentTag = value; }
    }

    protected virtual Dictionary<MessageType, Action<Message>> GetSupportedMessageMap()
    {
        return new Dictionary<MessageType, Action<Message>>();
    }

    protected void Listen()
    {
        IMessageHandler thisHandler = (IMessageHandler)this;
        foreach (MessageType mesageType in SupportedMessageMap.Keys)
        {
            NPCMessageBus.AddMessageListener(mesageType, thisHandler);
        }
    }

    public void HandleMessage(Message message)
    {
        // Call this message type's handler function from the SupportedMessageMap
        SupportedMessageMap[message.MessageType](message);
    }
}

