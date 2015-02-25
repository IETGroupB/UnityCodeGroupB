using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	
	public GameObject enemy;
	public float spawnTime = 3f;
	public Transform[] spawPoints;
	public bool switchOn;
	public int MaxEnemy;
	public int numEnemy;
	// Use this for initialization
	void Start () {
		numEnemy = 0;
		MaxEnemy = 1;
		switchOn = false;
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update(){
		
		if (numEnemy < MaxEnemy && switchOn)
		{
			Spawn ();
			numEnemy++;
		}
	}
	void Spawn () {
		//if (switchOn) {
		int spawnPointIndex = Random.Range (0, spawPoints.Length);
		Instantiate (enemy, spawPoints [spawnPointIndex].position, spawPoints [spawnPointIndex].rotation);
		//}
	}
}
