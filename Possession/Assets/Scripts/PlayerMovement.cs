using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // NOTE - our grid is 0.9 per square which is annoying yes but keep that in mind

    [SerializeField] private float moveSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite skelester;
    [SerializeField] private Sprite ghoster;
    [SerializeField] private GameObject skelly;
    [SerializeField] private bool possess = false;
    [SerializeField] private bool skellyForm = false;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        // enable gizmos, shows raycast detection
        Debug.DrawRay(transform.position, Vector3.left * (float) 0.9, Color.green, 0);
        Debug.DrawRay(transform.position, Vector3.right * (float) 0.9, Color.green, 0);
        Debug.DrawRay(transform.position, Vector3.up * (float) 0.9, Color.green, 0);
        Debug.DrawRay(transform.position, Vector3.down * (float) 0.9, Color.green, 0);

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, (float) 0.9, LayerMask.GetMask("Obstacle"));

        // if (hit.collider != null)
        // {
        //     Debug.DrawRay(transform.position, Vector3.down * (float)0.9, Color.red, 0);
        // }

        // NOTE - this whole section needs to be updated with better logic - currently we are raycasting from the player's transform
        // but this takes into account any obstacle they're currently standing on. We should only be concerned with the next tile's 
        // info which is entirely doable if we just make a really small ray that starts and ends on the next tile. I however cannot
        // be bothered to refactor the code, and barring a few edge cases this won't introduce game breaking bugs (but one of such 
        // bugs includes having 3 boxes and 2 holes and 1 wall, filling both holes and then trying to push the 3rd box against the wall
        // will somehow not move the box, yet will move the player. Again this is all easily fixed with just... better logic... my bad lol
        if (!skellyForm)
        {
            // Debug.Log("left");
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Debug.Log("right");
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            // Debug.Log("up");
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Debug.Log("down");
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
        }
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(possess);
            if (possess && !skellyForm) // if it is possible to possess right now
            {
                skellyForm = true;
                spriteRenderer.sprite = skelester;

                transform.position = skelly.transform.position;

                // GetComponent<Collider2D>().isTrigger = false;

                // change the alpha transparency
                Color tmp = this.GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                this.GetComponent<SpriteRenderer>().color = tmp;

                skelly.SetActive(false);
            } else if (skellyForm && !possess)
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
        //else if (collision.tag == "Goal")
        //{
        //    if (SceneManager.GetActiveScene().buildIndex < 9)
        //    {
        //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //    }   
        //}
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

        else if (skellyForm && collision.gameObject.tag == "Goal")
        {
            if (SceneManager.GetActiveScene().buildIndex < 9)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
