using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth;
    public bool conversing = false;

    Tongue tongue;
    private bool justDamaged;
    private float cooldown = 1f;
    private int health;
    [SerializeField] private Transform respawnPoint;

    void Start()
    {
        health = maxHealth;
        tongue = GetComponentInChildren<Tongue>();
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

            // Play hurt anim

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
        SetRendererActive(false);
        StartCoroutine(Respawn());      
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        health = maxHealth;
        gameObject.transform.position = respawnPoint.position;   
        SetRendererActive(true);           
        justDamaged = true;     
    }

    public void SetRendererActive(bool active)
    {
        SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            if (!string.Equals(renderers[i].name, "Tongue"))
            {
                renderers[i].enabled = active;           
            }          
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !tongue.attacking)
        {
            TakeDamage(1);
            GetComponent<PlayerMovement>().Knockback();
        }
        else if (collision.CompareTag("Hazard") || collision.CompareTag("Projectile"))
        {
            TakeDamage(1);
            GetComponent<PlayerMovement>().Knockback();
        }
        else if (collision.CompareTag("Health"))
        {
            if (health < maxHealth)
            {
                health++;
                GetComponent<HealthUI>().UpdateHearts();
            }
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("RespawnPoint"))
        {
            respawnPoint = collision.gameObject.transform;
        }
    }
}
