using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumOfPlayers : MonoBehaviour
{

    public int HowManyPlayers = Settings.NumOfPlayers;
    public int player1class = 0;
    public int player2class = 1;
    public int player3class = 2;
    public int player4class = 3;



    private void Start()
    {
        //automatically gets set to "1" if irregular number
        if (HowManyPlayers < 1 || HowManyPlayers > 4)
        {
            HowManyPlayers = 1;
        }
        Settings.NumOfPlayers = HowManyPlayers;


        MenuInputSelector.PlayerClasses[0] = player1class;
        MenuInputSelector.PlayerClasses[1] = player2class;
        MenuInputSelector.PlayerClasses[2] = player3class;
        MenuInputSelector.PlayerClasses[3] = player4class;
        BindAllPlayersToKeyboard();
    }


    private void BindAllPlayersToKeyboard()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && !Settings.inputAssigned[0])
        //{
        print("Keyboard Detected");
        //bind to player inputs to keyboard
        MenuInputSelector.menuControl = new MyControllerInput[4];
        MenuInputSelector.menuControl[0] = new MyControllerInput(InputType.KEYBOARD, 1);

        MenuInputSelector.menuControl[1] = new MyControllerInput(InputType.KEYBOARD, 2);
        MenuInputSelector.menuControl[2] = new MyControllerInput(InputType.KEYBOARD, 3);
        MenuInputSelector.menuControl[3] = new MyControllerInput(InputType.KEYBOARD, 4);

        Settings.inputAssigned[0] = true;

    }

}
