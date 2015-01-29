using UnityEngine;
using System.Collections;

public class testRoomGeneration : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var rg = new RoomGrid();

        Debug.Log(rg.ToString());

        var points = rg.getSolutionPath();
        foreach (Point p in points)
        {
            Debug.Log(p);
        }

        var a = new Point(0, 0);
        var b = new Point(2, 1);
        var c = new Point(2, 1);

        Debug.Log("Equals works? " + b.Equals(c));
        Debug.Log("Not equals works? " + !a.Equals(b));
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
