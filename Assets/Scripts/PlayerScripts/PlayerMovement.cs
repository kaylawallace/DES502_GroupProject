using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed, jumpForce, swingForce;
    public Transform feetPos;
    public float checkRadius;
    public float maxKnockbackTime;

    private Rigidbody2D rb;
    private float movInput;
    private float knockbackTime;
    private bool grounded = false, jumping = false, swinging = false, knocked = false, isOnPlatform = false;
    [SerializeField] private LayerMask whatIsGround, whatIsPlatform;
    [SerializeField] private float knockbackForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockbackTime = maxKnockbackTime;
    }

    // Update is called once per frame
    void Update()
    {
        movInput = Input.GetAxis("Horizontal");
        Jump();
    }

    private void FixedUpdate()
    {
        if (!knocked && !GetComponent<Player>().conversing && !swinging)
        {
            Move();
        }
        else if (swinging)
        {
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
        //if (isGrounded())
        //{
            if (movInput != 0)
            {
                rb.velocity = new Vector2(movInput * speed, rb.velocity.y);

                if (movInput < 0)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                }
                else if (movInput > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }
            else
            {
                return;
            }
        //}

       
    }

    public void Swing()
    {
        if (swinging)
        {
            if (movInput != 0)
            {
                if (movInput > 0)
                {
                    rb.AddForce(new Vector2(1, 0) * swingForce);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (movInput < 0)
                {
                    rb.AddForce(new Vector2(-1, 0) * swingForce);
                    transform.eulerAngles = new Vector3(0, 180, 0);
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
        if (IsGrounded())
        {
            jumping = false;

            if (Input.GetButtonDown("Jump"))
            {
                jumping = true;
                rb.velocity = Vector2.up * jumpForce;
            }
        }
    }

    bool IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        return grounded;
    }

    bool IsOnPlatform()
    {
        isOnPlatform = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsPlatform);
        return isOnPlatform;
    }

    public bool isSwinging(bool _swinging)
    {
        if (!IsGrounded() && _swinging)
        {
            return swinging = true;
        }
        else
        {
            return swinging = false;
        }
    }

    //void OnPlatform()
    //{
    //    if (IsOnPlatform())
    //    {
    //        transform.parent =
    //    }S
    //}

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
}
