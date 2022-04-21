using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
   [SerializeField] private float attackTime;
    private SpriteRenderer renderer;
    private BoxCollider2D collider;
    [HideInInspector] public bool attacking;
    public GameObject slobberEffect;
    public int damage;
    public Transform firePoint;

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
    }

    void Attack()
    {
        StartCoroutine(Attack(attackTime));
    }

    IEnumerator Attack(float attackTime)
    {
        attacking = true;
        //int state = anim.GetInteger("state");
        anim.SetTrigger("attack");
        //anim.SetInteger("state", state);
        GameObject newSlobber = (Instantiate(slobberEffect, firePoint.position, Quaternion.identity));
        newSlobber.transform.parent = transform.parent.parent.parent;
        Destroy(newSlobber, 2f);
        renderer.enabled = true;
        collider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        renderer.enabled = false;
        collider.enabled = false;
        attacking = false;
    }
}
