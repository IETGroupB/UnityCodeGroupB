using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public float speed;
    public float chargeSpeed;
    public float chargeTime;
    public float dischargeTime;
    public float attackDistance;
    public float drainAmount;
    public float shuntForce;
	public int boltSegmentCount;
	
	public float shockDuration;
	public GameObject objSwitch;
	public GameObject target;
    public PlayerController targetScript;
    public Color LightOn;
    public Color LightCharge;
    public Color LightOff;

	SwitchSystem switchSystem;
	private RoomGrid roomGrid;
	private Point targetRoom, enemyRoom;
	private AudioSource audioSource;
    private AudioClip chargeSound;
    private AudioClip attackSound;
	private Sprite bolt;
	private GameObject[] segments;
    private GameObject enemyLight;
    public bool switchOn;




    enum EnemyState { chasing, charging, attack, discharging, ceaseFire, onFire };
	EnemyState currentState;

	float lastStateChange;

	void Start(){
        chargeSound = Resources.Load<AudioClip>("EnemyFiles/charge");
        attackSound = Resources.Load<AudioClip>("EnemyFiles/attack");

        enemyLight = transform.FindChild("EnemyLight").gameObject;
        enemyLight.GetComponent<Light>().color = LightOn;
        enemyLight.GetComponent<TrailRenderer>().material.SetColor("_TintColor", LightOn);
        enemyLight.GetComponent<SpriteRenderer>().color = LightOn;

		switchOn = true;
		objSwitch = GameObject.FindGameObjectWithTag ("LevelGenerator");
		switchSystem = objSwitch.GetComponent<SwitchSystem> ();
		audioSource = transform.GetComponent<AudioSource> ();

		roomGrid = GameObject.Find("LevelGeneration").GetComponent<LevelGenerator>().roomGrid;
        target = GameObject.Find("Character").transform.FindChild("Body").gameObject;
        targetScript = GameObject.Find("Character").GetComponent<PlayerController>();
		audioSource.mute = true;

        currentState = EnemyState.chasing;
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

		if (switchOn) {
            var targetEyePos = (new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y)).normalized * 0.3f;
            targetEyePos = Vector2.Lerp(new Vector2(enemyLight.transform.localPosition.x, enemyLight.transform.localPosition.y), targetEyePos, Time.deltaTime * 5.0f);
            enemyLight.transform.localPosition = new Vector3(targetEyePos.x, targetEyePos.y, enemyLight.transform.position.z);

            switch (currentState)
            {
                case EnemyState.chasing:
                    var colourOn = Color.Lerp(enemyLight.GetComponent<Light>().color, LightOn, Time.deltaTime);
                    enemyLight.GetComponent<Light>().color = colourOn;
                    enemyLight.GetComponent<TrailRenderer>().material.SetColor("_Color", colourOn);
                    enemyLight.GetComponent<SpriteRenderer>().color = colourOn;

                    // set layer to NoCollision to ignore level geometry
                    gameObject.layer = 9;
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

                    audioSource.mute = false;

                    //mid-point pathfinding//////////////////////////////////////////////
                    targetRoom = roomGrid.GetClosestRoom(target.transform.position);
                    enemyRoom = roomGrid.GetClosestRoom(transform.position);

                    for (var i = 0; i < roomGrid.solutionPath.Length; i++)
                    {
                        if (targetRoom == roomGrid.solutionPath[i])
                        {
                            targetRoomIndex = i;
                        }
                        if (enemyRoom == roomGrid.solutionPath[i])
                        {
                            enemyRoomIndex = i;
                        }
                    }

                    indexDiff = targetRoomIndex - enemyRoomIndex;

                    if (indexDiff <= 1 && indexDiff >= -1)
                    {
                        var movement = Vector3.Normalize(target.transform.position - transform.position);
                        // use forces for smoother movement when near player
                        GetComponent<Rigidbody2D>().AddForce(movement * speed * Time.deltaTime);
                    }
                    else if (indexDiff > 0)
                    {
                        var movement = new Vector3(roomGrid.solutionPath[enemyRoomIndex + 1].x * 16 + 8, -(roomGrid.solutionPath[enemyRoomIndex + 1].y * 16 + 8), 0) - transform.position;
                        GetComponent<Rigidbody2D>().velocity = (Vector3.Normalize(movement) * speed * indexDiff * indexDiff * Time.deltaTime);
                    }
                    else
                    {
                        var movement = new Vector3(roomGrid.solutionPath[enemyRoomIndex - 1].x * 16 + 8, -(roomGrid.solutionPath[enemyRoomIndex - 1].y * 16 + 8), 0) - transform.position;
                        GetComponent<Rigidbody2D>().velocity = (Vector3.Normalize(movement) * speed * indexDiff * indexDiff * Time.deltaTime);
                    }

                    // can't use 2d colliders with physics layers, divide attack distance by two to prevent flicking between states
                    if (
                        Mathf.Pow(
                        Mathf.Pow(transform.position.x - target.transform.position.x, 2) +
                        Mathf.Pow(transform.position.y - target.transform.position.y, 2)
                        , 0.5f)
                        < attackDistance * 0.5f)
                    {
                        SetCurrentState(EnemyState.charging);
                        audioSource.clip = chargeSound;
                        audioSource.Play();
                    }
                    break;
                case EnemyState.charging:
                    var colourCharge = Color.Lerp(enemyLight.GetComponent<Light>().color, LightCharge, Time.deltaTime / chargeTime);
                    enemyLight.GetComponent<Light>().color = colourCharge;
                    enemyLight.GetComponent<TrailRenderer>().material.SetColor("_Color", colourCharge);
                    enemyLight.GetComponent<SpriteRenderer>().color = colourCharge;

                    var chargeMovement = Vector3.Normalize(target.transform.position - transform.position);
                    var distance = Mathf.Pow(
                        Mathf.Pow(transform.position.x - target.transform.position.x, 2) +
                        Mathf.Pow(transform.position.y - target.transform.position.y, 2)
                        , 0.5f);
                    // use forces for smoother movement when near player

                    // don't get on top of the player
                    if(distance < attackDistance * 0.4f)
                        GetComponent<Rigidbody2D>().AddForce(chargeMovement * chargeSpeed * Time.deltaTime);

                    //play charge sound effect


                    // player has escaped attack
                    if (distance > attackDistance)
                    {
                        SetCurrentState(EnemyState.chasing);
                        audioSource.Stop();
                    }

                    if (GetStateElapsed() > chargeTime)
                        SetCurrentState(EnemyState.attack);

                    break;
                case EnemyState.attack:
                    audioSource.Stop();
                    audioSource.clip = attackSound;
                    audioSource.Play();

                    ShowBolt();
                    DrawBolt(new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y));
                    targetScript.DrainEnergy(drainAmount);

                    var attackShunt = -Vector3.Normalize(target.transform.position - transform.position);
                    GetComponent<Rigidbody2D>().AddForce(attackShunt * shuntForce);

                    GetComponent<AudioSource>().Play();

                    SetCurrentState(EnemyState.discharging);

                    break;
                case EnemyState.discharging:
                    var colourDischarge = Color.Lerp(enemyLight.GetComponent<Light>().color, LightOn, Time.deltaTime);
                    enemyLight.GetComponent<Light>().color = colourDischarge;
                    enemyLight.GetComponent<TrailRenderer>().material.SetColor("_Color", colourDischarge);
                    enemyLight.GetComponent<SpriteRenderer>().color = colourDischarge;


                    if (GetStateElapsed() < dischargeTime * 0.5f)
                        DrawBolt(new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y));
                    else
                        HideBolt();

                    if (GetStateElapsed() > dischargeTime)
                        SetCurrentState(EnemyState.chasing);

                    break;
            }
		} else {
            var colourOff = Color.Lerp(enemyLight.GetComponent<Light>().color, LightOff, Time.deltaTime);
            enemyLight.GetComponent<Light>().color = colourOff;
            enemyLight.GetComponent<TrailRenderer>().material.SetColor("_Color", colourOff);
            enemyLight.GetComponent<SpriteRenderer>().color = colourOff;

            // set layer to default for collision
            gameObject.layer = 0;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;

			enemyLight.GetComponent<Light>().color = LightOff;
			audioSource.mute = true;
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

