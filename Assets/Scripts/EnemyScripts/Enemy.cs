using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 30;

    private int currHealth;
    private bool justDamaged = false;
    private float damageCooldown, maxDamageCooldown = .8f;
    private AudioManager am;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();

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

            if (currHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        if (gameObject.GetComponent<ShootingEnemy>())
        {
            am.Play("PlantDeathSound");
        }
        else if (gameObject.GetComponent<PatrollerEnemy>())
        {
            am.Play("BirdDeathSound");
        }

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
