using UnityEngine;
using System.Collections;

public class TilePrefabManager : MonoBehaviour {
    public Hashtable tileGameObjects { get; private set; }

    private string tilePrefabsFolder = "Tiles/";
    
    void Start()
    {
        tileGameObjects = new Hashtable();
        // load tile prefabs
        foreach (TileType tileType in Tile.prefabs.Keys)
        {
            GameObject currentTile = Resources.Load(tilePrefabsFolder + Tile.prefabs[tileType]) as GameObject;
            currentTile.GetComponent<Tile>().type = tileType;
            tileGameObjects.Add(tileType, currentTile);
        }
    }
}
