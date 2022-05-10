using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * REFERENCE: Unity Forum: https://answers.unity.com/questions/781748/using-mathfsin-to-move-an-object.html
 */

/*
 * Script to handle the bobbing movement of the buffs in the scene 
 */
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
