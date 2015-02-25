using UnityEngine;
using System.Collections;

public class testRoomGeneration : MonoBehaviour {
    SwitchSystem switchSys;

	// Use this for initialization
	void Start () {

        /*LevelGenerator lg = gameObject.AddComponent<LevelGenerator>();
        lg.GenerateLevel(4, 4, 0.4f);


        //Debug.Log(lg.RoomGridToString());
        lg.DrawLevel();


        switchSys = gameObject.GetComponent<SwitchSystem>();
        switchSys.SetUp(lg.roomGrid);*/
        GameSetup gs = gameObject.GetComponent<GameSetup>();
        gs.Initialise(4, 4, 0.4f);
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
