using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerEnemy : MonoBehaviour
{
    public Transform pos1, pos2;

    [SerializeField] Transform startPos;
    Vector3 nextPos;
    [SerializeField] float speed;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Patrol();
        Flip();
    }

    void Patrol()
    {
        if (transform.position == pos1.position)
        {
            nextPos = pos2.position;
            // transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (transform.position == pos2.position)
        {
            nextPos = pos1.position;
            // transform.eulerAngles = new Vector3(0, -180, 0);
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    void Flip() 
    {
        // print(rb.velocity.x);
        if (nextPos == pos1.position)
        {
            // moving left 
            // print("moving left");
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (nextPos == pos2.position)
        {
            // moving right 
            // print("moving right");
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }
}
