using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGrid  {
    private float goDownProbability = 0.4f;
    private enum TravelDirection { L, R, D, NotDecided };
    private Room[,] roomGrid;
    public Point[] solutionPath { get; private set; }
    // must be at least 2x2 to work
    private int width;
    private int height;

    public RoomGrid(int width, int height, float goDownProbability)
    {
        this.width = width;
        this.height = height;
        this.goDownProbability = goDownProbability;
        roomGrid  = new Room[width, height];

        TilePrefabManager tfb = GameObject.Find("LevelGenerator").GetComponent<TilePrefabManager>();

        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; j < roomGrid.GetLength(1); j++)
            {
                roomGrid[i, j] = new Room(ExitType.None, tfb);
            }
        }
        GeneratePath();
    }

    private void GeneratePath()
    {
        var tempSolutionPath = new List<Point>();

        var currentLocation = new Point((int)(Random.value * width), 0);
        var solutionPathComplete = false;
        var travelDirection = TravelDirection.NotDecided;

        roomGrid[currentLocation.x, currentLocation.y].isStart = true;
        roomGrid[currentLocation.x, currentLocation.y].isSolutionPath = true;
        roomGrid[currentLocation.x, currentLocation.y].exits = ExitType.LR;

        tempSolutionPath.Add(new Point(currentLocation));

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
                    roomGrid[currentLocation.x, currentLocation.y].exits = ExitType.LRB;

                    // check if we need to change the room to a top and bottom exit room
                    if (currentLocation.y != 0)
                    {
                        if ( roomGrid[currentLocation.x, currentLocation.y - 1].exits == ExitType.LRB || roomGrid[currentLocation.x, currentLocation.y - 1].exits == ExitType.LRTB)
                        {
                            roomGrid[currentLocation.x, currentLocation.y].exits = ExitType.LRTB;
                        }
                    }
                }
                else
                {
                    // it's ok to move left or right
                    if (travelDirection == TravelDirection.L)
                        currentLocation.x--;
                    else
                        currentLocation.x++;

                    roomGrid[currentLocation.x, currentLocation.y].exits = ExitType.LR;

                    // check if we need a top enteranct
                    if (currentLocation.y != 0)
                    {
                        if ( roomGrid[currentLocation.x, currentLocation.y - 1].exits == ExitType.LRB || roomGrid[currentLocation.x, currentLocation.y - 1].exits == ExitType.LRTB)
                        {
                            roomGrid[currentLocation.x, currentLocation.y].exits = ExitType.LRT;
                        }
                    }

                    // add position to solution path
                    tempSolutionPath.Add(new Point(currentLocation));
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

                    if (roomGrid[currentLocation.x, currentLocation.y - 1].exits == ExitType.LRB || roomGrid[currentLocation.x, currentLocation.y - 1].exits == ExitType.LRTB)
                    {
                        roomGrid[currentLocation.x, currentLocation.y].exits = ExitType.LRT;
                    }
                    else
                    {
                        roomGrid[currentLocation.x, currentLocation.y].exits = ExitType.LR;
                    }

                    
                    if (!(tempSolutionPath[tempSolutionPath.Count - 1] as Point).Equals(currentLocation))
                        tempSolutionPath.Add(new Point(currentLocation));
                }
                // travel down
                else
                {
                    currentLocation.y++;
                    roomGrid[currentLocation.x, currentLocation.y].exits = ExitType.LRT;

                    if (currentLocation.y != 0)
                    {
                        if (
                            roomGrid[currentLocation.x, currentLocation.y - 1].exits != ExitType.LRB ||
                            roomGrid[currentLocation.x, currentLocation.y - 1].exits != ExitType.LRTB)
                        {
                            if (roomGrid[currentLocation.x, currentLocation.y - 1].exits == ExitType.LRT)
                            {
                                roomGrid[currentLocation.x, currentLocation.y - 1].exits = ExitType.LRTB;
                            }
                            else
                            {
                                roomGrid[currentLocation.x, currentLocation.y - 1].exits = ExitType.LRB;
                            }
                        }
                    }

                    tempSolutionPath.Add(new Point(currentLocation));
                }
                roomGrid[currentLocation.x, currentLocation.y].isSolutionPath = true;
            }
        }

        solutionPath = tempSolutionPath.ToArray();
    }

    public Room GetRoom(int x, int y)
    {
        return roomGrid[x, y];
    }

    public Room GetRoom(Point point)
    {
        return GetRoom(point.x, point.y);
    }

    public override string ToString()
    {
        var ret = "";

        // reverse order, row first
        for (int j = 0; j < roomGrid.GetLength(1); j++)
        {
            for (int i = 0; i < roomGrid.GetLength(0); i++)
            {
                switch (roomGrid[i, j].exits)
                {
                    case ExitType.LR:
                        ret += "−";
                        break;
                    case ExitType.LRT:
                        ret += "⊥";
                        break;
                    case ExitType.LRB:
                        ret += "T";
                        break;
                    case ExitType.LRTB:
                        ret += "+";
                        break;
                    case ExitType.None:
                        ret += "O";
                        break;
                }   
            }
            ret += "\n";
        }

        return ret;
    }
}
