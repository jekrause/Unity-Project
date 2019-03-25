using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuButtonScript : MonoBehaviour
{
    public Vector2Int buttonID;
    public UnityEvent gotoMenu;
    private string OS = Settings.OS;
    //private Player Player1;

    // Start is called before the first frame update
    void Start()
    {
        //Player1 = MenuInputSelector.playerLists[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonID.x == -1)   // ignore x items
        {
            if (MenuScript.menuSelect.y == buttonID.y)
            {
                highlightButton(true);
            }
            else
            {
                highlightButton(false);
            }
        }
        else if (buttonID.y == -1)  // ignore y items
        {
            if (MenuScript.menuSelect.x == buttonID.x)
            {
                highlightButton(true);
            }
            else
            {
                highlightButton(false);
            }
        }
        else 
        { 
            if (MenuScript.menuSelect == buttonID)
            {
                highlightButton(true);
            }
            else
            {
                highlightButton(false);
            }
        }
    }


    private void pressSelectButton(int playerIndex)
    {
        if (MenuInputSelector.menuControl[playerIndex] != null)
        {
            if (MenuInputSelector.menuControl[playerIndex].inputType == InputType.KEYBOARD)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gotoMenu.Invoke();
                    //Debug.Log("Should have did command!");
                }
            }
            else
            {
                //same for windows and mac
                if (Input.GetButtonDown(MenuInputSelector.menuControl[0].DownButton))
                {
                    gotoMenu.Invoke();
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

            //old other
            /*if (MenuInputSelector.menuControl[0] != null)
            {
                if (Input.GetButtonDown(MenuInputSelector.menuControl[0].DownButton))
                {
                    gotoMenu.Invoke();
                }
            }*/

            pressSelectButton(0);
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

