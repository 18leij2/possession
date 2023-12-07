using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite boxFill;
    [SerializeField] private AudioSource sfxPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hole" && collision.collider.isTrigger == false)
        {
            sfxPlayer.Play();
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = boxFill;
            // spriteRenderer.sprite = boxFill;
            transform.position = new Vector3(-9999.9f, -9999.9f, -9999.9f);
            // this.gameObject.SetActive(false);
            collision.collider.isTrigger = true;
        }
    }

}
