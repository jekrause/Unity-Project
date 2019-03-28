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
    private GameObject previousMenu;
    private string OS = Settings.OS;

    private int player1InputAssigned = 0;


    private void Awake()
    {
        //deactivate all menus besides titlescreen
        titlescreen.SetActive(true);
        mainmenu.SetActive(false);
        //howmanymenu.SetActive(false);
        characterselectmenu.SetActive(false);
        currentMenu = titlescreen;
        previousMenu = currentMenu;

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

        if (currentMenu == titlescreen)
        {
            BindPlayer1();
        }



        if (currentMenu != characterselectmenu)
        {
            menuNavigate();
        }


        //to go back to previous menu for now
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gotoPreviousMenu();
        }

    }
    

    private void pressDirection(int playerIndex)
    {
        if (MenuInputSelector.menuControl[playerIndex] != null)
        {
            if (MenuInputSelector.menuControl[playerIndex].inputType == InputType.KEYBOARD)
            {

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

            }
            else
            {
                if (OS.Equals("Mac"))
                {


                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadUp_Mac))
                    {
                        menuSelect.y = menuSelect.y + 1;
                        if (menuSelect.y > totalMenuItemsY - 1)
                        {
                            menuSelect.y = 0;
                        }
                        Debug.Log("menuSelect.y = " + menuSelect.y);
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadDown_Mac))
                    {
                        menuSelect.y = menuSelect.y - 1;
                        if (menuSelect.y < 0)
                        {
                            menuSelect.y = totalMenuItemsY - 1;
                        }
                        Debug.Log("menuSelect.y = " + menuSelect.y);
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadRight_Mac))
                    {
                        menuSelect.x = menuSelect.x + 1;
                        if (menuSelect.x > totalMenuItemsX - 1)
                        {
                            menuSelect.x = 0;
                        }
                        Debug.Log("menuSelect.x = " + menuSelect.x);
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadLeft_Mac))
                    {
                        menuSelect.x = menuSelect.x - 1;
                        if (menuSelect.x < 0)
                        {
                            menuSelect.x = totalMenuItemsX - 1;
                        }
                        Debug.Log("menuSelect.x = " + menuSelect.x);
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
                            menuSelect.y = menuSelect.y + 1;
                            if (menuSelect.y > totalMenuItemsY - 1)
                            {
                                menuSelect.y = 0;
                            }
                            Debug.Log("menuSelect.y = " + menuSelect.y);
                        }
                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) < 0)
                        {
                            menuSelect.y = menuSelect.y - 1;
                            if (menuSelect.y < 0)
                            {
                                menuSelect.y = totalMenuItemsY - 1;
                            }
                            Debug.Log("menuSelect.y = " + menuSelect.y);
                        }

                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) > 0)
                        {
                            menuSelect.x = menuSelect.x + 1;
                            if (menuSelect.x > totalMenuItemsX - 1)
                            {
                                menuSelect.x = 0;
                            }
                            Debug.Log("menuSelect.x = " + menuSelect.x);
                        }

                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) < 0)
                        {
                            menuSelect.x = menuSelect.x - 1;
                            if (menuSelect.x < 0)
                            {
                                menuSelect.x = totalMenuItemsX - 1;
                            }
                            Debug.Log("menuSelect.x = " + menuSelect.x);
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

        }
    }

    private void menuNavigate()
    {
        pressDirection(0);
    }



    public void GotoTitleScreen()
    {
        previousMenu = titlescreen;
        menuSelect.x = 0;
        menuSelect.y = 0;
        titlescreen.SetActive(true);
        currentMenu.SetActive(false);
        currentMenu = titlescreen;
        totalMenuItemsX = 1;
        totalMenuItemsY = 1;
        //MenuInputSelector.menuControl[0] = null;    //set player 1 to null
        UnAssignAllInputs();    //unassign all inputs to restart input selection

    }

    public void GotoMainMenu()
    {
        previousMenu = titlescreen;
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
        previousMenu = mainmenu;
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

    public void gotoPreviousMenu()
    {
        if (currentMenu != previousMenu)
        {
            if(currentMenu == characterselectmenu)
            {
                GotoTitleScreen();  //for now
                return;
                /*Debug.Log("before inputAssigned = " + player1InputAssigned);
                Debug.Log("player1assigned = " + Settings.inputAssigned[player1InputAssigned]);
                UnAssignAllInputsExceptPlayer1();
                GotoMainMenu();
                Debug.Log("after inputAssigned = " + player1InputAssigned);
                Debug.Log("player1assigned = " + Settings.inputAssigned[player1InputAssigned]);
                return;
                */               
            }
            if(previousMenu == titlescreen)
            {
                GotoTitleScreen();
                return;
            }
            if(previousMenu == mainmenu)
            {
                GotoMainMenu();
                return;
            }
        }
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
        else if (Input.GetButton("J1PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[1])
        {
            print("PS4 Joystick 1 Detected");
            //bind player's inputs to PS4 controller
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.PS4_CONTROLLER, 1);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 1);
            Settings.inputAssigned[1] = true;
            player1InputAssigned = 1;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";

        }
        else if (Input.GetButton("J2PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[2])
        {
            print("PS4 Joystick 2 detected");
            //bind to player inputs to PS4 2nd controller
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.PS4_CONTROLLER, 2);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 2);
            Settings.inputAssigned[2] = true;
            player1InputAssigned = 2;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";

        }
        else if (Input.GetButton("J3PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("PS4 Joystick 3 detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.PS4_CONTROLLER, 3);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 3);
            Settings.inputAssigned[3] = true;
            player1InputAssigned = 3;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";
        }
        else if (Input.GetButton("J4PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("PS4 Joystick 4 detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.PS4_CONTROLLER, 4);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 4);
            Settings.inputAssigned[4] = true;
            player1InputAssigned = 4;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";
        }
        else if (Input.GetButton("J1XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[1])
        {
            print("XBOX Joystick 1 Detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.XBOX_CONTROLLER, 1);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 1);
            Settings.inputAssigned[1] = true;
            player1InputAssigned = 1;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButton("J2XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[2])
        {
            print("XBOX Joystick 2 Detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.XBOX_CONTROLLER, 2);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 2);
            Settings.inputAssigned[2] = true;
            player1InputAssigned = 2;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButton("J3XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("XBOX Joystick 3 Detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.XBOX_CONTROLLER, 3);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 3);
            Settings.inputAssigned[3] = true;
            player1InputAssigned = 3;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButton("J4XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("XBOX Joystick 4 Detected");
            MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.XBOX_CONTROLLER, 4);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 4);
            Settings.inputAssigned[4] = true;
            player1InputAssigned = 4;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
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
