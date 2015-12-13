using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(CombatModule))]
[RequireComponent (typeof(MovementModule))]
[RequireComponent (typeof(BounceModule))]
[RequireComponent (typeof(HandleOpponentModule))]
[RequireComponent (typeof(VisionModule))]
[RequireComponent (typeof(Animator))]

/**
 * The NPC class acts largely as a "Mediator" object between
 * the various Modules that make up an NPC Entity.
 * 
 * TODO / IN PROGRESS:
 * Rework NPC to use the MessageBus object for all inter-component communication.
 */

public class NPC : Entity, INPC, IDamageable, IKillable, IMessageHandler, IHealthBarAttachment
{
	protected MessageBus NPCMessageBus = new MessageBus ();

	protected Dictionary<string, NPCModule> Modules;
	protected Dictionary<string, string> NPCModuleTypes = new Dictionary<string, string> ()
	{
		{ "Combat", "CombatModule" },
		{ "Movement", "MovementModule" },
		{ "Bounce", "BounceModule" },
		{ "HandleOpponent", "HandleOpponentModule" },
		{ "Vision", "VisionModule" }
	};

	protected CombatModule combatModule;
	protected MovementModule movementModule;
	protected BounceModule bounceModule;
	protected HandleOpponentModule handleOpponentModule;
	protected VisionModule visionModule;
	protected Animator animator;

	protected string opponentTag;
	protected bool isFighting = false;

    public Facing facing;
    public HandleOpponentBehavior behavior;

    private Dictionary<MessageType, Action<Message>> SupportedMessageMap;

    public void Awake()
    {
        SupportedMessageMap = new Dictionary<MessageType, Action<Message>>()
        {
            { MessageType.Died, HandleDied },
            { MessageType.Attacking, HandleAttacking },
            { MessageType.Collided, HandleCollided },
            { MessageType.HealthUpdate, HandleHealthUpdate },
            { MessageType.Faced, HandleFaced },
            { MessageType.FightEngaged, HandleFightEngaged },
            { MessageType.FightResolved, HandleFightResolved },
        };
    }

  //  protected virtual void Awak()
  //  {
  //      GetModules();
		//animator = GetComponent<Animator>();

  //  }
	// Use this for initialization
	protected virtual void Start ()
	{
        Listen();

        GetModules();
        animator = GetComponent<Animator>();

        Reset ();
	}

    protected void GetModules()
    {
        Modules = new Dictionary<string, NPCModule>();
        foreach (KeyValuePair<string, string> entry in NPCModuleTypes)
        {
            NPCModule module = (NPCModule)GetComponent(entry.Value);
            Modules.Add(entry.Key, module);
        };

        combatModule = (CombatModule)Modules["Combat"];
        movementModule = (MovementModule)Modules["Movement"];
        bounceModule = (BounceModule)Modules["Bounce"];
        handleOpponentModule = (HandleOpponentModule)Modules["HandleOpponent"];
        visionModule = (VisionModule)Modules["Vision"];
    }

    public MessageBus MessageBus {
		get { return NPCMessageBus; }
		private set { return; }
	}

    public Facing GetFacing()
    {
        return this.facing;
    }

    public HandleOpponentBehavior GetBehavior()
    {
        return this.behavior;
    }

    // ------------------------------------------
    // Message Handling
    // ------------------------------------------
    private void Listen()
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


    private void HandleDied(Message message)
    {
        Kill();
    }
    private void HandleCollided(Message collidedMessage)
    {
        // TODO 
    }
    private void HandleHealthUpdate(Message healthUpdateMessage)
    {
        healthBarController.UpdateHealthBar(healthUpdateMessage.FloatValue);
    }
    private void HandleAttacking(Message attackingMessage)
    {
        if (animator.isInitialized)
        {
            animator.SetBool("isAttacking", attackingMessage.BoolValue);
        }
    }
    private void HandleFaced(Message facedMessage)
    {
        healthBarController.SetFacing(facedMessage.FacingValue);
    }
    private void HandleFightEngaged(Message fightEngagedMessage)
    {
        isFighting = true;
        animator.SetBool("isFighting", isFighting);
    }
    private void HandleFightResolved(Message fightEngagedMessage)
    {
        isFighting = false;
        animator.SetBool("isFighting", isFighting);
    }

    // -------------------------------
    // END Message Handling
    // -------------------------------


    // Update is called once per frame
    protected virtual void Update ()
	{
    }

	// INPC
	public virtual void Reset ()
	{
		foreach (NPCModule module in Modules.Values) {
			 module.Reset();
		}
	}

    //public virtual void Attack ()
    //{
    //       combatModule.Attack();
    //}

    // IDamageable
    public virtual void Hurt(float dmgTaken)
    {
        NPCMessageBus.TriggerMessage(
            MessageBuilder.BuildFloatMessage(MessageType.Damaged, dmgTaken));
    }

    public virtual bool IsAlive()
	{
		return combatModule.IsAlive ();
	}

	// IKillable
	public virtual void Kill()
	{
        // Should be overridden in most cases. e.g. object pooling
        if (this != null)
        {
            Destroy(gameObject);
        }
	}

	public virtual string OpponentTag
	{
		get { return opponentTag; }
		protected set { opponentTag = value; }
	}

	protected virtual void HandleTriggerEnter2D (Collider2D other)
    {
        SendCollideMessage(other);
    }
	
	protected virtual void HandleTriggerStay2D(Collider2D other)
	{
        SendCollideMessage(other);
	}
    
    private void SendCollideMessage(Collider2D other)
    {
        NPCMessageBus.TriggerMessage(
            MessageBuilder.BuildGameObjectMessage(MessageType.Collided, other.gameObject));
    }
}
