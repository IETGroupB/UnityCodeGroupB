using UnityEngine;
using System.Collections;

public class TilePrefabManager : MonoBehaviour {
    public Hashtable tileGameObjects { get; private set; }
    
    void Awake()
    {
        tileGameObjects = new Hashtable();
        // load tile prefabs
        foreach (var tileType in Tile.prefabs.Keys)
        {
            var currentTile = Resources.Load<GameObject>(Tile.prefabs[tileType]) as GameObject;
            currentTile.GetComponent<Tile>().type = tileType;
            tileGameObjects.Add(tileType, currentTile);
        }
    }
}
