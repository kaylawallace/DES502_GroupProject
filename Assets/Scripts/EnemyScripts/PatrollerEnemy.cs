using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerEnemy : MonoBehaviour
{
    public Transform pos1, pos2;

    [SerializeField] Transform startPos;
    Vector3 nextPos;
    [SerializeField] float speed;


    // Start is called before the first frame update
    void Start()
    {
        nextPos = startPos.position;
    }

    void Update()
    {
        Patrol();
    }

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
}
