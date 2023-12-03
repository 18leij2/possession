using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    private string[] sentences;  // Array to hold your dialogue sentences
    private int currentSentenceIndex;

    void Start()
    {
        dialogueBox.SetActive(false);
        sentences = new string[]
        {
            "This is Dialogue",
            "This is also Dialogue",
            "Crazy, right?",
            "But this is the last one. :("
        };
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue()
    {
        dialogueBox.SetActive(true);
        currentSentenceIndex = 0;
        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if (currentSentenceIndex < sentences.Length)
        {
            dialogueText.text = sentences[currentSentenceIndex];
            currentSentenceIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
    }
}