using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OpeningSlides : MonoBehaviour
{

    public TextMeshProUGUI slideText;
    public Image slideImage;

    public Sprite[] slides;
    public string[] dialogue;
    public float textSpeed;

    private int index;

    [SerializeField] public AudioSource sfxPlayer;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = new string[]
        {
            "Meet Esther",
            " ",
            "Esther just got invited to a Halloween party in the most cursed location in all of Atlanta",
            " ",
            "What's the worst that could happen?",
            " ",
            " \"There's no damn way...\" ",
            " \"...that THIS is the place.\" ",
            " \"I-Is this even Atlanta anymore?\" ",
            " \"Well, I guess some people get really into decorating?\" ",
            " \"Same bunch that hangs up christmas lights on November 1st I gue-\" ",
            "FWOOOOOOOOOOOOOOOOSH", 
            "ssssssssssssssssssssssssssssssss",
            " ",
            "thud",
            "Later...",
            " \"To think a vampire huntress could be so cute. Must be my lucky day.\" ",
            " \"Didn't even have to break into the graveyard for this doll.\" ",
            " \"Mmnh-\" ",
            " \"S-She's not dead!?\" ",
            " \"wh-\" ",
            " \"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\" ",
            "fffff",
            "CLINK",
            " ",
            "SLICE"
        };
        sfxPlayer = GetComponent<AudioSource>();
        StartScene();
    }

    // Update is called once per frame
     void Update()
     {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (slideText.text == dialogue[index])
            {
                NextSlide();
            }
            else
            {
                StopAllCoroutines();
                slideText.text = dialogue[index];
            }
        }
     }

    void StartScene()
    {
        index = 0;
        slideText.text = dialogue[index];
        slideImage.sprite = slides[index];
    }

    void NextSlide()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            slideText.text = string.Empty;
            sfxPlayer.Play();
            StartCoroutine(TypeLine());
            slideImage.sprite = slides[index];
        }
        else
        {
            EndScene();
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in dialogue[index].ToCharArray())
        {
            slideText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void EndScene()
    {
         SceneManager.LoadScene("SceneOne");
    }
}
