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
            "Esther"
        };
        poses = new int[] 
        {
            0,
            1,
            2,
            3
        };
        sentences = new string[]
        {
            "This is Dialogue",
            "This is also Dialogue",
            "Crazy, right?",
            "But this is the last one. :("
        };
    }

    void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(speaker, poses, sentences);
    }
}

