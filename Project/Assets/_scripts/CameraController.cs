using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    GameObject playerCharacter;
    public float trackSpeed;
    public float yOffset;
    public float tiltScale;
    public float xyTiltRatio;
	public bool CanMute;
	public Texture2D sound;
	public Texture2D mute;
	public Texture2D MuteTexture;

    private Vector2 lastLocation;
    private Vector2 tilt;
	private AudioSource[] cameraSound;
	private AudioSource backgroundSound;
	private AudioSource endLevelSound;
	private AudioSource gameOverSound;


	//private AudioSource alarmSound;
	// Use this for initialization
	void Awake () {
        playerCharacter = GameObject.Find("Character");
		//switchButton = GameObject.Find ("Switch");
        lastLocation = new Vector2(0.0f, 0.0f);
        tilt = new Vector2(0.0f, 0.0f);
		cameraSound = GetComponents<AudioSource> ();
		backgroundSound = cameraSound [0];
		endLevelSound = cameraSound [1];
		gameOverSound = cameraSound [2];
		CanMute = true; 
		MuteTexture = sound;
    }
	
    public void ResetTilt()
    {
        lastLocation = new Vector2(playerCharacter.transform.position.x, playerCharacter.transform.position.y);
        tilt = new Vector2(0.0f, 0.0f);
    }

	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, playerCharacter.transform.position.x, Time.deltaTime * trackSpeed * 
            Mathf.Pow(Mathf.Abs(transform.position.x - playerCharacter.transform.position.x), 2)),

            Mathf.Lerp(transform.position.y, playerCharacter.transform.position.y + yOffset, Time.deltaTime * trackSpeed * 
            Mathf.Pow(Mathf.Abs(transform.position.y - (playerCharacter.transform.position.y + yOffset)), 3)),
            
            transform.position.z);

        var xAngle = (transform.position.x - playerCharacter.transform.position.x);
        var yAngle = (transform.position.y - (playerCharacter.transform.position.y - yOffset));

        var velocityTiltModifier = (new Vector2(playerCharacter.transform.position.x, playerCharacter.transform.position.y) - lastLocation) * tiltScale;

        tilt = new Vector2(Mathf.Lerp(tilt.x, velocityTiltModifier.x, Time.deltaTime), Mathf.Lerp(tilt.y, velocityTiltModifier.y, Time.deltaTime));

        transform.rotation = Quaternion.Euler(new Vector3(
            -tilt.y,
            tilt.x * xyTiltRatio,
            0.0f
            ));

        lastLocation = new Vector2(playerCharacter.transform.position.x, playerCharacter.transform.position.y);

		if (Door.endLevel == false && playerCharacter.GetComponent<PlayerController> ().alive == true) {
			backgroundSound.mute = false;
			endLevelSound.mute = true;
			gameOverSound.mute = true;
		} else if (Door.endLevel == true) {
			backgroundSound.mute = true;
			endLevelSound.mute = false;
			gameOverSound.mute = true;
		} else if (playerCharacter.GetComponent<PlayerController> ().alive == false) {
			backgroundSound.mute = true;
			endLevelSound.mute = true;
			gameOverSound.mute = false;
		} else {
			backgroundSound.mute = true;
			endLevelSound.mute = true;
			gameOverSound.mute = true;
		}
	}

	void OnGUI(){
		if (GUI.Button (new Rect (0, Screen.height-30, 50, 30), MuteTexture)) {
		if(CanMute){
				AudioListener.pause = true;
				MuteTexture = mute;
				CanMute = false;
		}else{
				AudioListener.pause = false;
				MuteTexture = sound;
				CanMute = true;
			}
		}
	}
}
