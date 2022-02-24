using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI nameTxt, dialogueTxt;
    private Queue<string> sentences;
    private Player plr;

    void Start()
    {
        //dialogueUI = GameObject.FindGameObjectWithTag("Dialogue");
        sentences = new Queue<string>();
        plr = FindObjectOfType<Player>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameTxt.text = dialogue.name;
        sentences.Clear();

        dialogueUI.SetActive(true);

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            sentences.Enqueue(dialogue.sentences[i]);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueTxt.text = sentence;
    }

    private void EndDialogue()
    {
        Debug.Log("Ending conversation");
        dialogueUI.SetActive(false);
        plr.conversing = false; 
    }

}
