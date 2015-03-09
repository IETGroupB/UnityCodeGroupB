using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public float speed;
	public bool switchOn;
	public float drainAmount;
	Light enemyLight;

	public GameObject objSwitch;
	public GameObject target;
	SwitchSystem switchSystem;
	private RoomGrid roomGrid;
	private Point targetRoom, enemyRoom;
	private AudioSource enemySound;

	public float attackRate;
	public float nextAttack;



	void Start(){
		speed = 100;
		drainAmount = 0.15f;
		enemyLight = transform.GetComponent<Light>();
		switchOn = true;
		objSwitch = GameObject.FindGameObjectWithTag ("LevelGenerator");
		switchSystem = objSwitch.GetComponent<SwitchSystem> ();
		enemySound = transform.GetComponent<AudioSource> ();

		roomGrid = GameObject.Find("LevelGeneration").GetComponent<LevelGenerator>().roomGrid;
		target = GameObject.Find ("Character");
		enemySound.mute = true;

		attackRate = 3.0f;
		nextAttack = 0.0f;
	}

	void Update(){
		switchOn = switchSystem.alarmActive;
		int enemyRoomIndex = 0; 
		int targetRoomIndex = 0;
		int indexDiff = 0;


		Vector3 movement = Vector3.Normalize(target.transform.position - transform.position);
		if (switchOn) {
			GetComponent<Rigidbody2D>().isKinematic = true;
			enemySound.mute = false;
			targetRoom = roomGrid.GetClosestRoom (target.transform.position);
			enemyRoom = roomGrid.GetClosestRoom (transform.position);

			for(int i = 0;i<roomGrid.solutionPath.Length;i++){
				if(targetRoom ==  roomGrid.solutionPath[i]){
					targetRoomIndex = i;
				}
				if(enemyRoom == roomGrid.solutionPath[i]){
					enemyRoomIndex = i;
				}
			}

			indexDiff = targetRoomIndex-enemyRoomIndex;

			if(indexDiff<=1&&indexDiff>=-1){
				movement = target.transform.position-transform.position;
				GetComponent<Rigidbody2D>().velocity = (0.5f*movement * speed* Time.deltaTime);
			}

			else if(indexDiff>0){
				movement = new Vector3(roomGrid.solutionPath[enemyRoomIndex+1].x*16+8,-(roomGrid.solutionPath[enemyRoomIndex+1].y*16+8),0)-transform.position;
				GetComponent<Rigidbody2D>().velocity = (Vector3.Normalize(movement) * speed*indexDiff*indexDiff * Time.deltaTime);
			}
			else{
				movement = new Vector3(roomGrid.solutionPath[enemyRoomIndex-1].x*16+8,-(roomGrid.solutionPath[enemyRoomIndex-1].y*16+8),0)-transform.position;
				GetComponent<Rigidbody2D>().velocity = (Vector3.Normalize(movement) * speed*indexDiff*indexDiff * Time.deltaTime);
			}

			enemyLight.enabled = true;

		} else {
			GetComponent<Rigidbody2D>().isKinematic = false;
			enemyLight.enabled = false;
			enemySound.mute = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.name == "Character"&&switchOn) {
			//if(Time.time>nextAttack){
			//	nextAttack=Time.time+attackRate;
				float distance = Vector3.Distance(transform.position,other.transform.position);
				other.GetComponent<PlayerController>().DrainEnergy(drainAmount/(distance*distance));		
			//}
		}
	}
}
