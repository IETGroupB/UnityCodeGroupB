using UnityEngine;
using System.Collections;

public class RoomGrid  {
    public float goDownProbability = 0.4f;

    private enum TravelDirection { L, R, D, NotDecided };
    private static int width = 4;
    private static int height = 4;
    private Room[,] roomGrid = new Room[width, height];

    public RoomGrid()
    {
        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; j < roomGrid.GetLength(1); j++)
            {
                roomGrid[i, j] = new Room(Room.ExitType.None);
            }
        }
        GeneratePath();
    }

    private void GeneratePath()
    {
        var currentLocation = new Point((int)(Random.value * 4), 0);
        var solutionPathComplete = false;
        var travelDirection = TravelDirection.NotDecided;

        Debug.Log(currentLocation.ToString());
        roomGrid[currentLocation.x, currentLocation.y].isStart = true;
        roomGrid[currentLocation.x, currentLocation.y].isSolutionPath = true;
        roomGrid[currentLocation.x, currentLocation.y].exits = Room.ExitType.LR;


        while (!solutionPathComplete)
        {
            roomGrid[currentLocation.x, currentLocation.y].isSolutionPath = true;
            if (travelDirection == TravelDirection.NotDecided)
            {
                // move away from wall or randomly choose left or right
                if (currentLocation.x == 0)
                    travelDirection = TravelDirection.R;
                else if (currentLocation.x == width - 1)
                    travelDirection = TravelDirection.L;
                else
                    travelDirection = Random.value > 0.5 ? TravelDirection.L : TravelDirection.R;
            }

            // travel left or right
            if (Random.value > goDownProbability && travelDirection != TravelDirection.D)
            {
                // bumped into a wall
                if (
                    (currentLocation.x == 0 && travelDirection == TravelDirection.L) ||
                    (currentLocation.x == width - 1 && travelDirection == TravelDirection.R)
                    )
                {
                    travelDirection = TravelDirection.D;

                    // go down
                    roomGrid[currentLocation.x, currentLocation.y].exits = Room.ExitType.LRB;

                    // check if exit above
                    if (currentLocation.y != 0)
                    {
                        if ( roomGrid[currentLocation.x, currentLocation.y - 1].exits == Room.ExitType.LRB || roomGrid[currentLocation.x, currentLocation.y - 1].exits == Room.ExitType.LRTB)
                        {
                            roomGrid[currentLocation.x, currentLocation.y].exits = Room.ExitType.LRTB;
                        }
                    }
                }
                else
                {
                    if (travelDirection == TravelDirection.L)
                        currentLocation.x--;
                    else
                        currentLocation.x++;

                    roomGrid[currentLocation.x, currentLocation.y].exits = Room.ExitType.LR;

                    if (currentLocation.y != 0)
                    {
                        if ( roomGrid[currentLocation.x, currentLocation.y - 1].exits == Room.ExitType.LRB || roomGrid[currentLocation.x, currentLocation.y - 1].exits == Room.ExitType.LRTB)
                        {
                            roomGrid[currentLocation.x, currentLocation.y].exits = Room.ExitType.LRT;
                        }
                    }
                }
            }
            else
            {
                travelDirection = TravelDirection.NotDecided;

                // make exit
                if(currentLocation.y == height - 1)
                {
                    roomGrid[currentLocation.x, currentLocation.y].isSolutionPath = true;
                    roomGrid[currentLocation.x, currentLocation.y].isExit = true;
                    solutionPathComplete = true;

                    if (roomGrid[currentLocation.x, currentLocation.y - 1].exits == Room.ExitType.LRB || roomGrid[currentLocation.x, currentLocation.y - 1].exits == Room.ExitType.LRTB)
                    {
                        roomGrid[currentLocation.x, currentLocation.y].exits = Room.ExitType.LRT;
                    }
                    else
                    {
                        roomGrid[currentLocation.x, currentLocation.y].exits = Room.ExitType.LR;
                    }
                }
                // travel down
                else
                {
                    currentLocation.y++;
                    roomGrid[currentLocation.x, currentLocation.y].exits = Room.ExitType.LRT;

                    if (currentLocation.y != 0)
                    {
                        if (
                            roomGrid[currentLocation.x, currentLocation.y - 1].exits != Room.ExitType.LRB ||
                            roomGrid[currentLocation.x, currentLocation.y - 1].exits != Room.ExitType.LRTB)
                        {
                            if (roomGrid[currentLocation.x, currentLocation.y - 1].exits == Room.ExitType.LRT)
                            {
                                roomGrid[currentLocation.x, currentLocation.y - 1].exits = Room.ExitType.LRTB;
                            }
                            else
                            {
                                roomGrid[currentLocation.x, currentLocation.y - 1].exits = Room.ExitType.LRB;
                            }
                        }
                    }
                }
            }
            roomGrid[currentLocation.x, currentLocation.y].isSolutionPath = true;
        }
    }

    public Room GetRoom(Point point)
    {
        return roomGrid[point.x, point.y];
    }

    public override string ToString()
    {
        var ret = "";

        // reverse order, row first
        for (int j = 0; j < roomGrid.GetLength(0); j++)
        {
            for (int i = 0; i < roomGrid.GetLength(1); i++)
            {
                switch (roomGrid[i, j].exits)
                {
                    case Room.ExitType.LR:
                        ret += "−";
                        break;
                    case Room.ExitType.LRT:
                        ret += "⊥";
                        break;
                    case Room.ExitType.LRB:
                        ret += "T";
                        break;
                    case Room.ExitType.LRTB:
                        ret += "+";
                        break;
                    case Room.ExitType.None:
                        ret += "O";
                        break;
                }   
            }
            ret += "\n";
        }

        return ret;
    }
}
