using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Reference: Dani Krossing: PARALLAX AND INFINITE BACKGROUND IN UNITY: https://www.youtube.com/watch?v=TccZzs1kJQM&t=1010s
 */

/*
 * Script to apply a parallax effect to the background 
 */
public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallax;

    private float startPos;
    private GameObject cam;    
    
    void Start()
    {
        cam = GameObject.Find("CM_Cam");
        startPos = transform.position.x;
    }

    void Update()
    {
        // Only apply the parallax on the x-axis
        float dist = cam.transform.position.x * parallax;
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
