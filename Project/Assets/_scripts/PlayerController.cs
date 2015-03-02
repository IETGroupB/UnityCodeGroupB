using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
    public string energyFormat;
    public float energy;
    public float energyDrainRate;
    private Text energyText;
    private float chargeEnergy = 0.0f;
    private static float chargeRate = 20.0f;
    private static float maxEnergy = 105.0f;
    // rate at which text fades after death
    public float textFadeRate;

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

    private bool facingRight = true;
	private bool grounded = false;
    private float groundRadius = 0.2f;
    private GameObject[] parts;
    private RoomGrid roomGrid;
	private AudioSource jump;
    private Color ambientLight;

    void Awake()
    {
        parts = new GameObject[3];
        parts[0] = transform.FindChild("Head").gameObject;
        parts[1] = transform.FindChild("Body").gameObject;
       	parts[2] = transform.FindChild("Wheel").gameObject;

        body = parts[1];
        ambientLight = new Color(0.0f, 0.0f, 0.0f);
		jump = transform.GetComponent<AudioSource>();

        energyText = GameObject.Find("Energy").GetComponent<Text>(); 
    }

    void Start()
    {
        roomGrid = GameObject.Find("LevelGeneration").GetComponent<LevelGenerator>().roomGrid;
        energy = maxEnergy;
        energyFormat = energyFormat.Replace("\\n", "\n");
    }

	void FixedUpdate () 
	{
        if (!alive) return;

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);

		var move = Input.GetAxis ("Horizontal");

		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

		if(move > 0 && !facingRight)
		{
			Flip();
		}
		else if (move < 0 && facingRight)
		{
			Flip();
		}


        //drain energy (update to do it based on movement??)
        energy -= Time.deltaTime * energyDrainRate;

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

        energyText.text = string.Format(energyFormat, energyToString);
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
            rigidbody2D.AddForce(new Vector2(0, jumpForce));
			jump.Play ();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Fire2Down = true;
        }
        else
        {
            Fire2Down = false;
        }

        //do ambient light fade
        {
            //switch on light state of closest room
            switch (roomGrid.GetRoom(roomGrid.GetClosestRoom(new Vector2(transform.position.x, transform.position.y))).lightState)
            {
                case LightingType.Dark:
                    ambientLight = new Color(
                        Mathf.Lerp(ambientLight.r, ambientDark.r, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.g, ambientDark.g, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.b, ambientDark.b, Time.deltaTime * ColourFadeRate));
                    break;
                    
                case LightingType.Dim:
                    ambientLight = new Color(
                        Mathf.Lerp(ambientLight.r, ambientDim.r, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.g, ambientDim.g, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.b, ambientDim.b, Time.deltaTime * ColourFadeRate));
                    break;
                    
                case LightingType.Bright:
                    ambientLight = new Color(
                        Mathf.Lerp(ambientLight.r, ambientBright.r, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.g, ambientBright.g, Time.deltaTime * ColourFadeRate),
                        Mathf.Lerp(ambientLight.b, ambientBright.b, Time.deltaTime * ColourFadeRate));
                    break;
            }

            RenderSettings.ambientLight = ambientLight;
        }
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
        Destroy(rigidbody2D);

        for (var i = 0; i < parts.Length; i++)
        {
            parts[i].collider2D.enabled = true;
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
	}
}