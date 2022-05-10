using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script to handle enemies taking damage and death 
 */
public class Enemy : MonoBehaviour
{
    public int maxHealth;

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

    /*
     * Method to handle taking damage
     * Params: int damage - the amount of damage the enemy will take 
     */
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

    /*
     * Method to handle enemy death 
     */
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
