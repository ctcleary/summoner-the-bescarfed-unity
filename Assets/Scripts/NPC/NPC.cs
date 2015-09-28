using UnityEngine;
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
 * It also has to handle things like changing animation states
 * which seem coupled pretty tightly by Unity itself.
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
		NPCMessageBus.AddMessageListener (EntityMessage.Died, this);
		NPCMessageBus.AddMessageListener (EntityMessage.Collided, this);

		Modules = new Dictionary<string, NPCModule> ();
		foreach (KeyValuePair<string, string> entry in NPCModuleTypes) {
			NPCModule module = (NPCModule)GetComponent(entry.Value);
			Modules.Add (entry.Key, module);
		};
		
		combatModule = (CombatModule)Modules["Combat"];
		movementModule = (MovementModule)Modules["Movement"];
		bounceModule = (BounceModule)Modules["Bounce"];
		handleOpponentModule = (HandleOpponentModule)Modules["HandleOpponent"];
		visionModule = (VisionModule)Modules["Vision"];

		animator = GetComponent<Animator>();

		Reset ();
	}

	public MessageBus MessageBus {
		get { return NPCMessageBus; }
		private set { return; }
	}
	
	// IMessageHandler
	public virtual void HandleMessage(Message message) {
		//Debug.Log ("Received message of type: " + message.MessageType);
		//Debug.Log ("messageGameObject: " + message.GameObjectValue);
		switch (message.MessageType) {
		case EntityMessage.Died:
			this.Kill();
			break;
		}
	}

	// Update is called once per frame
	protected virtual void Update ()
	{
		healthBarController.UpdateHealthBar(combatModule.GetPercentageOfMaxHealth(), GetFacing());

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
		// All modules should implement INPCModule
		foreach (NPCModule module in Modules.Values) {
			 module.Reset();
		}
//		combatModule.Reset ();
//		movementModule.Reset ();
//		bounceModule.Reset ();
//		handleOpponentModule.Reset ();
//		visionModule.Reset ();
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
		// Should be overridden in most cases.
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

	public virtual void HandleOnVisionEnter(Collider2D other)
	{
		// As long as we haven't seen an ally...
		if (!other.CompareTag(tag)) {
			handleOpponentModule.HandleSawOpponent(other);
		}
		// TODO
		// Eventually, things like handleObstacleModule?
		// - Or should Obstacles be also considered Opponents?
		// - Some Obstacles will be IDamageable, and some summons
		//   may eventually prioritize attacking obstacles.
	}

	protected virtual void HandleTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag(OpponentTag)) {
			Message CollideMessage = new Message ();
			CollideMessage.MessageType = EntityMessage.Collided;
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
