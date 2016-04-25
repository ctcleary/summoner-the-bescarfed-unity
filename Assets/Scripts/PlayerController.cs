using UnityEngine;
using System.Collections;

public class PlayerController : Entity, IDamageable, IKillable, IHealthBarAttachment
{
    private static MessageBus GameMessageBus = GlobalMessageBus.Instance;

    public CombatProperties combatProperties;
	public MovementProperties movementProperties;

    private bool isDead = false;
	private float health;
	private float attackDmg;

	private float moveSpeed;
	private Vector2 maxVelocity;

	public float summonDelay;
	private float timeSpentOnSummonDelay;
	private bool isOnSummonDelay;

	public GameObject summonedPrefab;
	private Transform summonedSpawnPoint;
	private Transform summonedGroup;

    private AudioSource audioSource;
	private Animator anim;
	private bool isMoving;
    private Facing facing = Facing.RIGHT;

    public AudioClip summonSound;

	private float movementGrowRate = 0.2f;
	private float movementDecayRate = 0.8f;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
        audioSource = GetComponent<AudioSource>();
        health = combatProperties.health;
		//attackDmg = combatProperties.attackDmg;

		moveSpeed = movementProperties.moveSpeed;
		maxVelocity = movementProperties.maxVelocity;

		isOnSummonDelay = false;
		timeSpentOnSummonDelay = 0;

		GameObject summGroupGameObj = GameObject.Find ("SummonedGroup");
		summonedGroup = summGroupGameObj.transform;
		summonedSpawnPoint = transform.FindChild ("SummonedSpawner").transform;

		AttachHealthBar(24, 3, 1.2f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		HandleDelays ();

		HandleMovement ();

		HandleInput ();

		EnforceBounds ();

		float percHealth = health/combatProperties.health;
		healthBarController.UpdateHealthBar(percHealth); // TODO implement facing checks

		isMoving = IsMoving ();
		anim.SetBool ("isMoving", isMoving);
	}

	private void HandleMovement ()
    {
        Facing prevFacing = facing;

        // HANDLE MOVEMENT
        Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
		Vector2 velocityIncrease = new Vector2 (0, 0);

        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");
        if (inputHorizontal != 0) {
			velocityIncrease.x += moveSpeed * inputHorizontal;
            if (inputHorizontal > 0) {
                facing = Facing.RIGHT;
                healthBarController.SetFacing(facing);
            } else {
                facing = Facing.LEFT;
                healthBarController.SetFacing(facing);
            }

        } else if (newVelocity.x != 0) {
			newVelocity.x -= newVelocity.x * movementDecayRate;
		}
		
		if (inputVertical != 0) {
			velocityIncrease.y += moveSpeed * inputVertical;
		} else if (newVelocity.y != 0) {
			newVelocity.y -= newVelocity.y * movementDecayRate;
		}
		newVelocity += velocityIncrease * movementGrowRate;
		newVelocity.x = Mathf.Clamp (newVelocity.x, -maxVelocity.x, maxVelocity.x);
		newVelocity.y = Mathf.Clamp (newVelocity.y, -maxVelocity.y, maxVelocity.y);
		GetComponent<Rigidbody2D>().velocity = newVelocity;

        // HANDLE FLIPPING
        if (prevFacing != facing)
        {
            Vector3 newScale = transform.localScale;
            switch (facing)
            {
                case Facing.LEFT:
                    newScale.x = -1;
                    break;
                case Facing.RIGHT:
                    newScale.x = 1;
                    break;
            }
            transform.localScale = newScale;
        }
	}

    //private void DetermineFacing()
    //{
    //    if (GetComponent<Rigidbody2D>().velocity.x < 0)
    //    {
    //        this.facing = Facing.LEFT;
    //    }
    //    else if (GetComponent<Rigidbody2D>().velocity.x > 0)
    //    {
    //        this.facing = Facing.RIGHT;
    //    }
    //    // else do nothing
    //}

    public Facing GetFacing()
    {
        return this.facing;
    }

    private bool IsMoving()
	{
		Vector2 currVelocity = GetComponent<Rigidbody2D>().velocity;
		return currVelocity.x > 0.25 || currVelocity.x < -0.25 ||
			   currVelocity.y > 0.25 || currVelocity.y < -0.25;
	}

	private void HandleInput ()
	{
		if (Input.GetButtonDown ("Fire1")) {
			//Debug.Log("fire pressed");
			FireSummon ();
		}
	}

	private void HandleDelays ()
	{	
		if (isOnSummonDelay) {
			timeSpentOnSummonDelay += Time.deltaTime;

			if (timeSpentOnSummonDelay > summonDelay) {
				isOnSummonDelay = false;
				timeSpentOnSummonDelay = 0;
			}
		}
	}
	
	private void EnforceBounds ()
	{
		Camera mainCam = Camera.main;
		float aspect = mainCam.aspect;
		float orthoSize = mainCam.orthographicSize;
		
		Vector3 camPosition = mainCam.transform.position;
		Vector3 spriteSize = GetComponent<Renderer>().bounds.size;

		float xDist = orthoSize * aspect;
		float xMax = camPosition.x + xDist - spriteSize.x / 2;
		float xMin = camPosition.x - xDist + spriteSize.x / 2;

		float yDist = orthoSize;
		float yMax = camPosition.y + yDist - spriteSize.y / 2;
		float yMin = camPosition.y - yDist + spriteSize.y / 2;

		
		Vector3 newPosition = transform.position;
		if (newPosition.x > xMax || newPosition.x < xMin) {
			newPosition.x = Mathf.Clamp (newPosition.x, xMin, xMax);
		}
		if (newPosition.y > yMax || newPosition.y < yMin) {
			newPosition.y = Mathf.Clamp (newPosition.y, yMin, yMax);
		}

		transform.position = newPosition;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag (NPCKind.ENEMY.Tag)) {
            // TODO Need to do this in some better way, inter-entity message handler? Conductor?
            EnemyController enemyController = other.transform.GetComponent<EnemyController>();
            enemyController.KilledByPlayer = true;
            enemyController.Kill ();
			Hurt (1);
		}
	}

	// IDamageable
	public void Hurt (float dmgTaken = 1)
	{
		health = health - dmgTaken;
		if (health <= 0 && !isDead) {
			Kill ();
		}
	}

    public bool IsAlive()
    {
        return health > 0;
    }

    // IKillable
    public void Kill ()
	{
        if (!isDead) // temp so we only send the log once.
        {
            isDead = true;
            GameMessageBus.TriggerMessage(
                MessageBuilder.BuildMessage(MessageType.Lost));
        }
	}

	private void FireSummon ()
	{
		if (!isOnSummonDelay) {
			isOnSummonDelay = true;
			Object newObj = Instantiate (summonedPrefab, summonedSpawnPoint.position, Quaternion.identity);
			GameObject newSummoned = newObj as GameObject;

			newSummoned.transform.parent = summonedGroup;

            PlaySummonSound();
		}
	}
    
    private void PlaySummonSound()
    {
        if (summonSound != null)
        {
            audioSource.PlayOneShot(summonSound);
        }
    }
}
