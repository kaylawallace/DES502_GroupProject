using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Camera cam;
    public Transform head, firePos;
    public LayerMask grappleable;
    public Animator anim; 

    private PlayerMovement controller; 
    private Vector2 mousePos, headPos, aimDir;
    private bool grappling; 
    private LineRenderer aim, rope;
    private DistanceJoint2D distJoint;
    private float minLookX = 2f, minLookY = -2f, maxLookY = 4f;
    private bool facingRight;
    [SerializeField] private float reelRate;
    [SerializeField] private GameObject dialogueTriggerBtn;
    private GameObject slobberEffect;

    // Start is called before the first frame update
    void Start()
    {
        grappling = false;
        aim = head.GetComponent<LineRenderer>();
        rope = GetComponent<LineRenderer>();
        distJoint = GetComponent<DistanceJoint2D>();
        controller = GetComponentInParent<PlayerMovement>();
        slobberEffect = GameObject.Find("Spit");
        distJoint.enabled = false;
        rope.enabled = false;
        aim.enabled = true;
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Aim();

        if (transform.rotation.y == 180)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && (!dialogueTriggerBtn.activeSelf || gameObject.GetComponent<Player>().conversing))
        {
            StartGrapple();
            
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopGrapple();
            anim.SetInteger("state", 7);
        }

        if (distJoint.enabled)
        {
            rope.SetPosition(1, firePos.transform.position);
        }

        if (grappling)
        {
            ReelGrapple();
        }
    }

    private void LateUpdate()
    {
        Aim();
        //DrawAim();
        if (Input.GetKey(KeyCode.Mouse1))
        {           
            DrawAim();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopAim();
        }
    }

    void Aim()
    {
        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        headPos = head.position;
        aimDir = mousePos - headPos;

        head.transform.right = aimDir;
    }

    void StopAim()
    {
        aim.enabled = false;
    }


    void DrawAim()
    {
        Ray2D aimRay = new Ray2D(transform.position, aimDir);
        aim.sortingOrder = 50;

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
        RaycastHit2D ray = Physics2D.Raycast(transform.position, aimDir, 20f, grappleable);

        GameObject newSlobber = (Instantiate(slobberEffect, firePos.position, Quaternion.identity));
        newSlobber.transform.parent = transform.parent;

        if (ray)
        {
            if (ray.collider.CompareTag("Grappleable"))
            {
                Vector2 hitPoint = ray.point;
                rope.SetPosition(0, hitPoint);
                rope.SetPosition(1, headPos);
                distJoint.connectedAnchor = hitPoint;

                if (controller.IsGrounded())
                {
                    float dist = Vector2.Distance(transform.position, hitPoint);
                    //print("here");
                    distJoint.distance = dist * .6f;
                }

                distJoint.enabled = true;
                rope.enabled = true;
                grappling = true;
                controller.isSwinging(grappling);
                anim.SetTrigger("swing");
            }
            else
            {
                return;
            }
        } 
        else
        {
            return;
        }
    }

    void StopGrapple()
    {
        distJoint.enabled = false;
        rope.enabled = false;
        grappling = false;
        controller.isSwinging(grappling);
    }

    void ReelGrapple()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            distJoint.distance -= reelRate * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            distJoint.distance += reelRate * Time.deltaTime;
        }
    }
}
