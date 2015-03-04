using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    GameObject playerCharacter;
    public float trackSpeed;
    public float yOffset;
    public float tiltScale;
    public float xyTiltRatio;

    private Vector2 lastLocation;
    private Vector2 tilt;

	// Use this for initialization
	void Start () {
        playerCharacter = GameObject.Find("Character");
        lastLocation = new Vector2(playerCharacter.transform.position.x, playerCharacter.transform.position.y);
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
	}
}
