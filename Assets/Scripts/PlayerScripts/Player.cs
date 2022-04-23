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
    private GameObject sprite; 

    public Animator anim;

    //public enum ePlayerState {
    //    Idle, 
    //    Walk,
    //    Attack,
    //    Swing,
    //    SwingLand,
    //    Jump,
    //    JumpLand,
    //    Hurt,
    //    Dead
    //}
    //public ePlayerState currState;

    void Start()
    {
        health = maxHealth;
        tongue = GetComponentInChildren<Tongue>();
        sprite = GameObject.Find("Bones_Anim");
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
            //int state = anim.GetInteger("state");
            anim.SetTrigger("hit");
            //anim.SetInteger("state", state);

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
        sprite.SetActive(active);
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
