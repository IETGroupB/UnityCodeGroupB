using UnityEngine;
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
	private bool attack = false;
	private float shockTime;

	public float attackRate;
	public float nextAttack;



	enum EnemyState{chasing, charging, attacking, ceaseFire, onFire};
	EnemyState currentState;

	float lastStateChange;

	void Start(){
		drainAmount = 0.15f;
        transform.GetChild(0);
        enemyLight = transform.FindChild("EnemyLight").GetComponent<Light>();
		switchOn = true;
		objSwitch = GameObject.FindGameObjectWithTag ("LevelGenerator");
		switchSystem = objSwitch.GetComponent<SwitchSystem> ();
		enemySound = transform.GetComponent<AudioSource> ();

		roomGrid = GameObject.Find("LevelGeneration").GetComponent<LevelGenerator>().roomGrid;
		target = GameObject.Find ("Character");
		enemySound.mute = true;

		attackRate = 3.0f;
		nextAttack = 0.0f;

		currentState = EnemyState.onFire;
		lastStateChange = Time.time;

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

	void SetCurrentState(EnemyState state){
		currentState = state;
		lastStateChange = Time.time;
	}

	float GetStateElapsed(){
		return Time.time - lastStateChange;
	}

	void Update(){
		switchOn = switchSystem.alarmActive;
		var enemyRoomIndex = 0;
        var targetRoomIndex = 0;
        var indexDiff = 0;


		Vector3 movement = Vector3.Normalize(target.transform.position - transform.position);
		if (switchOn) {
            // set layer to NoCollision to ignore level geometry
            gameObject.layer = 9;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

			enemySound.mute = false;

			//mid-point pathfinding//////////////////////////////////////////////
			targetRoom = roomGrid.GetClosestRoom (target.transform.position);
			enemyRoom = roomGrid.GetClosestRoom (transform.position);

            for (var i = 0; i < roomGrid.solutionPath.Length; i++)
            {
				if(targetRoom == roomGrid.solutionPath[i]){
					targetRoomIndex = i;
				}
				if(enemyRoom == roomGrid.solutionPath[i]){
					enemyRoomIndex = i;
				}
			}

			indexDiff = targetRoomIndex - enemyRoomIndex;

			//calculate harmming rate by distance//////////////////////////////
			if(attack){
				if(indexDiff<= 1 && indexDiff>= -1){
					movement = target.transform.position-transform.position;
					GetComponent<Rigidbody2D>().AddForce(movement * speed* Time.deltaTime);
				}

				else if(indexDiff>0){
					movement = new Vector3(roomGrid.solutionPath[enemyRoomIndex+1].x*16+8,-(roomGrid.solutionPath[enemyRoomIndex+1].y*16+8),0)-transform.position;
					GetComponent<Rigidbody2D>().velocity = (Vector3.Normalize(movement) * speed*indexDiff*indexDiff * Time.deltaTime);
				}
				else{
					movement = new Vector3(roomGrid.solutionPath[enemyRoomIndex-1].x*16+8,-(roomGrid.solutionPath[enemyRoomIndex-1].y*16+8),0)-transform.position;
					GetComponent<Rigidbody2D>().velocity = (Vector3.Normalize(movement) * speed*indexDiff*indexDiff * Time.deltaTime);
				}
			}

			enemyLight.enabled = true;


            switch (currentState)
            {
                case EnemyState.ceaseFire:
                    attack = false;
                    movement = target.transform.position - transform.position;
                    GetComponent<Rigidbody2D>().velocity = (Vector3.Normalize(movement) * (-50.0f) * Time.deltaTime);
                    if (GetStateElapsed() > 1.0f)
                        SetCurrentState(EnemyState.onFire);
                    break;
                case EnemyState.onFire:
                    attack = true;
                    speed = 100;
                    if (GetStateElapsed() > 1.5f)
                        SetCurrentState(EnemyState.ceaseFire);
                    break;
            }



		} else {
            // set layer to default for collision
            gameObject.layer = 0;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;

			enemyLight.enabled = false;
			enemySound.mute = true;
		}

		if (isShocking) {
			DrawBolt (new Vector2 (target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y));
		
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
		if (other.gameObject.name == "Character" && switchOn && attack) {
            var distance = Vector3.Distance(transform.position, other.transform.position);
			other.GetComponent<PlayerController> ().DrainEnergy (drainAmount / (distance * distance));
		}
	
		if (switchOn && !isShocking && attack)
		{
			//GameObject other = coll.transform.gameObject;
			if (other.name == "Character")
			{
				//other.GetComponent<PlayerController>().KillPlayer();	
				isShocking = true;
				ShowBolt();
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
		segments[index].transform.parent.eulerAngles = new Vector3 (0.0f, 0.0f, 0.0f);
		segments[index].transform.localPosition = new Vector3(start.x, start.y, -0.5f);
		segments[index].transform.localScale = new Vector3(difference.magnitude, 1.0f, 1.0f);
		segments[index].transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
	}
}
