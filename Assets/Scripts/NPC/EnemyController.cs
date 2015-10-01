using UnityEngine;
using System.Collections;

public class EnemyController : NPC, IKillable
{

	private Transform spawnPoint;
	
	// Use this for initialization
	protected override void Start ()
	{
		OpponentTag = NPCKind.SUMMONED.Tag;
        NPCMessageBus.TriggerMessage(
            MessageBuilder.BuildNPCKindValueMessage(MessageType.OpponentsChange, NPCKind.SUMMONED));

		AttachHealthBar(24f, 1f, 1.1f);
		base.Start ();
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

	public override void Kill ()
	{
		// TODO don't add to score if enemy hits player.
		ScoreKeeper.addScore (1);
		Destroy (gameObject);
	}

	void OnBecameInvisible ()
	{
		if (combatModule.GetHealth() >= 1) {
			// Don't lose a villageLives life if the Enemy was
			// killed by a friendly Summoned!
			ScoreKeeper.loseLives (1);
		}
		Destroy (gameObject);

	}
}
