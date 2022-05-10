using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * REFERENCE: bendux: How To Make 2D Jump Pads in Unity: https://www.youtube.com/watch?v=0e3Ld6-RzIU
 */

/*
 * Script to handle bouncy nature of mushrooms in the scene 
 */
public class JumpPad : MonoBehaviour
{
    [SerializeField] private float bounce = 20f;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("bounce");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }
    }
}
