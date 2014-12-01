using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour {

	public float minDelay = 0.5f;
	public float maxDelay = 2f;

	public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
		Invoke("CreateEnemy", Random.Range(minDelay, maxDelay));	
	}

	void CreateEnemy() {
		Instantiate (enemyPrefab);
		Invoke("CreateEnemy", Random.Range(minDelay, maxDelay));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
