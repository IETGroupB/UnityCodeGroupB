using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//16
public enum TileType {Solid, Empty, Switch};

public class Tile : MonoBehaviour {
    public static Dictionary<TileType, string> prefabs = new Dictionary<TileType, string>() {    
        {TileType.Solid, "Tiles/solid_tile"},
        {TileType.Switch, "Switch/switch"},
    };

    public TileType type;
}
