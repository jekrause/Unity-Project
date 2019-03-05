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

    // Player events that other class can subscribe to
    // Using Observer design pattern where the observers (subscribers) will listen to the events

    // Player Health events
    public delegate void PlayerHealthEvent(float amount);
    public event PlayerHealthEvent OnPlayerDamagedEvent;
    public event PlayerHealthEvent OnPlayerHealedEvent;
    public event PlayerHealthEvent OnPlayerDeathEvent;

    // Player stats
    protected const int iBaseAttackRate = 1;

    protected float fAttackTime = 3;  //The higher this number, the more frequent you can shoot
    protected float fHP = 100f;

    protected float fDamage = 10f;         //default damage if no weapon is equipped
    protected float fMoveRate = 1f;
    protected float fAttackRadius = 2f;
    protected float fProjSpeed = 20f;
    public readonly int playerNumber;
    private Animator anim;

    // Inventory management
    [SerializeField] protected InventoryHUD InventoryHUD;
    protected Item itemOnGround;
    protected List<GameObject> ObjectsPickedUp = new List<GameObject>();
    protected bool ItemFocused;
    protected bool InventoryHUDFocused;
    protected bool actionInProgress; // to elminate multiple button press
    protected Inventory MainInventory;
    protected Inventory WeaponInventory;

    // player's movement
    private Rigidbody2D rb;
    protected Vector2 velocity;

    //attacking
    public Item basicWeapon;

    // mouse
    private Vector2 direction;

    //camera for the specific player
    [SerializeField] private Camera myCamera;
    
    // reference to the controller that is attached to the player
    public MyControllerInput myControllerInput;

    // Start is called before the first frame update

    private void Awake()
    {
    
    }
    protected void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        myControllerInput = new MyControllerInput();
        MainInventory = new Inventory(6);
        WeaponInventory = new Inventory(3);
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        //check for less than 1 so we can simply subtract enemy damage rather than checking for 0.
        if (fHP < 1f)
        {
            Death();
        }

        ReadControllerInput();

        //movement
        try
        {
            getRotationPosition();
            rb.MovePosition(rb.position + velocity * Time.deltaTime); // move the player after updating user input
        }
        catch
        {

        }
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

    private void ReadControllerInput()
    {
        if (myControllerInput.inputType != InputType.NONE)
        {

            if (Input.GetButtonDown(myControllerInput.UpButton) || Input.GetButton(myControllerInput.UpButton))
            {
                if(actionInProgress == false)
                {
                    InventoryHUDFocused = !InventoryHUDFocused; // toggle inventory selection
                    InventoryHUD.InventoryToggled(InventoryHUDFocused); 
                    Debug.Log("Inventory toggled");
                    actionInProgress = true;
                    StartCoroutine(DelayReadingInput());
                }
                
            }

            if (InventoryHUDFocused)
            {
                if (Input.GetAxis(myControllerInput.DPadX_Windows) > 0) // D-Pad right
                {
                    if (actionInProgress == false)
                    {
                        MainInventory.GetNextItem();
                        InventoryHUD.IterateRight(); 
                        actionInProgress = true;
                        StartCoroutine(DelayReadingInput());
                    }

                }
                else if (Input.GetAxis(myControllerInput.DPadX_Windows) < 0) // D-Pad left
                {
                    if (actionInProgress == false)
                    {
                        MainInventory.GetPrevItem();
                        InventoryHUD.IterateLeft();
                        actionInProgress = true;
                        StartCoroutine(DelayReadingInput());
                    }
                }else if(Input.GetButtonDown(myControllerInput.LeftButton) || Input.GetButton(myControllerInput.LeftButton))
                {
                    if (actionInProgress == false)
                    {
                        int slotNum = MainInventory.GetCurrentSlotNum();
                        Item itemToDrop = MainInventory.GetCurrentItem();
                        if(itemToDrop != null)
                        {
                            int itemIndex = 0;
                            for(int i = 0; i < ObjectsPickedUp.Count; i++)
                            {
                                if(ObjectsPickedUp[i].GetComponent<Item>().GetType() == itemToDrop.GetType())
                                {
                                    ObjectsPickedUp[i].transform.position = transform.position;
                                    ObjectsPickedUp[i].SetActive(true);
                                    MainInventory.RemoveItem(slotNum, false);
                                    actionInProgress = true;
                                    StartCoroutine(DelayReadingInput());
                                    itemIndex = i;
                                    break;
                                }
                            }
                            InventoryHUD.OnItemRemove(MainInventory.GetQuantityInSlot(slotNum));
                            ObjectsPickedUp.RemoveAt(itemIndex);
                        }
                        
                    }
                    
                }else if (Input.GetButtonDown(myControllerInput.DownButton) || Input.GetButton(myControllerInput.DownButton))
                {
                    Item itemToUse = MainInventory.GetCurrentItem();
                    if(itemToUse != null)
                    {
                        if (MainInventory.UseItem(this, MainInventory.GetCurrentSlotNum()))
                        {
                            InventoryHUD.OnItemRemove(MainInventory.GetQuantityInSlot(MainInventory.GetCurrentSlotNum()));
                            ObjectsPickedUp.Remove(itemToUse.gameObject);
                            if(MainInventory.GetCurrentItem() == null)
                                Destroy(itemToUse.gameObject);
                        }
                        
                    }
                    
                }
            }
            else
            {
                if (Input.GetButtonDown(myControllerInput.DownButton) || Input.GetButton(myControllerInput.DownButton))
                {
                    if (ItemFocused)
                    {
                        if (itemOnGround != null)
                        {
                            int slot = MainInventory.AddItem(itemOnGround);
                            if(slot != -1)
                            {
                                InventoryHUD.OnItemAdd(itemOnGround, slot, MainInventory.GetQuantityInSlot(slot));
                                itemOnGround.gameObject.SetActive(false);
                                ObjectsPickedUp.Add(itemOnGround.gameObject);
                                ItemFocused = false;
                                InventoryHUD.RemovePickUpItemMsg();
                            }
                            
                        }

                    }

                }
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        itemOnGround = collision.collider.GetComponent<Item>();
        Player player = collision.collider.GetComponent<Player>();
        if (itemOnGround != null)
        {
            ItemFocused = true;
            InventoryHUD.ShowPickUpItemMsg(myControllerInput.inputType);
        }else if(player != null)
        {
            Damaged(10); // testing Health HUD
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        itemOnGround = collision.collider.GetComponent<Item>();
        
        if (itemOnGround != null)
        {
            ItemFocused = false;
            InventoryHUD.RemovePickUpItemMsg();
        }
    }

    private IEnumerator DelayReadingInput()
    {

        yield return new WaitForSeconds(.25f);
        actionInProgress = false;

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
            OnPlayerHealedEvent?.Invoke(fHP); // if there are subscribers to the Event, invoke the methods
        }
        return gotHealed;
    }

    public bool Damaged(float f)
    {
        if (fHP > 0)
        {
            fHP -= f;
            OnPlayerDamagedEvent?.Invoke(fHP); // if there are subscribers to the Event, invoke the methods

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
}
