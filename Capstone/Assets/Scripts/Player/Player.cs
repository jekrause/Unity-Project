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
    public const int iBaseAttackRate = 1;
    public float fHP = 100f;
    protected float fDamage = 10f;         //default damage if no weapon is equipped
    public float fMoveRate = 1f;
    protected float fAttackRadius = 2f;
    protected float fProjSpeed = 20f;
    public int playerNumber;
    public Inventory WeaponInventory = new Inventory(3);   //player can have at most 3 weapons
    public Inventory MainInventory = new Inventory(6); //player spawns with empty inventory. Max of 6
    private Animator anim;

    // player's movement
    private Rigidbody2D rb;
    protected Vector2 velocity;

    //attacking
    protected Rigidbody2D bullet;

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

    // Update is called once per frame
    protected virtual void Update()
    {
        //TODO
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Attack();
        }

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

        //movement
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

    protected void Damaged(float f)
    {
        fHP -= f;
    }

    virtual protected void Attack() {
        Rigidbody2D bulletInstance = Instantiate(Resources.Load("Bullet"), transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
        bulletInstance.velocity = transform.forward * fProjSpeed;
    }
}
