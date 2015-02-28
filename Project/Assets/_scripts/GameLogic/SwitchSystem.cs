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
    private int initialToggle;

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
            furthestSwitch = -1;

            switchSystemActive = true;
            initialToggle = 0;
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
                 
                var alarmFlag = 
                    !alarmActive &&
                    Random.value < alarmProbability && 
                    !roomGrid.GetRoom(switchRooms[i]).isStart && 
                    !roomGrid.GetRoom(switchRooms[i]).isExit;
                
                alarmActive = alarmFlag;
				ToggleAlarm(alarmFlag);
                ToggleTraps(alarmFlag);
                

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

            // no switches hit
            if (furthestSwitch < 0)
            {
                if (i <= switchRoomsGlobalIndex[0])
                {
                    room.lightState = LightingType.Dim;
                }
            }
            else
            {
                if (i <= switchRoomsGlobalIndex[furthestSwitch])
                {
                    room.lightState = LightingType.Bright;
                }
                // check that it is not the last room
                else if (furthestSwitch + 1 < switchRooms.Length)
                {
                    if (i <= switchRoomsGlobalIndex[furthestSwitch + 1])
                    {
                        room.lightState = LightingType.Dim;
                    }
                    else
                    {
                        room.lightState = LightingType.Dark;
                    }
                }
            }
			room.UpdateRoomLight();

        }
        Debug.Log(lightStatesToString());

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
