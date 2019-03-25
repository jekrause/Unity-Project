using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputSelector : MonoBehaviour
{
    static public MyControllerInput[] menuControl;

    // Start is called before the first frame update
    void Start()
    {
        menuControl = new MyControllerInput[4];

        //print to console as of now, I will create a character selection scene later
        print("Choose an input for the player:\nSpaceBar - use keyboard and mouse as input\n");
        print("X Button - use PS4 controller as input\n");
        print("A Button - use XBOX controller as input");
    }

    void Update()
    {

    }



    /*
    protected void BindInputToPlayer()
    {



    }
    */   
}
