using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TileType { Solid, Empty };

public class Tile : MonoBehaviour {
    public static Dictionary<TileType, string> prefabs = new Dictionary<TileType, string>() {    
        {TileType.Solid, "solid_tile"},
    };

    public TileType type;
}
