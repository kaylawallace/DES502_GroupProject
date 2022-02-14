using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Camera cam;
    public Transform head, firePos;

    private PlayerController controller; 
    private Vector2 mousePos, headPos, aimDir;
    private bool grappling; 
    private LineRenderer aim, rope;
    private DistanceJoint2D distJoint;
    private float minLookX = 2f, minLookY = -2f, maxLookY = 4f;

    // Start is called before the first frame update
    void Start()
    {
        grappling = false;
        aim = head.GetComponent<LineRenderer>();
        rope = GetComponent<LineRenderer>();
        distJoint = GetComponent<DistanceJoint2D>();
        controller = GetComponentInParent<PlayerController>();
        distJoint.enabled = false;
        rope.enabled = false;
        aim.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartGrapple();
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopGrapple();
        }

        if (distJoint.enabled)
        {
            rope.SetPosition(1, transform.position);
        }
    }

    private void LateUpdate()
    {
        DrawAim();
    }

    void Aim()
    {
        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        headPos = head.position;
        aimDir = mousePos - headPos;

        //if (aimDir.y < minLook)
        //{
        //    aimDir.y = minLook;
        //}
        //else if (aimDir.y > maxLook)
        //{
        //    aimDir.y = maxLook;
        //}

        if (aimDir.x < minLookX)
        {
            aimDir.x = minLookX;
        }

        head.transform.right = aimDir;
    }

    void DrawAim()
    {
        Ray2D aimRay = new Ray2D(transform.position, aimDir);

        if (!grappling)
        {
            aim.enabled = true;
            aim.positionCount = 2;
            aim.SetPosition(0, headPos);
        }
        else
        {
            aim.enabled = false; 
        }
        aim.SetPosition(1, aimRay.GetPoint(50));
    }

    void StartGrapple()
    {
        Vector2 mousePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        rope.SetPosition(0, mousePos);
        rope.SetPosition(1, headPos);
        distJoint.connectedAnchor = mousePos;
        distJoint.enabled = true;
        rope.enabled = true;
        grappling = true;
        controller.isSwinging(grappling);
    }

    void StopGrapple()
    {
        distJoint.enabled = false;
        rope.enabled = false;
        grappling = false;
        controller.isSwinging(grappling);
    }
}
