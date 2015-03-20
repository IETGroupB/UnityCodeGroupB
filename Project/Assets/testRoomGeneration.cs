using UnityEngine;
using System.Collections;

public class testRoomGeneration : MonoBehaviour {
    SwitchSystem switchSys;

	public static int levelSize = 2;
    public float  goDownProb;
	// Use this for initialization
    void Start()
    {
        GameSetup gs = gameObject.GetComponent<GameSetup>();
        gs.Initialise(levelSize, levelSize, goDownProb);
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
