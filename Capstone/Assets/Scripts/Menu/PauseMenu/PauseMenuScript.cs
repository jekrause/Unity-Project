using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject PauseMenu;
    public GameObject[] Players = { null, null, null, null };
    public static bool GameIsPaused = false;
    private int NumOfPlayers;

    private void Start()
    {
        NumOfPlayers = Settings.NumOfPlayers;
    }

    // Update is called once per frame
    void Update()
    {
        if(PressedPauseButton())
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
    }

    private bool PressedPauseButton()
    {
        for (int i = 0; i < NumOfPlayers; i += 1)
        {
            if (Players[i] != null)
            {
                if (Players[i].GetComponent<Player>() != null)
                {
                    if (Players[i].GetComponent<Player>().myControllerInput != null)
                    {
                        if (Players[i].GetComponent<Player>().myControllerInput.inputType == InputType.KEYBOARD)
                        {
                            if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            // no need to check for mac os as DownButton work for both platform
                            if (Input.GetButtonDown(Players[i].GetComponent<Player>().myControllerInput.StartButton))
                            {
                               return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }


    public void ResumeGame()
    {
        AudioManager.Play("Menu_Back");
        PauseMenu.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        AudioManager.Play("Menu_Move");
        PauseMenu.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void QuitToTitleScreen()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioManager.Play("Menu_Back");
        SceneManager.LoadScene("TitleScreen");
        Debug.Log("Tried to load level???");
    }

}
