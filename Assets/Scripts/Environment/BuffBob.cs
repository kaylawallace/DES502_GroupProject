using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBob : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float height;

    void Update()
    {
        float newY = Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(transform.position.x, transform.position.y + newY, transform.position.z);
    }
}
