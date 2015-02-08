﻿using UnityEngine;
using System.Collections;
using System.IO;

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
            GameObject currentTile =  Resources.Load(tilePrefabsFolder + Tile.prefabs[tileType]) as GameObject;
            currentTile.GetComponent<Tile>().type = tileType;
            tileGameObjects.Add(tileType, currentTile);
        }

        // file reading goes here
		ArrayList fileReadLR = new ArrayList ();
		ArrayList fileReadLRB = new ArrayList ();
		ArrayList fileReadLRT = new ArrayList ();
		ArrayList fileReadLRTB = new ArrayList ();
		ArrayList fileReadEmpty = new ArrayList ();

		foreach (string fileName_1 in Directory.GetFiles (Application.dataPath + "\\Resources\\RoomFiles\\LR\\")) {
			TileType[,] lrRoomsFiletiles = FileHandler.Load(fileName_1);
			fileReadLR.Add(lrRoomsFiletiles);
		}

		foreach (string fileName_2 in Directory.GetFiles (Application.dataPath + "\\Resources\\RoomFiles\\LRB\\")) {
			TileType[,] lrbRoomsFiletiles = FileHandler.Load(fileName_2);
			fileReadLRB.Add(lrbRoomsFiletiles);
		}

		foreach (string fileName_3 in Directory.GetFiles (Application.dataPath + "\\Resources\\RoomFiles\\LRT\\")) {
			TileType[,] lrtRoomsFiletiles = FileHandler.Load(fileName_3);
			fileReadLRT.Add(lrtRoomsFiletiles);
		}

		foreach (string fileName_4 in Directory.GetFiles (Application.dataPath + "\\Resources\\RoomFiles\\LRTB\\")) {
			TileType[,] lrtbRoomsFiletiles = FileHandler.Load(fileName_4);
			fileReadLRTB.Add(lrtbRoomsFiletiles);
		}

		foreach (string fileName_5 in Directory.GetFiles (Application.dataPath + "\\Resources\\RoomFiles\\empty\\")) {
			TileType[,] emptyRoomsFiletiles = FileHandler.Load(fileName_5);
			fileReadEmpty.Add(emptyRoomsFiletiles);
		}

        // store each room as 16x16 TileTypes
        {
            // temp add films
			lrRooms.Add(fileReadLR[0]);
			lrtRooms.Add(fileReadLRT[0]);
			lrbRooms.Add(fileReadLRB[0]);
			lrtbRooms.Add(fileReadLRTB[0]);
			emptyRoom.Add(fileReadEmpty[0]);
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
