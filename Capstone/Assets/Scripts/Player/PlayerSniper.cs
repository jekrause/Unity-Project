using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSniper : Player
{
    // Start is called before the first frame update
    new void Start()
    { 
        base.Start();

        fMoveRate = 7f;
        fHP *= 0.75f;          
        fAttackRadius = 10f;
        //TODO: Setup other class specific stuff here.
        
    }

    protected override void Update()
    {
        GetMovementInput();
        base.Update();
    }

    protected override void GetMovementInput()
    {
        try
        {
            float moveHorizontal = Input.GetAxis(myControllerInput.LeftHorizontalAxis);
            float moveVertical = Input.GetAxis(myControllerInput.LeftVerticalAxis);
            velocity = new Vector2(moveHorizontal, moveVertical) * fMoveRate;
        }
        catch
        {

        }
    }

}