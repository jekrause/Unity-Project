using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

/*
 *  I started using the tutorial below, but ended up on a bit of a different path.
 * 
 */
// https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-player-script
public abstract class Player : MonoBehaviour
{
    // Player stats
    protected const int iBaseAttackRate = 1;

    protected float fAttackTime = 3;  //The higher this number, the more frequent you can shoot
    protected float fHP = 100f;

    protected float fDamage = 10f;         //default damage if no weapon is equipped
    protected float fMoveRate = 1f;
    protected float fAttackRadius = 2f;
    protected float fProjSpeed = 20f;
    [SerializeField] public int playerNumber;
    private Animator anim;

    // player's movement
    private Rigidbody2D rb;
    protected Vector2 velocity;

    //attacking
    public Item basicWeapon;

    // mouse
    private Vector2 direction;

    //camera for the specific player
    [SerializeField] public Camera myCamera;

    // reference to the controller that is attached to the player
    public MyControllerInput myControllerInput = new MyControllerInput();

    public readonly Inventory MainInventory = new Inventory(6);
    public readonly Inventory WeaponInventory = new Inventory(3);

    // Start is called before the first frame update

    private void Awake()
    {

    }
    protected void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        //check for less than 1 so we can simply subtract enemy damage rather than checking for 0.
        if (fHP < 1f)
        {
            Death();
        }

        //movement

        getRotationPosition();
        rb.MovePosition(rb.position + velocity * Time.deltaTime); // move the player after updating user input

        GetAttackInput();
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
    public bool Healed(float f)
    {
        //TODO: maybe a healing animation gets called here
        bool gotHealed = fHP == 100f ? false : true; // used to indicate if player is already max health
        float difference = 100f - f;
        fHP += f;
        if (fHP > 100f)
        {
            fHP = 100f;
        }
        if (gotHealed)
        {
            EventAggregator.GetInstance().Publish(new PlayerHealedEvent(fHP, playerNumber));
        }
        return gotHealed;
    }

    public bool Damaged(float f)
    {
        if (fHP > 0)
        {
            fHP -= f;
            EventAggregator.GetInstance().Publish(new PlayerDamagedEvent(fHP, playerNumber)); // fire event
            return true;
        }
        return false;
    }

    protected void Death()
    {
        Settings.NumOfPlayers--;
        if (Settings.NumOfPlayers == 0)
        {
            //TODO: Call game over screen
        }
        Destroy(gameObject);
    }

    protected void GetAttackInput()
    {
        try
        {
            if (Input.GetAxis(myControllerInput.RTrigger) == 1 && Time.time > fAttackTime)
            {
                print("weapon inventory size = " + WeaponInventory.GetNumOfSlotUsed());
                print("Trying to fire " + WeaponInventory.GetCurrentItem().name);
                fAttackTime = Time.time + 1 / iBaseAttackRate;
                if (typeof(IRangedWeapon).IsAssignableFrom(WeaponInventory.GetCurrentItem()?.GetType()))
                {
                    print("weapon firing");
                    ((IRangedWeapon)WeaponInventory.GetCurrentItem()).Fire();
                }

            }
        }
        catch (System.ArgumentException)
        {
            /* 
             * Nothing to worry about.
             * This exception is thrown when the game has started, but the players have not been assigned a controller.
             */
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            Damaged(10);
        }
    }
}
