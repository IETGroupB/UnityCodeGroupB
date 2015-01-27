using UnityEngine;
using System.Collections;

public class Room {
    // L = Left, R = Right, T = Top, B = Bottom
    // SolutionPath = temp room id for generation
    public static enum ExitType { LR, LRT, LRB, LRTB, None, SolutionPath };
   
    public bool isExit = false;
    public bool isStart = false; 
    public ExitType exits;

    public Room(ExitType exits)
    {
        this.exits = exits;
    }
}
