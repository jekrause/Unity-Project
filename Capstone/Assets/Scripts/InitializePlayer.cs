using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePlayer : MonoBehaviour
{
    //public GameObject playerPrefab;
    public GameObject playerCrossHair;
    private int playerLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "Player1")
        {
            setPlayerInfo(0);
        }
        else if(gameObject.name == "Player2")
        {
            if(Settings.NumOfPlayers >= 2)
            {
                setPlayerInfo(1);
            }
        }
        else if (gameObject.name == "Player3")
        {
            if (Settings.NumOfPlayers >= 3)
            {
                setPlayerInfo(2);
            }
        }
        else if (gameObject.name == "Player4")
        {
            if (Settings.NumOfPlayers == 4)
            {
                setPlayerInfo(3);
            }
        }
    }





    private void setPlayerInfo(int playerIndex)
    {
        if (MenuInputSelector.PlayerClasses[playerIndex] == 0)        //if player1 is Assault class
        {
            gameObject.AddComponent<PlayerFast>();
            gameObject.GetComponent<Player>().shootPosition = transform.GetChild(1).transform;  //may have to change this per class
            playerLevel = LoadProfileList.GetAssaultLevel(MenuInputSelector.PlayerNames[playerIndex]);
        }
        else if (MenuInputSelector.PlayerClasses[playerIndex] == 1)   //if player1 is Heavy class
        {
            gameObject.AddComponent<PlayerHeavy>();
            gameObject.GetComponent<Player>().shootPosition = transform.GetChild(1).transform;  //may have to change this per class
            playerLevel = LoadProfileList.GetHeavyLevel(MenuInputSelector.PlayerNames[playerIndex]);
        }
        else if (MenuInputSelector.PlayerClasses[playerIndex] == 2)   //if player1 is Shotgun class
        {
            gameObject.AddComponent<PlayerShotgun>();
            gameObject.GetComponent<Player>().shootPosition = transform.GetChild(1).transform;  //may have to change this per class
            playerLevel = LoadProfileList.GetShotgunLevel(MenuInputSelector.PlayerNames[playerIndex]);
        }
        else if (MenuInputSelector.PlayerClasses[playerIndex] == 3)   //if player1 is Sniper class
        {
            gameObject.AddComponent<PlayerSniper>();
            gameObject.GetComponent<Player>().shootPosition = transform.GetChild(1).transform;  //may have to change this per class
            playerLevel = LoadProfileList.GetSniperLevel(MenuInputSelector.PlayerNames[playerIndex]);
        }


        //then
        gameObject.GetComponent<Player>().playerNumber = playerIndex+1;   //set player number
        //gameObject.GetComponentInChildren<TextMesh>().text = "P" + (1) + " (Keyboard input)";
        gameObject.GetComponentInChildren<TextMesh>().text = MenuInputSelector.PlayerNames[playerIndex];  //get player1 name
        gameObject.GetComponent<Player>().myControllerInput = MenuInputSelector.menuControl[playerIndex];   // set player input



        if (playerIndex == 0)
        {
            gameObject.GetComponent<Player>().myCamera = GameObject.FindWithTag("Camera1").GetComponent<Camera>();    //set player camera
        }
        else if(playerIndex == 1)
        {
            gameObject.GetComponent<Player>().myCamera = GameObject.FindWithTag("Camera2").GetComponent<Camera>();    //set player camera
        }
        else if (playerIndex == 2)
        {
            gameObject.GetComponent<Player>().myCamera = GameObject.FindWithTag("Camera3").GetComponent<Camera>();    //set player camera
        }
        else if (playerIndex == 3)
        {
            gameObject.GetComponent<Player>().myCamera = GameObject.FindWithTag("Camera4").GetComponent<Camera>();    //set player camera
        }
        SetCameraDistance(playerIndex);
        SetCrossHair();
        Debug.Log("Should have initialized Player"+(playerIndex+1));
    }


    private void SetCameraDistance(int playerIndex)
    {
        int playerClassIndex = MenuInputSelector.PlayerClasses[playerIndex];

        switch (playerClassIndex)
        {
            case 0:
                gameObject.GetComponent<Player>().myCamera.orthographicSize = 16;//14;
                break;
            case 1:
                gameObject.GetComponent<Player>().myCamera.orthographicSize = 18;//16;
                break;
            case 2:
                gameObject.GetComponent<Player>().myCamera.orthographicSize = 14;//10;
                break;
            case 3:
                gameObject.GetComponent<Player>().myCamera.orthographicSize = 22;//20;
                break;
        }
    }

    private void SetCrossHair()
    {
        if (gameObject.GetComponent<Player>() != null && gameObject.GetComponent<Player>().myControllerInput != null)
        {
            if (gameObject.GetComponent<Player>().myControllerInput.inputType == InputType.KEYBOARD)
            {
                playerCrossHair.SetActive(false);
            }
            else
            {
                playerCrossHair.SetActive(true);
            }
        }
    }

    public int GetLevel()
    {
        return playerLevel;
    }


}
