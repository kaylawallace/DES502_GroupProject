using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Camera cam;
    public Transform head, firePos;
    public LayerMask grappleable;
    public Animator anim;
    public GameObject headSprite;
    public GameObject tongueAnim;

    [SerializeField] private float reelRate;
    [SerializeField] private GameObject dialogueTriggerBtn;

    private PlayerMovement controller;      
    private LineRenderer aim, rope;
    private DistanceJoint2D distJoint; 
    private GameObject slobberEffect;  
    private AudioManager am;
    private Player plr;
    private Vector2 mousePos, headPos, aimDir;
    private bool grappling;

    // Start is called before the first frame update
    void Start()
    {
        plr = GetComponent<Player>();
        aim = head.GetComponent<LineRenderer>();
        rope = GetComponent<LineRenderer>();
        distJoint = GetComponent<DistanceJoint2D>();
        controller = GetComponentInParent<PlayerMovement>();
        am = FindObjectOfType<AudioManager>();
        slobberEffect = GameObject.Find("Slobber");

        grappling = false;
        distJoint.enabled = false;
        rope.enabled = false;
        aim.enabled = true;      
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (!dialogueTriggerBtn.activeSelf && !gameObject.GetComponent<Player>().conversing))
        {
            StartGrapple();
            
        }
        else if((Input.GetKeyUp(KeyCode.Mouse0) && grappling))
        {
            StopGrapple();
            am.Stop("SwingSound");
        }

        if (plr.GetHealth() <= 0 && grappling)
        {
            distJoint.enabled = false;
            rope.enabled = false;
            grappling = false;
            controller.SetIsSwinging(grappling);
        }

        if (grappling)
        {
            ReelGrapple();
        }
    }

    private void LateUpdate()
    {
        Aim();

        //if (Input.GetKey(KeyCode.Mouse1))
        //{           
        //    DrawAim();
        //}
        //else if (Input.GetKeyUp(KeyCode.Mouse1))
        //{
        //    StopAim();
        //}

        if (distJoint.enabled)
        {
            rope.SetPosition(1, new Vector3(firePos.transform.position.x, firePos.transform.position.y, -0.1f));
        }
    }

    void Aim()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        headPos = head.position;
        aimDir = mousePos - headPos;

        aimDir.Normalize();

        float zRot = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        if (zRot < -90 || zRot > 90)
        {
            if (controller.IsFacingRight())
            {
                Quaternion rot = Quaternion.Euler(180, 180, zRot);
                head.localRotation = Quaternion.Euler(-rot.eulerAngles);
            }
            else
            {
                head.localRotation = Quaternion.Euler(180, 180, zRot);
            }
        }
        else
        {
            if (!controller.IsFacingRight())
            {
                head.transform.rotation = Quaternion.Euler(0, 180, -zRot);
            }
            else
            {
                head.transform.rotation = Quaternion.Euler(0, 0, zRot);
            }
        }
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

        if (ray)
        {
            if (ray.collider.CompareTag("Grappleable"))
            {
                Vector2 hitPoint = ray.point;
                rope.SetPosition(0, new Vector3(hitPoint.x, hitPoint.y, -.1f));
                rope.SetPosition(1, new Vector3(headPos.x, headPos.y, -.1f));
                distJoint.connectedAnchor = hitPoint;

                GameObject newSlobber = (Instantiate(slobberEffect, firePos.position, Quaternion.identity));
                newSlobber.transform.parent = transform.parent;
                Destroy(newSlobber, 1f);

                headPos = headSprite.transform.position;
                aimDir = hitPoint - headPos;

                grappling = true;
                controller.SetIsSwinging(grappling);
                am.Play("SwingSound");
               
                distJoint.enabled = true;

                if (controller.IsGrounded())
                {
                    float dist = Vector2.Distance(transform.position, hitPoint);
                    distJoint.distance = dist * .8f;
                }

                rope.enabled = true;
                
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

    public void StopGrapple()
    {       
        distJoint.enabled = false;
        rope.enabled = false;
        grappling = false;
        controller.SetIsSwinging(grappling);
        anim.SetTrigger("swing_land");
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
