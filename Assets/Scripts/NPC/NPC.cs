using UnityEngine;
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

public class NPC : Entity, INPC, IDamageable, IKillable, IMessageHandler
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


    protected void Listen()
    {
        NPCMessageBus.AddMessageListener(MessageType.Died, this);
        NPCMessageBus.AddMessageListener(MessageType.Attacking, this);
        NPCMessageBus.AddMessageListener(MessageType.Collided, this);
        NPCMessageBus.AddMessageListener(MessageType.HealthUpdate, this);
        NPCMessageBus.AddMessageListener(MessageType.Faced, this);

        NPCMessageBus.AddMessageListener(MessageType.FightEngaged, this);
        NPCMessageBus.AddMessageListener(MessageType.FightResolved, this);
    }

    // IMessageHandler
    public virtual void HandleMessage(Message message) {
        //Debug.Log ("Received message of type: " + message.MessageType);
        //Debug.Log ("messageGameObject: " + message.GameObjectValue);

        switch (message.MessageType)
        {
            case MessageType.Died:
                this.Kill();
                break;
            case MessageType.Collided:
                this.HandleCollidedMessage(message);
                break;
            case MessageType.HealthUpdate:
                this.HandleHealthUpdate(message);
                break;
            case MessageType.Attacking:
                this.HandleAttackingMessage(message);
                break;
            case MessageType.Faced:
                this.HandleFacedMessage(message);
                break;
            default:
                Debug.Log("A message type doesn't have a handler :: " + message.MessageType);
                break;
        }
	}
    private void HandleCollidedMessage(Message collidedMessage)
    {
        // TODO 
    }
    private void HandleHealthUpdate(Message healthUpdateMessage)
    {
        healthBarController.UpdateHealthBar(healthUpdateMessage.FloatValue);
    }
    private void HandleAttackingMessage(Message attackingMessage)
    {
        if (animator.isInitialized)
        {
            animator.SetBool("isAttacking", attackingMessage.BoolValue);
        }
    }
    private void HandleFacedMessage(Message facedMessage)
    {
        healthBarController.SetFacing(facedMessage.FacingValue);
    }
    private void HandleFightEngagedMessage(Message fightEngagedMessage)
    {
        animator.SetBool("isFighting", true);
    }
    private void HandleFightResolvedMessage(Message fightEngagedMessage)
    {
        animator.SetBool("isFighting", false);
    }

    // Update is called once per frame
    protected virtual void Update ()
	{
		if (isFighting && combatModule.GetAttackTarget ().Equals(null)) {
			StopFighting ();
		}
		if (handleOpponentModule.HasAnyTargets()) {
			movementModule.SetMovementAdjustment(handleOpponentModule.GetMovementDirection());
		} else {
			movementModule.SetMovementAdjustment(new Vector2(0,0));
		}
        animator.SetBool("isFighting", isFighting);
    }

	// INPC
	public virtual void Reset ()
	{
		// All modules should inherit NPCModule abstract base class.
		foreach (NPCModule module in Modules.Values) {
			 module.Reset();
		}
	}
	
	public virtual void SetAttackTarget (IDamageable attackTarget)
	{
		combatModule.SetAttackTarget (attackTarget);
		movementModule.IsImmovable = true;
		isFighting = true;
	}

	public virtual void StopFighting ()
	{
		combatModule.SetAttackTarget (null);
		movementModule.IsImmovable = false;
		isFighting = false;

//		AquireNewAttackTarget ();
	}
	
	public virtual void Attack ()
	{
		combatModule.Attack ();
	}

	// IDamageable
	public virtual void Hurt(float dmgTaken)
	{
		combatModule.Hurt (dmgTaken);
	}

	public virtual bool IsAlive()
	{
		return combatModule.IsAlive ();
	}

	// IKillable
	public virtual void Kill()
	{
		// Should be overridden in most cases. e.g. object pooling
		Destroy (gameObject);
	}

	public virtual string OpponentTag
	{
		get { return opponentTag; }
		protected set { opponentTag = value; }
	}	

	public Facing GetFacing()
	{
		return movementModule.GetFacing ();
	}

	protected virtual void HandleTriggerEnter2D (Collider2D other)
	{
        if (other.tag == null) {
            Debug.Log("An other with no tag? " + other.gameObject.ToString());
            return;
        }
		if (other.CompareTag(OpponentTag)) {
			Message CollideMessage = new Message ();
			CollideMessage.MessageType = MessageType.Collided;
			CollideMessage.GameObjectValue = other.gameObject;
			NPCMessageBus.TriggerMessage (CollideMessage);
		}
//		if (other.CompareTag (OpponentTag)) {
//			IDamageable opponentCombatModule = other.GetComponentInParent<NPC> ();
//			SetAttackTarget (opponentCombatModule);
//		}
//		} else if (other.CompareTag (transform.tag)) {
//
//			if (!movementModule.IsImmovable) {
//				if (!other.Equals(null)) {
//					bounceModule.BounceAgainst(other);
//				}
//			}
//		}
	}
	
	protected virtual void HandleTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag (OpponentTag)) {
			IDamageable opponentCombatModule = other.GetComponentInParent<NPC> ();
			SetAttackTarget (opponentCombatModule);
		}
		if (other.CompareTag (transform.tag)) {
			BounceAgainstAlly(other);
		}
	}

	public virtual void BounceAgainstAlly(Collider2D other)
	{
		if (!movementModule.IsImmovable) {
			if (!other.Equals (null)) {
				bounceModule.BounceAgainst (other);
			}
		}
	}
}
