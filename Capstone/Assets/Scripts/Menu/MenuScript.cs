using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject titlescreen;
    //public GameObject howmanymenu;
    public GameObject characterselectmenu;
    public static Vector2Int menuSelect;
    public MyControllerInput myControllerInput;

    private Player Player1;// = MenuInputSelector.playerLists[0];

    private int totalMenuItemsX;
    private int totalMenuItemsY;
    private GameObject currentMenu;


    private void Awake()
    {
        //deactivate all menus besides titlescreen
        titlescreen.SetActive(true);
        mainmenu.SetActive(false);
        //howmanymenu.SetActive(false);
        characterselectmenu.SetActive(false);
        currentMenu = titlescreen;

        //set menu select variables to 0
        menuSelect.x = 0;
        menuSelect.y = 0;

        totalMenuItemsX = 1;
        totalMenuItemsY = 1;

       
    }


    // Start is called before the first frame update
    void Start()
    {
        //Player1 = MenuInputSelector.playerLists[0];
    }


    private void Update()
    {


        if(currentMenu != characterselectmenu)
        {
            menuNavigate();
        }




    }


    private void menuNavigate()
    {

        //if (Player1.myControllerInput.inputType == InputType.KEYBOARD) 
        //{ 

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                menuSelect.y = menuSelect.y + 1;
                if (menuSelect.y > totalMenuItemsY - 1)
                {
                    menuSelect.y = 0;
                }
                Debug.Log("menuSelect.y = " + menuSelect.y);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                menuSelect.y = menuSelect.y - 1;
                if (menuSelect.y < 0)
                {
                    menuSelect.y = totalMenuItemsY - 1;
                }
                Debug.Log("menuSelect.y = " + menuSelect.y);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                menuSelect.x = menuSelect.x + 1;
                if (menuSelect.x > totalMenuItemsX - 1)
                {
                    menuSelect.x = 0;
                }
                Debug.Log("menuSelect.x = " + menuSelect.x);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                menuSelect.x = menuSelect.x - 1;
                if (menuSelect.x < 0)
                {
                    menuSelect.x = totalMenuItemsX - 1;
                }
                Debug.Log("menuSelect.x = " + menuSelect.x);
            }

        /*}
        else
        {
            if (Input.GetButtonDown("UpButton"))
            {
                menuSelect.y = menuSelect.y + 1;
                if (menuSelect.y > totalMenuItemsY - 1)
                {
                    menuSelect.y = 0;
                }
                Debug.Log("menuSelect.y = " + menuSelect.y);
            }

            if (Input.GetButtonDown("DownButton"))
            {
                menuSelect.y = menuSelect.y - 1;
                if (menuSelect.y < 0)
                {
                    menuSelect.y = totalMenuItemsY - 1;
                }
                Debug.Log("menuSelect.y = " + menuSelect.y);
            }

            if (Input.GetButtonDown("RightButton"))
            {
                menuSelect.x = menuSelect.x + 1;
                if (menuSelect.x > totalMenuItemsX - 1)
                {
                    menuSelect.x = 0;
                }
                Debug.Log("menuSelect.x = " + menuSelect.x);
            }

            if (Input.GetButtonDown("LeftButton"))
            {
                menuSelect.x = menuSelect.x - 1;
                if (menuSelect.x < 0)
                {
                    menuSelect.x = totalMenuItemsX - 1;
                }
                Debug.Log("menuSelect.x = " + menuSelect.x);
            }
        }
        */
    }



    public void GotoTitleScreen()
    {
        menuSelect.x = 0;
        menuSelect.y = 0;
        titlescreen.SetActive(true);
        currentMenu.SetActive(false);
        currentMenu = titlescreen;
        totalMenuItemsX = 1;
        totalMenuItemsY = 1;
    }

    public void GotoMainMenu()
    {
        menuSelect.x = 0;
        menuSelect.y = 0;
        mainmenu.SetActive(true);
        currentMenu.SetActive(false);
        currentMenu = mainmenu;
        totalMenuItemsX = 1;
        totalMenuItemsY = 2;
    }


    public void GotoCharacterSelectMenu()
    {
        menuSelect.x = 0;
        menuSelect.y = 0;
        characterselectmenu.SetActive(true);
        currentMenu.SetActive(false);
        currentMenu = characterselectmenu;
        totalMenuItemsX = 1;
        totalMenuItemsY = 1;
    }

    public void GotoLevel(int howManyPlayers)
    {
        Settings.NumOfPlayers = howManyPlayers;
        Debug.Log("NumOfPlayers = " + Settings.NumOfPlayers);
        SceneManager.LoadScene("SampleScene");
    }


}
