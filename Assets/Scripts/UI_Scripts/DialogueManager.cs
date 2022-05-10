using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * REFERENCE: Brackeys - How to make a Dialogue System in Unity - https://www.youtube.com/watch?v=_nRzoTzeyxU
 */

/*
 * Script to handle dialogue interactions in the scene 
 */
public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI nameTxt, dialogueTxt;
    public bool triggerPressed = false;

    private Queue<string> sentences;
    private Player plr;   
    private AudioManager am;

    void Start()
    {
        sentences = new Queue<string>();
        plr = FindObjectOfType<Player>();
        am = FindObjectOfType<AudioManager>();
    }

    /*
     * Method to set the triggerPressed var to true, meaning the player has pressed the 'Listen' button in the scene 
     */
    public void SetActive()
    {
        triggerPressed = true;
    }

    /*
     * Method to start a dialogue interaction 
     * Params: Dialogue dialogue - the Dialogue object for the specific dialogue interaction 
     */
    public void StartDialogue(Dialogue dialogue)
    {
        nameTxt.text = dialogue.name;
        sentences.Clear();
        plr.conversing = true;
        dialogueUI.SetActive(true);
        am.Play("FrogCroakSound");

        // Add all sentences in the dialogue object to a queue 
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            sentences.Enqueue(dialogue.sentences[i]);
        }

        DisplayNextSentence();
    }

    /*
     * Method to display the next sentence in the sentences queue 
     */
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    /*
     * Coroutine to type letters in a sentence one per frame to add polish 
     * Params: string sentence - the sentence to be typed out 
     */
    IEnumerator TypeSentence(string sentence)
    {
        dialogueTxt.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueTxt.text += letter;
            yield return null;
        }
    }

    /*
     * Method to handle ending dialogue 
     */
    private void EndDialogue()
    {
        Debug.Log("Ending conversation");
        dialogueUI.SetActive(false);
        plr.conversing = false;
        triggerPressed = false;
    }

}
