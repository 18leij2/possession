using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MenuManager : MonoBehaviour {

    public Transform cursor;
    [SerializeField] public int currentOption;
    [SerializeField] public GameObject[] menuItems;

    private float dist;
    private float offset;

    [SerializeField] public KeyCode upKey;
    [SerializeField] public KeyCode downKey;
    [SerializeField] public KeyCode selectKey;

    [SerializeField] public AudioSource sfxPlayer;
    [SerializeField] public AudioClip uiTap;
    [SerializeField] public AudioClip uiSelect;

    // Start is called before the first frame update
    void Start() {

        Debug.Log(SceneManager.GetActiveScene().buildIndex);

        cursor = this.transform.GetChild(0).transform;
        currentOption = 0;

        menuItems = new GameObject[5];
        for (int i = 0; i < 5; i++) {
            menuItems[i] = this.transform.GetChild(i + 1).gameObject;

        } // for

        dist = menuItems[1].transform.localPosition.y - menuItems[0].transform.localPosition.y;
        offset = 0.9f - menuItems[0].transform.localPosition.y;

        upKey = KeyCode.W;
        downKey = KeyCode.S;
        selectKey = KeyCode.Space;

        sfxPlayer = GetComponent<AudioSource>();

    } // Start

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(upKey)) {
            currentOption = Mathf.Clamp(currentOption - 1, 0, 4);
            sfxPlayer.volume = 1.0f;
            sfxPlayer.clip = uiTap;
            sfxPlayer.Play();
        
        } // if

        if (Input.GetKeyDown(downKey)) {
            currentOption = Mathf.Clamp(currentOption + 1, 0, 4);
            sfxPlayer.volume = 1.0f;
            sfxPlayer.clip = uiTap;
            sfxPlayer.Play();
        
        } // if

        if (Input.GetKeyDown(selectKey)) {
            OptionSelect();
            sfxPlayer.clip = uiSelect;
            sfxPlayer.volume = 0.2f;
            sfxPlayer.Play();
        
        } // if

        float y = menuItems[0].transform.localPosition.y + offset + (currentOption * dist);
        Vector3 cursorPos = new Vector3(-14.1f, y, -1.0f);
        cursor.localPosition = cursorPos;

    } // Update

    void OptionSelect() {

        switch (currentOption) {

            case 0: 
                // Debug.Log("Continue Game");
                SceneManager.LoadScene(1); // Level 1
                break;

            case 1:
                Debug.Log("Level Select");
                break;

            case 2: 
                // Debug.Log("New Game");
                SceneManager.LoadScene(11); // Opening
                break;

            case 3:
                // Debug.Log("Credits");
                SceneManager.LoadScene(10); // Credits
                break;

            case 4: 
                // Debug.Log("Exit");
                Application.Quit();
                break;

            default:
                break;

        } // switch

    } // OptionSelect
                
} // MenuManager
