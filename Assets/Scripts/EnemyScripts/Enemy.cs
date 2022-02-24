using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;

    private int currHealth;
    private bool justDamaged;
    private float cooldown = .5f;

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
        Debug.Log("Enemy died");

        // Die anim 
        // Remove enemy
    }
}
