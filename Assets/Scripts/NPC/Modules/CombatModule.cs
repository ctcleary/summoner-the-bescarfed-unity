using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CombatProperties
{
	public float health;
	public float dmgResistance;
	public float attackDmg;
	public float attackSpd;
}

public class CombatModule : NPCModule, INPCModule, IDamageable
{
	
	public CombatProperties combatProperties;
	//private Animator animator;

	// Health and Defense props
	private float health;
	private float dmgResistance;

	// Attack props
	private float attackDmg;
	private float attackSpd;
	private float attackDelay;

	// Utilities
	private Timer.CallbackFunc attackTimerCallback;
	private Timer attackTimer;

	private bool isAlive;
	private IDamageable attackTarget;

	// Fancy shit
	public string attackAnim;
	protected bool isAttacking = false;
    

    protected override Dictionary<MessageType, Action<Message>> GetSupportedMessageMap()
    {
        return new Dictionary<MessageType, Action<Message>>()
        {
            { MessageType.Damaged, HandleDamaged },
            { MessageType.OpponentsChange, HandleOpponentsChange },
            { MessageType.Collided, HandleCollided }
        };
    }

    // Use this for initialization
    public override void Start ()
	{
		base.Start ();

		// TODO TEMP, figure out actual attack delay based on attackSpd.
		attackTimerCallback = Attack;
		attackTimer = new Timer (2f, attackTimerCallback);

		//animator = GetComponentInParent<Animator> ();	
		isAlive = true;
		Reset (); // Set private variables.
	}

    private void HandleDamaged(Message message)
    {
        this.Hurt(message.FloatValue);
    }

    private void HandleOpponentsChange(Message message)
    {
        OpponentTag = message.NPCKindValue.Tag;
    }

    private void HandleCollided(Message message)
    {
        GameObject other = message.GameObjectValue;
        IDamageable opponentCombatModule = other.GetComponentInParent<NPC>();
        if (other.CompareTag(OpponentTag))
        {
            // TODO: implement a non-entity, combathandler messageBus
            // so we're not directly referencing another gameobject.
            SetAttackTarget(opponentCombatModule);
            NPCMessageBus.TriggerMessage(
                MessageBuilder.BuildGameObjectMessage(MessageType.FightEngaged, other));
        }
    }

    public override void Reset()
	{
		isAlive = true;
		
		// Set to private variables so we can reset based on `combatProperties`
		health = combatProperties.health;
		//dmgResistance = combatProperties.dmgResistance;
		
		attackDmg = combatProperties.attackDmg;
		//attackSpd = combatProperties.attackSpd;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (attackTimer != null) {
		    if (attackTarget != null && attackTarget.IsAlive()) {
			    StartCoroutine (attackTimer.DoTimer ());
		    } else if (attackTarget != null && !attackTarget.IsAlive()) {
                ResolveFight();
				attackTimer.Reset ();
		    }
        }
    }

    private void ResolveFight()
    {
        attackTarget = null;
        NPCMessageBus.TriggerMessage(
            MessageBuilder.BuildMessage(MessageType.FightResolved));
    }

	public void Hurt (float dmgTaken)
	{
		health -= dmgTaken;
		if (isAlive && health <= 0) {
            NPCMessageBus.TriggerMessage(MessageBuilder.BuildMessage(MessageType.Died));

            isAlive = false;
		} else {
            SendHealthUpdateMessage();
        }

	}

    private void SendHealthUpdateMessage()
    {
        NPCMessageBus.TriggerMessage(MessageBuilder
            .BuildFloatMessage(MessageType.HealthUpdate, GetPercentageOfMaxHealth()));
    }

    private float GetPercentageOfMaxHealth()
	{
		if (combatProperties.health == 0) {
			return 0;
		}
		float percHealth = health / combatProperties.health;
		return percHealth;
	}

	public void SetAttackTarget (IDamageable attackTarget)
	{
		// Only set if CURRENT target is `null`, or if the assignment is TO `null`
		if (this.attackTarget == null || attackTarget == null) {
			this.attackTarget = attackTarget;
			if (attackTarget != null) {
				Attack ();
			}
		}
		if (attackTarget == null) {	
			StopAttackAnimation();
            ResolveFight();
        }
	}

	public IDamageable GetAttackTarget ()
	{
		return attackTarget;
	}

	public void Attack ()
	{
		if (attackTarget != null) {
			isAttacking = true;
            NPCMessageBus.TriggerMessage(CreateAttackingMessage(true));
            Invoke("DoDamage", 0.3f);

		} else {
			Debug.Log ("attack target was nullified!");
			SetAttackTarget (null);
		}
	}
    private Message CreateAttackingMessage(bool isAttacking)
    {
        Message attackMessage = MessageBuilder.BuildBoolMessage(MessageType.Attacking, isAttacking);
        return attackMessage;
    }

    private void StopAttackAnimation()
	{
		isAttacking = false;
        NPCMessageBus.TriggerMessage(CreateAttackingMessage(false));
    }

    private void DoDamage()
	{
		if (attackTarget != null && attackTarget.IsAlive()) {
            //Debug.Log("DoDamage, attackTarget null ? " + (attackTarget == null));
			attackTarget.Hurt (attackDmg);
			StopAttackAnimation();
		} else {
            SetAttackTarget(null);
            StopAttackAnimation();
        }
	}

	public bool IsAlive ()
	{
		return isAlive;
	}

	public float GetHealth()
	{
		return health;
	}
}
