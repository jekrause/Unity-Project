using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFast : Player
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        fMoveRate = 1.5f;
        //TODO: Setup other class specific stuff here.

    }

    protected override void Move(int xDir, int yDir)
    {

    }

}