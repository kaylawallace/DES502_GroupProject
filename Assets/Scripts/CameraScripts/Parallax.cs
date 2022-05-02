using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallax;

    private float startPos;
    private GameObject cam;    
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("CM_Cam");
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = cam.transform.position.x * parallax;
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
