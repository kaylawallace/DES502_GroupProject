using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script to handle the behaviour of the patroller enemies 
 */
public class PatrollerEnemy : MonoBehaviour
{
    public Transform pos1, pos2;

    [SerializeField] private Transform startPos;
    [SerializeField] private float speed;

    private Vector3 nextPos;

    void Start()
    {
        nextPos = startPos.position;
    }

    void Update()
    {
        Patrol();
        Flip();
    }

    /*
     * Method to handle the patrolling of enemies between two positions 
     */
    void Patrol()
    {
        if (transform.position == pos1.position)
        {
            nextPos = pos2.position;
        }
        else if (transform.position == pos2.position)
        {
            nextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    /*
     * Method to handle flipping the enemy based on the direction it is moving 
     */
    void Flip() 
    {
        if (nextPos == pos1.position)
        {
            // Moving left 
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (nextPos == pos2.position)
        {
            // Moving right 
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }
}
