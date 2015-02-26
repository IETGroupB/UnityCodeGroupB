using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    public Vector2 roomGridSize;
    public Vector3 starScale;
    public int starCount;
    public float starSpeed;
    public float zPos;

    private GameObject starPrefab;
    private GameObject[] starRef;

    private const int RoomSize = 16;

	// Use this for initialization
	void Start () {
        starPrefab = Resources.Load<GameObject>("star");
        starRef = new GameObject[starCount];

        for (var i = 0; i < starCount; i++)
        {
            starRef[i] = Instantiate(starPrefab) as GameObject;
            starRef[i].transform.parent = transform;
            starRef[i].transform.localPosition = new Vector3(
                Random.value * (roomGridSize.x * RoomSize + RoomSize * 2.0f) - RoomSize,
                Random.value * -(roomGridSize.y * RoomSize + RoomSize * 2.0f) + RoomSize,
                zPos
                );
            starRef[i].transform.localScale = starScale;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        var move = new Vector3(-starSpeed, 0, 0) * Time.deltaTime;
        for (var i = 0; i < starCount; i++)
        {
            starRef[i].transform.Translate(move);

            if (starRef[i].transform.position.x < -RoomSize)
            {
                starRef[i].transform.position = new Vector3(roomGridSize.x * RoomSize + RoomSize, starRef[i].transform.position.y, starRef[i].transform.position.z);
            }
        }
	}
}
