using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script repsponsible for triggering the loading of the next level when the player reaches the end of the current level 
 */
public class EndOfLevel : MonoBehaviour
{
    public LevelLoader loader;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            loader.LoadNextLevel();
        }
    }   
}
