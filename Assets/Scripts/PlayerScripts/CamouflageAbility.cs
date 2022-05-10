using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script responsible for handling the camouflage ability of the player when they collect the corresponding bug buff
 */
public class CamouflageAbility : MonoBehaviour
{
    public float maxTime;

    private SpriteRenderer[] renderers;
    private Color[] col;
    private float time;
    private bool invisible;
    private Player plr;

    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        plr = FindObjectOfType<Player>();
        time = 0;
        invisible = false;

        // Initialise the renderers array to all of the sprite renderers of the player 
        if (renderers[0])
        {
            col = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                col[i] = renderers[i].color;
            }
        }
        else
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();
        }
       
    }


    void Update()
    {
        CheckInvisibility(); 
    }

    /*
     * Method responsible for the timer on the invisibility of the player 
     */
    void CheckInvisibility()
    {
        time -= Time.deltaTime;
        if (invisible && time <= 0)
        {
            invisible = false;
            plr.SetInvisible(invisible);

            for (int i = 0; i < renderers.Length; i++)
            {
                col[i].a = 1;
                renderers[i].color = col[i];
            }       
        }
    }

    /*
     * Method that can be used in other scripts to get the invisible property of the player 
     * Returns: bool invisible - whether or not the player is currently invisible 
     */
    public bool GetInvisible()
    {      
        return invisible;
    }

    /*
     * Method to turn the player invisible upon collecting the bug buff
     */
    void GoInvisible()
    {
        if (!invisible)
        {
            invisible = true;
            plr.SetInvisible(invisible);
            time = maxTime;

            // Reduce the alpha of all renderers to .2 (so that the player can still see where they are)
            for (int i = 0; i < renderers.Length; i++)
            {
                col[i].a = .2f;
                renderers[i].color = col[i];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Invisible"))
        {
            GoInvisible();
            GameObject col = collision.gameObject;
            Destroy(col);
        }
    }
}
