using UnityEngine;
using System.Collections;

public class testRoomGeneration : MonoBehaviour {
    SwitchSystem switchSys;

	// Use this for initialization
    void Start()
    {

        GameSetup gs = gameObject.GetComponent<GameSetup>();
        gs.Initialise(4, 4, 0.4f);
       
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
