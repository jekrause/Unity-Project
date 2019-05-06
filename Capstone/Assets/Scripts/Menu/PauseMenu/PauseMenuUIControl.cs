using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;


public class PauseMenuUIControl : MonoBehaviour
{

    PlayerMenuScript.PlayerMenuNode nextButtons;
    public GameObject firstButton;
    public int currentButton;
    [HideInInspector]
    //public GameObject[] Players = { null, null, null, null };
    private string OS = Settings.OS;
    public static bool GameIsPaused = false;

    //for windows gamepad checks
    public static bool[] playerAxisInUse = new bool[4];

    // Start is called before the first frame update
    void Start()
    {
        //Players = GetComponentInParent<PauseMenuScript>().Players;
        nextButtons = firstButton.GetComponentInChildren<PauseButtonScript>().nextButtons;
    }

    // Update is called once per frame
    void Update()
    {
        pressDirection(0);
        pressDirection(1);
        pressDirection(2);
        pressDirection(3);
    }

    private void GotoNextButton(GameObject next)
    {
        int nextButton = next.GetComponentInChildren<PauseButtonScript>().buttonID;
        if (currentButton != nextButton)
        {
            AudioManager.Play("Menu_Move");
            currentButton = nextButton;
        }
        //currentButton = next.GetComponentInChildren<MenuPlayerButtonScript>().buttonID;
        nextButtons = next.GetComponentInChildren<PauseButtonScript>().nextButtons;
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

    /*
    public void ResumeGame()
    {
        AudioManager.Play("Menu_Back");
    }

    public void PauseGame()
    {
        AudioManager.Play("Menu_Move");
    }

    public void QuitToTitleScreen()
    {
        AudioManager.Play("Menu_Back");
        SceneManager.LoadScene("TitleScreen");
        Debug.Log("Tried to load level???");
    }
    */
}
