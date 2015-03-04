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
		for (var h = 0; h < roomGridSize.y; h++)
		{
			for (var w = 0; w < roomGridSize.x; w++)
			{
				var currentRoom = roomGrid.GetRoom(w, h);
				GameObject room = new GameObject("room_" + w + "_" + h);
				room.transform.parent = levelContainer.transform;
				room.transform.localPosition = new Vector3(w * currentRoom.tiles.GetLength(0), -(h * currentRoom.tiles.GetLength(1)), 0.0f);
				
				currentRoom.DrawRoom(room);
			}
		}

        var case0 = Resources.Load<Material>("Tiles/SolidMaterials/all");
        var case2 = Resources.Load<Material>("Tiles/SolidMaterials/case2");
        var case6 = Resources.Load<Material>("Tiles/SolidMaterials/case6");
        var case10 = Resources.Load<Material>("Tiles/SolidMaterials/case10");
        var case14 = Resources.Load<Material>("Tiles/SolidMaterials/case14");
        var case30 = Resources.Load<Material>("Tiles/SolidMaterials/case30");

        // fill in tile 
        for (var h = 0; h < roomGridSize.y; h++)
        {
            for (var w = 0; w < roomGridSize.x; w++)
            {
                // check that room is not empty
                if (roomGrid.GetRoom(w, h).exits == ExitType.None) continue;

                for (var x = 0; x < 16; x++)
                {
                    for (var y = 0; y < 16; y++)
                    {
                        // make sure tile is a solid tile
                        if(roomGrid.GetRoom(w, h).tiles[x, y] != TileType.Solid) continue;

                        var action = 0;

                        // check left
                        if (x > 0)
                        {
                            if (roomGrid.GetRoom(w, h).tiles[x - 1, y] == TileType.Solid) action += 2;
                        }
                        else
                        {
                            // boundry condition
                            if (w == 0)
                            {
                                action += 2;
                            }
                            else if (roomGrid.GetRoom(w - 1, h).exits == ExitType.None)
                            {
                                action += 2;
                            }
                            else if (roomGrid.GetRoom(w - 1, h).tiles[15, y] == TileType.Solid)
                            {
                                action += 2;
                            }
                        }

                        // check right
                        if (x < 15)
                        {
                            if (roomGrid.GetRoom(w, h).tiles[x + 1, y] == TileType.Solid) action += 4;
                        }
                        else
                        {
                            // boundry condition
                            if (w == roomGrid.width - 1)
                            {
                                action += 4;
                            }
                            else if (roomGrid.GetRoom(w + 1, h).exits == ExitType.None)
                            {
                                action += 4;
                            }
                            else if (roomGrid.GetRoom(w + 1, h).tiles[0, y] == TileType.Solid)
                            {
                                action += 4;
                            }
                        }

                        // check up
                        if (y > 0)
                        {
                            if (roomGrid.GetRoom(w, h).tiles[x, y - 1] == TileType.Solid) action += 8;
                        }
                        else
                        {
                            // boundry condition
                            if (h == 0)
                            {
                                action += 8;
                            }
                            else if (roomGrid.GetRoom(w, h - 1).exits == ExitType.None)
                            {
                                action += 8;
                            }
                            else if (roomGrid.GetRoom(w, h - 1).tiles[x, 15] == TileType.Solid)
                            {
                                action += 8;
                            }
                        }

                        // check down
                        if (y < 15)
                        {
                            if (roomGrid.GetRoom(w, h).tiles[x, y + 1] == TileType.Solid) action += 16;
                        }
                        else
                        {
                            // boundry condition
                            if (h == roomGrid.height - 1)
                            {
                                action += 16;
                            }
                            else if (roomGrid.GetRoom(w, h + 1).exits == ExitType.None)
                            {
                                action += 16;
                            }
                            else if (roomGrid.GetRoom(w, h + 1).tiles[x, 0] == TileType.Solid)
                            {
                                action += 16;
                            }
                        }

                        

                        switch(action){
                            case 0: // all sides lit
                                // **UNIQUE**
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case0;
                                break;
                            case 2: // rtb
                                // **UNIQUE**
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case2;
                                break;
                            case 4: // ltb
                                // case 2 rotated 180 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case2;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
                                break;
                            case 6: // tb 
                                // **UNIQUE**
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case6;
                                break;
                            case 8: // lrb
                                // case 2 rotated 90 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case2;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
                                break;
                            case 10: // rb
                                // **UNIQUE**
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case10;
                                break;
                            case 12: // lb
                                // case 10 rotated 270 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case10;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 270.0f));
                                break;
                            case 14: // b
                                // **UNIQUE**
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case14;
                                break;
                            case 16: // lrt
                                // case 2 rotated 270 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case2;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 270.0f));
                                break;
                            case 18: // rt
                                // case 10 rotated 90 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case10;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
                                break;
                            case 20: // lt
                                // case 10 rotated 180 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case10;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
                                break;
                            case 22: // t
                                // case 14 rotated 180 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case14;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
                                break;
                            case 24: // lr
                                // case 6 rotated 90 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case6;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
                                break;
                            case 26:// r
                                // case 14 rotated 90 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case14;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
                                break;
                            case 28: // l
                                // case 14 rotated 270 degrees
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case14;
                                roomGrid.GetRoom(w, h).solidTiles[x, y].transform.Rotate(new Vector3(0.0f, 0.0f, 270.0f));
                                break;
                            case 30: // none
                                // **UNIQUE** 
                                roomGrid.GetRoom(w, h).solidTiles[x, y].GetComponent<SpriteRenderer>().material = case30;
                                break;

                        }

                    }
                }
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
            wall.transform.localPosition = new Vector3(i, y, 0.002f);
		}


	}

	public void DrawVerticalWall(int x, int y)
	{
		for(int i=y; i>y-18; i--)
		{
			GameObject wall = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Tiles/wall", typeof(GameObject)))  as GameObject;
			wall.transform.parent = wallContainer.transform;
            wall.transform.localPosition = new Vector3(x, i, 0.002f);
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
