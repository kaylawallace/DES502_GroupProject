using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * REFERENCE: Xlaugts: Unity 2D Moving Platform Tutorial: https://www.youtube.com/watch?v=8aSzWGKiDAM
 */

/*
 * Script to handle the behaviour of the moving platforms
 */
public class MovingPlatform : MonoBehaviour
{
    public Transform pos1, pos2;

    [SerializeField] private Transform startPos;   
    [SerializeField] private float speed;

    private Vector3 nextPos;

    void Start()
    {
        nextPos = startPos.position;
        transform.position = startPos.position;
    }

    void FixedUpdate()
    {
        MovePlatform();
    }

    /*
     * Method to handle moving the platforms between two points in the scene 
     */
    void MovePlatform()
    {
        if (Vector3.Distance(transform.position, pos1.position) <= 0.1f)
        {
            nextPos = pos2.position;
        }
        else if (Vector3.Distance(transform.position, pos2.position) <= 0.1f)
        {
            nextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    /*
     * Two methods below handle the parenting/deparenting of the player to the platforms 
     * This ensures that they move with the platforms when on them 
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
