using UnityEngine;
using System.Collections;
using System.IO;

public class LevelGenerator : MonoBehaviour {
    private ArrayList lrRooms = new ArrayList();
    private ArrayList lrtRooms = new ArrayList();
    private ArrayList lrbRooms = new ArrayList();
    private ArrayList lrtbRooms = new ArrayList();
    private ArrayList emptyRoom = new ArrayList();

    
    public RoomGrid roomGrid { get; private set; }
    public Point roomGridSize { get; private set; }
    public GameObject levelContainer { get; set; }

    // should this be Start() or Awake()?
    public LevelGenerator()
    {
        
		TileType[,] empty = FileReader.LoadFile ("RoomFiles/Empty/empty");
		TileType[,] lr = FileReader.LoadFile ("RoomFiles/LR/LR");
		TileType[,] lrt = FileReader.LoadFile ("RoomFiles/LRT/LRT");
		TileType[,] lrb = FileReader.LoadFile ("RoomFiles/LRB/LRB");
		TileType[,] lrtb = FileReader.LoadFile ("RoomFiles/LRTB/LRTB");

        // store each room as 16x16 TileTypes
        {
            // temp add films
			lrRooms.Add(lr);
			lrtRooms.Add(lrt);
			lrbRooms.Add(lrb);
			lrtbRooms.Add(lrtb);
			emptyRoom.Add(empty);
        } // end temporary hardcoded room types
    }

    public void GenerateLevel(int width, int height, float goDownProbability)
    {
        roomGridSize = new Point(width, height);
        roomGrid = new RoomGrid(width, height, goDownProbability);

        levelContainer = new GameObject("LevelContainer");
        levelContainer.transform.parent = transform;
        
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Room currentRoom = roomGrid.GetRoom(w, h);

                // assign tiles
                switch (currentRoom.exits)
                {
                    case ExitType.LR:
                        currentRoom.tiles = lrRooms[Mathf.FloorToInt(lrRooms.Count * Random.value)] as TileType[,];
                        break;
                    case ExitType.LRT:
                        currentRoom.tiles = lrtRooms[Mathf.FloorToInt(lrtRooms.Count * Random.value)] as TileType[,];
                        break;
                    case ExitType.LRB:
                        currentRoom.tiles = lrbRooms[Mathf.FloorToInt(lrbRooms.Count * Random.value)] as TileType[,];
                        break;
                    case ExitType.LRTB:
                        currentRoom.tiles = lrtbRooms[Mathf.FloorToInt(lrtbRooms.Count * Random.value)] as TileType[,];
                        break;
                    case ExitType.None:
                        currentRoom.tiles = emptyRoom[Mathf.FloorToInt(lrtbRooms.Count * Random.value)] as TileType[,];
                        break;
                }
            }
        }

    }

    public void DrawLevel()
    {
        // Draw Rooms
        for (int h = 0; h < roomGridSize.y; h++)
        {
            for (int w = 0; w < roomGridSize.x; w++)
            {
                var currentRoom = roomGrid.GetRoom(w, h);
                GameObject room = new GameObject("room_" + w + "_" + h);
                room.transform.parent = levelContainer.transform;
                room.transform.localPosition = new Vector3(w * currentRoom.tiles.GetLength(0), -(h * currentRoom.tiles.GetLength(1)), 0.0f);

                currentRoom.DrawRoom(room);   
            }
        }


    }

    public string RoomGridToString()
    {
        return roomGrid.ToString();
    }

    public void DeleteLevel()
    {
        Destroy(levelContainer);
    }
}
