using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	
	public GameObject enemy;
	public float spawnTime = 3f;
	//public Transform[] spawnPoints;
	public bool switchOn;
	public int MaxEnemy;
	public int numEnemy;

	public GameObject objSwitch;
	SwitchSystem switchSystem;
	// Use this for initialization
	void Start () {
		numEnemy = 0;
		MaxEnemy = 1;
		switchOn = false;
		objSwitch = GameObject.FindGameObjectWithTag ("LevelGenerator");
		switchSystem = objSwitch.GetComponent<SwitchSystem> ();
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update(){
		if (switchSystem.alarmActive) {
			switchOn = true;		
		}
		
		if (numEnemy < MaxEnemy && switchOn)
		{
			Spawn ();
			numEnemy++;
		}
	}
	void Spawn () {
		Instantiate (enemy, this.transform.position, this.transform.rotation);
	}
}


