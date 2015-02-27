using UnityEngine;
using System.Collections;


public class RoomLight : Tile {
	Light rLight;
	private float radius;
	private LightingType state;
	public bool isAlarmActive;
	Light alarmLight;
	private AudioSource alarmSound;

	void Start () {
		rLight = transform.GetComponent<Light>();
		rLight.intensity = 0.0f;
		rLight.range = 1.0f;
		isAlarmActive = false;
		alarmLight = transform.GetChild (0).GetComponent<Light> ();
		alarmLight.intensity = 0.0f;
		alarmSound = alarmLight.audio;
		alarmSound.mute = true;

	}

	 void Update(){
		if (isAlarmActive) {
			alarmLight.intensity = 5.0f;
			alarmLight.transform.Rotate (300 * Time.deltaTime, 300 * Time.deltaTime, 300 * Time.deltaTime, Space.Self);
			alarmSound.mute = false;
		} else {
			alarmLight.intensity = 0.0f;
			alarmSound.mute = true;
		}
	}
	
}
