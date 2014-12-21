using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour {

	public float minDelay = 0.5f;
	public float maxDelay = 2f;

	public GameObject enemyPrefab;

	// Use this for initialization
	void Start ()
	{
		Invoke("CreateEnemy", Random.Range(minDelay, maxDelay));	
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Fire3")) {
			CreateEnemy(true);
		}
	}

	void CreateEnemy()
	{
		Object newObj = Instantiate (enemyPrefab);
		GameObject newEnemy = newObj as GameObject;
		newEnemy.transform.parent = transform;

		Invoke("CreateEnemy", Random.Range(minDelay, maxDelay));
	}
	void CreateEnemy(bool skipTimer)
	{
		Object newObj = Instantiate (enemyPrefab);
		GameObject newEnemy = newObj as GameObject;
		newEnemy.transform.parent = transform;
	}
}
