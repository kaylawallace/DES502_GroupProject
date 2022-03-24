using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBob : MonoBehaviour
{
    [SerializeField] float speed = .8f;
    [SerializeField] float height = 0.0001f;

    void Update()
    {
        float newY = Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, transform.position.y + newY, transform.position.z);
    }
}
