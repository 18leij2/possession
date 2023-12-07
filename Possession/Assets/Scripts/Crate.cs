using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite boxFill;
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private AudioClip slide;
    [SerializeField] private AudioClip thunk;
    [SerializeField] public bool moving;
    
    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        sfxPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) PlaySFX(slide, 1.0f);
        moving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hole" && collision.collider.isTrigger == false)
        {
            PlaySFX(thunk, 1.0f);
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = boxFill;
            // spriteRenderer.sprite = boxFill;
            transform.position = new Vector3(-9999.9f, -9999.9f, -9999.9f);
            // this.gameObject.SetActive(false);
            collision.collider.isTrigger = true;
        }
    }

    private void PlaySFX(AudioClip clip, float volume)
    {
        sfxPlayer.volume = volume;
        sfxPlayer.clip = clip;
        sfxPlayer.Play();
    }

}
