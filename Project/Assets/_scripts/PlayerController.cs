using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
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

    private Color ambientLight;

    void Awake()
    {
        parts = new GameObject[3];
        parts[0] = transform.FindChild("Head").gameObject;
        parts[1] = transform.FindChild("Body").gameObject;
       	parts[2] = transform.FindChild("Wheel").gameObject;

        body = parts[1];
        ambientLight = new Color(0.0f, 0.0f, 0.0f);
    }

    void Start()
    {
        roomGrid = GameObject.Find("LevelGeneration").GetComponent<LevelGenerator>().roomGrid;
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
	}

	void Update ()
    {
        if (!alive) return;

        if (grounded && Input.GetButtonDown("Fire1"))
        {
            rigidbody2D.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Fire2Down = true;
        }
        else
        {
            Fire2Down = false;
        }

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