using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour {

	public float minDelay = 0.5f;
	public float maxDelay = 2f;

	public GameObject enemyPrefab;

	private Camera mainCam;

	// Use this for initialization
	void Start ()
	{
		mainCam = Camera.main;
		Invoke("CreateEnemy", Random.Range(minDelay, maxDelay));	
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Fire3")) {
			CreateEnemy(true);
		}
	}

	void PositionAtSpawn (GameObject enemy)
	{

		Vector3 oldPosition = enemy.transform.position;
		float yMin = -mainCam.orthographicSize + 1f;
		float yMax = mainCam.orthographicSize - 1f;

		float randY = Random.Range(yMin+1f, yMax-1f);
		float offscreenX = mainCam.orthographicSize * mainCam.aspect + 1;

//		Vector3 newPosition = new Vector3 (spawner.position.x, // spawner.position.x does not work in web player
		Vector3 newPosition = new Vector3 (offscreenX,
		                                   randY,
		                                   oldPosition.z);

		enemy.transform.position = newPosition;
	}

	void CreateEnemy()
	{
		CreateEnemy(false);
	}
	void CreateEnemy(bool skipTimer)
	{
		Object newObj = Instantiate (enemyPrefab);
		GameObject newEnemy = newObj as GameObject;
		newEnemy.transform.parent = transform;

		PositionAtSpawn(newEnemy);

		if (!skipTimer) {
			Invoke("CreateEnemy", Random.Range(minDelay, maxDelay));
		}
	}
}
