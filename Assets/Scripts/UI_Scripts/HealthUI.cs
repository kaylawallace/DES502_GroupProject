using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * REFERENCE: Blackthornprod - HOW TO MAKE HEART/HEALTH SYSTEM - UNITY TUTORIAL - https://www.youtube.com/watch?v=3uyolYVsiWc
 */

/*
 * Script to handle the health UI in-game 
 */

public class HealthUI : MonoBehaviour
{
    public Player plr; 
    public int numHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private void Start()
    {
        plr = GetComponent<Player>();
        numHearts = plr.maxHealth;
    }

    private void Update()
    {
        UpdateHearts();
    }

    /*
     * Method responsible for updating the hearts in the UI 
     */
    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Sets sprites to full hearts if below the health of the player 
            if (i < plr.GetHealth())
            {
                hearts[i].sprite = fullHeart;
            }
            // Sets sprites to empty hearts if above the health of the player 
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            
            // Ensures heart sprites are active if below the number of hearts to be on the UI
            if (i < numHearts)
            {
                hearts[i].enabled = true;
            }
            // Ensures heart sprites are inactive if above the number of hearts to be on the UI 
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
