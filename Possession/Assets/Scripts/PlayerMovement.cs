using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite skelester;
    [SerializeField] private Sprite ghoster;
    [SerializeField] private GameObject skelly;
    private bool possess = false;
    private bool skellyForm = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            // Debug.Log("left");
            transform.position = new Vector2(transform.position.x - moveSpeed, transform.position.y);
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Debug.Log("right");
            transform.position = new Vector2(transform.position.x + moveSpeed, transform.position.y);
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            // Debug.Log("up");
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Debug.Log("down");
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(possess);
            if (possess && !skellyForm) // if it is possible to possess right now
            {
                skellyForm = true;
                spriteRenderer.sprite = skelester;
                GetComponent<Collider2D>().isTrigger = false;

                // change the alpha transparency
                Color tmp = this.GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                this.GetComponent<SpriteRenderer>().color = tmp;

                skelly.SetActive(false);
            } else if (skellyForm)
            {
                skellyForm = false;
                spriteRenderer.sprite = ghoster;
                GetComponent<Collider2D>().isTrigger = true;

                // change the alpha transparency
                Color tmp2 = this.GetComponent<SpriteRenderer>().color;
                tmp2.a = 0.6f;
                this.GetComponent<SpriteRenderer>().color = tmp2;

                // respawn the bones
                skelly.transform.position = this.transform.position;
                skelly.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bones")
        {
            possess = true;
            skelly = collision.gameObject;
        }
        else if (collision.tag == "Goal")
        {
            if (SceneManager.GetActiveScene().buildIndex < 9)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }   
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Bones")
        {
            possess = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
        }
    }
}
