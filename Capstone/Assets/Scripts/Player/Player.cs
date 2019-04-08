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
    public Item CurrentWeapon;
    public Transform shootPosition;
    public Ammunition Ammunition = new Ammunition(500);

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

    protected void FixedUpdate()
    {
        //movement
        GetMovementInput();
        rb.MovePosition(rb.position + velocity * Time.deltaTime); // move the player after updating user input
        getRotationPosition();
        GetAttackInput();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //check for less than 1 so we can simply subtract enemy damage rather than checking for 0.
        if (fHP < 1f)
        {
            Death();
        }

    }

    protected void GetMovementInput()
    {
        try
        {
            float moveHorizontal = Input.GetAxis(myControllerInput.LeftHorizontalAxis);
            float moveVertical = Input.GetAxis(myControllerInput.LeftVerticalAxis);
            velocity = new Vector2(moveHorizontal, moveVertical)* fMoveRate; 
        }
        catch
        {

        }
    }

    private void getRotationPosition()
    {
        if (myControllerInput.inputType == InputType.KEYBOARD)
        {
            Vector3 position = Input.mousePosition;
            position = myCamera.ScreenToWorldPoint(position);
            direction = new Vector2(position.x - transform.position.x, position.y - transform.position.y);
            //Debug.Log("direction: " +direction + "\nx: " + position.x + "\ny: " + position.y );
            transform.right = direction; //transform may vary depending on sprite's image
        }
        else //use controller inputs 
        {
            try
            {
                float horizontal = Input.GetAxisRaw(myControllerInput.RightHorizontalAxis);
                float vertical = Input.GetAxisRaw(myControllerInput.RightVerticalAxis);
                float deadzone = 0.15f;
                Vector2 stickInput = new Vector2(horizontal, vertical);
                if (stickInput.magnitude < deadzone)
                {
                    stickInput = Vector2.zero;
                    rb.freezeRotation = true;
                }  
                else
                {
                    
                    rb.freezeRotation = false;
                    float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
                    Quaternion eulerRot = Quaternion.Euler(0.0f, 0.0f, angle);
                    transform.localRotation = Quaternion.Slerp(transform.rotation, eulerRot, Time.deltaTime * 120);
                    
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
        Debug.Log("Received " + f + " damage");
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
        EventAggregator.GetInstance().Publish(new OnPlayerDeathEvent(playerNumber));
        gameObject.SetActive(false);
    }

    protected void GetAttackInput()
    {
        try
        {
            if (Input.GetAxis(myControllerInput.RTrigger) == 1 && Time.time > fAttackTime)
            {
                if (CurrentWeapon != null)
                {
                    print("weapon inventory size = " + WeaponInventory.GetNumOfSlotUsed());
                    print("Trying to fire " + CurrentWeapon.name);
                    fAttackTime = Time.time + 1 / iBaseAttackRate;
                    CurrentWeapon.UseItem(this);
                }

            }

            if (Input.GetButtonDown(myControllerInput.LeftButton))
            {
                // Reload 
                if (CurrentWeapon != null && CurrentWeapon is RangedWeapon)
                {
                    if (Ammunition.Amount <= 0)
                    {
                        Debug.Log("I have no more ammunition");
                    }
                    else
                    {

                        StartCoroutine(((RangedWeapon)CurrentWeapon).Reload(playerNumber, Ammunition));

                    }

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

    public abstract bool CanEquipWeapon(Item item);

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public float GetMoveRate()
    {
        return fMoveRate;
    }

    public void SetMoveRate(float rate)
    {
        fMoveRate = rate;
    }

    public void MultiplyMoveRate(float rate)
    {
        fMoveRate *= rate;
    }

    //used to access methods from outside Player class
    //may not be needed
    public static explicit operator Player(GameObject v)
    {
        throw new NotImplementedException();
    }

    public void SetLocation(float xPos, float yPos)
    {
        Vector2 newLocation;
        newLocation.x = xPos;
        newLocation.y = yPos;
        rb.transform.position = newLocation;
    }
}
