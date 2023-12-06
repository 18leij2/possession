using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueBox;
    public Image portrait1;
    public Image portrait2;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public float textSpeed;

    private string[] dialogue;
    private string speaker;
    private int index;


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
        portrait1.enabled = false;
        portrait2.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void StartDialogue(string newSpeaker, string[] newDialogue)
    {
        dialogueBox.SetActive(true);
        Debug.Log("Starting Dialogue");
        speaker = newSpeaker;
        dialogue = newDialogue;
        index = 0;

        speakerText.text = speaker;
        dialogueText.text = dialogue[index];
    }

    void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in dialogue[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
    }
}