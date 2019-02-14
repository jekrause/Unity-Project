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
    public int playerNumber;
    public int iAvailInvSlots = 6;      //player spawns with no inventory. Max of 6
    public int iAvailWeaponSlots = 3;   //player can have at most 3 weapons

    private Animator anim;

    // player's movement
    private Rigidbody2D rb;
    protected Vector2 velocity;

    // mouse
    private Vector2 direction;

    //camera for the specific player
    private Camera myCamera;

    // reference to the controller that is attached to the player
    public MyControllerInput myControllerInput;

    // Start is called before the first frame update
    protected void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        myControllerInput = new MyControllerInput();
        myCamera = GameObject.FindWithTag("Camera" + playerNumber).GetComponent<Camera>();
    }

    private void OnDisable()
    {
        //TODO: Not sure if we need this yet, but it's used to update the game manager
    }

    // Update is called once per frame
    protected virtual void Update()
    { 
        //check for less than 1 so we can simply subtract enemy damage rather than checking for 0.
        if (fHP < 1f)
        {
            Settings.NumOfPlayers--;
            if (Settings.NumOfPlayers == 0)
            {
                //TODO: Call game over screen
            }
            Destroy(gameObject);
        }

        try
        {
            getRotationPosition();
            rb.MovePosition(rb.position + velocity * Time.deltaTime); // move the player after updating user input
        }
        catch
        {

        }

    }

    // Must be implemented in other subclasses
    protected abstract void GetMovementInput();

    private void getRotationPosition()
    {
        if (myControllerInput.inputType == InputType.KEYBOARD)
        {
            Vector3 position = Input.mousePosition;
            position = myCamera.ScreenToWorldPoint(position);
            direction = new Vector2(position.x - transform.position.x, position.y - transform.position.y);
            transform.right = direction; //transform may vary depending on sprite's image
        }
        else //use controller inputs 
        {
            try
            {
                float horizontal = Input.GetAxisRaw(myControllerInput.RightHorizontalAxis);
                float vertical = Input.GetAxisRaw(myControllerInput.RightVerticalAxis);
                if (horizontal != 0 || vertical != 0)
                {
                    Vector2 lookDirection = new Vector2(horizontal, vertical);
                    transform.right = lookDirection;
                }
            }
            catch
            {

            }

        }

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
        if (fHP > 100f)
        {
            fHP = 100f;
        }
    }
}
