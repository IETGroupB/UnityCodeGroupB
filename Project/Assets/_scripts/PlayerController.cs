using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    public bool Fire2Down = false;

	public float maxSpeed = 1f;
	bool facingRight = true;
	public float jumpForce = 700f;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

    public void KillPlayer()
    {

    }

	void FixedUpdate () 
	{
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

	void Flip()
	{
		facingRight = !facingRight;
		var theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}