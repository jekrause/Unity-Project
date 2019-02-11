using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
 *  I started using the tutorial below, but ended up on a bit of a different path.
 * 
 */
// https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-player-script
public abstract class Player : MonoBehaviour
{
    //TODO: List of objects of type "Inventory"
    //TODO: List of objects of type "Weapon"

    public const int iBaseAttackRate = 1;

    protected float fHP = 100f;
    protected float fDamage = 10f;         //default damage if no weapon is equipped
    protected float fMoveRate = 1f;
    protected float fAttackRadius = 2f;

    public int iAvailInvSlots = 6;      //player spawns with no inventory. Max of 6
    public int iAvailWeaponSlots = 3;   //player can have at most 3 weapons

    private Animator anim;

    // Start is called before the first frame update
    protected void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        //TODO: Not sure if we need this yet, but it's used to update the game manager
    }

    // Update is called once per frame
    void Update()
    {
        //check for less than 1 so we can simply subtract enemy damage rather than checking for 0.
        if(fHP < 1f)
        {
            //TODO: if players remaining == 0, game over
        }

        //do move
        int iHorizontal = 0; //TODO: maybe these should be floats?
        int iVertical = 0;

        //TODO: Set according to input from controller

    }

    virtual protected void Move(int xDir, int yDir)
    {

    }
    
    //Used to manage collisions with impermeable objects.
    protected void AttemptMove(int xDir, int yDir)
    {
        //TODO
    }

    //Called when player is healed
    protected void Healed(float f)
    {
        //TODO: maybe a healing animation gets called here
        fHP += f;
        if(fHP > 100f)
        {
            fHP = 100f;
        }
    }
}
