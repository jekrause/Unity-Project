using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavy : Player
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        fMoveRate = 0.5f;
        fHP *= 1.5f;
        fAttackRadius = 5f;
        //TODO: Setup other class specific stuff here.

    }

    protected override void Move(int xDir, int yDir)
    {

    }

}