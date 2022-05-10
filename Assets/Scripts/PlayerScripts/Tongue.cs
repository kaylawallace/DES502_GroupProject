using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script to handle the player attacking 
 */
public class Tongue : MonoBehaviour
{
    public GameObject slobberEffect;
    public int damage;
    public Transform head;

    [HideInInspector] public bool attacking;

    [SerializeField] private PlayerMovement controller;
    [SerializeField] private float attackTime;

    public Animator anim;
    public GameObject tongueAnim;
    private AudioManager am;
    private Quaternion animRot;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        am = FindObjectOfType<AudioManager>();

        spriteRenderer.enabled = false;
        boxCollider.enabled = false;        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Do not allow player to attack while swinging (as that would involve having 2 tongues)
            if (!attacking && !controller.GetIsSwinging())
            {
                Attack();
            }          
        }
    }

    /*
     * Method to handle attacking
     */
    void Attack()
    {
        StartCoroutine(AttackCoroutine(attackTime));
    }

    /*
     * Coroutine to handle attacking 
     */
    IEnumerator AttackCoroutine(float attackTime)
    {       
        attacking = true;
        anim.SetTrigger("attack");

        // MUCH LIKE IN THE GRAPPLE SCRIPT 
        // Calculate the direction between the player's head sprite and mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 headPos = head.position;
        Vector2 aimDir = mousePos - headPos;
        aimDir.Normalize();

        float zRot = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        Vector3 initPos = transform.position;

        // Set the z-rotation based on the mouse position and orientation of player 
        // If the mouse position is on the left side of the player 
        if (zRot < -90 || zRot > 90)
        {
            if (controller.IsFacingRight())
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                head.localRotation = Quaternion.Euler(-180, -180, -zRot);
            }
            else
            {
                head.localRotation = Quaternion.Euler(180, 180, zRot);
            }
        }
        // If the mouse position is on the right side of the player 
        else
        {
            if (controller.IsFacingRight())
            {
                head.transform.rotation = Quaternion.Euler(0, 0, zRot);     
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                head.transform.rotation = Quaternion.Euler(0, 180, -zRot);
            }
        }

        // Controls the rotation of the tongue animation
        if (controller.IsFacingRight())
        {
            animRot = Quaternion.Euler(head.localRotation.eulerAngles.x, 180, head.localRotation.eulerAngles.z);
        }
        else
        {
            animRot = Quaternion.Euler(head.localRotation.eulerAngles.x, 0, head.localRotation.eulerAngles.z);
        }

        // Instantiate the tongue animation
        GameObject newTongueAnim = (Instantiate(tongueAnim, transform.position, animRot));
        newTongueAnim.transform.parent = transform.parent;
        Destroy(newTongueAnim, .7f);

        transform.position = new Vector3(initPos.x, initPos.y, initPos.z);

        // Instantiate the slobber particle effect 
        GameObject newSlobber = (Instantiate(slobberEffect, transform.position, Quaternion.identity));
        newSlobber.transform.parent = transform.parent.parent.parent;
        Destroy(newSlobber, 2f);

        am.Play("AttackSound");

        boxCollider.enabled = true;

        // Ensures the player is attacking for more than a single frame 
        yield return new WaitForSeconds(attackTime);

        boxCollider.enabled = false;       
        attacking = false;
    }
}
