using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour {

    public Transform cursor;
    [SerializeField] public int currentOption;
    [SerializeField] public GameObject[] menuItems;

    private float dist;
    private float offset;

    [SerializeField] public KeyCode upKey;
    [SerializeField] public KeyCode downKey;
    [SerializeField] public KeyCode selectKey;

    // Start is called before the first frame update
    void Start() {
        selectKey = KeyCode.Space;

    } // Start

    // Update is called once per frame
    void Update() {
            
        if (Input.GetKeyDown(selectKey)) {
            SceneManager.LoadScene(0); // Title Screen
        
        } // if

    } // Update

} // MenuManager
