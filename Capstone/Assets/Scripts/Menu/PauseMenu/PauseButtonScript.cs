using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseButtonScript : MonoBehaviour
{

    public int buttonID;
    public UnityEvent EventToCall;
    public PlayerMenuScript.PlayerMenuNode nextButtons;
    private string OS = Settings.OS;


    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {

        if (this.GetComponentInParent<PauseMenuUIControl>().currentButton == buttonID)
        {
            highlightButton(true);
        }
        else
        {
            highlightButton(false);
        }

    }


    private void pressSelectButton(int playerIndex)
    {
        if (transform.parent.GetComponentInParent<PauseMenuScript>().Players[playerIndex] != null)
        {
            if (transform.parent.GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>() != null)
            {
                if (transform.parent.GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput != null)
                {
                    if (transform.parent.GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.inputType == InputType.KEYBOARD)
                    {
                        
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            Debug.Log("HEY YO I GOT TO KEYBOARD!!");
                            EventToCall.Invoke();
                        }
                    }
                    else
                    {
                        Debug.Log("HEY YO I GOT Here by the gamepads!!");
                        // no need to check for mac os as DownButton work for both platform
                        if (Input.GetButtonDown(transform.parent.GetComponentInParent<PauseMenuScript>().Players[playerIndex].GetComponent<Player>().myControllerInput.DownButton))
                        {
                            Debug.Log("HEY YO I GOT TO GAMEPADS!!");
                            EventToCall.Invoke();
                        }
                    }
                }
            }
        }
    }

    private void highlightButton(bool highlight)
    {
        if (highlight == true)
        {

            if (GetComponent<Image>().enabled == false)
            {
                GetComponent<Image>().enabled = true;
            }

            pressSelectButton(0);
            pressSelectButton(1);
            pressSelectButton(2);
            pressSelectButton(3);
            
        }
        else
        {
            if (GetComponent<Image>().enabled == true)
            {
                GetComponent<Image>().enabled = false;
            }
        }

    }


}
