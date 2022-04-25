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

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        renderer.enabled = false;
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Attack();
        }

        Rotate();
    }

    void Attack()
    {
        StartCoroutine(Attack(attackTime));
    }

    public static float ConvertToAngle180(float input)
    {
        while (input > 360)
        {
            input = input - 360;
        }
        while (input < -360)
        {
            input = input + 360;
        }
        if (input > 180)
        {
            input = input - 360;
        }
        if (input < -180)
            input = 360 + input;

        print(input);
        return input;
    }

    public void Rotate()
    {
       

        //print(aimDir);

        //if (controller.IsFacingRight())
        //{
            //head.transform.rotation = Quaternion.Euler(0, 0, zRot);
        //}
        //else
        //{
        //    //print("here");
        //    head.transform.rotation = Quaternion.Euler(180, 0, zRot);
        //}

       

        //transform.eulerAngles = new Vector3(head.transform.eulerAngles.x, head.transform.eulerAngles.y, head.transform.eulerAngles.z + 90);

        //else
        //{
        //    if (!controller.IsFacingRight())
        //    {
        //        head.localRotation = Quaternion.Euler(180, 0, -zRot);

        //    }
        //}
    }


    IEnumerator Attack(float attackTime)
    {
        attacking = true;
        anim.SetTrigger("attack");

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 headPos = head.position;
        Vector2 aimDir = mousePos - headPos;
        aimDir.Normalize();

        float zRot = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        head.transform.rotation = Quaternion.Euler(0, 0, zRot);

        if (!controller.IsFacingRight())
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        }


        if (zRot < -90 || zRot > 90)
        {
            if (controller.IsFacingRight())
            {
                head.localRotation = Quaternion.Euler(180, 0, zRot);
            }
            else
            {
                head.localRotation = Quaternion.Euler(180, 180, zRot-20);
            }
        }

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

        GameObject newSlobber = (Instantiate(slobberEffect, firePoint.position, Quaternion.identity));
        newSlobber.transform.parent = transform.parent.parent.parent;
        Destroy(newSlobber, 2f);
        renderer.enabled = true;
        collider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        renderer.enabled = false;
        collider.enabled = false;

        if (!controller.IsFacingRight())
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        }

        attacking = false;
    }
}
