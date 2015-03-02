using UnityEngine;
using System.Collections;

public class Switch : Tile {
    Light switchIndicator;
    private bool switchActive = false;
	private AudioSource switchSound;

    Color offColour = new Color(1.0f, 0.0f, 0.0f);
    Color onColour = new Color(0.0f, 1.0f, 0.0f);

	void Start () 
    {
        switchIndicator = transform.GetChild(0).GetComponent<Light>();
        switchIndicator.color = offColour;
		switchSound = transform.GetComponent<AudioSource>();
	}

    public bool GetState()
    {
        return switchActive;
    }

        
    void OnTriggerStay2D(Collider2D coll)
    {
        GameObject other = coll.transform.gameObject;
        if (other.name == "Character")
        {
            if (other.GetComponent<PlayerController>().Fire2Down)
            {
                switchIndicator.color = onColour;
                switchActive = true;
				switchSound.Play ();
                // no need to keep checking after switch is activated
                transform.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }
}
