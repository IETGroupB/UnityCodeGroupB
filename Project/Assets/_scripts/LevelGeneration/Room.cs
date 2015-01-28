using UnityEngine;
using System.Collections;

public class Room {
    // L = Left, R = Right, T = Top, B = Bottom
    public enum ExitType { LR, LRT, LRB, LRTB, None};

    public bool isSolutionPath = false;
    public bool isExit = false;
    public bool isStart = false; 
    public ExitType exits;

    public Room(ExitType exits)
    {
        this.exits = exits;
    }
}
