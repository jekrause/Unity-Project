using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSelection : MonoBehaviour
{
    private int numOfPlayers;
    private GameObject[] objList;
    private Player[] playerLists;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        numOfPlayers = GameObject.FindWithTag("SceneControl").GetComponent<NumOfPlayers>().HowManyPlayers;
        objList = new GameObject[4];
        playerLists = new Player[4];
        for (int i = 0; i < 4; i++)
        {
            objList[i] = transform.GetChild(i).gameObject; //store the object for later reference
            playerLists[i] = transform.GetChild(i).gameObject.GetComponent<Player>(); //get the player object and store it for later reference
            if (i >= numOfPlayers)
            {
                objList[i].SetActive(false); // set all other players not active to be invisible
            }
        }

        //print to console as of now, I will create a character selection scene later
        print("Choose an input for the player:\nSpaceBar - use keyboard and mouse as input\n");
        print("X Button - use PS4 controller as input\n");
        print("A Button - use XBOX controller as input");
    }

    void Update()
    {
        if(playerLists[index].myControllerInput.inputType == InputType.NONE)
        {
            // listen for a button press from connected controllers
            BindInputToPlayer();
        }
        else
        {
            if (index == numOfPlayers - 1) this.enabled = false; // end the script, all players have input
            else index++;
        }
    }

    protected void BindInputToPlayer()
    {

        if (Input.GetKey(KeyCode.Space) && !Settings.inputAssigned[0])
        {
            print("Keyboard Detected");
            //bind to player inputs to keyboard
            playerLists[index].myControllerInput = new MyControllerInput(InputType.KEYBOARD, 1);
            Settings.inputAssigned[0] = true;
            transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (Keyboard input)";
        }
        else if (Input.GetButton("J1PS4_DownButton_"+Settings.OS) && !Settings.inputAssigned[1])
        {
            print("PS4 Joystick 1 Detected");
            //bind player's inputs to PS4 controller
            playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 1);
            Settings.inputAssigned[1] = true;
            transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";

        }
        else if (Input.GetButton("J2PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[2])
        {
            print("PS4 Joystick 2 detected");
            //bind to player inputs to PS4 2nd controller
            playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 2);
            Settings.inputAssigned[2] = true;
            transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";

        }
        else if(Input.GetButton("J3PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("PS4 Joystick 3 detected");
            playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 3);
            Settings.inputAssigned[3] = true;
            transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";
        }
        else if (Input.GetButton("J4PS4_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("PS4 Joystick 4 detected");
            playerLists[index].myControllerInput = new MyControllerInput(InputType.PS4_CONTROLLER, 4);
            Settings.inputAssigned[4] = true;
            transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (PS4 input)";
        }
        else if (Input.GetButton("J1XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[1])
        {
            print("XBOX Joystick 1 Detected");
            playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 1);
            Settings.inputAssigned[1] = true;
            transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButton("J2XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[2])
        {
            print("XBOX Joystick 2 Detected");
            playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 2);
            Settings.inputAssigned[2] = true;
            transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButton("J3XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("XBOX Joystick 3 Detected");
            playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 3);
            Settings.inputAssigned[3] = true;
            transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }
        else if (Input.GetButton("J4XBOX_DownButton_" + Settings.OS) && !Settings.inputAssigned[3])
        {
            print("XBOX Joystick 4 Detected");
            playerLists[index].myControllerInput = new MyControllerInput(InputType.XBOX_CONTROLLER, 4);
            Settings.inputAssigned[4] = true;
            transform.GetChild(index).GetComponentInChildren<TextMesh>().text = "P" + (index + 1) + " (XBOX input)";
        }

    }

}
