using UnityEngine;
using System.Collections;

public class Door : Tile 
{
	void OnTriggerStay2D(Collider2D coll)
	{
		Debug.Log("OnTriggerStay2D");

		GameObject other = coll.transform.gameObject;
		if (other.name == "Character")
		{
			if (other.GetComponent<PlayerController>().Fire2Down)
			{
				transform.GetComponent<CircleCollider2D>().enabled = false;
				testRoomGeneration.levelSize++;
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
}
