using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
    GameObject levelContainer;
    RoomGrid currentRoomGrid;
    Point currentRoomGridSize;

    ArrayList lrRooms = new ArrayList();
    ArrayList lrtRooms = new ArrayList();
    ArrayList lrbRooms = new ArrayList();
    ArrayList lrtbRooms = new ArrayList();
    ArrayList emptyRoom = new ArrayList();

    private string tilePrefabsFolder = "Tiles/";
    public Hashtable tileGameObjects = new Hashtable();



    // should this be Start() or Awake()?
    public LevelGenerator()
    {
        
        // load tile prefabs
        foreach (Tile.TileType tileType in Tile.tilePrefabs.Keys)
        {
            Debug.Log(tilePrefabsFolder + Tile.tilePrefabs[tileType]);
            tileGameObjects.Add(tileType, Resources.Load(tilePrefabsFolder + Tile.tilePrefabs[tileType]));
        }

        // file reading goes here
        // store each room as 16x16 TyleTypes
        {
            // temp add films
            lrRooms.Add(
                new Tile.TileType[,] {
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid}
            }
                );
            lrtRooms.Add(
                new Tile.TileType[,] {
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
            }
                );
            lrbRooms.Add(
               new Tile.TileType[,] {
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid}
            }
                );
            lrtbRooms.Add(
                 new Tile.TileType[,] {
               {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Solid, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Solid},
            }
                );
            emptyRoom.Add(
                new Tile.TileType[,] {
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty},
                {Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty, Tile.TileType.Empty}
            }
                );
        } // end temporary hardcoded room types
    }

    public void GenerateLevel(int width, int height)
    {
        currentRoomGridSize = new Point(width, height);
        currentRoomGrid = new RoomGrid(width, height);

        levelContainer = new GameObject("LevelContainer");
        levelContainer.transform.SetParent(transform);

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Room currentRoom = currentRoomGrid.GetRoom(w, h);

                // assign tiles
                switch (currentRoom.exits)
                {
                    case Room.ExitType.LR:
                        currentRoom.tiles = lrRooms[Mathf.FloorToInt(lrRooms.Count * Random.value)] as Tile.TileType[,];
                        break;
                    case Room.ExitType.LRT:
                        currentRoom.tiles = lrtRooms[Mathf.FloorToInt(lrtRooms.Count * Random.value)] as Tile.TileType[,];
                        break;
                    case Room.ExitType.LRB:
                        currentRoom.tiles = lrbRooms[Mathf.FloorToInt(lrbRooms.Count * Random.value)] as Tile.TileType[,];
                        break;
                    case Room.ExitType.LRTB:
                        currentRoom.tiles = lrtbRooms[Mathf.FloorToInt(lrtbRooms.Count * Random.value)] as Tile.TileType[,];
                        break;
                    case Room.ExitType.None:
                        currentRoom.tiles = emptyRoom[Mathf.FloorToInt(lrtbRooms.Count * Random.value)] as Tile.TileType[,];
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
                room.transform.SetParent(levelContainer.transform);

                for (int x = 0; x < currentRoom.tiles.GetLength(0); x++)
                {
                    for (int y = 0; y < currentRoom.tiles.GetLength(1); y++)
                    {
                        // positions in array are translated to positions in worldspace as array[x,y] = world[x,-y]
                        var position = new Point(w * currentRoom.tiles.GetLength(0) + x, -(h * currentRoom.tiles.GetLength(1) + y));

                        // check that it is not empty space
                        if (currentRoom.tiles[x, y] != Tile.TileType.Empty)
                        {
                            GameObject thisTile = (GameObject)Instantiate(tileGameObjects[currentRoom.tiles[x, y]] as GameObject);
                            thisTile.transform.position = new Vector3(position.x, position.y, 0.0f);
                            thisTile.transform.SetParent(room.transform);
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
        //Delete all tiles/lights/etc here
    }
}
