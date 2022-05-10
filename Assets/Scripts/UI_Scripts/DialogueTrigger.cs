using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * REFERENCE: Brackeys - How to make a Dialogue System in Unity - https://www.youtube.com/watch?v=_nRzoTzeyxU
 */


/*
 * Script to handle storing dialogue and triggering specific interactions in the scene based on the player's location/collision with specific hit boxes
 */
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject triggerBtn;

    /*
     * Method to trigger the start of a dialogue interaction 
     */
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    /*
     * Method to set the UI button that triggers the dialogue active/inactive
     * Params: bool active - true if the button is to be set to active, false otherwise 
     */
    public void TriggerButton(bool active)
    {
        triggerBtn.SetActive(active);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player plr = collision.GetComponent<Player>();

        if (plr)
        {
            // Trigger the UI button to be active if the player is not already in conversation 
            if (!plr.conversing)
            {
                TriggerButton(true);
                // Trigger the start of the dialogue if the button is pressed 
                if (FindObjectOfType<DialogueManager>().triggerPressed)
                {
                    TriggerDialogue();
                }
            }
            else
            {
                TriggerButton(false);
            }
        }
    }

    /*
     * Ensure the UI button does not stay active when the player leaves the trigger box 
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player plr = collision.GetComponent<Player>();

        if (plr)
        {
            if (!plr.conversing)
            {
                TriggerButton(false);
            }
        }
    }
}
