using UnityEngine;
using System.Collections;

public class modelpose : MonoBehaviour {
	public float rotateAng;
	GameObject EnemyLight;
	public Vector3 EnemyPos;
	public Vector3 LightPos;

	void Start(){
		EnemyLight = GameObject.Find("EnemyLight").gameObject;
		rotateAng = 0.0f;
	}

	void Update(){
		Vector3 lightToCenter = new Vector3(EnemyLight.transform.position.x-transform.position.x,
		                               EnemyLight.transform.position.y-transform.position.y,
		                               0);
		EnemyPos = transform.position;
		LightPos = EnemyLight.transform.position;
		if(lightToCenter.x>0){
			if(lightToCenter.y>0)
				rotateAng = Vector3.Angle( new Vector3(1,0,0), lightToCenter);
			else
				rotateAng = Vector3.Angle( new Vector3(-1,0,0), lightToCenter);
		}
		else{
			if(lightToCenter.y>0)
				rotateAng = Vector3.Angle( new Vector3(1,0,0), lightToCenter);
			else
				rotateAng = Vector3.Angle( new Vector3(-1,0,0), lightToCenter);
		}
		transform.eulerAngles = new Vector3(0,0,rotateAng);
	}
}
