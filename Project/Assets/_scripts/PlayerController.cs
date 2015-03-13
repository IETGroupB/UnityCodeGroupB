using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
    public float energy;
    public float energyDrainRate;
    private string guiText;
    private Text energyText;
    private float chargeEnergy = 0.0f;
    private float drainEnergy = 0.0f;
    private static float chargeRate = 20.0f;
    private static float maxEnergy = 105.0f;
    public float textFadeRate;
	public Texture2D fgImage;
	public Texture2D bgImage;
	public float energyX;
	public float energyY;

    public Color ambientDark;
    public Color ambientDim;
    public Color ambientBright;
    public float ColourFadeRate;

	public float maxSpeed = 1f;
	public float jumpForce = 700f;
    public bool alive = true;

    public bool Fire2Down = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public GameObject body;
	private AudioSource[] playerSound;
	private AudioSource jumpSound;
	private AudioSource movementSound;

    private bool facingRight = true;
	private bool grounded = false;
    private float groundRadius = 0.2f;
    private GameObject[] parts;
    private SpriteRenderer[] flipParts;
    private RoomGrid roomGrid;    
    
    private Color ambientLight;
    public float headlightIntensityDark;
    public float headlightEnergyDrain;
    public float headlightIntensityDim;
    private GameObject headlight;

    void Awake()
    {
        parts = new GameObject[3];
        parts[0] = transform.FindChild("Head").gameObject;
        parts[1] = transform.FindChild("Body").gameObject;
       	parts[2] = transform.FindChild("Wheel").gameObject;

        flipParts = new SpriteRenderer[2];
        flipParts[0] = transform.FindChild("HeadFlip").gameObject.GetComponent<SpriteRenderer>();
        flipParts[1] = transform.FindChild("BodyFlip").gameObject.GetComponent<SpriteRenderer>();

        headlight = parts[0].transform.FindChild("Spotlight").gameObject;

        body = parts[1];
        ambientLight = new Color(0.0f, 0.0f, 0.0f);

        energyText = GameObject.Find("Energy").GetComponent<Text>(); 
		playerSound = GetComponents<AudioSource> ();
		jumpSound = playerSound [0];
		movementSound = playerSound [1];
    }

    void Start()
    {
        roomGrid = GameObject.Find("LevelGeneration").GetComponent<LevelGenerator>().roomGrid;
        energy = maxEnergy;
        guiText = energyText.text;
    }

	void FixedUpdate () 
	{
        if (!alive) return;

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);

		var move = Input.GetAxis ("Horizontal");

		GetComponent<Rigidbody2D>().velocity = new Vector2 (move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		if(move > 0 && !facingRight)
		{
			Flip();
		}
		else if (move < 0 && facingRight)
		{
			Flip();
		}


        // don't drain energy if charging
        if (chargeEnergy <= 0.0f)
            energy -= Time.deltaTime * energyDrainRate;

        // drain energy after being attacked
        if (drainEnergy > 0.0f)
        {
            var drainAmount = chargeRate * Time.deltaTime;

            if (drainAmount > drainEnergy) drainAmount = drainEnergy;
            drainEnergy -= drainAmount;
            energy -= drainAmount;
        }

        // increase energy after a charge
        if (chargeEnergy > 0.0f)
        {
            var chargeAmount = chargeRate * Time.deltaTime;

            if (chargeAmount > chargeEnergy) chargeAmount = chargeEnergy;

            chargeEnergy -= chargeAmount;
            energy += chargeAmount;

            // add drain delay if 100% max energy is reached
            if (energy > 100.0f) energy = maxEnergy;
        }
        
        string energyToString;

        if( energy >= 100.0f - float.Epsilon)
            energyToString = "100";
        // catch edge case
        else if (energy >= 99.0f)
            energyToString = " 99";
        else if(energy >= 10.0f)
            energyToString = " " + energy.ToString("00");
        else if (energy > 0.0f)
            energyToString = "  " + energy.ToString("0");
        else
        {
            energyToString = "  0";
            KillPlayer();
        }

        energyText.text = string.Format(guiText, energyToString);
	}

	void Update ()
    {
        if (!alive)
        {
            if(energyText.color.a > 0.0f)
                energyText.color = new Color(energyText.color.r, energyText.color.g, energyText.color.b, energyText.color.a - textFadeRate * Time.deltaTime);

            return;
        }

		if (grounded && Input.GetButtonDown("Fire1"))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
			jumpSound.Play ();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Fire2Down = true;
        }
        else
        {
            Fire2Down = false;
        }

        //do light fade
        {
            //switch on light state of closest room
            switch (roomGrid.GetRoom(roomGrid.GetClosestRoom(new Vector2(transform.position.x, transform.position.y))).lightState)
            {
                case LightingType.Dark:
                    ambientLight = new Color(
                        Mathf.Lerp(ambientLight.r, ambientDark.r, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.g, ambientDark.g, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.b, ambientDark.b, Time.deltaTime * ColourFadeRate));

                    headlight.GetComponent<Light>().intensity = Mathf.Lerp(headlight.GetComponent<Light>().intensity, headlightIntensityDark, Time.deltaTime * ColourFadeRate);
                    break;
                    
                case LightingType.Dim:
                    ambientLight = new Color(
                        Mathf.Lerp(ambientLight.r, ambientDim.r, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.g, ambientDim.g, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.b, ambientDim.b, Time.deltaTime * ColourFadeRate));

                    headlight.GetComponent<Light>().intensity = Mathf.Lerp(headlight.GetComponent<Light>().intensity, headlightIntensityDim, Time.deltaTime * ColourFadeRate);
                    break;
                    
                case LightingType.Bright:
                    ambientLight = new Color(
                        Mathf.Lerp(ambientLight.r, ambientBright.r, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.g, ambientBright.g, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.b, ambientBright.b, Time.deltaTime * ColourFadeRate));


                    headlight.GetComponent<Light>().intensity = Mathf.Lerp(headlight.GetComponent<Light>().intensity, 0.0f, Time.deltaTime * 1.5f);
                    break;
            }

            RenderSettings.ambientLight = ambientLight;
        }
    }

    public void DrainEnergy(float amount)
    {
        drainEnergy += amount;
    }

    public void ChargePlayer(float amount)
    {
        if (energy + amount + chargeEnergy > maxEnergy)
            amount -= energy + amount + chargeEnergy - maxEnergy;

        chargeEnergy += amount;
    }

    public void KillPlayer()
    {
        if (!facingRight) Flip();

        Destroy(GetComponent<CircleCollider2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());

        for (var i = 0; i < parts.Length; i++)
        {
            parts[i].GetComponent<Collider2D>().enabled = true;
            parts[i].AddComponent<Rigidbody2D>();
        }
        
        alive = false;
    }

    void Flip()
	{
		facingRight = !facingRight;
		var theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
        headlight.transform.rotation = Quaternion.Euler(new Vector3(0.0f, -headlight.transform.rotation.eulerAngles.y, 0.0f));

        flipParts[0].enabled = !flipParts[0].enabled;
        flipParts[1].enabled = !flipParts[1].enabled;
        parts[2].transform.localScale = new Vector3(-parts[2].transform.localScale.x, parts[2].transform.localScale.y, parts[2].transform.localScale.z);
	}

	void OnGUI(){
		if (alive == true && Door.endLevel == false) {
			GUI.BeginGroup (new Rect (energyX, energyY, 256, 32));
			GUI.Box (new Rect (0, 0, 256, 32), bgImage);
			GUI.BeginGroup (new Rect (0, 0, (energy / 100) * 256, 32));
			GUI.Box (new Rect (0, 0, 256, 32), fgImage);
			GUI.EndGroup ();
			GUI.EndGroup ();
		}
	}
}