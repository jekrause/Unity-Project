using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSniper : Player
{
    // Start is called before the first frame update
    new void Start()
    { 
        base.Start();

        fMoveRate = 0.75f;
        fHP *= 0.75f;          
        fAttackRadius = 10f;
        //TODO: Setup other class specific stuff here.
        
    }

    protected override void Move(int xDir, int yDir)
    {

    }

}