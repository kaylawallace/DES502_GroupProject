using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 30;

    private int currHealth;
    private bool justDamaged = false;
    private float damageCooldown, maxDamageCooldown = .8f;


    private void Start()
    {
        damageCooldown = maxDamageCooldown;
        currHealth = maxHealth;
    }
    private void Update()
    {
        if (justDamaged)
        {
            damageCooldown -= Time.deltaTime;

            if (damageCooldown <= 0)
            {
                justDamaged = false;
                damageCooldown = maxDamageCooldown;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!justDamaged)
        {
            justDamaged = true;
            currHealth -= damage;

            // Play hurt anim

            if (currHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Tongue>())
        {
            TakeDamage(other.GetComponent<Tongue>().damage);
        }
    }
}
