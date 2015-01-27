using UnityEngine;
using System.Collections;

public class RoomGrid  {
    public float goDownProbability = 0.3f;
    
    public enum TravelDirection { L, R, NotDecided };

    // if we want to make rooms off the solution path we should extend this to a [4,4,2] array,
    // with the second operator saying whether or not it is on the solution path to include specific off path rooms
    private static int width = 4;
    private static int height = 4;
    private Room[,] roomGrid = new Room[width, height];

    public RoomGrid()
    {
        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; i < roomGrid.GetLength(1); i++)
            {
                roomGrid[i, j] = new Room(Room.ExitType.None);
            }
        }
    }

    private void GeneratePath()
    {
        var currentRoom = new Point((int)(Random.value * 4), 0);
        var solutionPathComplete = false;
        var travelDirection = TravelDirection.NotDecided;
        
        /*
         * First Step: Generate Solution Path
         */
        while (!solutionPathComplete)
        {
            if (Random.value > goDownProbability)
            {
                // 50/50 chance of going left or right
                if (travelDirection == TravelDirection.NotDecided) 
                    travelDirection = Random.value > 0.5 ? TravelDirection.L : TravelDirection.R;

                // move left or right

            }
            else
            {
                // reset travel direction
                travelDirection = TravelDirection.NotDecided;

                // go down or finish level

            }
        }

        /*
         * Second Step: Fill in exit type
         */
        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; i < roomGrid.GetLength(1); i++)
            {
                //get neighbours

                // use top bottom power of two sum method similar to 3d game tilling
            }
        }
    }

    public Room GetRoom(Point point)
    {
        return roomGrid[point.x, point.y];
    }
}
