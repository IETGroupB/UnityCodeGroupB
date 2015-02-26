using UnityEngine;
using System.Collections;

public class testRoomGeneration : MonoBehaviour {
    SwitchSystem switchSys;

	// Use this for initialization
    IEnumerator Start()
    {

        /*LevelGenerator lg = gameObject.AddComponent<LevelGenerator>();
        lg.GenerateLevel(4, 4, 0.4f);


        //Debug.Log(lg.RoomGridToString());
        lg.DrawLevel();


        switchSys = gameObject.GetComponent<SwitchSystem>();
        switchSys.SetUp(lg.roomGrid);*/
        GameSetup gs = gameObject.GetComponent<GameSetup>();
        gs.Initialise(4, 4, 0.4f);

        Debug.Log("first");
        yield return new WaitForSeconds(3);
        Debug.Log("second");

        gs.DestroyLevel();
        gs.Initialise(2, 2, 0.4f);
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
