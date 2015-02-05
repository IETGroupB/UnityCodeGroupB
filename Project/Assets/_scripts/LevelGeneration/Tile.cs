using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//16
public enum TileType { Solid, Up, Down, Left, Right, 
					LeftUp, UpRight, RightDown, DownLeft, 
					UpDonw, LeftRight, 
					UpDownLeft, UpDownRight, LeftRightUp, LeftRightDown, Empty };

public class Tile : MonoBehaviour {
    public static Dictionary<TileType, string> prefabs = new Dictionary<TileType, string>() {    
        {TileType.Solid, "solid_tile"},
    };

    public TileType type;
}
