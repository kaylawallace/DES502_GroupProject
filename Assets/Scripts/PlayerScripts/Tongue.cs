using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
   [SerializeField] private float attackTime;
    private SpriteRenderer renderer;
    private BoxCollider2D collider;
    [SerializeField] private PlayerMovement controller;
    [HideInInspector] public bool attacking;
    public GameObject slobberEffect;
    public int damage;
    public Transform firePoint;
    public Transform head;

    public Animator anim;
    public GameObject tongueAnim;
    private AudioManager am;
    //public Vector2 mouseOnLeftRotLimits, mouseOnRightRotLimits;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        renderer.enabled = false;
        collider.enabled = false;

        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!attacking && !controller.GetIsSwinging())
            {
                Attack();
            }          
        }
    }

    void Attack()
    {
        StartCoroutine(AttackCoroutine(attackTime));
    }

    IEnumerator AttackCoroutine(float attackTime)
    {
        
        attacking = true;
        anim.SetTrigger("attack");

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 headPos = head.position;
        Vector2 aimDir = mousePos - headPos;
        aimDir.Normalize();

        float zRot = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        
        //head.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        //print(zRot);

        if (!controller.IsFacingRight())
        {
           
        }

        Vector3 initPos = transform.position;

        if (zRot < -90 || zRot > 90)
        {
            if (controller.IsFacingRight())
            {
                //print("here");
                //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                //  head.localRotation = Quaternion.Euler(180, 0, zRot);
                // DO NOT CHANGE 
                //zRot = Mathf.Clamp(zRot, -mouseOnLeftRotLimits.x, mouseOnLeftRotLimits.y);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                Quaternion rot = Quaternion.Euler(180, 180, zRot);
                head.localRotation = Quaternion.Euler(-rot.eulerAngles);
            }
            else
            {
                //zRot = Mathf.Clamp(zRot, mouseOnLeftRotLimits.x, -mouseOnLeftRotLimits.y);
                //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                //transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
                head.localRotation = Quaternion.Euler(180, 180, zRot);
            }
        }
        else
        {
            if (!controller.IsFacingRight())
            {
                //zRot = Mathf.Clamp(zRot, -mouseOnRightRotLimits.x, mouseOnRightRotLimits.y);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                head.transform.rotation = Quaternion.Euler(0, 180, -zRot);
            }
            else
            {
                //zRot = Mathf.Clamp(zRot, mouseOnRightRotLimits.x, -mouseOnRightRotLimits.y);
                // DO NOT CHANGE
                //transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
                head.transform.rotation = Quaternion.Euler(0, 0, zRot);
            }
        }

        //print(zRot);
        //print(head.localRotation.eulerAngles);

        //PlayerMovement.IsFacingRight()
        //if (controller.IsFacingRight())
        //{
        //    head.transform.right = aimDir;
        //    float zRot = ConvertToAngle180(head.eulerAngles.z);
        //    head.eulerAngles = new Vector3(head.eulerAngles.x, head.eulerAngles.y, Mathf.Clamp(zRot, -15f, 60f));
        //}
        //else
        //{
        //    //print("here");
        //    head.transform.right = -aimDir;
        //    float zRot = ConvertToAngle180(head.eulerAngles.z);
        //    head.localScale = new Vector3(-head.localScale.x, head.localScale.y, head.localScale.z);
        //    head.eulerAngles = new Vector3(head.eulerAngles.x, head.eulerAngles.y, Mathf.Clamp(zRot, -60f, 60f));
        //    //print(zRot);
        //}

        Quaternion animRot;

        if (controller.IsFacingRight())
        {
            animRot = Quaternion.Euler(head.localRotation.eulerAngles.x, 180, head.localRotation.eulerAngles.z);
        }
        else
        {
            animRot = Quaternion.Euler(head.localRotation.eulerAngles.x, 0, head.localRotation.eulerAngles.z);
        }


        GameObject newTongueAnim = (Instantiate(tongueAnim, transform.position, animRot));
        newTongueAnim.transform.parent = transform.parent;
        Destroy(newTongueAnim, .7f);
        transform.position = new Vector3(initPos.x, initPos.y, initPos.z);
        GameObject newSlobber = (Instantiate(slobberEffect, transform.position, Quaternion.identity));
        newSlobber.transform.parent = transform.parent.parent.parent;
        Destroy(newSlobber, 2f);
        am.Play("AttackSound");
        //renderer.enabled = true;
        collider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        //renderer.enabled = false;
        collider.enabled = false;
        

        //if (!controller.IsFacingRight())
        //{
           
        //}

        attacking = false;
    }
}
