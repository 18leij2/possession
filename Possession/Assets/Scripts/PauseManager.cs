using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    
    [SerializeField] public bool paused;
    [SerializeField] public bool wait;
    [SerializeField] public KeyCode pauseKey;
    
    // Start is called before the first frame update
    void Start() {
        wait = false;
        paused = false;
        pauseKey = KeyCode.Space;

    } // Start
 
    // Update is called once per frame
    void Update() {

        if (!wait && !paused && Input.GetKeyDown(pauseKey)) {
            paused = true;

        } // if

        wait = false;

        int scale = (paused) ? 1 : 0;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);

    } // Update

} // PauseManager
