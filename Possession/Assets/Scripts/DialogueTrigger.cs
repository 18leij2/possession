using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string[] sentences;
    public int[] poses;
    public string[] speaker;
    public Button debugButton;

    void Start()
    {
        debugButton.onClick.AddListener(StartDialogue);
        speaker = new string[]
        {
            "Esther",
            "Naascha",
            "Naascha",
            "Esther",
            "Naascha",
            "Naascha",
            "Esther",
            "Esther",
            "Esther",
            "Esther"
        };
        poses = new int[] 
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8
        };
        sentences = new string[]
        {
            "This is Dialogue, press space to continue, please",
            "The dialogue unfortunately needed to be cut due to some unforseen implimentation issues",
            "However, a lot of work was put into this system",
            "and future iterations of the project will be able to use it",
            "so this is just a tiny showcase of what the system would look like if it were properly implimented.",
            "In the email/zip we sent, there should be the written dialogue",
            "which if you could play through the game while referencing it, that would more accurately reflect the intended experience",
            "Thank you, please enjoy our prototype"
        };
    }

    void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(speaker, poses, sentences);
        debugButton.gameObject.SetActive(false);
    }
}

