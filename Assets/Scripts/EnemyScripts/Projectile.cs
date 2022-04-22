using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    public float speed;

    private Transform player;
    private Vector2 target;
    private Vector3 dir;
    [SerializeField] private int rangedDamage;
    private GameObject hitEffect;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        target = new Vector2(player.position.x, player.position.y+1.5f);
        dir = ((Vector3)target - transform.position).normalized;
        hitEffect = GameObject.Find("SporesEffect");
    }

    private void Update()
    {
        Shoot();
    }

    void Shoot()
    {      
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // do damage to player 
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

        GameObject newHit = Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
        Destroy(newHit, 2f);
        Destroy(gameObject, 3f);
        
    }
}
