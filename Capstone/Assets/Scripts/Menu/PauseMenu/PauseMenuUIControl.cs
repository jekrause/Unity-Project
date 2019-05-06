using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;


public class PauseMenuUIControl : MonoBehaviour
{

    PlayerMenuScript.PlayerMenuNode nextButtons;
    public GameObject firstButton;
    public int currentButton;
    private string OS = Settings.OS;
    private int numOfPlayers;

    //for windows gamepad checks
    public static bool[] playerAxisInUse = new bool[4];

    // Start is called before the first frame update
    void Start()
    {
        numOfPlayers = Settings.NumOfPlayers;
        nextButtons = firstButton.GetComponentInChildren<PauseButtonScript>().nextButtons;
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i < numOfPlayers; i += 1)
        {
            pressDirection(i);
        }

        /*
        pressDirection(0);
        pressDirection(1);
        pressDirection(2);
        pressDirection(3);
        */
    }

    private void GotoNextButton(GameObject next)
    {
        int nextButton = next.GetComponentInChildren<PauseButtonScript>().buttonID;

        if (currentButton != nextButton)
        {
            AudioManager.Play("Menu_Move");
            currentButton = nextButton;
        }
        nextButtons = next.GetComponentInChildren<PauseButtonScript>().nextButtons;
    }

    private void pressDirection(int playerIndex)
    {
        if (GetComponentInParent<PauseMenuScript>().Players[playerIndex] != null)
        {
            if (GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>() != null)
            {
                if (GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput != null)
                {
                    if (GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.inputType == InputType.KEYBOARD)
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
                            if (Input.GetButtonDown(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadUp_Mac))
                            {
                                GotoNextButton(nextButtons.upButton);
                            }

                            if (Input.GetButtonDown(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadDown_Mac))
                            {
                                GotoNextButton(nextButtons.downButton);
                            }

                            if (Input.GetButtonDown(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadRight_Mac))
                            {
                                GotoNextButton(nextButtons.rightButton);
                            }

                            if (Input.GetButtonDown(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadLeft_Mac))
                            {
                                GotoNextButton(nextButtons.leftButton);
                            }
                        }
                        else   //for xbox(windows) and PS4
                        {
                            if (!playerAxisInUse[playerIndex] && GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput != null)
                            {
                                playerAxisInUse[playerIndex] = true;
                                if (Input.GetAxis(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadY_Windows) > 0)
                                {
                                    GotoNextButton(nextButtons.upButton);
                                }
                                if (Input.GetAxis(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadY_Windows) < 0)
                                {
                                    GotoNextButton(nextButtons.downButton);
                                }

                                if (Input.GetAxis(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadX_Windows) > 0)
                                {
                                    GotoNextButton(nextButtons.rightButton);
                                }

                                if (Input.GetAxis(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadX_Windows) < 0)
                                {
                                    GotoNextButton(nextButtons.leftButton);
                                }

                            }

                            if (GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput != null
                                && (Input.GetAxis(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadY_Windows) == 0 
                                && Input.GetAxis(GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DPadX_Windows) == 0))
                            {
                                // if player is not pressing any axis, reset boolean to allow us to check user input again. 
                                // The player shouldn't be pressing more than 1 D-Pad button at the same time when searching.
                                playerAxisInUse[playerIndex] = false;
                            }
                        }
                    }
                }
            }
        }
    }



    // For Debugging only
    private void PrintPlayerInputs()
    {
        for(int i = 0; i < 4; i += 1)
        {
            if (GetComponentInParent<PauseMenuScript>().Players[i] != null)
            {
                if (GetComponentInParent<PauseMenuScript>().Players[i].GetComponent<Player>() != null)
                {
                    Debug.Log("Player"+(i+1)+"Input = " + GetComponentInParent<PauseMenuScript>().Players[i].GetComponent<Player>().myControllerInput.inputType);
                }
                else
                {
                    Debug.Log("Player" + (i+1) + "Input = null");
                }
            }
        }
    }

}
