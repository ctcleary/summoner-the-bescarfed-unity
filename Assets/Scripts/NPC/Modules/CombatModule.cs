using UnityEngine;
using System;
using System.Collections;

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
	private Animator animator;

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

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();

		// TODO TEMP, figure out actual attack delay based on attackSpd.
		attackTimerCallback = Attack;
		attackTimer = new Timer (2f, attackTimerCallback);

		animator = GetComponentInParent<Animator> ();	
		isAlive = true;
		Reset (); // Set private variables.
	}
    
    // Implement NPCModule abstracts
    protected override void Listen()
    {

    }

    public override void HandleMessage(Message message)
    {

    }

    public override void Reset()
	{
		isAlive = true;
		
		// Set to private variables so we can reset based on `combatProperties`
		health = combatProperties.health;
		//dmgResistance = combatProperties.dmgResistance;
		
		attackDmg = combatProperties.attackDmg;
		//attackSpd = combatProperties.attackSpd;
		
		StopAttackAnimation();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (attackTarget != null) {
			StartCoroutine (attackTimer.DoTimer ());
		} else {
			if (attackTimer != null) {
				attackTimer.Reset ();
			}
		}
	}

	public void Hurt (float dmgTaken)
	{
		health -= dmgTaken;
		if (health <= 0) {
			Message DeathMessage = new Message();
			DeathMessage.MessageType = EntityMessage.Died;
			MessageBus.TriggerMessage(DeathMessage);

			isAlive = false;
		}

        SendHealthUpdateMessage();
	}

    private void SendHealthUpdateMessage()
    {
        Message healthUpdateMessage = new Message();
        healthUpdateMessage.MessageType = EntityMessage.HealthUpdate;
        healthUpdateMessage.FloatValue = GetPercentageOfMaxHealth();
        MessageBus.TriggerMessage(healthUpdateMessage);
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
		}
	}

	public IDamageable GetAttackTarget ()
	{
		return attackTarget;
	}

	public void Attack ()
	{
		if (attackTarget != null) {
//			if (attackAnim != null) {
//				animator.Play (attackAnim);
//			}

			isAttacking = true;
			animator.SetBool("isAttacking", isAttacking);
			Invoke ("DoDamage", 0.3f);

		} else {
			Debug.Log ("attack target was nullified!");
			SetAttackTarget (null);
		}
	}

	private void StopAttackAnimation()
	{
		isAttacking = false;
		if (animator != null) {
			animator.SetBool("isAttacking", isAttacking);
		}
	}

	private void DoDamage()
	{
		if (attackTarget != null) {
			attackTarget.Hurt (attackDmg);
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
