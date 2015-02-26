using UnityEngine;
using System.Collections;

public class RoomBackgrounds {
    public static Sprite[] sprites = new Sprite[] {
        Resources.Load<Sprite>("RoomBacks/wall"),
        Resources.Load<Sprite>("RoomBacks/window"),
    };

    public static GameObject RoomBackground()
    {
        var bg = new GameObject();
        bg.AddComponent<SpriteRenderer>();

        var sr = bg.GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("RoomBackgrounds/window.png");

        return bg;
    }
}
