using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavy : Player
{
    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        fMoveRate = 5f;
        fHP *= 1.5f;
        fAttackRadius = 5f;
        fProjSpeed = 10f;

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