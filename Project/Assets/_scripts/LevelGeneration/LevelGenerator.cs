using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelGenerator : MonoBehaviour 
{
    private ArrayList lrRooms = new ArrayList();
    private ArrayList lrtRooms = new ArrayList();
    private ArrayList lrbRooms = new ArrayList();
    private ArrayList lrtbRooms = new ArrayList();
    private ArrayList emptyRoom = new ArrayList();
	   
    public RoomGrid roomGrid { get; private set; }
    public Point roomGridSize { get; private set; }
    public GameObject levelContainer { get; set; }
	public GameObject wallContainer { get; set; }

    void Awake()
    {
		//load all text files for different roomType
		var files = Resources.LoadAll <TextAsset>("RoomFiles/LR/");
        for (int i = 0; i < files.Length; i++)
			lrRooms.Add (FileReader.LoadFile(files[i]));
		
        files = Resources.LoadAll<TextAsset>("RoomFiles/LRB/");
        for (int i = 0; i < files.Length; i++)
			lrbRooms.Add (FileReader.LoadFile(files[i]));
		
        files = Resources.LoadAll<TextAsset>("RoomFiles/LRT/");
        for (int i = 0; i < files.Length; i++)
			lrtRooms.Add (FileReader.LoadFile(files[i]));
		
        files = Resources.LoadAll<TextAsset>("RoomFiles/LRTB/");
        for (int i = 0; i < files.Length; i++)
			lrtbRooms.Add (FileReader.LoadFile(files[i]));
		
        files = Resources.LoadAll<TextAsset>("RoomFiles/Empty/");
        for (int i = 0; i < files.Length; i++)
			emptyRoom.Add (FileReader.LoadFile(files[i]));
		

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
                        currentRoom.tiles = emptyRoom[Mathf.FloorToInt(emptyRoom.Count * Random.value)] as TileType[,];
                        break;
                }
            }
        }

    }


	public void DrawLevel()
	{
		wallContainer = new GameObject("WallContainer");
		wallContainer.transform.parent = transform;

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

		// Draw Wall 
		// new Vector3 (w * 16 + 8 , -h * 16 -8 , 0)
		for (int h = 0; h < roomGridSize.y; h++)
		{
			for (int w = 0; w < roomGridSize.x; w++)
			{
				// Vertical Wall for right and left side of room, not including room along the eage.
				var currentRoom = roomGrid.GetRoom(w, h);
				if(currentRoom.exits == ExitType.None)
				{
					if(w + 1 < roomGridSize.x )
					{
						var nextRoom = roomGrid.GetRoom(w + 1, h);
						if(nextRoom.exits != ExitType.None)
						{
							DrawVerticalWall((w + 1) * 16 - 1, -h * 16 + 1);
						}
					}
				}
				else
				{
					if(w + 1 < roomGridSize.x )
					{
						var nextRoom = roomGrid.GetRoom(w + 1, h);
						if(nextRoom.exits == ExitType.None)
						{
							DrawVerticalWall((w + 1) * 16, -h * 16 + 1);
						}
					}
				}

				// Horizontal Wall for up and down side of room, not including room along the eage.
				if(currentRoom.exits == ExitType.None)
				{
					if(h + 1 < roomGridSize.y )
					{
						var nextRoom = roomGrid.GetRoom(w, h + 1);
						if(nextRoom.exits != ExitType.None)
						{
							DrawHorizontalWall(w * 16, (-h - 1) * 16 + 1);

						}
					}
				}
				else
				{
					if(h + 1 < roomGridSize.y )
					{
						var nextRoom = roomGrid.GetRoom(w, h + 1);
						if(nextRoom.exits == ExitType.None)
						{
							DrawHorizontalWall(w * 16, (-h - 1) * 16);
						}
					}
				}

				// Edge Up
				if(h == 0 && currentRoom.exits != ExitType.None)
				{
					DrawHorizontalWall(w * 16 , -h * 16 + 1);
				}

				// Edge down
				if((h + 1) == roomGridSize.y && currentRoom.exits != ExitType.None)
				{
					DrawHorizontalWall(w * 16 , (-h - 1) * 16);
				}

				// Edge left
				if(w == 0 && currentRoom.exits != ExitType.None)
				{
					DrawVerticalWall(w * 16 - 1, -h * 16 + 1);
				}

				// Edge right
				if((w + 1) == roomGridSize.x && currentRoom.exits != ExitType.None)
				{
					DrawVerticalWall((w + 1) * 16, -h * 16 + 1);
				}
			}
		}
	}

	public void DrawHorizontalWall(int x, int y)
	{
		for(int i=x; i<x+16; i++)
		{
			GameObject wall = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Tiles/wall", typeof(GameObject)))  as GameObject;
			wall.transform.parent = wallContainer.transform;
			wall.transform.localPosition = new Vector3(i, y, 1.0f);
		}


	}

	public void DrawVerticalWall(int x, int y)
	{
		for(int i=y; i>y-18; i--)
		{
			GameObject wall = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Tiles/wall", typeof(GameObject)))  as GameObject;
			wall.transform.parent = wallContainer.transform;
			wall.transform.localPosition = new Vector3(x, i, 1.0f);
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
