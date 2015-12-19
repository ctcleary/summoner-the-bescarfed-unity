using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour {

    private float absoluteMinimum = 0.2f;
	public float minDelay = 0.5f;
	public float maxDelay = 2f;

    private Timer.CallbackFunc spawnSpeedIncreaseTimerCallback;
    private Timer spawnSpeedIncreaseTimer;
    private float spawnSpeedModifier = 1f;
    private float spawnSpeedIncreasePerTick = 0.05f;

	public GameObject enemyPrefab;

    private int enemiesSpawnedCt = 0;

	private Camera mainCam;

	// Use this for initialization
	void Start ()
	{
		mainCam = Camera.main;
		Invoke("CreateEnemy", Random.Range(minDelay, maxDelay));

        spawnSpeedIncreaseTimerCallback = IncreaseSpeed;
        BeginSpeedIncreaseTimer();
	}
	
	// Update is called once per frame
	void Update()
    {
        if (spawnSpeedIncreaseTimer != null)
        {
            StartCoroutine(spawnSpeedIncreaseTimer.DoTimer());
        }
        if (Input.GetButtonDown("Fire3")) {
			CreateEnemy(true);
		}
	}

    void BeginSpeedIncreaseTimer()
    {
        spawnSpeedIncreaseTimer = null;
        spawnSpeedIncreaseTimer = new Timer(5, spawnSpeedIncreaseTimerCallback);
    }
    void IncreaseSpeed()
    {
        spawnSpeedModifier -= spawnSpeedIncreasePerTick;
        BeginSpeedIncreaseTimer();
    }

    void PositionAtSpawn (GameObject enemy)
	{

		Vector3 oldPosition = enemy.transform.position;
		float yMin = -mainCam.orthographicSize + 1f;
		float yMax = mainCam.orthographicSize - 1f;

		float randY = Random.Range(yMin+1f, yMax-1f);
		float offscreenX = mainCam.orthographicSize * mainCam.aspect + 1;
        
		Vector3 newPosition = new Vector3 (offscreenX,
		                                   randY,
		                                   oldPosition.z);

		enemy.transform.position = newPosition;
	}

    /**
     * There's something about calling "Invoke" that requires this hacky setup.
     * TODO: Investigate, because this is ugly.
     */
    void CreateEnemy() { CreateEnemy(false); }
	void CreateEnemy(bool skipTimer = false)
	{
		Object newObj = Instantiate (enemyPrefab);
		GameObject newEnemy = newObj as GameObject;

        newObj.name = newObj.name + "_" + enemiesSpawnedCt;
        enemiesSpawnedCt++;

        newEnemy.transform.parent = transform;

		PositionAtSpawn(newEnemy);

		if (!skipTimer) {
            float min = DetermineDelay(minDelay);
            float max = DetermineDelay(maxDelay);
            Invoke("CreateEnemy",
                Random.Range(min, max));
		}
	}
    private float DetermineDelay(float srcDelay)
    {
        float delay = srcDelay * spawnSpeedModifier;
        if (delay < absoluteMinimum)
        {
            delay = absoluteMinimum;
        }
        return delay;
    }
}
