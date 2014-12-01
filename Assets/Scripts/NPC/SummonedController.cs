using UnityEngine;
using System.Collections;

public class SummonedController : NPC, IKillable {

	// Use this for initialization
	protected override void Start ()
	{
		OpponentTag = NPCKind.ENEMY.Tag;
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
		combatModule.attackAnim = "Summoned_ClawDemon_Attack";
	}

	// INPC
	//public override void Reset() {}
	//public void SetAttackTarget(IDamageable attackTarget) {}
	//public void Attack() {}

	// IDamageable

	
	void OnTriggerEnter2D(Collider2D other)
	{
		base.HandleTriggerEnter2D (other);
	}

	// IKillable
	public override void Kill() {
		// TODO Recycle
		Destroy (gameObject);
	}

	void OnBecameInvisible() {
		Kill ();
	}
}
