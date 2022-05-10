using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Script to handle player collision detection and health (inc. taking damage, death, and respawn)
 */
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

    /*
     * Method to handle the player taking damage 
     * Params: int damage - amout of damage to decrement the player's health by 
     */
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

    /*
     * Method used by other scripts to get the player's current health 
     * Returns: int health - player's current health value 
     */
    public int GetHealth()
    {
        return health;
    }

    /*
     * Method to handle player death 
     */
    public void Death()
    {
        anim.SetTrigger("died");
        StartCoroutine(Respawn());      
    }

    /*
     * Method to set the player's invisible property 
     * Params: bool _invisible - whether the player is currently invisible
     */
    public void SetInvisible(bool _invisible)
    {
        invisible = _invisible;
    }

    /*
     * Method used by other scripts to get whether the player is invisible 
     * Returns: bool invisible - whether the player is currently invisible 
     */
    public bool GetInvisible()
    {
        return invisible;
    }

    /*
     * Coroutine to handle respawning the player at the last respawnPoint
     */
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
        // Cannot take damage while attacking as enemy collision detection picks up tongue as player and would therefore do damage when player is attacking 
        if (collision.CompareTag("Enemy") && !tongue.attacking)
        {
            if (!invisible)
            {
                TakeDamage(1);
                controller.Knockback();
            }
        }
        // Take maximum damage if fallen into hazard as there is no way to get out and so want player to immediately respawn 
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
        // Executes when the player collects a health bug 
        else if (collision.CompareTag("Health"))
        {
            if (health < maxHealth)
            {
                health++;
                healthUI.UpdateHearts();
            }
            Destroy(collision.gameObject);
        }
        // Sets respawn points throughout the level 
        else if (collision.CompareTag("RespawnPoint"))
        {
            respawnPoint = collision.gameObject.transform;
        }
    }
}
