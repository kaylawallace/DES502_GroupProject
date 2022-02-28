using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
   [SerializeField] private float attackTime;
    private SpriteRenderer renderer;
    private BoxCollider2D collider;
    [HideInInspector] public bool attacking;
    public int damage;

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
        renderer.enabled = true;
        collider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        renderer.enabled = false;
        collider.enabled = false;
        attacking = false;
    }
}
