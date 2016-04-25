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

    public virtual void Awake() { }

    public virtual void Start()
	{
		NPCGameObject = GetComponentInParent<NPC>();
        this.Controller = NPCGameObject;
        this.OpponentTag = this.Controller.OpponentTag;
        this.NPCMessageBus = this.Controller.MessageBus;

        SupportedMessageMap = GetSupportedMessageMap();
        Listen();
    }

    public NPC Controller
    {
        get { return NPCGameObject; }
        private set { this.NPCGameObject = value; }
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
        foreach (MessageType messageType in SupportedMessageMap.Keys)
        {
            NPCMessageBus.AddMessageListener(messageType, thisHandler);
        }
    }

    public void HandleMessage(Message message)
    {
        // Call this message type's handler function from the SupportedMessageMap
        SupportedMessageMap[message.MessageType](message);
    }
}

