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


    private void highlightButton(bool highlight)
    {
        if (highlight == true)
        {
            //if (ignoreHighlight == false)
            //{
                if (GetComponent<Image>().enabled == false)
                {
                    GetComponent<Image>().enabled = true;
                    //Debug.Log("PLayerNum = " + PlayerNum);
                }
            //}
            if (PlayerNum == 1)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gotoMenu.Invoke();
                    //Debug.Log("Should have did command!");
                }
            }
            else if (PlayerNum == 2)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gotoMenu.Invoke();
                }
            }
            else if (PlayerNum == 3)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    gotoMenu.Invoke();
                }
            }
            else if (PlayerNum == 4)
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    gotoMenu.Invoke();
                }
            }
        }
        else
        {
            //if (ignoreHighlight == false)
            //{
                if (GetComponent<Image>().enabled == true)
                {
                    GetComponent<Image>().enabled = false;
                }
            //}
        }

    }


}
