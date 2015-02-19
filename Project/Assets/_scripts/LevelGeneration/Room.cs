using UnityEngine;
using System.Collections;

public enum ExitType { LR, LRT, LRB, LRTB, None };

public class Room {
    public bool isSolutionPath = false;
    public bool isExit = false;
    public bool isStart = false; 
    public ExitType exits;
    public TileType[,] tiles = new TileType[16, 16];

    private TilePrefabManager prefabs;

    //switch specific parameters
    private Point switchLocation;
    private bool hasSwitch;
    public Switch switchParams;

    public Room(ExitType exits, TilePrefabManager prefabsLink)
    {
        this.exits = exits;
        prefabs = prefabsLink;
    }

    public void DrawRoom(GameObject room)
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                // positions in array are translated to positions in worldspace as array[x,y] = world[x,-y]
                //var tilePosition = new Point(tiles.GetLength(0) + x, - (tiles.GetLength(1) + y));

                // just place solid tiles for the moment
                if (tiles[x, y] == TileType.Solid)
                {
                    GameObject currentTile = (GameObject) MonoBehaviour.Instantiate(prefabs.tileGameObjects[tiles[x, y]] as GameObject);
                    currentTile.transform.parent = room.transform;
                    currentTile.transform.localPosition = new Vector3(x, -y, 0.0f);

                }
            }
        }
    }

    public void AddSwitch() 
    {
        hasSwitch = true;

        //TODO implement code for drawing switch in room
    }
}
