using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// https://www.youtube.com/watch?v=8aSzWGKiDAM
public class MovingPlatform : MonoBehaviour
{
    public Transform pos1, pos2;

    [SerializeField] Transform startPos;
    Vector3 nextPos;
    [SerializeField] float speed;

    void Start()
    {
        //startPos = transform;
        nextPos = startPos.position;
        transform.position = startPos.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (Vector3.Distance(transform.position, pos1.position) <= 0.1f)
        {
            //print("next pos is pos2");
            nextPos = pos2.position;
        }
        else if (Vector3.Distance(transform.position, pos2.position) <= 0.1f)
        {
            //print("next pos is pos1");
            nextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

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
