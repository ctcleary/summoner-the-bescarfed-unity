using UnityEngine;
using System.Collections;

public class EnemyController : NPC, IKillable
{

	private Transform spawnPoint;
	
	// Use this for initialization
	protected override void Start ()
	{
		OpponentTag = NPCKind.SUMMONED.Tag;
		AttachHealthBar(24f, 1f, 1.1f);
		base.Start ();
		combatModule.attackAnim = "Enemy_ZombieOrcGenie_Attack";

		spawnPoint = GameObject.Find ("EnemySpawner").transform;
		PositionAtSpawn ();

	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

	void PositionAtSpawn ()
	{
		Vector3 oldPosition = transform.position;
		Camera mainCam = Camera.main;
		float yMin = -mainCam.orthographicSize + 1f;
		float yMax = mainCam.orthographicSize - 1f;
		
		Vector3 newPosition = new Vector3 (spawnPoint.transform.position.x,
		                                  //TEMP
		                                  //spawnPoint.transform.position.y,
		                                  //Random.Range (yMin, yMax),
		                                   Random.Range(yMin+1f, yMax-1f),
		                                  oldPosition.z);
		
		transform.position = newPosition;
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
		ScoreKeeper.addScore (1);
		Destroy (gameObject);
	}

	void OnBecameInvisible ()
	{
		ScoreKeeper.loseLives (1);
		Destroy (gameObject);

	}
}
