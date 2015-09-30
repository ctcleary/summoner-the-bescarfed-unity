using UnityEngine;
using System;

public abstract class NPCModule : MonoBehaviour, IMessageHandler
{
	private NPC NPCGameObject;
	private MessageBus messageBus;

	public virtual void Start()
	{
		NPCGameObject = GetComponent<NPC>();
		this.MessageBus = NPCGameObject.MessageBus;
	}

	public MessageBus MessageBus {
		get { return messageBus; }
		private set { this.messageBus = value; }
	}

	public abstract void Reset();
    public abstract void HandleMessage(Message message);
    protected abstract void Listen();
}

