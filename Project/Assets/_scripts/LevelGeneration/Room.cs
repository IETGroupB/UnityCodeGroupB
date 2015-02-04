﻿using UnityEngine;
using System.Collections;

public enum ExitType { LR, LRT, LRB, LRTB, None };

public class Room {
    public bool isSolutionPath = false;
    public bool isExit = false;
    public bool isStart = false; 
    public ExitType exits;

    public TileType[,] tiles = new TileType[16, 16];

    public Room(ExitType exits)
    {
        this.exits = exits;
    }
}
