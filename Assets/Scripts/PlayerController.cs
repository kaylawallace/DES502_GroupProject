using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed, jumpForce;
    public Transform feetPos;
    public float checkRadius;

    private Rigidbody2D rb;
    private float velocity, movInput;
    private bool grounded, jumping;
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

    bool isGrounded()
    {
        grounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        return grounded; 
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
}
