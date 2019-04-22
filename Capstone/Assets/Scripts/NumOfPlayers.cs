using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumOfPlayers : MonoBehaviour
{

    public int HowManyPlayers = Settings.NumOfPlayers;
    public int playerclass = 0;



    private void Start()
    {
        //automatically gets set to "1" if irregular number
        if (HowManyPlayers < 1 || HowManyPlayers > 4)
        {
            HowManyPlayers = 1;
        }
        Settings.NumOfPlayers = HowManyPlayers;


        MenuInputSelector.PlayerClasses[0] = playerclass;
        BindPlayer1ToKeyboard();
    }


    private void BindPlayer1ToKeyboard()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && !Settings.inputAssigned[0])
        //{
        print("Keyboard Detected");
        //bind to player inputs to keyboard
        MenuInputSelector.menuControl = new MyControllerInput[4];
        MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.KEYBOARD, 1);

            //playerLists[index].myControllerInput = new MyControllerInput(InputType.KEYBOARD, 1);
        Settings.inputAssigned[0] = true;
        //player1InputAssigned = 0;
        //transform.GetChild(1).GetComponentInChildren<TextMesh>().text = "P" + (1) + " (Keyboard input)";
        //transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (Keyboard input)";
        //}
    }

}
