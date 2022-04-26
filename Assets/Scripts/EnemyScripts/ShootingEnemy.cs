using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public GameObject projectile;
    
    GameObject target;
    [SerializeField] float shootDist; 
    float currShotTime, maxShotTime = 3f;
    private Animator anim;
    private AudioManager am;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        currShotTime = maxShotTime;
        anim = GetComponentInChildren<Animator>();
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
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

    float DistanceFromPlayer() 
    {
        float dist = Vector2.Distance(target.transform.position, transform.position);
        return dist;
    }
}
