using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public float speed;
	public bool switchOn;
	//Transform player;
	//NavMeshAgent nav;

	public GameObject objSwitch;
	SwitchSystem switchSystem;


	void Start(){
		speed = 100;
		switchOn = true;
		objSwitch = GameObject.FindGameObjectWithTag ("LevelGenerator");
		switchSystem = objSwitch.GetComponent<SwitchSystem> ();

	}

	void Update(){
		switchOn = switchSystem.alarmActive;
		Vector3 movement = GameObject.FindGameObjectWithTag ("PlayerGibs").transform.position - transform.position;
		if (switchOn) {
			rigidbody2D.velocity= (movement * speed * Time.deltaTime);
		}
	}
	//	void turnSwitch(){
	//		if (Time.timeSinceLevelLoad > 3.0f)
	//						switchOn = true;
	//				else
	//						switchOn = false;
	//	}
	

	
	// Update is called once per frame
	/*void Update () {
		nav.SetDestination (player.position);
	}*/
}
