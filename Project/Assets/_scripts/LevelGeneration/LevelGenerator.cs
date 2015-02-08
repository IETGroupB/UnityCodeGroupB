using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
    private ArrayList lrRooms = new ArrayList();
    private ArrayList lrtRooms = new ArrayList();
    private ArrayList lrbRooms = new ArrayList();
    private ArrayList lrtbRooms = new ArrayList();
    private ArrayList emptyRoom = new ArrayList();
    private string tilePrefabsFolder = "Tiles/";

    public Hashtable tileGameObjects { get; private set; }
    public RoomGrid currentRoomGrid { get; private set; }
    public Point currentRoomGridSize { get; private set; }
    public GameObject levelContainer { get; set; }

    // should this be Start() or Awake()?
    public LevelGenerator()
    {
        tileGameObjects = new Hashtable();
        // load tile prefabs
        foreach (TileType tileType in Tile.prefabs.Keys)
        {
            Debug.Log(tilePrefabsFolder + Tile.prefabs[tileType]);
            GameObject currentTile =  Resources.Load(tilePrefabsFolder + Tile.prefabs[tileType]) as GameObject;
            currentTile.GetComponent<Tile>().type = tileType;
            tileGameObjects.Add(tileType, currentTile);
        }

        // file reading goes here
		TileType[,] lrRoomstiles = FileHandler.Load(Application.dataPath + "\\Resources\\RoomFiles\\LR.txt");
		TileType[,] lrbRoomstiles = FileHandler.Load(Application.dataPath + "\\Resources\\RoomFiles\\LRB.txt");
		TileType[,] lrtRoomstiles = FileHandler.Load(Application.dataPath + "\\Resources\\RoomFiles\\LRT.txt");
		TileType[,] lrtbRoomstiles = FileHandler.Load(Application.dataPath + "\\Resources\\RoomFiles\\LRTB.txt");
		TileType[,] emptyRoomstiles = FileHandler.Load(Application.dataPath + "\\Resources\\RoomFiles\\empty.txt");
        // store each room as 16x16 TileTypes
        {
            // temp add films
			lrRooms.Add(lrRoomstiles);
			lrtRooms.Add(lrtRoomstiles);
			lrbRooms.Add(lrbRoomstiles);
			lrtbRooms.Add(lrtbRoomstiles);
			emptyRoom.Add(emptyRoomstiles);
        } // end temporary hardcoded room types
    }

    public void GenerateLevel(int width, int height, float goDownProbability)
    {
        currentRoomGridSize = new Point(width, height);
        currentRoomGrid = new RoomGrid(width, height, goDownProbability);

        levelContainer = new GameObject("LevelContainer");
        levelContainer.transform.parent = transform;
        
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Room currentRoom = currentRoomGrid.GetRoom(w, h);

                // assign tiles
                switch (currentRoom.exits)
                {
                    case ExitType.LR:
					Debug.Log(lrRooms.Count);
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
        // looks like O(n^4) but really O(n^2) where O(n^2) = O(<total tiles width> * <total tiles height>) 
        for (int h = 0; h < currentRoomGridSize.y; h++)
        {
            for (int w = 0; w < currentRoomGridSize.x; w++)
            {
                var currentRoom = currentRoomGrid.GetRoom(w, h);
                GameObject room = new GameObject("room_" + w + "_" + h);
                room.transform.parent = levelContainer.transform;
                room.transform.localPosition = new Vector3(w * currentRoom.tiles.GetLength(0), -(h * currentRoom.tiles.GetLength(1)), 0.0f);

                for (int x = 0; x < currentRoom.tiles.GetLength(0); x++)
                {
                    for (int y = 0; y < currentRoom.tiles.GetLength(1); y++)
                    {
                        // positions in array are translated to positions in worldspace as array[x,y] = world[x,-y]
                        var tilePosition = new Point(w * currentRoom.tiles.GetLength(0) + x, -(h * currentRoom.tiles.GetLength(1) + y));

                        // check that it is not empty space
                        if (currentRoom.tiles[x, y] != TileType.Empty)
                        {
                            GameObject currentTile = (GameObject)Instantiate(tileGameObjects[currentRoom.tiles[x, y]] as GameObject);
                            currentTile.transform.parent = room.transform;
                            currentTile.transform.localPosition = new Vector3(x, -y, 0.0f);
                            
                        }
                    }
                }
            }
        }
    }

    public string RoomGridToString()
    {
        return currentRoomGrid.ToString();
    }

    public void DeleteLevel()
    {
        Destroy(levelContainer);
    }
}
