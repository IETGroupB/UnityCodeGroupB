using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//16
public enum TileType { Solid, Empty, Switch, Trap, RoomLight, Exit, CeilingLight};

public class Tile : MonoBehaviour {
    public static Dictionary<TileType, string> prefabs = new Dictionary<TileType, string>() {    
        {TileType.Solid, "Tiles/solid_tile"},
        {TileType.Switch, "Tiles/Switch/switch"},
        {TileType.Trap, "Tiles/Trap/trap"},
        {TileType.CeilingLight, "Tiles/CeilingLight/ceilingLight"},
		//{TileType.Exit, "Tiles/door"},
		//{TileType.RoomLight, "Tiles/RoomLight/roomLight"},
    };

    public TileType type;
}
