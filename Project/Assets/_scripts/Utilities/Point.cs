using UnityEngine;
using System.Collections;

/**
 * Simple 2D point class for discrete locations (useful for grids, etc where you don't want float based Vector2)
 */
public class Point {
    public int x { get; set; }
    public int y { get; set; }
    
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
