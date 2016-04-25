using UnityEngine;
using System.Collections;

public class SummonedController : NPC, IKillable {

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        OpponentTag = NPCKind.ENEMY.Tag;

        NPCMessageBus.TriggerMessage(
            MessageBuilder.BuildNPCKindValueMessage(
                MessageType.OpponentsChange, NPCKind.ENEMY));

        AttachHealthBar(24f, 1f, 0.5f);
        combatModule.attackAnim = "Summoned_ClawDemon_Attack";

    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update ();
	}

	// INPC
	//public override void Reset() {}
	//public void SetAttackTarget(IDamageable attackTarget) {}
	//public void Attack() {}

	
	void OnTriggerEnter2D(Collider2D other)
	{
		base.HandleTriggerEnter2D (other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		base.HandleTriggerStay2D (other);
	}

	// IKillable
	public override void Kill() {
        // TODO Recycle
        base.Kill();
	}

	void OnBecameInvisible() {
		Kill ();
	}
}
