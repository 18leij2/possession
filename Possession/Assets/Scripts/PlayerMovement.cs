using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] SpriteRenderer playerSprite;

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
    }
}
