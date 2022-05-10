using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * REFERENCE: Blackthornprod: SHOOTING/FOLLOW/RETREAT ENEMY AI WITH UNITY AND C#: https://www.youtube.com/watch?v=_Z1t7MNk0c4
 */

/*
 * Script to handle the behaviour of the projectiles shot by enemies
 */
public class Projectile : MonoBehaviour
{   
    public float speed;

    [SerializeField] private int rangedDamage;

    private Transform player;
    private Vector2 target;
    private Vector3 dir;
    private GameObject hitEffect;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        target = new Vector2(player.position.x, player.position.y+1.5f);

        // Calculate the direction only once to avoid 'homing' effect 
        dir = ((Vector3)target - transform.position).normalized;

        hitEffect = GameObject.Find("SporesEffect");
    }

    private void Update()
    {
        Shoot();
    }

    /*
     * Method to handle moving the projectile through the scene 
     */
    void Shoot()
    {      
        transform.position += dir * speed * Time.deltaTime;
    }

    /*
     * Collision detection for the projectile 
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(rangedDamage);
            GameObject newHitEffect = Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(newHitEffect, 2f);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Environment"))
        {
            GameObject newHitEffect = Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(newHitEffect, 2f);
            Destroy(gameObject);
        }

        // Create the hitEffect and destroy the projectile even if it doesn't hit anything 
        GameObject newHit = Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
        Destroy(newHit, 2f);
        Destroy(gameObject, 3f);
        
    }
}
