using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotgun : Player
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        fAttackRadius = 2f;
        fMoveRate = 6f;
        fProjSpeed = 20f;

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