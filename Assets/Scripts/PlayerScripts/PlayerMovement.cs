using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        am = FindObjectOfType<AudioManager>();
        knockbackTime = maxKnockbackTime;
        grassEffect = GameObject.Find("GrassParticleEffect");
    }

    // Update is called once per frame
    void Update()
    {
        movInput = Input.GetAxis("Horizontal");
        if(!swinging) Jump();

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

    private void Move()
    {
        if (movInput != 0)
        {
            rb.velocity = new Vector2(movInput * speed, rb.velocity.y);
            am.Play("WalkSound");

            if (!jumping)
            {
                anim.SetInteger("state", 1);
            }

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
                anim.SetInteger("state", 0);
            }
            return;
        }      
    }

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

    void Jump()
    {           
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {    
            jumping = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetInteger("state", 2);
            am.Play("JumpSound");
        }
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
            anim.SetInteger("state", 3);
        }


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
            GameObject newGrassEffect = (Instantiate(grassEffect, feetPos.position, Quaternion.Euler(-90, 0, 0)));
            justLanded = true;
            Destroy(newGrassEffect, 2f);
        }
    }

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

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public bool IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        return grounded;
    }

    public bool SetIsSwinging(bool _swinging)
    {
        return swinging = _swinging;
    }

    public bool GetIsSwinging()
    {
        return swinging;
    }
}
