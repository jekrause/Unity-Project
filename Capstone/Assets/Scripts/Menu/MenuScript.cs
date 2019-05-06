using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject titlescreen;
    public GameObject optionsmenu;
    //public GameObject howmanymenu;
    public GameObject characterselectmenu;
    public static Vector2Int menuSelect;
    public MyControllerInput myControllerInput;

    private Player Player1;// = MenuInputSelector.playerLists[0];

    private int totalMenuItemsX;
    private int totalMenuItemsY;
    private GameObject currentMenu;
    private GameObject previousMenu;
    private string OS = Settings.OS;

    private bool backButtonReleased = true;

    private int player1InputAssigned = 0;




    private void Awake()
    {
        UnAssignAllInputs();    //always reset all inputs when first loading titlescreen
        //deactivate all menus besides titlescreen
        titlescreen.SetActive(true);
        mainmenu.SetActive(false);
        //howmanymenu.SetActive(false);
        characterselectmenu.SetActive(false);
        optionsmenu.SetActive(false);
        currentMenu = titlescreen;
        previousMenu = currentMenu;

        //set menu select variables to 0
        menuSelect.x = 0;
        menuSelect.y = 0;

        totalMenuItemsX = 1;
        totalMenuItemsY = 1;
        backButtonReleased = true;
       
    }


    // Start is called before the first frame update
    void Start()
    {
        //Player1 = MenuInputSelector.playerLists[0];
    }


    private void Update()
    {

        if (currentMenu == titlescreen)
        {
            BindPlayer1();
        }
        else if (currentMenu == optionsmenu)
        {
            OptionsNavigate(0);
        }
        else if (currentMenu == mainmenu)
        {
            menuNavigate();
        }

        if (currentMenu != characterselectmenu  && backButtonReleased)
        {
            BackButtonChecker();
        }
        
        if (backButtonReleased == false)
        {
            CheckForBackButtonRelease();
        }
        

    }

    private bool BackButtonisPressed()
    {
        if (MenuInputSelector.menuControl[0] != null)
        {
            if (MenuInputSelector.menuControl[0].inputType == InputType.KEYBOARD)
            {

                if (Input.GetKey(KeyCode.Escape))
                {
                    return true;
                }
            }
            else
            {
                if (Input.GetButton(MenuInputSelector.menuControl[0].RightButton))
                {
                    return true;
                }

            }

        }

        return false;
    }

    private void CheckForBackButtonRelease()
    {
        if (MenuInputSelector.menuControl[0] != null)
        {
            if (MenuInputSelector.menuControl[0].inputType == InputType.KEYBOARD)
            {

                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    backButtonReleased = true;
                }
            }
            else
            {
                if (Input.GetButtonUp(MenuInputSelector.menuControl[0].RightButton))
                {
                    backButtonReleased = true;
                }

            }

        }

    }

    private void BackButtonChecker()
    {
        if (MenuInputSelector.menuControl[0] != null)
        {
            if (MenuInputSelector.menuControl[0].inputType == InputType.KEYBOARD)
            {

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gotoPreviousMenu();
                }
            }
            else
            {
                if (Input.GetButtonDown(MenuInputSelector.menuControl[0].RightButton))
                {
                    gotoPreviousMenu();
                }

            }

        }
    }

   

    private void OptionsNavigate(int playerIndex)
    {
        if (MenuInputSelector.menuControl[playerIndex] != null)
        {
            bool isPressed = false; //if a button is pressed than play sound

            if (MenuInputSelector.menuControl[playerIndex].inputType == InputType.KEYBOARD)
            {

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    int oldMenuSelect = menuSelect.y;
                    menuSelect.y = menuSelect.y + 1;
                    if (menuSelect.y > totalMenuItemsY - 1)
                    {
                        menuSelect.y = 0;
                    }
                    if (oldMenuSelect != menuSelect.y)
                    {
                        isPressed = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    int oldMenuSelect = menuSelect.y;
                    menuSelect.y = menuSelect.y - 1;
                    if (menuSelect.y < 0)
                    {
                        menuSelect.y = totalMenuItemsY - 1;
                    }
                    if (oldMenuSelect != menuSelect.y)
                    {
                        isPressed = true;
                    }
                }


                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (menuSelect.y == 0)
                    {
                        isPressed = Settings.IncreaseMasterVolume();
                    }else if (menuSelect.y == 1)
                    {
                        isPressed = Settings.IncreaseSFXVolume();
                    }
                    else
                    {
                        isPressed = Settings.IncreaseMusicVolume();
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (menuSelect.y == 0)
                    {
                        isPressed = Settings.DecreaseMasterVolume();
                    }
                    else if (menuSelect.y == 1)
                    {
                        isPressed = Settings.DecreaseSFXVolume();
                    }
                    else
                    {
                        isPressed = Settings.DecreaseMusicVolume();
                    }
                }

            }
            else
            {
                if (OS.Equals("Mac"))
                {

                    
                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadUp_Mac))
                    {
                        int oldMenuSelect = menuSelect.y;
                        menuSelect.y = menuSelect.y + 1;
                        if (menuSelect.y > totalMenuItemsY - 1)
                        {
                            menuSelect.y = 0;
                        }
                        if (oldMenuSelect != menuSelect.y)
                        {
                            isPressed = true;
                        }
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadDown_Mac))
                    {
                        int oldMenuSelect = menuSelect.y;
                        menuSelect.y = menuSelect.y - 1;
                        if (menuSelect.y < 0)
                        {
                            menuSelect.y = totalMenuItemsY - 1;
                        }
                        if (oldMenuSelect != menuSelect.y)
                        {
                            isPressed = true;
                        }
                    }
                    

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadRight_Mac))
                    {
                        if (menuSelect.y == 0)
                        {
                            isPressed = Settings.IncreaseMasterVolume();
                        }
                        else if (menuSelect.y == 1)
                        {
                            isPressed = Settings.IncreaseSFXVolume();
                        }
                        else
                        {
                            isPressed = Settings.IncreaseMusicVolume();
                        }
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadLeft_Mac))
                    {
                        if (menuSelect.y == 0)
                        {
                            isPressed = Settings.DecreaseMasterVolume();
                        }
                        else if (menuSelect.y == 1)
                        {
                            isPressed = Settings.DecreaseSFXVolume();
                        }
                        else
                        {
                            isPressed = Settings.DecreaseMusicVolume();
                        }
                    }
                }
                else
                {
                    // for xbox(windows) and PS4

                    if (!PlayerMenuScript.playerAxisInUse[playerIndex] && MenuInputSelector.menuControl[playerIndex] != null)
                    {
                        PlayerMenuScript.playerAxisInUse[playerIndex] = true;
                        
                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) > 0)
                        {
                            int oldMenuSelect = menuSelect.y;
                            menuSelect.y = menuSelect.y + 1;
                            if (menuSelect.y > totalMenuItemsY - 1)
                            {
                                menuSelect.y = 0;
                            }
                            if (oldMenuSelect != menuSelect.y)
                            {
                                isPressed = true;
                            }
                        }
                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) < 0)
                        {
                            int oldMenuSelect = menuSelect.y;
                            menuSelect.y = menuSelect.y - 1;
                            if (menuSelect.y < 0)
                            {
                                menuSelect.y = totalMenuItemsY - 1;
                            }
                            if (oldMenuSelect != menuSelect.y)
                            {
                                isPressed = true;
                            }
                        }
                        

                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) > 0)
                        {
                                if (menuSelect.y == 0)
                                {
                                    isPressed = Settings.IncreaseMasterVolume();
                                }
                                else if (menuSelect.y == 1)
                                {
                                    isPressed = Settings.IncreaseSFXVolume();
                                }
                                else
                                {
                                    isPressed = Settings.IncreaseMusicVolume();
                                }
                        }

                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) < 0)
                        {
                                if (menuSelect.y == 0)
                                {
                                    isPressed = Settings.DecreaseMasterVolume();
                                }
                                else if (menuSelect.y == 1)
                                {
                                    isPressed = Settings.DecreaseSFXVolume();
                                }
                                else
                                {
                                    isPressed = Settings.DecreaseMusicVolume();
                                }
                        }

                    }

                    if (MenuInputSelector.menuControl[playerIndex] != null
                        && (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) == 0 && Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) == 0))
                    {
                        // if player is not pressing any axis, reset boolean to allow us to check user input again. 
                        // The player shouldn't be pressing more than 1 D-Pad button at the same time when searching.
                        PlayerMenuScript.playerAxisInUse[playerIndex] = false;
                    }

                }

            }

            if (isPressed == true)
            {
                AudioManager.Play("Move");
                Settings.PrintAudioVolumes();
            }

        }
    }

    private void pressDirection(int playerIndex)
    {
        if (MenuInputSelector.menuControl[playerIndex] != null)
        {
            bool isPressed = false; //if a button is pressed than play sound

            if (MenuInputSelector.menuControl[playerIndex].inputType == InputType.KEYBOARD)
            {

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    int oldMenuSelect = menuSelect.y;
                    menuSelect.y = menuSelect.y + 1;
                    if (menuSelect.y > totalMenuItemsY - 1)
                    {
                        menuSelect.y = 0;
                    }
                    if (oldMenuSelect != menuSelect.y)
                    {
                        isPressed = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    int oldMenuSelect = menuSelect.y;
                    menuSelect.y = menuSelect.y - 1;
                    if (menuSelect.y < 0)
                    {
                        menuSelect.y = totalMenuItemsY - 1;
                    }
                    if (oldMenuSelect != menuSelect.y)
                    {
                        isPressed = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    int oldMenuSelect = menuSelect.x;
                    menuSelect.x = menuSelect.x + 1;
                    if (menuSelect.x > totalMenuItemsX - 1)
                    {
                        menuSelect.x = 0;
                    }
                    if (oldMenuSelect != menuSelect.x)
                    {
                        isPressed = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    int oldMenuSelect = menuSelect.x;
                    menuSelect.x = menuSelect.x - 1;
                    if (menuSelect.x < 0)
                    {
                        menuSelect.x = totalMenuItemsX - 1;
                    }
                    if (oldMenuSelect != menuSelect.x)
                    {
                        isPressed = true;
                    }
                }

            }
            else
            {
                if (OS.Equals("Mac"))
                {


                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadUp_Mac))
                    {
                        int oldMenuSelect = menuSelect.y;
                        menuSelect.y = menuSelect.y + 1;
                        if (menuSelect.y > totalMenuItemsY - 1)
                        {
                            menuSelect.y = 0;
                        }
                        if (oldMenuSelect != menuSelect.y)
                        {
                            isPressed = true;
                        }
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadDown_Mac))
                    {
                        int oldMenuSelect = menuSelect.y;
                        menuSelect.y = menuSelect.y - 1;
                        if (menuSelect.y < 0)
                        {
                            menuSelect.y = totalMenuItemsY - 1;
                        }
                        if (oldMenuSelect != menuSelect.y)
                        {
                            isPressed = true;
                        }
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadRight_Mac))
                    {
                        int oldMenuSelect = menuSelect.x;
                        menuSelect.x = menuSelect.x + 1;
                        if (menuSelect.x > totalMenuItemsX - 1)
                        {
                            menuSelect.x = 0;
                        }
                        if (oldMenuSelect != menuSelect.x)
                        {
                            isPressed = true;
                        }
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadLeft_Mac))
                    {
                        int oldMenuSelect = menuSelect.x;
                        menuSelect.x = menuSelect.x - 1;
                        if (menuSelect.x < 0)
                        {
                            menuSelect.x = totalMenuItemsX - 1;
                        }
                        if (oldMenuSelect != menuSelect.x)
                        {
                            isPressed = true;
                        }
                    }
                }
                else
                {
                    // for xbox(windows) and PS4

                    if (!PlayerMenuScript.playerAxisInUse[playerIndex] && MenuInputSelector.menuControl[playerIndex] != null)
                    {
                        PlayerMenuScript.playerAxisInUse[playerIndex] = true;
                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) > 0)
                        {
                            int oldMenuSelect = menuSelect.y;
                            menuSelect.y = menuSelect.y + 1;
                            if (menuSelect.y > totalMenuItemsY - 1)
                            {
                                menuSelect.y = 0;
                            }
                            if (oldMenuSelect != menuSelect.y)
                            {
                                isPressed = true;
                            }
                        }
                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) < 0)
                        {
                            int oldMenuSelect = menuSelect.y;
                            menuSelect.y = menuSelect.y - 1;
                            if (menuSelect.y < 0)
                            {
                                menuSelect.y = totalMenuItemsY - 1;
                            }
                            if (oldMenuSelect != menuSelect.y)
                            {
                                isPressed = true;
                            }
                        }

                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) > 0)
                        {
                            int oldMenuSelect = menuSelect.x;
                            menuSelect.x = menuSelect.x + 1;
                            if (menuSelect.x > totalMenuItemsX - 1)
                            {
                                menuSelect.x = 0;
                            }
                            if (oldMenuSelect != menuSelect.x)
                            {
                                isPressed = true;
                            }
                        }

                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) < 0)
                        {
                            int oldMenuSelect = menuSelect.x;
                            menuSelect.x = menuSelect.x - 1;
                            if (menuSelect.x < 0)
                            {
                                menuSelect.x = totalMenuItemsX - 1;
                            }
                            if (oldMenuSelect != menuSelect.x)
                            {
                                isPressed = true;
                            }
                        }

                    }

                    if (MenuInputSelector.menuControl[playerIndex] != null
                        && (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) == 0 && Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) == 0))
                    {
                        // if player is not pressing any axis, reset boolean to allow us to check user input again. 
                        // The player shouldn't be pressing more than 1 D-Pad button at the same time when searching.
                        PlayerMenuScript.playerAxisInUse[playerIndex] = false;
                    }

                }

            }

            if (isPressed == true)
            {
                AudioManager.Play("Move");
            }

        }
    }

    private void menuNavigate()
    {
        pressDirection(0);
    }



    public void GotoTitleScreen()
    {
        AudioManager.Play("Back");
        previousMenu = titlescreen;
        menuSelect.x = 0;
        menuSelect.y = 0;
        titlescreen.SetActive(true);
        currentMenu.SetActive(false);
        currentMenu = titlescreen;
        totalMenuItemsX = 1;
        totalMenuItemsY = 1;
        backButtonReleased = true;
        //MenuInputSelector.menuControl[0] = null;    //set player 1 to null
        UnAssignAllInputs();    //unassign all inputs to restart input selection

    }

    public void GotoMainMenu()
    {
        if (currentMenu == titlescreen)
        {
            AudioManager.Play("Select");
        }
        else
        {
            AudioManager.Play("Back");
            if (BackButtonisPressed())
            {
                backButtonReleased = false;
            }
            
        }
        previousMenu = titlescreen;
        menuSelect.x = 0;
        menuSelect.y = 0;
        mainmenu.SetActive(true);
        currentMenu.SetActive(false);
        currentMenu = mainmenu;
        totalMenuItemsX = 1;
        totalMenuItemsY = 3;
    }


    public void GotoCharacterSelectMenu()
    {
        AudioManager.Play("Select");
        previousMenu = mainmenu;
        menuSelect.x = 0;
        menuSelect.y = 0;
        characterselectmenu.SetActive(true);
        currentMenu.SetActive(false);
        currentMenu = characterselectmenu;
        totalMenuItemsX = 1;
        totalMenuItemsY = 1;
    }

    public void GotoOptionsMenu()
    {
        AudioManager.Play("Select");
        previousMenu = mainmenu;
        menuSelect.x = 0;
        menuSelect.y = 0;
        optionsmenu.SetActive(true);
        currentMenu.SetActive(false);
        currentMenu = optionsmenu;
        totalMenuItemsX = 1;
        totalMenuItemsY = 3;
    }

    public void GotoLevel(int howManyPlayers)
    {
        Settings.NumOfPlayers = howManyPlayers;
        Debug.Log("NumOfPlayers = " + Settings.NumOfPlayers);
        SceneManager.LoadScene("SampleScene");
    }

    public void gotoPreviousMenu()
    {
        if (previousMenu == titlescreen)
        {
            if (currentMenu != previousMenu)
            {
                GotoTitleScreen();
            }
        }
        else if(previousMenu == mainmenu)
        {
            GotoMainMenu();
        }
    }

    public void QuitGame()
    {
        //Quits the game when running a build
        Application.Quit();

        //Quits game within unity editor
        //UnityEditor.EditorApplication.isPlaying = false;
    }


    private void BindPlayer1()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Settings.inputAssigned[0])
        {
            print("Keyboard Detected");
            //bind to player inputs to keyboard
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.KEYBOARD, 1);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.KEYBOARD, 1);
            Settings.inputAssigned[0] = true;
            player1InputAssigned = 0;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (Keyboard input)";
        }
        else if (Input.GetButtonDown("J1PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[1])
        {
            print("PS4 Joystick 1 Detected");
            //bind player's inputs to PS4 controller
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.PS4_CONTROLLER, 1);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 1);
            Settings.inputAssigned[1] = true;
            player1InputAssigned = 1;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";

        }
        else if (Input.GetButtonDown("J2PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[2])
        {
            print("PS4 Joystick 2 detected");
            //bind to player inputs to PS4 2nd controller
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.PS4_CONTROLLER, 2);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 2);
            Settings.inputAssigned[2] = true;
            player1InputAssigned = 2;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";

        }
        else if (Input.GetButtonDown("J3PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("PS4 Joystick 3 detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.PS4_CONTROLLER, 3);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 3);
            Settings.inputAssigned[3] = true;
            player1InputAssigned = 3;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";
        }
        else if (Input.GetButtonDown("J4PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("PS4 Joystick 4 detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.PS4_CONTROLLER, 4);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 4);
            Settings.inputAssigned[4] = true;
            player1InputAssigned = 4;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";
        }
        else if (Input.GetButtonDown("J1XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[1])
        {
            print("XBOX Joystick 1 Detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.XBOX_CONTROLLER, 1);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 1);
            Settings.inputAssigned[1] = true;
            player1InputAssigned = 1;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButtonDown("J2XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[2])
        {
            print("XBOX Joystick 2 Detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.XBOX_CONTROLLER, 2);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 2);
            Settings.inputAssigned[2] = true;
            player1InputAssigned = 2;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButtonDown("J3XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("XBOX Joystick 3 Detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.XBOX_CONTROLLER, 3);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 3);
            Settings.inputAssigned[3] = true;
            player1InputAssigned = 3;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButtonDown("J4XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("XBOX Joystick 4 Detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.XBOX_CONTROLLER, 4);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 4);
            Settings.inputAssigned[4] = true;
            player1InputAssigned = 4;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        MenuInputSelector.Player1InputAssigned = player1InputAssigned;
    }

    private void UnAssignAllInputs()
    {
        MenuInputSelector.menuControl[0] = null;
        MenuInputSelector.menuControl[1] = null;
        MenuInputSelector.menuControl[2] = null;
        MenuInputSelector.menuControl[3] = null;
        Settings.inputAssigned[0] = false;
        Settings.inputAssigned[1] = false;
        Settings.inputAssigned[2] = false;
        Settings.inputAssigned[3] = false;
        Settings.inputAssigned[4] = false;
    }


    private void UnAssignAllInputsExceptPlayer1()
    {
        for(int i = 0; i < 5; i++)
        {
            if (i != 4)
            {
                if (i != player1InputAssigned)
                {
                    MenuInputSelector.menuControl[i] = null;
                    Settings.inputAssigned[i] = false;
                }
            }
            else
            {
                if (i != player1InputAssigned)
                {
                    Settings.inputAssigned[i] = false;
                }
            }
        }
    }

}
