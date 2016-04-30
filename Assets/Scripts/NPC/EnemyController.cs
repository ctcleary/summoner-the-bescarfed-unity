using UnityEngine;
using System.Collections;

public class EnemyController : NPC, IKillable {

	private Transform spawnPoint;
    private bool killedByPlayer = false;
	
	// Use this for initialization
	protected override void Start ()
    {
        base.Start();
        OpponentTag = NPCKind.SUMMONED.Tag;

        NPCMessageBus.TriggerMessage(
            MessageBuilder.BuildNPCKindValueMessage(
                MessageType.OpponentsChange, NPCKind.SUMMONED));

		AttachHealthBar(24f, 1f, 1.1f);
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		base.HandleTriggerEnter2D (other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		base.HandleTriggerStay2D (other);
	}


    public virtual bool KilledByPlayer
    {
        get { return killedByPlayer; }
        set { killedByPlayer = value; }
    }

    public override void Kill ()
	{
		// TODO don't add to score if enemy hits player.
		ScoreKeeper.addScore (1);
        //this.killedByPlayer = killedByPlayer;
        base.Kill();
	}

	void OnBecameInvisible ()
	{
        if (killedByPlayer)
        {
            Destroy(gameObject);
            return;
        }

		if (gameObject != null && combatModule.GetHealth() >= 1) {
			// Don't lose a villageLives life if the Enemy was
			// killed by a friendly Summoned!
			ScoreKeeper.loseLives (1);
        }
        Destroy(gameObject);
    }

    public override void Reset()
    {
        base.Reset();
        killedByPlayer = false;
    }
}
