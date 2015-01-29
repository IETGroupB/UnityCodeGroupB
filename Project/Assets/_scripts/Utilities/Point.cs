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

    public Point(Point p)
    {
        this.x = p.x;
        this.y = p.y;
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType().Equals(this.GetType()))
        {
            Point p = (Point)obj;
            return (p.x == this.x && p.y == this.y);
        }
        else
            return false;
    }
}
