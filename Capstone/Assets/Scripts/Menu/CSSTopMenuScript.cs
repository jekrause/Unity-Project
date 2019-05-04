using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSTopMenuScript : MonoBehaviour
{

    public bool[] playersReady = { false, false, false, false };
    public GameObject player1Menu;
    public GameObject player2Menu;
    public GameObject player3Menu;
    public GameObject player4Menu;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void setPlayerReady(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                playersReady[0] = true;
                break;
            case 2:
                playersReady[1] = true;
                break;
            case 3:
                playersReady[2] = true;
                break;
            case 4:
                playersReady[3] = true;
                break;
        }
    }
    */

    /*
    public bool AllPlayersReady()
    {
        if (Settings.NumOfPlayers == 1)
        {
            if (playersReady[0] == true)
            {
                return true;
            }
        }

        if (Settings.NumOfPlayers == 2)
        {
            if (playersReady[0] == true && playersReady[1] == true)
            {
                return true;
            }
        }

        if (Settings.NumOfPlayers == 3)
        {
            if (playersReady[0] == true && playersReady[1] == true && playersReady[2] == true)
            {
                return true;
            }
        }

        if (Settings.NumOfPlayers == 4)
        {
            if (playersReady[0] == true && playersReady[1] == true && playersReady[2] == true && playersReady[3] == true)
            {
                return true;
            }
        }

        return false;
    }
    */

    /*
    public bool NameIsTaken(string name, int playerNum)
    {
        Debug.Log("Name: " + name + " playerNum: " + playerNum);
        Debug.Log("player1name = " + player1Menu.GetComponent<PlayerMenuScript>().playerName);
        Debug.Log("player2name = " + player2Menu.GetComponent<PlayerMenuScript>().playerName);
        Debug.Log("player3name = " + player3Menu.GetComponent<PlayerMenuScript>().playerName);
        Debug.Log("player4name = " + player4Menu.GetComponent<PlayerMenuScript>().playerName);
        switch (playerNum)
        {
            case 1:
                if (name.Equals(player2Menu.GetComponent<PlayerMenuScript>().playerName) ||
                    name.Equals(player3Menu.GetComponent<PlayerMenuScript>().playerName) ||
                    name.Equals(player4Menu.GetComponent<PlayerMenuScript>().playerName))
                {
                    return true;
                }
                break;
            case 2:
                if (name.Equals(player1Menu.GetComponent<PlayerMenuScript>().playerName) ||
                    name.Equals(player3Menu.GetComponent<PlayerMenuScript>().playerName) ||
                    name.Equals(player4Menu.GetComponent<PlayerMenuScript>().playerName))
                {
                    return true;
                }
                break;
            case 3:
                if (name.Equals(player1Menu.GetComponent<PlayerMenuScript>().playerName) ||
                    name.Equals(player2Menu.GetComponent<PlayerMenuScript>().playerName) ||
                    name.Equals(player4Menu.GetComponent<PlayerMenuScript>().playerName))
                {
                    return true;
                }
                break;
            case 4:
                if (name.Equals(player1Menu.GetComponent<PlayerMenuScript>().playerName) ||
                    name.Equals(player2Menu.GetComponent<PlayerMenuScript>().playerName) ||
                    name.Equals(player3Menu.GetComponent<PlayerMenuScript>().playerName))
                {
                    return true;
                }
                break;
        }
        return false;

    }
    */

}
