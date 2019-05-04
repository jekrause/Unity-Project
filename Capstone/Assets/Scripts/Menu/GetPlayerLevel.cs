using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerLevel : MonoBehaviour
{
    private int playerIndex;
    public int playerClass;
    public GameObject playerMenu;
    public bool isAtReadyScreen;

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = playerMenu.GetComponent<PlayerMenuScript>().playerNum - 1;


        if (isAtReadyScreen)    //If already at ready screen, get the playerclass from MenuInputSelector
        {
            playerClass = MenuInputSelector.PlayerClasses[playerIndex];
        }

        switch (playerClass)
        {
            case 0:
                this.GetComponent<Text>().text = MenuInputSelector.PlayerAssaultLevels[playerIndex].ToString();
                break;
            case 1:
                this.GetComponent<Text>().text = MenuInputSelector.PlayerHeavyLevels[playerIndex].ToString();
                break;
            case 2:
                this.GetComponent<Text>().text = MenuInputSelector.PlayerShotgunLevels[playerIndex].ToString();
                break;
            case 3:
                this.GetComponent<Text>().text = MenuInputSelector.PlayerSniperLevels[playerIndex].ToString();
                break;
        }

        
    }
    private void OnEnable()
    {
        Start();
    }


}
