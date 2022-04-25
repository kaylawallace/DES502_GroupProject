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
    public bool triggerPressed = false;

    void Start()
    {
        sentences = new Queue<string>();
        plr = FindObjectOfType<Player>();
    }

    public void SetActive()
    {
        triggerPressed = true;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameTxt.text = dialogue.name;
        sentences.Clear();
        plr.conversing = true;
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
        //dialogueTxt.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueTxt.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueTxt.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        Debug.Log("Ending conversation");
        dialogueUI.SetActive(false);
        plr.conversing = false;
        triggerPressed = false;
    }

}
