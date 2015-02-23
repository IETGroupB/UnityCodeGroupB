using UnityEngine;
using System.Collections;

public class TrapTile : Tile {
    public bool isActive;

    private Sprite on;
    private Sprite off;

	void Start () 
    {
        audio.mute = true;
        particleSystem.Stop();
        
        on = Resources.Load<Sprite>("Tiles/Trap/trap_on");
        off = Resources.Load<Sprite>("Tiles/Trap/trap_off");

        GetComponent<SpriteRenderer>().sprite = off;
	}

    void FixedUpdate()
    {
        if (isActive)
        {
            if (audio.mute)
            {
                audio.mute = false;
                particleSystem.Play();
                GetComponent<SpriteRenderer>().sprite = on;
            }
        }
        else
        {
            if (!audio.mute)
            {
                audio.mute = true;
                particleSystem.Stop();
                GetComponent<SpriteRenderer>().sprite = off; 
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (isActive)
        {
            GameObject other = coll.transform.gameObject;
            if (other.name == "Character")
            {
                // kill player
                Debug.DrawLine(transform.position, other.transform.position);
            }
        }
    }
}
