using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script to handle all player movement
 */
public class PlayerMovement : MonoBehaviour
{
    public float speed, jumpForce, minJumpForce, swingForce;
    public Transform feetPos;
    public float checkRadius;
    public float maxKnockbackTime;
    public Animator anim;

    [SerializeField] private LayerMask whatIsGround, whatIsPlatform;
    [SerializeField] private float knockbackForce;

    private Rigidbody2D rb;
    private AudioManager am;
    private GameObject grassEffect;
    private float movInput;
    private float knockbackTime;
    private bool grounded = false, jumping = false, swinging = false, knocked = false, facingRight = true, justLanded = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        am = FindObjectOfType<AudioManager>();
        knockbackTime = maxKnockbackTime;
        grassEffect = GameObject.Find("GrassParticleEffect");
    }

    void Update()
    {
        movInput = Input.GetAxis("Horizontal");

        // Only allow player to jump if they are not grappling 
        if (!swinging)
        {
            Jump();
        }

        if (swinging || jumping || knocked)
        {
            am.Stop("WalkSound");
        }
    }

    private void FixedUpdate()
    {
        if (!knocked && !GetComponent<Player>().conversing && !swinging)
        {
            Move();
        }
        else if (swinging)
        {
            jumping = false;
            grounded = false;
            Swing();
        }
        else if (knocked)
        {
            // Timer for knockback 
            if (knockbackTime <= 0)
            {
                knocked = false;
                rb.velocity = Vector2.zero;
                knockbackTime = maxKnockbackTime;
            }
            else
            {
                knockbackTime -= Time.deltaTime;
            }
        }
        else
        {
            return;
        }
    }

    /*
     * Method to handle walking of player 
     */
    private void Move()
    {
        if (movInput != 0)
        {
            rb.velocity = new Vector2(movInput * speed, rb.velocity.y);
            am.Play("WalkSound");

            if (!jumping)
            {
                // Trigger jump animation state 
                anim.SetInteger("state", 1);
            }

            // Flip the player depending on their direction of movement
            if (movInput < 0)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingRight = false;
            }
            else if (movInput > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingRight = true;
            }
        }
        else
        {
            am.Stop("WalkSound");
            if (!jumping && IsGrounded())
            {
                // Trigger idle animation state 
                anim.SetInteger("state", 0);
            }
            return;
        }      
    }

    /*
     * Method to handle the force applied to the player when swinging/grappling 
     */
    public void Swing()
    {
        if (swinging)
        {
            am.Stop("WalkSound");
            if (movInput != 0)
            {
                if (movInput > 0)
                {
                    rb.AddForce(new Vector2(1, 0) * swingForce);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    facingRight = true;
                }
                else if (movInput < 0)
                {                  
                    rb.AddForce(new Vector2(-1, 0) * swingForce);
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    facingRight = false;
                }
            }
            else
            {
                return;
            }
        }
    }

    /*
     * Method to handle player jumping and all animation states involved in this (jump, fall, and land states)
     */
    void Jump()
    {          
        // Add initial force when jump is presssed
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {    
            jumping = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetInteger("state", 2);
            am.Play("JumpSound");
        }
        // Allow a higher jump when holding the jump button 
        else if (Input.GetButtonUp("Jump"))
        {
            if (rb.velocity.y > 0)
            {
                jumping = true;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }          
        }
        else if (rb.velocity.y < 0 && !swinging)
        {
            // Set animation state to falling 
            anim.SetInteger("state", 3);
        }

        // !swinging here ensures that these animations won't play while swinging, despite the velocity conditions matching in both situations 
        if (rb.velocity.y > 0 && !IsGrounded() && !swinging)
        {
            // Set animation state to jumping
            anim.SetInteger("state", 2);
            justLanded = false;
        }
        else if (rb.velocity.y < 0 && !IsGrounded() && !swinging)
        {
            // Set animation state to falling
            anim.SetInteger("state", 3);
        }
        else if (IsGrounded() && !justLanded)
        {
            // Set animation state to landing 
            jumping = false;
            anim.SetInteger("state", 4);

            // Instantiate grass particle effect on landing 
            GameObject newGrassEffect = (Instantiate(grassEffect, feetPos.position, Quaternion.Euler(-90, 0, 0)));
            justLanded = true;
            Destroy(newGrassEffect, 2f);
        }
    }

    /*
     * Method to handle knockback force applied to the player when damaged 
     */
    public void Knockback()
    {
        knocked = true;

        if (transform.rotation.eulerAngles.y == 180)
        {
            Vector2 knockbackDir = new Vector2(1, 0);
            rb.velocity = new Vector2(knockbackDir.x * knockbackForce, rb.velocity.y);
        }
        else if (transform.rotation.eulerAngles.y == 0)
        {
            Vector2 knockbackDir = new Vector2(-1, 0);
            rb.velocity = new Vector2(knockbackDir.x * knockbackForce, rb.velocity.y);
        }
    }

    /*
     * Method used by other scripts to determine whether the player is currently facing right 
     * Returns: bool facingRight - true if the player is facing right, false otherwise 
     */
    public bool IsFacingRight()
    {
        return facingRight;
    }

    /*
     * Method used by other scripts to determine whether the player is currently grounded 
     * Returns: bool grounded - true if the player is grounded, false otherwise 
     */
    public bool IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        return grounded;
    }

    /*
     * Method to set the player's swinging property 
     * Params: bool _swinging - true if the player is currently swinging 
     * Returns: bool swinging - the new value of swinging, true if swinging
     */
    public bool SetIsSwinging(bool _swinging)
    {
        return swinging = _swinging;
    }

    /*
     * Method used by other scripts to determine whether the player is currently swinging/grappling
     * Returns: bool swinging/grappling - true if the player is swinging/grappling, false otherwise 
     */
    public bool GetIsSwinging()
    {
        return swinging;
    }
}
