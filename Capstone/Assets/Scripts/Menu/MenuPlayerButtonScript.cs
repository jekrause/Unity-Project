using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuPlayerButtonScript : MonoBehaviour
{
    //public int PlayerNum;
    public int buttonID;
    public UnityEvent gotoMenu;
    public PlayerMenuScript.PlayerMenuNode nextButtons;
    //public bool ignoreHighlight = false;
    private int PlayerNum;
    private string OS = Settings.OS;


    // Start is called before the first frame update
    void Start()
    {

        PlayerNum = this.GetComponentInParent<PlayerMenuScript>().playerNum;
        //Debug.Log("PLayerNum = " + PlayerNum);

        //nextButtons = new PlayerMenuScript.PlayerMenuNode(nextButtons.upButton,nextButtons.downButton,nextButtons.rightButton,nextButtons.leftButton);
    }

    // Update is called once per frame
    void Update()
    {

        if (this.GetComponentInParent<PlayerMenuScript>().currentButton == buttonID)
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
        if (MenuInputSelector.menuControl[playerIndex] != null)
        {
            if (MenuInputSelector.menuControl[playerIndex].inputType == InputType.KEYBOARD)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gotoMenu.Invoke();
                }
            }
            else
            {
                if (OS.Equals("Mac"))
                {
                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DownButton))
                    {
                        gotoMenu.Invoke();
                    }
                }
                //else Windows(Xbox controller) or PS4
                //...
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
                //Debug.Log("PLayerNum = " + PlayerNum);
            }
            
            if (PlayerNum == 1)
            {
                pressSelectButton(0);
            }
            else if (PlayerNum == 2)
            {
                pressSelectButton(1);
            }
            else if (PlayerNum == 3)
            {
                pressSelectButton(2);
            }
            else if (PlayerNum == 4)
            {
                pressSelectButton(3);
            }
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
