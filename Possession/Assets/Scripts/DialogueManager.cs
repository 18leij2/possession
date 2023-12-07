using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueBox;
    public Image portraitImage1;
    public Image portraitImage2;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public float textSpeed;
    public Sprite[] portrait1;
    public Sprite[] portrait2;

    private string[] speaker;
    private int[] pose;
    private string[] dialogue;
    
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
        portraitImage1.enabled = false;
        portraitImage2.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogue[index];
            }
        }
    }

    public void StartDialogue(string[] newSpeaker, int[] newPose, string[] newDialogue)
    {
        dialogueBox.SetActive(true);
        Debug.Log("Starting Dialogue");
        speaker = newSpeaker;
        dialogue = newDialogue;
        pose = newPose;
        index = 0;

        speakerText.text = speaker[index];
        dialogueText.text = dialogue[index];
        UpdatePortrait();
    }

    void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            speakerText.text = speaker[index];
            UpdatePortrait();
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void UpdatePortrait()
    {
        if (speaker[index] == "Esther")
        {
            portraitImage1.enabled = true;
            portraitImage2.enabled = false;
            portraitImage1.sprite = portrait1[pose[index]];

        }
        else if (speaker[index] == "Naascha")
        {
            portraitImage1.enabled = false;
            portraitImage2.enabled = true;
            portraitImage2.sprite = portrait2[pose[index]];
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
        if (SceneManager.GetActiveScene().buildIndex < 11)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}