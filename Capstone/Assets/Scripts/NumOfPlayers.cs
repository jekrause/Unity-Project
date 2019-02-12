using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumOfPlayers : MonoBehaviour
{

    public int HowManyPlayers;


    private void Start()
    {
        //automatically gets set to "1" if irregular number
        if (HowManyPlayers < 1 || HowManyPlayers > 4)
        {
            HowManyPlayers = 1;
        }
    }

}
