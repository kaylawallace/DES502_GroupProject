using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script to handle the behaviour of the shooting enemies 
 */
public class ShootingEnemy : MonoBehaviour
{
    public GameObject projectile;   
    
    [SerializeField] float shootDist;

    private GameObject target;
    private float currShotTime, maxShotTime = 3f;
    private Animator anim;
    private AudioManager am;
    private Player plrComponent;
    
    void Start()
    {
        target = GameObject.Find("Player");
        plrComponent = target.GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        am = FindObjectOfType<AudioManager>();

        currShotTime = maxShotTime;    
    }


    void Update()
    {
        // Only allow the enemy to shoot if the player is not invisible and is within shooting distance 
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

    /*
     * Method to determine the distance from the player and the plant 
     * Returns: float dist - the distance calculated between the player position and the enemy position 
     */
    float DistanceFromPlayer() 
    {
        float dist = Vector2.Distance(target.transform.position, transform.position);
        return dist;
    }
}
