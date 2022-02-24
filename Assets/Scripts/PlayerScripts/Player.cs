using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth;

    private bool justDamaged;
    private float cooldown = 1f;
    //public HealthBar healthBar;

    private int health;

    void Start()
    {
        health = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
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
        if (!justDamaged)
        {
            justDamaged = true;
            health -= damage;

            // Play hurt anim

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player died");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(1);
            GetComponent<PlayerMovement>().Knockback();
        }
    }
}
