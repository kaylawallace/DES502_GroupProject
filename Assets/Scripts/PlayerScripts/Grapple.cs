using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * REFERENCE: Sean Duffy - Make a 2D Grappling Hook Game in Unity - Part 1: https://www.raywenderlich.com/348-make-a-2d-grappling-hook-game-in-unity-part-1
 */

/*
 * Method responsible for handling the grapple behaviour of the player 
 */
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

    void Update()
    {
        // Allow player to grapple only if they are not interacting with an NPC or the interact button for that is not active 
        if (Input.GetKeyDown(KeyCode.Mouse0) && (!dialogueTriggerBtn.activeSelf && !gameObject.GetComponent<Player>().conversing))
        {
            StartGrapple();
            
        }
        else if((Input.GetKeyUp(KeyCode.Mouse0) && grappling))
        {
            StopGrapple();
            am.Stop("SwingSound");
        }

        // Stop the player grappling (without triggering the land animation) if they die while grappling 
        if (plr.GetHealth() <= 0 && grappling)
        {
            distJoint.enabled = false;
            rope.enabled = false;
            grappling = false;
            controller.SetIsSwinging(grappling);
        }

        // Allow the player to reel their grapple if they are currently grappling 
        if (grappling)
        {
            ReelGrapple();
        }
    }

    private void LateUpdate()
    {
        Aim();

        /*
         * This section controlled the aim for the grapple that was removed in later development 
         */
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

    /*
     * Method reponsible for the aim of the grapple and the head rotation as a result 
     */
    void Aim()
    {
        // Use the position of the player's mouse and the head to calculate the direction they are aiming 
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        headPos = head.position;
        aimDir = mousePos - headPos;

        aimDir.Normalize();

        float zRot = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;


        // Set the z-rotation of the underlying head object based on the direction the player is facing and the position of the mouse
        
        // If the mouse is on the left side of the player 
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
        // If the mouse is on the right side of the player 
        else
        {
            if (controller.IsFacingRight())
            {
                head.transform.rotation = Quaternion.Euler(0, 0, zRot);
            }
            else
            {
                head.transform.rotation = Quaternion.Euler(0, 180, -zRot);
            }
        }
    }

    /*
     * Method responsible for disabling the aim ray 
     */
    void StopAim()
    {
        aim.enabled = false;
    }

    /*
     * Method to draw the aim ray 
     */
    void DrawAim()
    {
        Ray2D aimRay = new Ray2D(transform.position, aimDir);
        aim.sortingOrder = 50;

        // Only display the aim ray if not currently grappling 
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

    /*
     * Method to handle the grappling behaviour 
     */
    void StartGrapple()
    {
        // Case a ray out 20 units and continue with enabling the grapple if this ray hits a 'grappleable' GameObject
        RaycastHit2D ray = Physics2D.Raycast(transform.position, aimDir, 20f, grappleable);

        if (ray)
        {
            if (ray.collider.CompareTag("Grappleable"))
            {
                // Set the two points for the line renderer 'rope' and anchor the distance joint at the hitPoint of the ray 
                Vector2 hitPoint = ray.point;
                rope.SetPosition(0, new Vector3(hitPoint.x, hitPoint.y, -.1f));
                rope.SetPosition(1, new Vector3(headPos.x, headPos.y, -.1f));
                distJoint.connectedAnchor = hitPoint;

                // Instantiate the slobber particle effect 
                GameObject newSlobber = (Instantiate(slobberEffect, firePos.position, Quaternion.identity));
                newSlobber.transform.parent = transform.parent;
                Destroy(newSlobber, 1f);

                headPos = headSprite.transform.position;
                aimDir = hitPoint - headPos;
 
                grappling = true;
                controller.SetIsSwinging(grappling);
                am.Play("SwingSound");
               
                distJoint.enabled = true;

                // Set the distance joint to 80% length if the player is grounded to give them a boost 
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

    /*
     * Method to handle the player stopping grappling 
     */
    public void StopGrapple()
    {       
        distJoint.enabled = false;
        rope.enabled = false;
        grappling = false;
        controller.SetIsSwinging(grappling);
        anim.SetTrigger("swing_land");
    }

    /*
     * Method to handle the ability to reel the grapple in/out 
     */
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
