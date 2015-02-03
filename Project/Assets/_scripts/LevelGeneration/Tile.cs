using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public enum TileType {Solid, Empty};
    public static Hashtable tilePrefabs = new Hashtable() {
        {TileType.Solid, "solid_tile"},
    };


}
