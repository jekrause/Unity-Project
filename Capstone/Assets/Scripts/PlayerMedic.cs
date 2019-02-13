using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMedic : Player
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        fAttackRadius = 2f;
        fMoveRate = 6f;
        //TODO: Setup other class specific stuff here.

    }

    protected override void Update()
    {
        GetMovementInput();
        base.Update();
    }

    protected override void Move(int xDir, int yDir)
    {

    }

    public bool HealPlayer(Player p)
    {
        //TODO: might need rules for healing players.
        
        return p;
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