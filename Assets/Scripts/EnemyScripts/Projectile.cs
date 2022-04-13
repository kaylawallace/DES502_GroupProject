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

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        target = new Vector2(player.position.x, player.position.y+1.5f);
        dir = ((Vector3)target - transform.position).normalized;
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
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }

        Destroy(gameObject, 3f);
    }
}
