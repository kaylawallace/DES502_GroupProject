using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject triggerBtn;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerButton(bool active)
    {
        triggerBtn.SetActive(active);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player plr = collision.GetComponent<Player>();

        if (plr)
        {
            if (!plr.conversing)
            {
                TriggerButton(true);
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
