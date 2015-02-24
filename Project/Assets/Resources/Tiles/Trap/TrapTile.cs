using UnityEngine;
using System.Collections;

public class TrapTile : Tile {
    public bool isActive;

    private Sprite on;
    private Sprite off;
    private Light light;

	void Start () 
    {
        audio.mute = true;
        particleSystem.Stop();

        light = transform.GetChild(0).GetComponent<Light>();
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
                light.enabled = true;
            }
        }
        else
        {
            if (!audio.mute)
            {
                audio.mute = true;
                particleSystem.Stop();
                GetComponent<SpriteRenderer>().sprite = off;
                light.enabled = false;
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
                other.GetComponent<PlayerController>().KillPlayer();
            }
            else if (other.tag == "PlayerGibs")
            {
                if (Random.value <= 0.05f)
                {
                    other.rigidbody2D.AddForce((new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y)).normalized * 100.0f);
                }
            }
        }
    }
}
