using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Handles switch logic such as setting up switches triggering lights and alarms
 */
public class SwitchSystem : MonoBehaviour {
    // later these could be placed in SetUp to vary difficulty as game continues
    public float alarmProbability;
    public float switchDensity;
    public Point[] switchRooms;
    public bool alarmActive;
    
    private RoomGrid roomGrid;
    private int furthestSwitch;
    private bool switchSystemActive;

    /*
     * Called on creation of level to set up the switch system
     */
    public void SetUp(RoomGrid roomGrid)
    {
        alarmActive = false;
        this.roomGrid = roomGrid;
        

        // assign switches to rooms
        {
            var hasSwitchList = new List<Point>();

            foreach (Point p in roomGrid.solutionPath)
            {
                if (Random.value < switchDensity || roomGrid.GetRoom(p).isExit)
                {
                    roomGrid.GetRoom(p).AddSwitch();
                    hasSwitchList.Add(p);
                }
            }

            switchRooms = hasSwitchList.ToArray();
            furthestSwitch = 0;
        }
        switchSystemActive = true;
        UpdateLights();
    }

    /*
     * Called on tear down of level 
     */
    public void TearDown()
    {
        this.roomGrid = null;
        switchRooms = null;
    }

    public void FixedUpdate()
    {
        // exit if not set up
        if (switchSystemActive) return;
        
        // poll the switches to check for changes
        for (int i = furthestSwitch; i < switchRooms.Length; i++)
        {
            Room r = roomGrid.GetRoom(switchRooms[i]);
            r.switchParams.GetState();
            if (roomGrid.GetRoom(switchRooms[i]).switchParams.GetState())
            {
                // new furthest switch reached
                furthestSwitch = i;
                 
                if (
                    !alarmActive &&
                    Random.value < alarmProbability && 
                    !roomGrid.GetRoom(switchRooms[i]).isStart && 
                    !roomGrid.GetRoom(switchRooms[i]).isExit)
                {
                    alarmActive = true;
                    AlarmLights();
                }
                else
                {
                    alarmActive = false;
                    UpdateLights();
                }
                    
                break;
            }
        }
    }

    /*
     * bright lights up to furthestSwitch room, dim lights until furthestSwitch + 1 room
     */
    private void UpdateLights()
    {

    }
    /*
     * alarm lights up to furthestSwitch + 1 room
     */
    public void AlarmLights()
    {    
        // maybe dim lights until furthestSwitch + 1 as well as alarm lights? discus at standup
    }
}
