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

        Debug.Log("Player2 nextup = " + nextButtons.upButton.name +
                          "\nPlayer2 nextdown = " + nextButtons.downButton.name +
                          "\nPlayer2 nextright = " + nextButtons.rightButton.name +
                          "\nPlayer2 nextleft = " + nextButtons.leftButton.name + "\n");
        Debug.Log("ButtonIDold = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);

        createNameMenu.SetActive(false);
        selectNameMenu.SetActive(false);
        chooseClassMenu.SetActive(false);
        playerReadyScreen.SetActive(false);


        currentButton = 0;
    }


    private void Update()
    {

        menuNavigate(playerNum);

    }


    private void menuNavigate(int player)
    {
        if (player == 1) { //using this check for testing at the moment

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GotoNextButton(nextButtons.upButton);
                //Debug.Log("HeyPlayer1!!!");
                //PrintCurrentDebug();

            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                GotoNextButton(nextButtons.downButton);

                //PrintCurrentDebug();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GotoNextButton(nextButtons.rightButton);

                //PrintCurrentDebug();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GotoNextButton(nextButtons.leftButton);

                //PrintCurrentDebug();
            }

        }else if (player == 2)
        { //using this check for testing at the moment

            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Player2 nextup = "+ nextButtons.upButton.name+
                          "\nPlayer2 nextdown = " + nextButtons.downButton.name+
                          "\nPlayer2 nextright = " + nextButtons.rightButton.name+
                          "\nPlayer2 nextleft = " + nextButtons.leftButton.name +"\n");
                Debug.Log("ButtonIDold = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);
                GotoNextButton(nextButtons.upButton);
                Debug.Log("ButtonIDafter = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);
                //Debug.Log("HeyPlayer2!!!");
                //PrintCurrentDebug();

            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Player2 nextup = " + nextButtons.upButton.name +
                          "\nPlayer2 nextdown = " + nextButtons.downButton.name +
                          "\nPlayer2 nextright = " + nextButtons.rightButton.name +
                          "\nPlayer2 nextleft = " + nextButtons.leftButton.name + "\n");
                Debug.Log("ButtonIDold = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);
                GotoNextButton(nextButtons.downButton);
                Debug.Log("ButtonIDafter = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);

                //PrintCurrentDebug();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Player2 nextup = " + nextButtons.upButton.name +
                          "\nPlayer2 nextdown = " + nextButtons.downButton.name +
                          "\nPlayer2 nextright = " + nextButtons.rightButton.name +
                          "\nPlayer2 nextleft = " + nextButtons.leftButton.name + "\n");
                Debug.Log("ButtonIDold = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);
                GotoNextButton(nextButtons.rightButton);
                Debug.Log("ButtonIDafter = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);

                //PrintCurrentDebug();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Player2 nextup = " + nextButtons.upButton.name +
                          "\nPlayer2 nextdown = " + nextButtons.downButton.name +
                          "\nPlayer2 nextright = " + nextButtons.rightButton.name +
                          "\nPlayer2 nextleft = " + nextButtons.leftButton.name + "\n");
                Debug.Log("ButtonIDold = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);
                GotoNextButton(nextButtons.leftButton);
                Debug.Log("ButtonIDafter = " + nextButtons.upButton.GetComponentInChildren<MenuPlayerButtonScript>().buttonID);

                //PrintCurrentDebug();
            }

        }else if (player == 3)
        { //using this check for testing at the moment

            if (Input.GetKeyDown(KeyCode.T))
            {
                GotoNextButton(nextButtons.upButton);
                //Debug.Log("Hey!!!");
                //PrintCurrentDebug();

            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                GotoNextButton(nextButtons.downButton);

                //PrintCurrentDebug();
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                GotoNextButton(nextButtons.rightButton);

                //PrintCurrentDebug();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                GotoNextButton(nextButtons.leftButton);

                //PrintCurrentDebug();
            }

        }else if (player == 4)
        { //using this check for testing at the moment

            if (Input.GetKeyDown(KeyCode.I))
            {
                GotoNextButton(nextButtons.upButton);
                //Debug.Log("Hey!!!");
                //PrintCurrentDebug();

            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                GotoNextButton(nextButtons.downButton);

                //PrintCurrentDebug();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GotoNextButton(nextButtons.rightButton);

                //PrintCurrentDebug();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                GotoNextButton(nextButtons.leftButton);

                //PrintCurrentDebug();
            }

        }
    }

    //helper for gotomenu methods
    private void GotoMenuHelper(GameObject nextmenu)
    {
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
            Debug.Log("Cannot load Blank Name!!!");
        }
        else if (selectNameMenu.transform.parent.parent.GetComponent<CSSTopMenuScript>().NameIsTaken(playerName,playerNum))
        {
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
        currentButton = next.GetComponentInChildren<MenuPlayerButtonScript>().buttonID;
        nextButtons = next.GetComponentInChildren<MenuPlayerButtonScript>().nextButtons;
    }

    public void GotoLevel()
    {
        playerReadyScreen.transform.parent.parent.GetComponent<CSSTopMenuScript>().setPlayerReady(playerNum);
        //this.GetComponent<Image>().enabled = true;   //highlight "Ready" button


        if (playerReadyScreen.transform.parent.parent.GetComponent<CSSTopMenuScript>().AllPlayersReady())
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.Log("Not all players are ready yet!!");
        }
    }

    public void JoinGame()
    {
        joinScreen.SetActive(false);
        playerNumText.SetActive(true);
        Settings.NumOfPlayers = Settings.NumOfPlayers + 1;
        Debug.Log("numofplayers = " + Settings.NumOfPlayers);
        GotoMenuHelper(newGameOrLoadMenu);
        //newGameOrLoadMenu.SetActive(true);
        //currentMenu = newGameOrLoadMenu;
        //currentButton = 0;
    }

/*
    public void SetPlayerClass(int playerNum, int classNum)
    {
        switch (playerNum)
        {
            case 0: //player1
                Settings.playerClasses[0] = classNum;
                break;
            case 1: //player2
                Settings.playerClasses[1] = classNum;
                break;
            case 2: //player3
                Settings.playerClasses[2] = classNum;
                break;
            case 3: //player4
                Settings.playerClasses[3] = classNum;
                break;
        }
    }
*/

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
        return output;
    }

}
