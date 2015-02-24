using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    

	public float maxSpeed = 1f;
	public float jumpForce = 700f;
    public bool alive = true;

    public bool Fire2Down = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    private bool facingRight = true;
	private bool grounded = false;
    private float groundRadius = 0.2f;
    private GameObject[] parts;

    void Awake()
    {
        parts = new GameObject[3];
        parts[0] = transform.FindChild("Head").gameObject;
        parts[1] = transform.FindChild("Body").gameObject;
        parts[2] = transform.FindChild("Wheel").gameObject;
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

		if(grounded && Input.GetButtonDown("Fire1"))
		{
			rigidbody2D.AddForce(new Vector2(0,jumpForce));
		}

        if(Input.GetButtonDown("Fire2"))
        {
            Fire2Down = true;
        }
        else
        {
            Fire2Down = false;
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