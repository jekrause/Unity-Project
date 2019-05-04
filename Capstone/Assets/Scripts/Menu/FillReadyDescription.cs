using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillReadyDescription : MonoBehaviour
{
    public int logo;
    public GameObject playerMenu;
    public Text text;
    private int playerIndex;

    // Start is called before the first frame update
    void Start()
    {
        playerIndex = playerMenu.GetComponent<PlayerMenuScript>().playerNum - 1;

        if (logo == 0)
        {
            //text.text = playerMenu.GetComponent<PlayerMenuScript>().playerName;
            text.text = MenuInputSelector.PlayerNames[playerIndex];
        }
        else
        {
            //text.text = playerMenu.GetComponent<PlayerMenuScript>().playerClass;
            text.text = GetPlayerClassString(MenuInputSelector.PlayerClasses[playerIndex]);
        }
    }

    private void OnEnable()
    {
        Start();
    }


    private string GetPlayerClassString(int classNum)
    {
        switch (classNum)
        {
            case 0:
                return "Assault";
            case 1:
                return "Heavy";
            case 2:
                return "Shotgun";
            case 3:
                return "Sniper";
        }
        return "None";
    }

    
}
