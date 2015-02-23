﻿using UnityEngine;
using System.Collections;

public enum ExitType { LR, LRT, LRB, LRTB, None };
public enum LightingType {Dark, Dim, Bright};

public class Room {
    public bool isSolutionPath = false;
    public bool isExit = false;
    public bool isStart = false; 
    public ExitType exits;
    public TileType[,] tiles = new TileType[16, 16];
    public LightingType lightState;

    private TilePrefabManager prefabs;
    private GameObject roomObj;

    //switch specific parameters
    private Point switchLocation;
    private bool hasSwitch;
    public Switch switchParams;

    public Room(ExitType exits, TilePrefabManager prefabsLink)
    {
        this.exits = exits;
        prefabs = prefabsLink;
    }

    public void DrawRoom(GameObject room)
    {
        roomObj = room;
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                switch(tiles[x, y])
                {
                    case TileType.Solid:
                        var currentTile = (GameObject) MonoBehaviour.Instantiate(prefabs.tileGameObjects[tiles[x, y]] as GameObject);
                        currentTile.transform.parent = room.transform;
                        currentTile.transform.localPosition = new Vector3(x, -y, 0.0f);
                        break;
                    case TileType.Switch:
                        switchLocation = new Point(x, y);
                        break;
                }
            }
        }

        if (switchLocation == null && exits != ExitType.None)
        {
            Debug.LogError("Room file does not contain switch");
        }
    }

    public void AddSwitch() 
    {
        hasSwitch = true;

        var switchObj = (GameObject) MonoBehaviour.Instantiate(prefabs.tileGameObjects[TileType.Switch] as GameObject);

        //TODO update for switch position
        switchObj.transform.parent = roomObj.transform;
        switchObj.transform.localPosition = new Vector3(switchLocation.x, -switchLocation.y, 0.1f);

        switchParams = switchObj.GetComponent<Switch>();
    }
}
