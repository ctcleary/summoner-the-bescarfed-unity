using UnityEngine;
using System;

public abstract class NPCModule : MonoBehaviour, IMessageHandler
{
	private NPC NPCGameObject;
	private MessageBus messageBus;
    private string opponentTag;

	public virtual void Start()
	{
		NPCGameObject = GetComponentInParent<NPC>();
        this.OpponentTag = NPCGameObject.OpponentTag;
        this.NPCMessageBus = NPCGameObject.MessageBus;
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

    public abstract void Reset();
    public abstract void HandleMessage(Message message);
    protected abstract void Listen();
}

