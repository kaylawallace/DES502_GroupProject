using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public GameObject projectile;
    
    GameObject target;
    [SerializeField] float shootDist; 
    float currShotTime, maxShotTime = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        currShotTime = maxShotTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (DistanceFromPlayer() <= shootDist)
        {
            if (currShotTime <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
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
