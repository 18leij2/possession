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
    [SerializeField] private SpriteRenderer bookRenderer;
    [SerializeField] private SpriteRenderer dimRenderer;
    [SerializeField] private SpriteRenderer pageRenderer;
    [SerializeField] private Sprite skelester;
    [SerializeField] private Sprite ghoster;
    [SerializeField] private GameObject skelly;
    [SerializeField] private bool possess = false;
    [SerializeField] private bool skellyForm = false;
    [SerializeField] private bool book = false;
    [SerializeField] private bool bookPhase = false;

    [SerializeField] public AudioSource sfxPlayer;
    [SerializeField] public AudioClip possessSkellySFX;
    [SerializeField] public AudioClip dropSkellySFX;
    [SerializeField] public AudioClip skellyWalkSFX;

    // Start is called before the first frame update
    void Start()
    {
        sfxPlayer = GetComponent<AudioSource>();
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
        if (!skellyForm && !bookPhase)
        {
            if (Input.GetKey(KeyCode.A))
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
        }        
        else if (skellyForm && !bookPhase)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                // Debug.Log("left");
                RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, (float)0.9, LayerMask.GetMask("Obstacle"));
                if (hitLeft.collider != null)
                {
                    Debug.DrawRay(transform.position, Vector3.left * (float)0.9, Color.red, (float)0.1);
                    if (hitLeft.collider.gameObject.tag == "Wall")
                    {
                        Debug.Log("HIT A WALL");
                    }
                    else if (hitLeft.collider.gameObject.tag == "Goal")
                    {
                        if (SceneManager.GetActiveScene().buildIndex == 1)
                        {
                            SceneManager.LoadScene(12);
                        }
                        else if (SceneManager.GetActiveScene().buildIndex < 10)
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        }
                    } 
                    else if (hitLeft.collider.gameObject.tag == "Hole")
                    {
                        if (hitLeft.collider.isTrigger)
                        {
                            RaycastHit2D crateLeft = Physics2D.Raycast(transform.position, Vector3.left, (float)0.9, LayerMask.GetMask("Crate"));
                            if (crateLeft.collider != null)
                            {
                                GameObject crate = crateLeft.collider.gameObject;
                                crateLeft.collider.gameObject.transform.position = new Vector2(crate.transform.position.x - (float)0.9, transform.position.y);
                                crateLeft.collider.gameObject.GetComponent<Crate>().moving = true;
                            }

                            RaycastHit2D hitLeft2 = Physics2D.Raycast(new Vector2(transform.position.x + (float)0.9, transform.position.y), Vector3.left, (float)0.1, LayerMask.GetMask("Obstacle"));
                            if (hitLeft2.collider != null)
                            {
                                if (hitLeft2.collider.gameObject.tag == "Hole")
                                {
                                    if (hitLeft2.collider.isTrigger)
                                    {
                                        transform.position = new Vector2(transform.position.x - (float)0.9, transform.position.y);
                                        this.GetComponent<SpriteRenderer>().flipX = true;
                                        PlaySFX(skellyWalkSFX, 1.0f);
                                    }
                                }
                            } else
                            {
                                transform.position = new Vector2(transform.position.x - (float)0.9, transform.position.y);
                                this.GetComponent<SpriteRenderer>().flipX = true;
                                PlaySFX(skellyWalkSFX, 1.0f);
                            }
                        }
                    }
                }
                else
                {
                    RaycastHit2D crateLeft = Physics2D.Raycast(transform.position, Vector3.left, (float)0.9, LayerMask.GetMask("Crate"));
                    if (crateLeft.collider != null)
                    {
                        RaycastHit2D hitLeftExtra = Physics2D.Raycast(transform.position, Vector3.left, (float)1.8, LayerMask.GetMask("Obstacle"));
                        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector3.left, (float)1.8, LayerMask.GetMask("Crate"));
                        if (hitLeftExtra.collider != null) {
                            if (hitLeftExtra.collider.gameObject.tag == "Hole")
                            {
                                transform.position = new Vector2(transform.position.x - (float)0.9, transform.position.y);
                                this.GetComponent<SpriteRenderer>().flipX = true;
                                PlaySFX(skellyWalkSFX, 1.0f);

                                GameObject crate = crateLeft.collider.gameObject;
                                crateLeft.collider.gameObject.transform.position = new Vector2(crate.transform.position.x - (float)0.9, transform.position.y);
                                crateLeft.collider.gameObject.GetComponent<Crate>().moving = true;
                            }
                            else
                            {
                                Debug.Log("HIT A WALL THROUGH CRATE");
                            }                 
                        }
                        else if (hits.Length > 1)
                        {
                            Debug.Log("HIT A CRATE THROUGH CRATE");
                        }
                        else
                        {
                            transform.position = new Vector2(transform.position.x - (float)0.9, transform.position.y);
                            this.GetComponent<SpriteRenderer>().flipX = true;
                            PlaySFX(skellyWalkSFX, 1.0f);

                            GameObject crate = crateLeft.collider.gameObject;
                            crateLeft.collider.gameObject.transform.position = new Vector2(crate.transform.position.x - (float)0.9, transform.position.y);
                            crateLeft.collider.gameObject.GetComponent<Crate>().moving = true;
                        }
                    }
                    else
                    {
                        transform.position = new Vector2(transform.position.x - (float)0.9, transform.position.y);
                        this.GetComponent<SpriteRenderer>().flipX = true;
                        PlaySFX(skellyWalkSFX, 1.0f);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                // Debug.Log("right");
                RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, (float)0.9, LayerMask.GetMask("Obstacle"));
                if (hitRight.collider != null)
                {
                    Debug.DrawRay(transform.position, Vector3.right * (float)0.9, Color.red, (float)0.1);
                    if (hitRight.collider.gameObject.tag == "Wall")
                    {
                        Debug.Log("HIT A WALL");
                    }
                    else if (hitRight.collider.gameObject.tag == "Goal")
                    {
                        if (SceneManager.GetActiveScene().buildIndex == 1)
                        {
                            SceneManager.LoadScene(12);
                        }
                        else if (SceneManager.GetActiveScene().buildIndex < 10)
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        }
                    }
                    else if (hitRight.collider.gameObject.tag == "Hole")
                    {
                        if (hitRight.collider.isTrigger)
                        {
                            RaycastHit2D crateRight = Physics2D.Raycast(transform.position, Vector3.right, (float)0.9, LayerMask.GetMask("Crate"));
                            if (crateRight.collider != null)
                            {
                                GameObject crate = crateRight.collider.gameObject;
                                crateRight.collider.gameObject.transform.position = new Vector2(crate.transform.position.x + (float)0.9, transform.position.y);
                                crateRight.collider.gameObject.GetComponent<Crate>().moving = true;
                            }

                            RaycastHit2D hitRight2 = Physics2D.Raycast(new Vector2(transform.position.x + (float)0.9, transform.position.y), Vector3.right, (float)0.1, LayerMask.GetMask("Obstacle"));
                            if (hitRight2.collider != null)
                            {
                                if (hitRight2.collider.gameObject.tag == "Hole")
                                {
                                    if (hitRight2.collider.isTrigger)
                                    {
                                        transform.position = new Vector2(transform.position.x + (float)0.9, transform.position.y);
                                        this.GetComponent<SpriteRenderer>().flipX = false;
                                        PlaySFX(skellyWalkSFX, 1.0f);
                                    }
                                }
                            } else
                            {
                                transform.position = new Vector2(transform.position.x + (float)0.9, transform.position.y);
                                this.GetComponent<SpriteRenderer>().flipX = false;
                                PlaySFX(skellyWalkSFX, 1.0f);
                            }
                        }
                    }
                }
                else
                {
                    RaycastHit2D crateRight = Physics2D.Raycast(transform.position, Vector3.right, (float)0.9, LayerMask.GetMask("Crate"));
                    if (crateRight.collider != null)
                    {
                        RaycastHit2D hitRightExtra = Physics2D.Raycast(transform.position, Vector3.right, (float)1.8, LayerMask.GetMask("Obstacle"));
                        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector3.right, (float)1.8, LayerMask.GetMask("Crate"));
                        if (hitRightExtra.collider != null)
                        {
                            if (hitRightExtra.collider.gameObject.tag == "Hole")
                            {
                                transform.position = new Vector2(transform.position.x + (float)0.9, transform.position.y);
                                this.GetComponent<SpriteRenderer>().flipX = false;
                                PlaySFX(skellyWalkSFX, 1.0f);

                                GameObject crate = crateRight.collider.gameObject;
                                crateRight.collider.gameObject.transform.position = new Vector2(crate.transform.position.x + (float)0.9, transform.position.y);
                                crateRight.collider.gameObject.GetComponent<Crate>().moving = true;
                            }
                            else
                            {
                                Debug.Log("HIT A WALL THROUGH CRATE");
                            }
                        } 
                        else if (hits.Length > 1)
                        {
                            Debug.Log("HIT A CRATE THROUGH CRATE");
                        }
                        else
                        {
                            transform.position = new Vector2(transform.position.x + (float)0.9, transform.position.y);
                            this.GetComponent<SpriteRenderer>().flipX = false;
                            PlaySFX(skellyWalkSFX, 1.0f);

                            GameObject crate = crateRight.collider.gameObject;
                            crateRight.collider.gameObject.transform.position = new Vector2(crate.transform.position.x + (float)0.9, transform.position.y);
                            crateRight.collider.gameObject.GetComponent<Crate>().moving = true;
                        }
                    }
                    else
                    {
                        transform.position = new Vector2(transform.position.x + (float)0.9, transform.position.y);
                        this.GetComponent<SpriteRenderer>().flipX = false;
                        PlaySFX(skellyWalkSFX, 1.0f);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                // Debug.Log("up");
                RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector3.up, (float)0.9, LayerMask.GetMask("Obstacle"));
                if (hitUp.collider != null)
                {
                    Debug.DrawRay(transform.position, Vector3.up * (float)0.9, Color.red, (float)0.1);
                    if (hitUp.collider.gameObject.tag == "Wall")
                    {
                        Debug.Log("HIT A WALL");
                    }
                    else if (hitUp.collider.gameObject.tag == "Goal")
                    {
                        if (SceneManager.GetActiveScene().buildIndex == 1)
                        {
                            SceneManager.LoadScene(12);
                        }
                        else if (SceneManager.GetActiveScene().buildIndex < 10)
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        }
                    }
                    else if (hitUp.collider.gameObject.tag == "Hole")
                    {
                        if (hitUp.collider.isTrigger)
                        {
                            RaycastHit2D crateUp = Physics2D.Raycast(transform.position, Vector3.up, (float)0.9, LayerMask.GetMask("Crate"));
                            if (crateUp.collider != null)
                            {
                                GameObject crate = crateUp.collider.gameObject;
                                crateUp.collider.gameObject.transform.position = new Vector2(crate.transform.position.x, transform.position.y + (float)0.9);
                                crateUp.collider.gameObject.GetComponent<Crate>().moving = true;
                            }

                            RaycastHit2D hitUp2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + (float)0.9), Vector3.up, (float)0.1, LayerMask.GetMask("Obstacle"));
                            if (hitUp2.collider != null)
                            {
                                if (hitUp2.collider.gameObject.tag == "Hole")
                                {
                                    if (hitUp2.collider.isTrigger)
                                    {
                                        transform.position = new Vector2(transform.position.x, transform.position.y + (float)0.9);
                                        PlaySFX(skellyWalkSFX, 1.0f);
                                    }
                                }
                            } else
                            {
                                transform.position = new Vector2(transform.position.x, transform.position.y + (float)0.9);
                                PlaySFX(skellyWalkSFX, 1.0f);
                            }
                        }
                    }
                }
                else
                {
                    RaycastHit2D crateUp = Physics2D.Raycast(transform.position, Vector3.up, (float)0.9, LayerMask.GetMask("Crate"));
                    if (crateUp.collider != null)
                    {
                        RaycastHit2D hitUpExtra = Physics2D.Raycast(transform.position, Vector3.up, (float)1.8, LayerMask.GetMask("Obstacle"));
                        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector3.up, (float)1.8, LayerMask.GetMask("Crate"));
                        if (hitUpExtra.collider != null)
                        {
                            if (hitUpExtra.collider.gameObject.tag == "Hole")
                            {
                                transform.position = new Vector2(transform.position.x, transform.position.y + (float)0.9);
                                PlaySFX(skellyWalkSFX, 1.0f);

                                GameObject crate = crateUp.collider.gameObject;
                                crateUp.collider.gameObject.transform.position = new Vector2(crate.transform.position.x, transform.position.y + (float)0.9);
                                crateUp.collider.gameObject.GetComponent<Crate>().moving = true;
                            }
                            else
                            {
                                Debug.Log("HIT A WALL THROUGH CRATE");
                            }
                        }
                        else if (hits.Length > 1)
                        {
                            Debug.Log("HIT A CRATE THROUGH CRATE");
                        }
                        else
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y + (float)0.9);
                            PlaySFX(skellyWalkSFX, 1.0f);

                            GameObject crate = crateUp.collider.gameObject;
                            crateUp.collider.gameObject.transform.position = new Vector2(crate.transform.position.x, transform.position.y + (float)0.9);
                            crateUp.collider.gameObject.GetComponent<Crate>().moving = true;
                        }
                    }
                    else
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y + (float)0.9);
                        PlaySFX(skellyWalkSFX, 1.0f);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                // Debug.Log("down");
                RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector3.down, (float) 0.9, LayerMask.GetMask("Obstacle"));
                if (hitDown.collider != null)
                {
                    Debug.DrawRay(transform.position, Vector3.down * (float)0.9, Color.red, (float)0.1);
                    if (hitDown.collider.gameObject.tag == "Wall")
                    {
                        Debug.Log("HIT A WALL");
                    }
                    else if (hitDown.collider.gameObject.tag == "Goal")
                    {
                        if (SceneManager.GetActiveScene().buildIndex == 1)
                        {
                            SceneManager.LoadScene(12);
                        }
                        else if (SceneManager.GetActiveScene().buildIndex < 10)
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        }
                    }
                    else if (hitDown.collider.gameObject.tag == "Hole")
                    {
                
                        if (hitDown.collider.isTrigger)
                        {
                            RaycastHit2D crateDown = Physics2D.Raycast(transform.position, Vector3.down, (float)0.9, LayerMask.GetMask("Crate"));
                            if (crateDown.collider != null)
                            {
                                GameObject crate = crateDown.collider.gameObject;
                                crateDown.collider.gameObject.transform.position = new Vector2(crate.transform.position.x, transform.position.y - (float)0.9);
                                crateDown.collider.gameObject.GetComponent<Crate>().moving = true;
                            }

                            RaycastHit2D hitDown2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (float)0.9), Vector3.down, (float)0.1, LayerMask.GetMask("Obstacle"));
                            if (hitDown2.collider != null)
                            {
                                if (hitDown2.collider.gameObject.tag == "Hole")
                                {
                                    if (hitDown2.collider.isTrigger)
                                    {
                                        transform.position = new Vector2(transform.position.x, transform.position.y - (float)0.9);
                                        PlaySFX(skellyWalkSFX, 1.0f);
                                    }
                                }
                            } else
                            {
                                transform.position = new Vector2(transform.position.x, transform.position.y - (float)0.9);
                                PlaySFX(skellyWalkSFX, 1.0f);
                            }                      
                        }
                    }
                }
                else
                {
                    RaycastHit2D crateDown = Physics2D.Raycast(transform.position, Vector3.down, (float)0.9, LayerMask.GetMask("Crate"));
                    if (crateDown.collider != null)
                    {
                        RaycastHit2D hitDownExtra = Physics2D.Raycast(transform.position, Vector3.down, (float)1.8, LayerMask.GetMask("Obstacle"));
                        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector3.down, (float)1.8, LayerMask.GetMask("Crate"));
                        if (hitDownExtra.collider != null)
                        {
                            if (hitDownExtra.collider.gameObject.tag == "Hole")
                            {
                                transform.position = new Vector2(transform.position.x, transform.position.y - (float)0.9);
                                PlaySFX(skellyWalkSFX, 1.0f);

                                GameObject crate = crateDown.collider.gameObject;
                                crateDown.collider.gameObject.transform.position = new Vector2(crate.transform.position.x, transform.position.y - (float)0.9);
                                crateDown.collider.gameObject.GetComponent<Crate>().moving = true;
                            }
                            else
                            {
                                Debug.Log("HIT A WALL THROUGH CRATE");
                            }
                        }
                        else if (hits.Length > 1)
                        {
                            Debug.Log("HIT A CRATE THROUGH CRATE");
                        }
                        else
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y - (float)0.9);
                            PlaySFX(skellyWalkSFX, 1.0f);

                            GameObject crate = crateDown.collider.gameObject;
                            crateDown.collider.gameObject.transform.position = new Vector2(crate.transform.position.x, transform.position.y - (float)0.9);
                            crateDown.collider.gameObject.GetComponent<Crate>().moving = true;
                        }
                    }
                    else
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y - (float)0.9);
                        PlaySFX(skellyWalkSFX, 1.0f);
                    }
                }
            }
        }
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(possess);
            if (book)
            {
                if (!bookPhase)
                {
                    dimRenderer.enabled = true;
                    bookRenderer.enabled = true;
                    bookPhase = true;
                }
                else if (bookPhase && pageRenderer.enabled == false)
                {
                    bookRenderer.enabled = false;
                    pageRenderer.enabled = true;
                }
                else if (bookPhase && pageRenderer.enabled == true)
                {
                    dimRenderer.enabled = false;
                    bookRenderer.enabled = false;
                    pageRenderer.enabled = false;
                    bookPhase = false;
                }
            }
            else if (possess && !skellyForm) // if it is possible to possess right now
            {
                skellyForm = true;
                spriteRenderer.sprite = skelester;

                this.gameObject.GetComponent<Animator>().enabled = false;

                transform.position = skelly.transform.position;
                PlaySFX(possessSkellySFX, 1.0f);

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

                this.gameObject.GetComponent<Animator>().enabled = true;

                // change the alpha transparency
                Color tmp2 = this.GetComponent<SpriteRenderer>().color;
                tmp2.a = 0.6f;
                this.GetComponent<SpriteRenderer>().color = tmp2;

                // respawn the bones
                skelly.transform.position = this.transform.position;
                skelly.SetActive(true);
                PlaySFX(dropSkellySFX, 1.0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bones")
        {
            possess = true;

            if (!skellyForm)
            {
                skelly = collision.gameObject;
            }           
        }
        else if (collision.tag == "Book")
        {
            book = true;
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
        else if (collision.tag == "Book")
        {
            book = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
        }

        else if (skellyForm && collision.gameObject.tag == "Goal")
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                SceneManager.LoadScene(12);
            }
            else if (SceneManager.GetActiveScene().buildIndex < 10)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxPlayer.volume = volume;
        sfxPlayer.clip = clip;
        sfxPlayer.Play();
    }
}
