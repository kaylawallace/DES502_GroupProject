using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth;
    public bool conversing = false;
    public Animator anim;

    private Transform respawnPoint;
   
    private bool justDamaged;
    private bool invisible;
    private float cooldown = 1f;
    private int health;
    private Tongue tongue;
    private PlayerMovement controller;
    private GameObject startPos;
    private HealthUI healthUI;

    void Start()
    {
        tongue = GetComponentInChildren<Tongue>();     
        controller = GetComponent<PlayerMovement>();
        healthUI = GetComponent<HealthUI>();
        startPos = GameObject.Find("InitRespawnPoint");

        health = maxHealth;
        gameObject.transform.position = startPos.transform.position;
        respawnPoint = startPos.transform;
    }

    private void Update()
    {
        if (justDamaged)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                justDamaged = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!justDamaged && !tongue.attacking)
        {
            justDamaged = true;
            health -= damage;
            anim.SetTrigger("hit");

            if (health <= 0)
            {
                health = 0;
                Death();
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void Death()
    {
        anim.SetTrigger("died");
        StartCoroutine(Respawn());      
    }

    public void SetInvisible(bool _invisible)
    {
        invisible = _invisible;
    }

    public bool GetInvisible()
    {
        return invisible;
    }

    private IEnumerator Respawn()
    {
        controller.SetIsSwinging(false);

        yield return new WaitForSeconds(1.8f);

        health = maxHealth;
        gameObject.transform.SetPositionAndRotation(respawnPoint.position, Quaternion.Euler(0, 0, 0));          
        justDamaged = true;     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !tongue.attacking)
        {
            if (!invisible)
            {
                TakeDamage(1);
                controller.Knockback();
            }
        }
        else if (collision.CompareTag("Hazard"))
        {
            TakeDamage(maxHealth);
            controller.Knockback();
        }
        else if (collision.CompareTag("Projectile"))
        {
            TakeDamage(1);
            controller.Knockback();
        }
        else if (collision.CompareTag("Health"))
        {
            if (health < maxHealth)
            {
                health++;
                healthUI.UpdateHearts();
            }
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("RespawnPoint"))
        {
            respawnPoint = collision.gameObject.transform;
        }
    }
}
