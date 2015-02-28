using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class Intro: MonoBehaviour
	{
		public static bool playIntro = true;
		public float restartDelay = 4.5f;         // Time to wait before restarting the level
		
		Animator anim;                          // Reference to the animator component.
		float restartTimer;                     // Timer to count up to restarting the level
		
		void Awake ()
		{
			// Set up the reference.
			anim = GetComponent <Animator> ();
		}
		
		void Update ()
		{
			// If the player has run out of health...
			if(playIntro == true)
			{
				anim.SetTrigger ("introFinish");
				restartTimer += Time.deltaTime;

				if(restartTimer >= restartDelay)
				{
					playIntro = false;
					Application.LoadLevel(Application.loadedLevel);
				}
			}
		}
	}
}