﻿using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public float speed;
	public bool switchOn;
	public float drainAmount;
	public int boltSegmentCount;
	Light enemyLight;
	public float shockDuration;
	public GameObject objSwitch;
	public GameObject target;

	SwitchSystem switchSystem;
	private RoomGrid roomGrid;
	private Point targetRoom, enemyRoom;
	private AudioSource enemySound;
	private Sprite bolt;
	private GameObject[] segments;
	private bool isShocking = false;
	private float shockTime;

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

		//bolt setup
		{
			bolt = Resources.Load<Sprite>("Tiles/Trap/bolt");
			segments = new GameObject[boltSegmentCount];
			for (var i = 0; i < boltSegmentCount; i++)
			{
				segments[i] = new GameObject();
				segments[i].AddComponent<SpriteRenderer>();
				segments[i].GetComponent<SpriteRenderer>().sprite = bolt;
				segments[i].GetComponent<SpriteRenderer>().sortingOrder = 200;
			}
		}
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

			//mid-point pathfinding//////////////////////////////////////////////
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

			//calculate harmming rate by distance//////////////////////////////
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

		if (isShocking) {
			DrawBolt(new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y));
			
			shockTime += Time.deltaTime;
			if (shockTime > shockDuration)
			{
				shockTime = 0.0f;
				isShocking = false;
				HideBolt();
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.name == "Character" && switchOn) {
			//if(Time.time>nextAttack){
			//	nextAttack=Time.time+attackRate;
			float distance = Vector3.Distance (transform.position, other.transform.position);
			other.GetComponent<PlayerController> ().DrainEnergy (drainAmount / (distance * distance));
			//}
		} 

		if (switchOn && !isShocking)
		{
			//GameObject other = coll.transform.gameObject;
			if (other.name == "Character")
			{
				//other.GetComponent<PlayerController>().KillPlayer();
				
				isShocking = true;
				//shockTarget = other.GetComponent<PlayerController>().body;
				ShowBolt();
				//zap.Play();
			}
			else if (other.tag == "PlayerGibs")
			{
				if (Random.value <= 0.1f)
				{
					//other.GetComponent<Rigidbody2D>().AddForce((new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y)).normalized * shockForce);
					isShocking = true;
					//shockTarget = other;
					ShowBolt();
					//zap.Play();
				}
			}
		}
	}

	private void HideBolt()
	{
		foreach (GameObject segment in segments)
			segment.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	private void ShowBolt()
	{
		foreach (GameObject segment in segments)
			segment.GetComponent<SpriteRenderer>().enabled = true;
	}

	private void DrawBolt(Vector2 end)
	{
		var step = end / boltSegmentCount;
		var currentPosition = new Vector2(0.0f, 0.0f);
		
		for (var i = 0; i < boltSegmentCount - 1; i++)
		{
			var nextPosition = step * i + new Vector2(2 * step.y * Random.value, 2 * step.x * Random.value);
			DrawBoltSegment(currentPosition, nextPosition, i);
			currentPosition = nextPosition;
		}
		
		DrawBoltSegment(currentPosition, end, boltSegmentCount - 1);
	}
	
	private void DrawBoltSegment(Vector2 start, Vector2 end, int index)
	{
		var difference = end - start;
		var angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		
		segments[index].transform.parent = transform;
		segments[index].transform.localPosition = new Vector3(start.x, start.y, -0.5f);
		segments[index].transform.localScale = new Vector3(difference.magnitude, 1.0f, 1.0f);
		segments[index].transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
	}
}
