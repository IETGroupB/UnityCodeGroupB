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
    public int[] switchRoomsGlobalIndex;
    public bool alarmActive;
    
    private RoomGrid roomGrid;
    public int furthestSwitch;
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
            var hasSwitchGlobalList = new List<int>();

            for (int i = 0; i < roomGrid.solutionPath.Length; i++)
            {
                if (Random.value < switchDensity || roomGrid.GetRoom(roomGrid.solutionPath[i]).isExit)
                {
                    roomGrid.GetRoom(roomGrid.solutionPath[i]).AddSwitch();
                    hasSwitchList.Add(roomGrid.solutionPath[i]);
                    hasSwitchGlobalList.Add(i);
                }
            }

            switchRoomsGlobalIndex = hasSwitchGlobalList.ToArray();
            switchRooms = hasSwitchList.ToArray();
            furthestSwitch = 0;

            switchSystemActive = true;
        }

        
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
        if (!switchSystemActive) return;
        
        // poll the switches to check for changes
        for (int i = furthestSwitch + 1; i < switchRooms.Length; i++)
        {
            var room = roomGrid.GetRoom(switchRooms[i]);
            room.switchParams.GetState();
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
					ToggleAlarm (true);
                    ToggleTraps(true);
                }
                else
                {
                    alarmActive = false;
					ToggleAlarm (false);
                    ToggleTraps(false);
                }

                UpdateLights();
                break;
            }
        }
    }

    /*
     * bright lights up to furthestSwitch room, dim lights until furthestSwitch + 1 room
     */
    public void UpdateLights()
    {
        for (int i = 0; i < roomGrid.solutionPath.Length; i++)
        {
            var room = roomGrid.GetRoom(roomGrid.solutionPath[i]);
            if (i <= switchRoomsGlobalIndex[furthestSwitch])
            {
                room.lightState = LightingType.Bright;
				room.UpdateRoomLight();
            }
            else if (furthestSwitch + 1 < switchRooms.Length) // check that it is not the last room
            {
                if (i <= switchRoomsGlobalIndex[furthestSwitch + 1])
                {
                    room.lightState = LightingType.Dim;
					room.UpdateRoomLight();
                }
                else
                {
                    room.lightState = LightingType.Dark;
					room.UpdateRoomLight();
                }
            }
        }
    }

    private void ToggleTraps(bool on)
    {
        for (int i = 0; i < roomGrid.solutionPath.Length; i++)
        {
            roomGrid.GetRoom(roomGrid.solutionPath[i]).ToggleTraps(on);
        }
    }

	private void ToggleAlarm(bool on)
	{
		for (int i = 0; i < roomGrid.solutionPath.Length; i++)
		{
			roomGrid.GetRoom(roomGrid.solutionPath[i]).ToggleAlarm(on);
		}
	}
	
	public string lightStatesToString()
    {
        var s = "";
        foreach (var point in roomGrid.solutionPath)
        {
            s += roomGrid.GetRoom(point).lightState + ", ";
        }

        return s;
    }
}
