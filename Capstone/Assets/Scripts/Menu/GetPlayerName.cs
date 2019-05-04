using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerName : MonoBehaviour
{

    public GameObject playerMenu;
    private int playerIndex;

    private void Start()
    {
        playerIndex = playerMenu.GetComponent<PlayerMenuScript>().playerNum - 1;

        GetComponent<Text>().text = MenuInputSelector.PlayerNames[playerIndex];
    }

    private void OnEnable()
    {
        Start();
    }


}
