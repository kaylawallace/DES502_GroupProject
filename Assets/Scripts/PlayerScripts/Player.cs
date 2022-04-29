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
    private bool invisible;
    private float cooldown = 1f;
    private int health;
    [SerializeField] private Transform respawnPoint;
    private GameObject sprite;
    //private CamouflageAbility camo;

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
        //camo = GetComponent<CamouflageAbility>();
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
            anim.SetTrigger("hit");

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

    public void SetInvisible(bool _invisible)
    {
        //CamouflageAbility.GetInvisible();
        //invisible = camo.GetInvisible();
        invisible = _invisible;
    }

    public bool GetInvisible()
    {
        return invisible;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        health = maxHealth;
        gameObject.transform.SetPositionAndRotation(respawnPoint.position, Quaternion.identity);
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
            if (!invisible)
            {
                TakeDamage(1);
                GetComponent<PlayerMovement>().Knockback();
            }
        }
        else if (collision.CompareTag("Hazard"))
        {
            TakeDamage(3);
            GetComponent<PlayerMovement>().Knockback();
        }
        else if (collision.CompareTag("Projectile"))
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
