using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed, jumpForce, swingForce;
    public Transform feetPos;
    public float checkRadius;

    private Rigidbody2D rb;
    private float movInput;
    private bool grounded, jumping, swinging;
    [SerializeField] private LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movInput = Input.GetAxis("Horizontal");
        Jump();
    }

    private void FixedUpdate()
    {
        Move();       
    }

    private void Move()
    {
        if (isGrounded())
        {
            if (movInput != 0)
            {
                rb.velocity = new Vector2(movInput * speed, rb.velocity.y);

                if (movInput > 0)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (movInput < 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }
            else
            {
                return;
            }
        }
       
        if (swinging)
        {
            if (movInput != 0)
            {
                if (movInput > 0)
                {
                    rb.AddForce(new Vector2(1, 0) * swingForce);
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else if (movInput < 0)
                {
                    rb.AddForce(new Vector2(-1, 0) * swingForce);
                    transform.eulerAngles = new Vector3(0, 0, 0);
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
        if (isGrounded())
        {
            jumping = false;

            if (Input.GetButtonDown("Jump"))
            {
                jumping = true;
                rb.velocity = Vector2.up * jumpForce;
            }
        }
    }

    bool isGrounded()
    {
        grounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        return grounded; 
    }

    public bool isSwinging(bool _swinging)
    {
        if (!isGrounded() && _swinging)
        {
            return swinging = true;
        }
        else
        {
            return swinging = false; 
        }
    }
}
