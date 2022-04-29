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
    public GameObject tongueAnim;
    private AudioManager am;
    private Player plr;

    // Start is called before the first frame update
    void Start()
    {
        grappling = false;
        aim = head.GetComponent<LineRenderer>();
        rope = GetComponent<LineRenderer>();
        distJoint = GetComponent<DistanceJoint2D>();
        controller = GetComponentInParent<PlayerMovement>();
        slobberEffect = GameObject.Find("Spit");
        am = FindObjectOfType<AudioManager>();
        distJoint.enabled = false;
        rope.enabled = false;
        aim.enabled = true;
        facingRight = true;
        plr = GetComponent<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        //Aim();

        //if (transform.rotation.y == 180)
        //{
        //    facingRight = false;
        //}
        //else
        //{
        //    facingRight = true;
        //}

        if (Input.GetKeyDown(KeyCode.Mouse0) && (!dialogueTriggerBtn.activeSelf && !gameObject.GetComponent<Player>().conversing))
        {
            StartGrapple();
            
        }
        else if((Input.GetKeyUp(KeyCode.Mouse0) && grappling) || plr.GetHealth() <= 0)
        {
            StopGrapple();
            am.Stop("SwingSound");
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

        if (distJoint.enabled)
        {
            rope.SetPosition(1, new Vector3(firePos.transform.position.x, firePos.transform.position.y, -0.1f));
        }
    }

    void Aim()
    {
        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        headPos = head.position;
        aimDir = mousePos - headPos;

        //head.transform.right = aimDir;

        Vector2 copy_aimDir = aimDir;
        copy_aimDir.Normalize();

        float zRot = Mathf.Atan2(copy_aimDir.y, copy_aimDir.x) * Mathf.Rad2Deg;
        if (zRot < -90 || zRot > 90)
        {
            if (controller.IsFacingRight())
            {
                //print("here");
                //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                //  head.localRotation = Quaternion.Euler(180, 0, zRot);
                Quaternion rot = Quaternion.Euler(180, 180, zRot);
                head.localRotation = Quaternion.Euler(-rot.eulerAngles);
            }
            else
            {
                //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
                //GameObject newTongueAnim = (Instantiate(tongueAnim, firePos.position, head.transform.rotation));
                //newTongueAnim.transform.parent = transform;
                //Destroy(newTongueAnim, 0.5f);
                Destroy(newSlobber, 1f);

                headPos = headSprite.transform.position;
                aimDir = hitPoint - headPos;

                grappling = true;
                controller.SetIsSwinging(grappling);
                am.Play("SwingSound");
                

                //Invoke("EnableLineRenderer", 1f);
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

    void StopGrapple()
    {       
        distJoint.enabled = false;
        rope.enabled = false;
        grappling = false;
        controller.SetIsSwinging(grappling);
        anim.SetTrigger("swing_land");
        print("swing land triggered");
    }

    //void EnableLineRenderer()
    //{
    //    rope.enabled = true;
    //}

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
