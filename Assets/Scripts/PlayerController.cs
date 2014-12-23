using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamageable, IKillable
{

	public CombatProperties combatProperties;
	public MovementProperties movementProperties;

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

	private Animator anim;
	private bool isMoving;

	public GameObject healthBarPrefab;
	private GameObject healthBarInstance;
	private HealthBarController healthBarController;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
		health = combatProperties.health;
		//attackDmg = combatProperties.attackDmg;

		moveSpeed = movementProperties.moveSpeed;
		maxVelocity = movementProperties.maxVelocity;

		isOnSummonDelay = false;
		timeSpentOnSummonDelay = 0;

		GameObject summGroupGameObj = GameObject.Find ("SummonedGroup");
		summonedGroup = summGroupGameObj.transform;
		summonedSpawnPoint = transform.FindChild ("SummonedSpawner").transform;
		
		Debug.Log("transform.position 1 :: " + transform.position); 
		Object healthBarObject = Instantiate (healthBarPrefab, transform.position, Quaternion.identity);
		GameObject healthBarInstance = healthBarObject as GameObject;
		
		Debug.Log("healthBarInstance 1 :: " + healthBarInstance.transform.position); 
		healthBarInstance.transform.parent = transform;
//		healthBarInstance.transform.localPosition = new Vector3(0, 1.2f, 0);
		
		Debug.Log("healthBarInstance 2 :: " + healthBarInstance.transform.position); 
		healthBarController = healthBarInstance.GetComponent<HealthBarController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		HandleDelays ();

		HandleMovement ();

		HandleInput ();

		EnforceBounds ();

		isMoving = IsMoving ();
		anim.SetBool ("isMoving", isMoving);
	}

	private void HandleMovement ()
	{
		// HANDLE MOVEMENT
		Vector2 newVelocity = rigidbody2D.velocity;
		
		if (Input.GetAxis ("Horizontal") != 0) {
			newVelocity.x += moveSpeed * Input.GetAxis ("Horizontal");
			newVelocity.x = Mathf.Clamp (newVelocity.x, -maxVelocity.x, maxVelocity.x);
		}
		
		if (Input.GetAxis ("Vertical") != 0) {
			newVelocity.y += moveSpeed * Input.GetAxis ("Vertical");
			newVelocity.y = Mathf.Clamp (newVelocity.y, -maxVelocity.y, maxVelocity.y);
		}
		rigidbody2D.velocity = newVelocity;

		// HANDLE FLIPPING
		Vector3 newScale = transform.localScale;
		if (rigidbody2D.velocity.x < 0 && newScale.x != -1) {
			newScale.x = -1;
		} else if (rigidbody2D.velocity.x > 0 && newScale.x != 1) {
			newScale.x = 1;
		}

		transform.localScale = newScale;
	}

	private bool IsMoving()
	{
		Vector2 currVelocity = rigidbody2D.velocity;
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
		Vector3 spriteSize = renderer.bounds.size;

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
			other.transform.GetComponent<EnemyController> ().Kill ();
			Hurt (1);
		}
	}

	// IDamageable
	public void Hurt (float dmgTaken = 1)
	{
		health = health - dmgTaken;
		if (health <= 0) {
			Kill ();
		}
	}

	public bool IsAlive ()
	{
		return health <= 0;
	}

	// IKillable
	public void Kill ()
	{
		Debug.Log ("You're dead, bro.");
	}

	private void FireSummon ()
	{
		if (!isOnSummonDelay) {
			isOnSummonDelay = true;
			Object newObj = Instantiate (summonedPrefab, summonedSpawnPoint.position, Quaternion.identity);
			GameObject newSummoned = newObj as GameObject;

			newSummoned.transform.parent = summonedGroup;
		}
	}
}
