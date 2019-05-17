using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressDeleteNameButton : MonoBehaviour
{
    
    public GameObject playerMenu;
    //private int buttonID;
    
    public Text text;

    /*
    // Start is called before the first frame update
    void Start()
    {
        buttonID = GetComponent<MenuPlayerButtonScript>().buttonID;
    }
    private void OnEnable()
    {
        Start();
    }
    */

    /*
    // Update is called once per frame
    void Update()
    {
        if (playerMenu.GetComponent<PlayerMenuScript>().currentButton == buttonID)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                AudioManager.Play("Joined");
                LoadProfileList.RemoveName(text.text);
                Debug.Log("Should have removed" + text.text);
                playerMenu.GetComponent<PlayerMenuScript>().GotoPreviousMenu();
            }
        }
    }
    */

    public void RemoveName()
    {

        if (LoadProfileList.checkForName(text.text) == true)
        {
            LoadProfileList.RemoveName(text.text);
            playerMenu.GetComponent<PlayerMenuScript>().GotoPreviousMenu();
            AudioManager.Play("Back");
        }
        else
        {
            AudioManager.Play("Wait");
        }
    }
}
