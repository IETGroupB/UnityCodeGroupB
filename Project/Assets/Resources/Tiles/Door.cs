using UnityEngine;
using System.Collections;

public class Door : Tile 
{
	Animator anim;                          // Reference to the animator component.
	float restartTimer;   

	public static bool endLevel = false;

	void OnTriggerStay2D(Collider2D coll)
	{
		Debug.Log("OnTriggerStay2D");

		GameObject other = coll.transform.gameObject;
		if (other.name == "Character")
		{
			if (other.GetComponent<PlayerController>().Fire2Down)
			{
				transform.GetComponent<CircleCollider2D>().enabled = false;
				endLevel =  true;
			}
		}
	}
}
