using UnityEngine;
using System.Collections;

public class testRoomGeneration : MonoBehaviour {
    SwitchSystem switchSys;

	public static int levelSize = 4;
	// Use this for initialization
    void Start()
    {
        GameSetup gs = gameObject.GetComponent<GameSetup>();
		gs.Initialise(levelSize, levelSize, 0.4f);
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
