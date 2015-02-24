using UnityEngine;
using System.Collections;

public class TrapTile : Tile {
    public bool isActive;
    public int boltSegmentCount;
    public float shockDuration;
    public float shockForce;

    private Sprite on;
    private Sprite off;
    private Light light;
    
    private Sprite bolt;
    private GameObject[] segments;
    private bool isShocking = false;
    private GameObject shockTarget;
    private float shockTime;
    private AudioSource hum;
    private AudioSource zap;

	void Awake ()
    {
        
        particleSystem.Stop();
        light = transform.GetChild(0).GetComponent<Light>();

        hum = audio;
        hum.mute = true;
        zap = transform.GetChild(0).GetComponent<AudioSource>();

        on = Resources.Load<Sprite>("Tiles/Trap/trap_on");
        off = Resources.Load<Sprite>("Tiles/Trap/trap_off");

        GetComponent<SpriteRenderer>().sprite = off;

        // electricity bolt setup
        {
            bolt = Resources.Load<Sprite>("Tiles/Trap/bolt");
            segments = new GameObject[boltSegmentCount];
            for (var i = 0; i < boltSegmentCount; i++)
            {
                segments[i] = new GameObject();
                segments[i].AddComponent<SpriteRenderer>();
                segments[i].GetComponent<SpriteRenderer>().sprite = bolt;
            }
        }
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            if (hum.mute)
            {
                hum.mute = false;
                particleSystem.Play();
                GetComponent<SpriteRenderer>().sprite = on;
                light.enabled = true;
            }
        }
        else
        {
            if (!hum.mute)
            {
                hum.mute = true;
                particleSystem.Stop();
                GetComponent<SpriteRenderer>().sprite = off;
                light.enabled = false;
            }
        }
    }

    void Update()
    {
        if (isShocking)
        {
            DrawBolt(new Vector2(shockTarget.transform.position.x - transform.position.x, shockTarget.transform.position.y - transform.position.y));

            shockTime += Time.deltaTime;
            if (shockTime > shockDuration)
            {
                shockTime = 0.0f;
                isShocking = false;
                HideBolt();
            }
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (isActive && !isShocking)
        {
            GameObject other = coll.transform.gameObject;
            if (other.name == "Character")
            {
                other.GetComponent<PlayerController>().KillPlayer();

                isShocking = true;
                shockTarget = other.GetComponent<PlayerController>().body;
                ShowBolt();
                zap.Play();
            }
            else if (other.tag == "PlayerGibs")
            {
                if (Random.value <= 0.03f)
                {
                    other.rigidbody2D.AddForce((new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y)).normalized * shockForce);
                    isShocking = true;
                    shockTarget = other;
                    ShowBolt();
                    zap.Play();
                }
            }
        }
    }

    private void HideBolt()
    {
        foreach (GameObject segment in segments)
            segment.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void ShowBolt()
    {
        foreach (GameObject segment in segments)
            segment.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void DrawBolt(Vector2 end)
    {
        var step = end / boltSegmentCount;
        var currentPosition = new Vector2(0.0f, 0.0f);

        for (var i = 0; i < boltSegmentCount - 1; i++)
        {
            var nextPosition = step * i + new Vector2(2 * step.y * Random.value, 2 * step.x * Random.value);
            DrawBoltSegment(currentPosition, nextPosition, i);
            currentPosition = nextPosition;
        }

        DrawBoltSegment(currentPosition, end, boltSegmentCount - 1);
    }

    private void DrawBoltSegment(Vector2 start, Vector2 end, int index)
    {
        var difference = end - start;
        var angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        segments[index].transform.parent = transform;
        segments[index].transform.localPosition = new Vector3(start.x, start.y, -0.5f);
        segments[index].transform.localScale = new Vector3(difference.magnitude, 1.0f, 1.0f);
        segments[index].transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
    }
}
