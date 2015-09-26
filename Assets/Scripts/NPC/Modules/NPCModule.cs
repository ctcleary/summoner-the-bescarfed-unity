using UnityEngine;
using System;

public abstract class NPCModule : MonoBehaviour
{
	private NPC NPCGameObject;
	private MessageBus messageBus;

	public virtual void Start()
	{
		NPCGameObject = GetComponent<NPC>();
		this.MessageBus = NPCGameObject.MessageBus;
	}

	public MessageBus MessageBus {
		private get { return messageBus; }
		set { this.messageBus = value; }
	}

	public abstract void Reset();
}

