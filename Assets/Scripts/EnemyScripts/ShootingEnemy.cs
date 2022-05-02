using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public GameObject projectile;   
    
    [SerializeField] float shootDist;

    private GameObject target;
    private float currShotTime, maxShotTime = 3f;
    private Animator anim;
    private AudioManager am;
    private Player plrComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        plrComponent = target.GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        am = FindObjectOfType<AudioManager>();

        currShotTime = maxShotTime;    
    }

    // Update is called once per frame
    void Update()
    {
        if (!plrComponent.GetInvisible())
        {
            if (DistanceFromPlayer() <= shootDist)
            {
                if (currShotTime <= 0)
                {
                    Instantiate(projectile, transform.position, Quaternion.identity);
                    am.Play("PlantDeathSound");
                    anim.SetTrigger("shoot");
                    currShotTime = maxShotTime;
                }
                else
                {
                    currShotTime -= Time.deltaTime;
                }
            }
        }
    }

    float DistanceFromPlayer() 
    {
        float dist = Vector2.Distance(target.transform.position, transform.position);
        return dist;
    }
}
