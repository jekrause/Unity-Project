using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class PlayerMenuScript : MonoBehaviour
{
    public int playerNum;
    public GameObject joinScreen;
    public GameObject playerNumText;
    public GameObject newGameOrLoadMenu;
    public GameObject createNameMenu;
    public GameObject selectNameMenu;
    public GameObject chooseClassMenu;
    public GameObject playerReadyScreen;
    public int currentButton;
    public string playerName;
    public string playerClass;
    private string OS = Settings.OS;

    //for windows gamepad checks
    public static bool[] playerAxisInUse = new bool[4];

    private int playerInputIndex = 1;

    [System.Serializable]
    public class PlayerMenuNode
    {
        public GameObject upButton;
        public GameObject downButton;
        public GameObject rightButton;
        public GameObject leftButton;

        public PlayerMenuNode(GameObject up, GameObject down, GameObject right, GameObject left)
        {
            upButton = up;
            downButton = down;
            rightButton = right;
            leftButton = left;
        }
    }

    private PlayerMenuScript.PlayerMenuNode nextButtons;
    private GameObject currentMenu;

    /*
    private void Awake()
    {
        //deactivate all menus besides titlescreen


        if (playerNum == 1)
        { 
            joinScreen.SetActive(false);
            playerNumText.SetActive(true);
            newGameOrLoadMenu.SetActive(true);
            currentMenu = newGameOrLoadMenu;
            Settings.NumOfPlayers = 1;
            //nextButtons = initialButton.GetComponent<PlayerMenuInitialButtonScript>().initialButton.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
        }
        else
        {
            joinScreen.SetActive(true);
            playerNumText.SetActive(false);
            newGameOrLoadMenu.SetActive(false);
            currentMenu = joinScreen;
            //nextButtons = initialButton.GetComponent<PlayerMenuInitialButtonScript>().initialButton.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
        }
        nextButtons = initialMenu.GetComponent<PlayerMenuInitialButtonScript>().initialButton.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
        //nextButtons = initialButton.GetComponent<PlayerMenuInitialButtonScript>().initialButton.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
        createNameMenu.SetActive(false);
        selectNameMenu.SetActive(false);
        chooseClassMenu.SetActive(false);
        playerReadyScreen.SetActive(false);


        currentButton = 0;


    }
    */

    // Start is called before the first frame update
    void Start()
    {
        //deactivate all menus besides titlescreen


        if (playerNum == 1)
        {
            joinScreen.SetActive(false);
            playerNumText.SetActive(true);
            newGameOrLoadMenu.SetActive(true);
            currentMenu = newGameOrLoadMenu;
            Settings.NumOfPlayers = 1;
            //nextButtons = initialButton.GetComponent<PlayerMenuInitialButtonScript>().initialButton.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
        }
        else
        {
            joinScreen.SetActive(true);
            playerNumText.SetActive(false);
            newGameOrLoadMenu.SetActive(false);
            currentMenu = joinScreen;
            //nextButtons = initialButton.GetComponent<PlayerMenuInitialButtonScript>().initialButton.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
        }
        nextButtons = currentMenu.GetComponent<PlayerMenuInitialButtonScript>().initialButton.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
        //nextButtons = initialButton.GetComponent<PlayerMenuInitialButtonScript>().initialButton.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;

        //Debug.Log("Player2 nextup = " + nextButtons.upButton.name +
        //                  "\nPlayer2 nextdown = " + nextButtons.downButton.name +
        //                  "\nPlayer2 nextright = " + nextButtons.rightButton.name +
        //                  "\nPlayer2 nextleft = " + nextButtons.leftButton.name + "\n");
        //Debug.Log("ButtonIDold = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);

        createNameMenu.SetActive(false);
        selectNameMenu.SetActive(false);
        chooseClassMenu.SetActive(false);
        playerReadyScreen.SetActive(false);


        currentButton = 0;
    }


    private void Update()
    {

        //menuNavigate(playerNum);

        //to back a to title screen for now
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnAssignAllInputs();
            playerInputIndex = 1;
            Start();
        }


        if (MenuInputSelector.menuControl[playerInputIndex] == null)
        {
            // listen for a button press from connected controllers
            BindPlayerInput();
            if (playerInputIndex > 3)
            {
                playerInputIndex = 3;
            }
            //playerInputIndex++;
            //Debug.Log("Player " + (playerInputIndex + 1) + " has joined!");
        }


        /*
        if (Input.GetButton("J1XBOX_DownButton_" + Settings.OS))
        {
            print("XBOX Joystick 1 Detected, playerIndex = " + playerInputIndex);
        }
        else if (Input.GetButton("J2XBOX_DownButton_" + Settings.OS))
        {
            print("XBOX Joystick 2 Detected, playerIndex = " + playerInputIndex);
        }
        else if (Input.GetButton("J3XBOX_DownButton_" + Settings.OS))
        {
            print("XBOX Joystick 3 Detected, playerIndex = " + playerInputIndex);
        }
        else if (Input.GetButton("J4XBOX_DownButton_" + Settings.OS))
        {
            print("XBOX Joystick 4 Detected, playerIndex = " + playerInputIndex);
        }
        */

        //else
        //{
        //    playerInputIndex++;
        //}

        menuNavigate(playerNum);
    }


    private void pressDirection(int playerIndex)
    {
        if (MenuInputSelector.menuControl[playerIndex] != null)
        {
            if (MenuInputSelector.menuControl[playerIndex].inputType == InputType.KEYBOARD)
            {

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    GotoNextButton(nextButtons.upButton);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    GotoNextButton(nextButtons.downButton);
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    GotoNextButton(nextButtons.rightButton);
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    GotoNextButton(nextButtons.leftButton);
                }
            }
            else
            {
                if (OS.Equals("Mac"))
                {
                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadUp_Mac))
                    {
                        GotoNextButton(nextButtons.upButton);
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadDown_Mac))
                    {
                        GotoNextButton(nextButtons.downButton);
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadRight_Mac))
                    {
                        GotoNextButton(nextButtons.rightButton);
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadLeft_Mac))
                    {
                        GotoNextButton(nextButtons.leftButton);
                    }
                }
                else   //for xbox(windows) and PS4
                {
                    if (!playerAxisInUse[playerIndex] && MenuInputSelector.menuControl[playerIndex] != null)
                    {
                        playerAxisInUse[playerIndex] = true;
                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) > 0)
                        {
                            GotoNextButton(nextButtons.upButton);
                        }
                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) < 0)
                        {
                            GotoNextButton(nextButtons.downButton);
                        }

                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) > 0)
                        {
                            GotoNextButton(nextButtons.rightButton);
                        }

                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) < 0)
                        {
                            GotoNextButton(nextButtons.leftButton);
                        }

                    }

                    if (MenuInputSelector.menuControl[playerIndex] != null 
                        && (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) == 0 && Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) == 0))
                    {
                        // if player is not pressing any axis, reset boolean to allow us to check user input again. 
                        // The player shouldn't be pressing more than 1 D-Pad button at the same time when searching.
                        playerAxisInUse[playerIndex] = false;
                    }
                }
            }

        }
    }

    private void menuNavigate(int player)
    {
        if (player == 1)
        { 
            pressDirection(0);
        }
        else if (player == 2)
        { 
            pressDirection(1);
        }
        else if (player == 3)
        { 
            pressDirection(2);
        }
        else if (player == 4)
        { 
            pressDirection(3);
        }
    }

    //helper for gotomenu methods
    private void GotoMenuHelper(GameObject nextmenu)
    {
        if (nextmenu != newGameOrLoadMenu)  //play different sound when joining game
        {
            AudioManager.Play("Select");    //put this here for now for rest of menus
        }
        nextmenu.SetActive(true);
        currentMenu.SetActive(false);
        currentMenu = nextmenu;
        currentButton = 0;
        nextButtons = nextmenu.GetComponent<PlayerMenuInitialButtonScript>().initialButton.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
        //initialButton = nextmenu.GetComponent<PlayerMenuInitialButtonScript>().initialButton;
    }

    public void GotoNewNameMenu()
    {
        GotoMenuHelper(createNameMenu);
    }

    public void GotoLoadMenu()
    {
        GotoMenuHelper(selectNameMenu);
    }

    public void GotoChooseClassMenu(GameObject nameText)
    {
        //check if name is valid first
        if (nameText.GetComponent<UpdateNameText>().checkNameIsValid())
        {
            playerName = nameText.GetComponent<UpdateNameText>().sampleText.text;
            GotoMenuHelper(chooseClassMenu);
        }
    }
    public void GotoChooseClassMenuAfterLoad(GameObject nameText)
    {
        playerName = nameText.GetComponent<LoadCurrentName>().text.text;
        Debug.Log("playerName = " + playerName);

        if (playerName.Equals(""))  //cannot load blank name
        {
            AudioManager.Play("Wait");
            Debug.Log("Cannot load Blank Name!!!");
        }
        else if (selectNameMenu.transform.parent.parent.GetComponent<CSSTopMenuScript>().NameIsTaken(playerName,playerNum))
        {
            AudioManager.Play("Wait");
            Debug.Log("Name is already taken!");
        }
        else
        {
            GotoMenuHelper(chooseClassMenu);
        }

    }

    public void GotoReadyScreen(int classNum)
    {
        playerClass = SetPlayerClass(classNum);
        GotoMenuHelper(playerReadyScreen);
    }


    private void GotoNextButton(GameObject next)
    {
        int nextButton = next.GetComponentInChildren<MenuPlayerButtonScript>().buttonID;
        if (currentButton != nextButton)
        {
            AudioManager.Play("Move");
            currentButton = nextButton;
        }
        //currentButton = next.GetComponentInChildren<MenuPlayerButtonScript>().buttonID;
        nextButtons = next.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
    }

    public void GotoLevel()
    {
        playerReadyScreen.transform.parent.parent.GetComponent<CSSTopMenuScript>().setPlayerReady(playerNum);
        //this.GetComponent<Image>().enabled = true;   //highlight "Ready" button


        if (playerReadyScreen.transform.parent.parent.GetComponent<CSSTopMenuScript>().AllPlayersReady())
        {
            AudioManager.Play("StartLevel");
            //SceneManager.LoadScene("SampleScene");
            SceneManager.LoadScene("LoadingScreenScene");
            //SceneManager.LoadScene("SampleScene2");
        }
        else
        {
            AudioManager.Play("Wait");
            Debug.Log("Not all players are ready yet!!");
        }
    }

    public void JoinGame()
    {
        AudioManager.Play("Joined");
        joinScreen.SetActive(false);
        playerNumText.SetActive(true);
        Settings.NumOfPlayers = Settings.NumOfPlayers + 1;
        Debug.Log("numofplayers = " + Settings.NumOfPlayers);
        GotoMenuHelper(newGameOrLoadMenu);
        //newGameOrLoadMenu.SetActive(true);
        //currentMenu = newGameOrLoadMenu;
        //currentButton = 0;
    }
    

    private void PrintCurrentDebug()
    {
        Debug.Log("currentButton = " + currentButton);
        Debug.Log("nextButtonsUp = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);
        Debug.Log("nextButtonsDown = " + nextButtons.downButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);
        Debug.Log("nextButtonsRight = " + nextButtons.rightButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);
        Debug.Log("nextButtonsLeft = " + nextButtons.leftButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);
    }

    private string SetPlayerClass(int classNum)
    {
        string output = "";
        switch (classNum)
        {
            case 0:
                output = "Assault";
                break;
            case 1:
                output = "Heavy";
                break;
            case 2:
                output = "Shotgun";
                break;
            case 3:
                output = "Sniper";
                break;
        }
        MenuInputSelector.PlayerClasses[playerNum - 1] = classNum;  //set global PlayerClass variable
        MenuInputSelector.PlayerNames[playerNum - 1] = playerName;  //also set global PlayerName variable 
        return output;
    }


    private void BindPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !Settings.inputAssigned[0])
        {
            print("Keyboard Detected");
            //bind to player inputs to keyboard
            MenuInputSelector.menuControl[playerInputIndex] = new MyControllerInput(InputType.KEYBOARD, 1);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.KEYBOARD, 1);
            Settings.inputAssigned[0] = true;
            playerInputIndex++;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (Keyboard input)";
        }
        else if (Input.GetButton("J1PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[1])
        {
            print("PS4 Joystick 1 Detected");
            //bind player's inputs to PS4 controller
            MenuInputSelector.menuControl[playerInputIndex] = new MyControllerInput(InputType.PS4_CONTROLLER, 1);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 1);
            Settings.inputAssigned[1] = true;
            playerInputIndex++;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";

        }
        else if (Input.GetButton("J2PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[2])
        {
            print("PS4 Joystick 2 detected");
            //bind to player inputs to PS4 2nd controller
            MenuInputSelector.menuControl[playerInputIndex] = new MyControllerInput(InputType.PS4_CONTROLLER, 2);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 2);
            Settings.inputAssigned[2] = true;
            playerInputIndex++;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";

        }
        else if (Input.GetButton("J3PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("PS4 Joystick 3 detected");
            MenuInputSelector.menuControl[playerInputIndex] = new MyControllerInput(InputType.PS4_CONTROLLER, 3);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 3);
            Settings.inputAssigned[3] = true;
            playerInputIndex++;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";
        }
        else if (Input.GetButton("J4PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[4])
        {
            print("PS4 Joystick 4 detected");
            MenuInputSelector.menuControl[playerInputIndex] = new MyControllerInput(InputType.PS4_CONTROLLER, 4);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 4);
            Settings.inputAssigned[4] = true;
            playerInputIndex++;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";
        }
        else if (Input.GetButton("J1XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[1])
        {
            print("XBOX Joystick 1 Detected");
            MenuInputSelector.menuControl[playerInputIndex] = new MyControllerInput(InputType.XBOX_CONTROLLER, 1);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 1);
            Settings.inputAssigned[1] = true;
            Debug.Log("Player " + (playerInputIndex + 1) + " has joined!");
            playerInputIndex++;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButton("J2XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[2])
        {
            print("XBOX Joystick 2 Detected");
            MenuInputSelector.menuControl[playerInputIndex] = new MyControllerInput(InputType.XBOX_CONTROLLER, 2);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 2);
            Settings.inputAssigned[2] = true;
            Debug.Log("Player " + (playerInputIndex + 1) + " has joined!");
            playerInputIndex++;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButton("J3XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("XBOX Joystick 3 Detected");
            MenuInputSelector.menuControl[playerInputIndex] = new MyControllerInput(InputType.XBOX_CONTROLLER, 3);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 3);
            Settings.inputAssigned[3] = true;
            Debug.Log("Player " + (playerInputIndex + 1) + " has joined!");
            playerInputIndex++;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButton("J4XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[4])
        {
            print("XBOX Joystick 4 Detected");
            MenuInputSelector.menuControl[playerInputIndex] = new MyControllerInput(InputType.XBOX_CONTROLLER, 4);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 4);
            Settings.inputAssigned[4] = true;
            Debug.Log("Player " + (playerInputIndex + 1) + " has joined!");
            playerInputIndex++;
            //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }


        Debug.Log("menuControl[0] = " + MenuInputSelector.menuControl[0] +
        "\nmenuControl[1] = " + MenuInputSelector.menuControl[1] +
        "\nmenuControl[2] = " + MenuInputSelector.menuControl[2] +
       "\nmenuControl[3] = " + MenuInputSelector.menuControl[3] +
        "\ninputAssigned[0] = " + Settings.inputAssigned[0] +
        "\ninputAssigned[1] = " + Settings.inputAssigned[1] +
       "\ninputAssigned[2] = " + Settings.inputAssigned[2] +
        "\ninputAssigned[3] = " + Settings.inputAssigned[3] +
        "\ninputAssigned[4] = " + Settings.inputAssigned[4]);

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

}
