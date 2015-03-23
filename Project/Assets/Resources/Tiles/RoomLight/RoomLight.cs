using UnityEngine;
using System.Collections;


public class RoomLight : Tile {
	Light rLight;
	private float radius;
	public LightingType state;
	public bool isAlarmActive;
	Light alarmLight;
	public float DimIntensity;
	public float BrightIntensity;
	public float fadeRate;
	private AudioSource alarmSound;

	void Awake () {
		rLight = transform.GetComponent<Light>();
		rLight.intensity = 0.0f;
		rLight.range = 1.0f;
		isAlarmActive = false;
		alarmLight = transform.GetChild (0).GetComponent<Light> ();
		alarmLight.intensity = 0.0f;
		rLight.enabled = false;
		alarmSound = GetComponent<AudioSource>();
	}

	public void UpdatePointLightIntensity(LightingType state){
		this.state = state;
		if (state != LightingType.Dark) rLight.enabled = true;
	}

	public void SetRadius(int radius)
	{
		rLight.range = 5 + radius;
	}

	 void Update(){
		if (isAlarmActive) {
			alarmSound.Play();
			alarmLight.intensity = 5.0f;
			alarmLight.transform.Rotate (300 * Time.deltaTime, 300 * Time.deltaTime, 300 * Time.deltaTime, Space.Self);

		} else {
			alarmLight.intensity = 0.0f;
			alarmSound.Pause();

		}

		switch (state) {
		case LightingType.Bright:
			rLight.intensity = Mathf.Lerp(rLight.intensity, BrightIntensity, Time.deltaTime * fadeRate);
			break;
		case LightingType.Dim:
			rLight.intensity = Mathf.Lerp(rLight.intensity, DimIntensity, Time.deltaTime * fadeRate);
			break;
		case LightingType.Dark:
			rLight.intensity = Mathf.Lerp(rLight.intensity, 0.0f, Time.deltaTime * fadeRate);;
			break;
		}
	}
	
}
