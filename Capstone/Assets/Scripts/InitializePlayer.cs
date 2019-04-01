using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePlayer : MonoBehaviour
{
    //public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "Player1")
        {
            setPlayerInfo(0);
        }
        else if(gameObject.name == "Player2")
        {
            setPlayerInfo(1);
        }
        else if (gameObject.name == "Player3")
        {
            setPlayerInfo(2);
        }
        else if (gameObject.name == "Player4")
        {
            setPlayerInfo(3);
        }
    }





    private void setPlayerInfo(int playerIndex)
    {
        if (MenuInputSelector.PlayerClasses[playerIndex] == 0)        //if player1 is Assault class
        {
            gameObject.AddComponent<PlayerFast>();
            gameObject.GetComponent<Player>().shootPosition = transform.GetChild(1).transform;  //may have to change this per class
        }
        else if (MenuInputSelector.PlayerClasses[playerIndex] == 1)   //if player1 is Heavy class
        {
            gameObject.AddComponent<PlayerHeavy>();
            gameObject.GetComponent<Player>().shootPosition = transform.GetChild(1).transform;  //may have to change this per class
        }
        else if (MenuInputSelector.PlayerClasses[playerIndex] == 2)   //if player1 is Shotgun class
        {
            gameObject.AddComponent<PlayerShotgun>();
            gameObject.GetComponent<Player>().shootPosition = transform.GetChild(1).transform;  //may have to change this per class
        }
        else if (MenuInputSelector.PlayerClasses[playerIndex] == 3)   //if player1 is Sniper class
        {
            gameObject.AddComponent<PlayerSniper>();
            gameObject.GetComponent<Player>().shootPosition = transform.GetChild(1).transform;  //may have to change this per class
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
        Debug.Log("Should have initialized Player"+(playerIndex+1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
