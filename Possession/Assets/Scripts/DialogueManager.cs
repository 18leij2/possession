using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    private string[] sentences;
    private int currentSentenceIndex;


    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("duplicate instance of dialogue manager created");
        }

        dialogueBox.SetActive(false);
    }

    void Start()
    {
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(string[] newSentences)
    {
        dialogueBox.SetActive(true);
        sentences = newSentences;
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