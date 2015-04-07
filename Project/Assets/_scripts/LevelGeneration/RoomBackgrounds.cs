using UnityEngine;
using System.Collections;

public class RoomBackgrounds : MonoBehaviour {
    private GameObject[] backs;

    void Awake()
    {
        backs = new GameObject[] {
            Resources.Load<GameObject>("RoomBacks/wallPrefab"),
			Resources.Load<GameObject>("RoomBacks/windowPrefab"),
			Resources.Load<GameObject>("RoomBacks/windowPrefab_1"),
			Resources.Load<GameObject>("RoomBacks/windowPrefab_2")
        };
    }

    public GameObject GetRoomBackground()
    {
        return backs[(int) Mathf.Floor(Random.value * backs.Length)];
    }
}
