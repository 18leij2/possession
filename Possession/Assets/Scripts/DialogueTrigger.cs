using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string[] sentences;
    public Button debugButton;

    void Start()
    {
        debugButton.onClick.AddListener(StartDialogue);
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
        DialogueManager.Instance.StartDialogue(sentences);
    }
}

