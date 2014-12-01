﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CombatModule))]
[RequireComponent (typeof(MovementModule))]
[RequireComponent (typeof(Animator))]
public class NPC : MonoBehaviour, INPC, IDamageable, IKillable
{
	protected CombatModule combatModule;
	protected MovementModule movementModule;
	protected Animator animator;
	protected string opponentTag;

	protected bool isFighting = false;

	// Use this for initialization
	protected virtual void Start ()
	{
		combatModule = GetComponent<CombatModule> ();
		movementModule = GetComponent<MovementModule> ();
		animator = GetComponent<Animator> ();
		Reset ();
	}

	// Update is called once per frame
	protected virtual void Update ()
	{
		if (!combatModule.IsAlive()) {
			Kill ();
		}
		if (isFighting && combatModule.GetAttackTarget ().Equals(null)) {
			StopFighting ();
		}
		animator.SetBool("isFighting", isFighting);
	}
	
	// INPC
	public virtual void Reset ()
	{
		combatModule.Reset ();
		movementModule.Reset ();
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
	public virtual void Kill() {
		// Should be overridden in most cases.
		Destroy (gameObject);
	}

	public virtual string OpponentTag {
		get { return opponentTag; }
		protected set { opponentTag = value; }
	}

	protected virtual void HandleTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag (OpponentTag)) {
			IDamageable opponentCombatModule = other.GetComponentInParent<NPC> ();
			SetAttackTarget (opponentCombatModule);
		}
	}
}
