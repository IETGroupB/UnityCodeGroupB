using UnityEngine;
using System.Collections;


public class RoomLight : Tile {
	Light rLight;
	private float radius;
	private LightingType state;
	
	void Start () {
		rLight = transform.GetComponent<Light>();
		rLight.intensity = 0.0f;
		//Color  c = new Color(Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
		//rLight.color = c;
		//rLight.range = radius;
		//state = LightingType.Dark;
	}

	
}
