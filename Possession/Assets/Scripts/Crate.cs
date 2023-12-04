using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite boxFill;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = boxFill;
            // spriteRenderer.sprite = boxFill;
            this.gameObject.SetActive(false);
            collision.collider.isTrigger = true;
        }
    }
}
