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
        //TODO: Setup other class specific stuff here.

    }

    protected override void Move(int xDir, int yDir)
    {

    }

    public bool HealPlayer(Player p)
    {
        //TODO: might need rules for healing players.
        
        return p;
    }
}