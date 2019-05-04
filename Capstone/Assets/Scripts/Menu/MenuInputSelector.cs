using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputSelector : MonoBehaviour
{
    static public MyControllerInput[] menuControl = { null, null,null,null };
    static public int[] PlayerClasses = { 0, 0, 0, 0 };  //0= assault, 1= heavy, 2= shotgun, 3= sniper
    static public string[] PlayerNames = { "", "", "", "" };
    static public int[] PlayerAssaultLevels = { 1, 1, 1, 1 };
    static public int[] PlayerHeavyLevels = { 25, 1, 1, 1 };
    static public int[] PlayerShotgunLevels = { 50, 1, 1, 1 };
    static public int[] PlayerSniperLevels = { 100, 1, 1, 1 };
    static public bool[] PlayersReady = { false, false, false, false };
    static public int Player1InputAssigned = 0;

    // Start is called before the first frame update
    void Start()
    {
        //menuControl = new MyControllerInput[4];

        //print to console as of now, I will create a character selection scene later
        print("Choose an input for the player:\n");
        print("SpaceBar - use keyboard and mouse as input\n");
        print("X Button - use PS4 controller as input\n");
        print("A Button - use XBOX controller as input");
    }

    static public bool NameIsTaken(string name, int playerIndex)
    {
        Debug.Log("Name: " + name + " playerIndex: " + playerIndex);
        Debug.Log("player1name = " + PlayerNames[0]);
        Debug.Log("player2name = " + PlayerNames[1]);
        Debug.Log("player3name = " + PlayerNames[2]);
        Debug.Log("player4name = " + PlayerNames[3]);
        switch (playerIndex)
        {
            case 0:
                if (name.Equals(PlayerNames[1]) ||
                    name.Equals(PlayerNames[2]) ||
                    name.Equals(PlayerNames[3]))
                {
                    return true;
                }
                break;
            case 1:
                if (name.Equals(PlayerNames[0]) ||
                    name.Equals(PlayerNames[2]) ||
                    name.Equals(PlayerNames[3]))
                {
                    return true;
                }
                break;
            case 2:
                if (name.Equals(PlayerNames[0]) ||
                    name.Equals(PlayerNames[1]) ||
                    name.Equals(PlayerNames[3]))
                {
                    return true;
                }
                break;
            case 3:
                if (name.Equals(PlayerNames[0]) ||
                    name.Equals(PlayerNames[1]) ||
                    name.Equals(PlayerNames[2]))
                {
                    return true;
                }
                break;
        }
        return false;

    }

    public static bool AllPlayersReady()
    {
        if (Settings.NumOfPlayers == 1)
        {
            if (PlayersReady[0] == true)
            {
                return true;
            }
        }

        if (Settings.NumOfPlayers == 2)
        {
            if (PlayersReady[0] == true && PlayersReady[1] == true)
            {
                return true;
            }
        }

        if (Settings.NumOfPlayers == 3)
        {
            if (PlayersReady[0] == true && PlayersReady[1] == true && PlayersReady[2] == true)
            {
                return true;
            }
        }

        if (Settings.NumOfPlayers == 4)
        {
            if (PlayersReady[0] == true && PlayersReady[1] == true && PlayersReady[2] == true && PlayersReady[3] == true)
            {
                return true;
            }
        }

        return false;
    }


}
