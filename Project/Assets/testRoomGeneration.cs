﻿using UnityEngine;
using System.Collections;

public class testRoomGeneration : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LevelGenerator lg = gameObject.AddComponent<LevelGenerator>();
        lg.GenerateLevel(4, 4, 0.4f);
        Debug.Log(lg.RoomGridToString());
        lg.DrawLevel();

        foreach (Point p in lg.currentRoomGrid.solutionPath)
            Debug.Log(p);
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
