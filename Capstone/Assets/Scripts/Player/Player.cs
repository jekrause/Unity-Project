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
    protected float fAttackTime = 3;  //The higher this number, the more frequent you can shoot
    [SerializeField] protected float fHP = 100f; // serialize field for testing purposes

    protected float fDamage = 10f;         //default damage if no weapon is equipped
    protected float fMoveRate = 1f;
    protected float fAttackRadius = 2f;
    protected float fProjSpeed = 20f;
    [SerializeField] public int playerNumber;

    // player's movement
    private Rigidbody2D rb;
    protected Vector2 velocity;
    private Animator feetAnimation;

    //attacking
    private Sprite PlayerOriginalImage;
    public Weapon basicWeapon;
    public Weapon CurrentWeapon;
    public Transform shootPosition;
    public readonly Ammunition Ammunition = new Ammunition(500);

    // mouse
    private Vector2 direction;

    //camera for the specific player
    [SerializeField] public Camera myCamera;

    // reference to the controller that is attached to the player
    public MyControllerInput myControllerInput = new MyControllerInput();
    public string DownPlatformButton { get; private set; } = "N/A";
    public string RightPlatformButton { get; private set; } = "N/A";

    //player Interaction state
    public InteractionState InteractionState = InteractionState.OPEN_STATE;
    public InteractionHandler InteractionPanel { get; private set; }

    public readonly Inventory MainInventory = new Inventory(6);
    public readonly Inventory WeaponInventory = new Inventory(3);
	private InventoryHandler InventoryHandler;
    private LootBagHandler LootBagHandler;

    // Revive functionality
    private ReviveBarHandler reviveBarHandler;
    public PlayerState PlayerState { get; protected set; } = PlayerState.ALIVE;
    public const float MAX_DOWN_TIME = 25f;
    public const float MAX_REVIVE_TIME = 7f;
    private float ReviveTimer = 0; // counts up to TIME_TO_REVIVE
    private float DownStateTimer = MAX_DOWN_TIME; // counts down to 0
    private bool IsBeingRevived = false; // when true, pause the DownStateTimer to allow reviving
    private Player downPlayer;

    // Used to keep track what was our last Raycast since they don't have RaycastExit like colliderExit
    private Collider2D MostRecentCollider;

    public GameObject MyHUD;

    // Start is called before the first frame update

    private void Awake()
    {

    }
    protected void Start()
    {
        feetAnimation = transform.Find("Feet").GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        InventoryHandler = GetComponent<InventoryHandler>();
        reviveBarHandler = transform.Find("PlayerDownCanvas").GetComponent<ReviveBarHandler>();
        PlayerOriginalImage = GetComponent<SpriteRenderer>().sprite;
        InteractionPanel = MyHUD.transform.Find("InteractionPanel").GetComponent<InteractionHandler>();
        LootBagHandler = GetComponent<LootBagHandler>();

        switch (myControllerInput.inputType)
        {

            case InputType.KEYBOARD:
                DownPlatformButton = "E";
                RightPlatformButton = "Esc";
                break;

            case InputType.PS4_CONTROLLER:
                DownPlatformButton = "X";
                RightPlatformButton = "O";
                break;

            case InputType.XBOX_CONTROLLER:
                DownPlatformButton = "A";
                RightPlatformButton = "B";
                break;
        }
    }

    protected void FixedUpdate()
    {
        //movement
        try
        {
            GetMovementInput();
            getRotationPosition();
            rb.MovePosition(rb.position + velocity * Time.deltaTime); // move the player after updating user input

            if (PlayerState == PlayerState.DOWN)
            {
                fMoveRate = 1f; // crawl speed
            }
            else if(PlayerState == PlayerState.ALIVE)
            {
                if(InteractionState == InteractionState.OPEN_STATE)
                    GetAttackInput();
            }
            
            
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (PlayerState == PlayerState.ALIVE)
        {
            if(fHP <= 0)
            {
                PlayerState = PlayerState.DOWN;
                reviveBarHandler.OnReviveHandler(MAX_DOWN_TIME, 0); //start the timer
            }
            else
            {
                if(InteractionState != InteractionState.INVENTORY_STATE)
                    CastRayCast();
            }
        }
        else if(PlayerState == PlayerState.DOWN)
        {
            if (!IsBeingRevived)
            {
                DownStateTimer -= Time.deltaTime;
                reviveBarHandler.OnReviveHandler(DownStateTimer, 0);
            }
            if (DownStateTimer <= 0)
                Death();
            
        }

    }

    public void WaitForFireSprite(Sprite originalSprite, float delaySeconds)
    {
        StartCoroutine(FireSpriteDelay(originalSprite,delaySeconds));
    }
    public IEnumerator FireSpriteDelay(Sprite originalSprite, float delaySeconds)
    {
        yield return new WaitForSecondsRealtime(delaySeconds);
        GetComponent<SpriteRenderer>().sprite = originalSprite; //change sprite back to regular image
    }

    protected void GetMovementInput()
    {
        try
        {
            float moveHorizontal = Input.GetAxisRaw(myControllerInput.LeftHorizontalAxis);
            float moveVertical = Input.GetAxisRaw(myControllerInput.LeftVerticalAxis);
            velocity = new Vector2(moveHorizontal, moveVertical)* fMoveRate;
            if (moveHorizontal == 0 && moveVertical == 0)
            {
                feetAnimation.SetBool("Moving", false);
            }
            else
            {
                feetAnimation.SetBool("Moving", true);
            }
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

    public bool Damaged(object[] storage)
    {
        try
        {
            Debug.Log("Received " + storage[0] + " damage");
            if (fHP > 0)
            {
                fHP -= (int)storage[0];
                EventAggregator.GetInstance().Publish(new PlayerDamagedEvent(fHP, playerNumber)); // fire event
                return true;
            }
            else
        	{
	            if(PlayerState == PlayerState.ALIVE)
	            {
	                PlayerState = PlayerState.DOWN;
	            }
   
        	}
            
            return false;
        }
        catch(InvalidCastException e)
        {
            return false;
        }
    }

    protected void Death()
    {
        PlayerState = PlayerState.KILLED;
        EventAggregator.GetInstance().Publish(new OnPlayerDeathEvent(playerNumber));
        GameObject prefab = Resources.Load("Prefabs/Bag") as GameObject;
        GameObject bagObj = Instantiate(prefab, transform.position, transform.rotation); // create the player loot bag

        Inventory ptr = bagObj.GetComponent<LootBag>().Inventory;

        for (int i = 0; i < MainInventory.MAX_SLOT_SIZE; i++)
        {
            if (MainInventory.GetItemInSlot(i) != null)
                ptr.AddItem(MainInventory.GetItemInSlot(i));
        }

        for (int i = 0; i < WeaponInventory.MAX_SLOT_SIZE; i++)
        {
            if (WeaponInventory.GetItemInSlot(i) != null)
                ptr.AddItem(WeaponInventory.GetItemInSlot(i));
        }
        GetComponent<InventoryHandler>().ClearInventoryHUD();
        MyHUD.gameObject.SetActive(false);
        bagObj.gameObject.SetActive(true);
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
                    fAttackTime = Time.time + CurrentWeapon.GetAttackRate();
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

    private void CastRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 2f);
        if(hit.collider != null)
        {
            switch (hit.collider.tag)
            {
                case ("Player"):
                    //Attempt to revive down player
                        downPlayer = hit.collider.GetComponent<Player>();
                    if (downPlayer.PlayerState == PlayerState.DOWN) // only one player can revive another one at a time
                    {
                        MostRecentCollider = hit.collider;
                        InteractionState = InteractionState.REVIVING_STATE;
                        if (ReviveTimer == 0)
                        {
                            downPlayer.OnReviveStart(myControllerInput.inputType);
                        }
                           
                        RevivePlayer(downPlayer);
                    }
                        
                    break;

                case ("Item"):
                    MostRecentCollider = hit.collider;
                    InventoryHandler.OnRayCastItemEnter(hit.collider);
                    break;

                case ("Weapon"):
                    MostRecentCollider = hit.collider;
                    InventoryHandler.OnRayCastItemEnter(hit.collider);
                    break;

                case ("Helicopter"):
                    break;

                case ("LootBag"):
                    MostRecentCollider = hit.collider;
                    LootBagHandler.OnRayCastLootBagEnter(hit.collider);
                    break;
            }
            
        }
        else
        {
            if(MostRecentCollider != null)
            {
                switch (MostRecentCollider.tag)
                {
                    case ("Player"):
                        if (ReviveTimer < MAX_REVIVE_TIME && downPlayer != null)
                        {
                            ReviveTimer = 0; // reset as player went away from down player
                            downPlayer.OnReviveCancel();
                            downPlayer = null;
                            InteractionState = InteractionState.OPEN_STATE;
                        }
                        break;

                    case ("Item"):
                        Debug.Log("RaycastExit Item");
                        InventoryHandler.OnRayCastItemExit();
                        break;

                    case ("Weapon"):
                        Debug.Log("RaycastExit Weapon");
                        InventoryHandler.OnRayCastItemExit();
                        break;

                    case ("Helicopter"):
                        Debug.Log("RaycastExit Helicopter");
                        break;

                    case ("LootBag"):
                        Debug.Log("RaycastExit Bag");
                        LootBagHandler.OnRayCastLootBagExit();
                        break;
                }
            }

            MostRecentCollider = null; // reset
            
           
        }

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


    private void RevivePlayer(Player player)
    {
        if (Input.GetButton(myControllerInput.LeftButton))
        {
            if (ReviveTimer < MAX_REVIVE_TIME)
            {
                InteractionState = InteractionState.REVIVING_STATE;
                ReviveTimer += Time.deltaTime;
                player.reviveBarHandler.OnReviveHandler(player.DownStateTimer, ReviveTimer);
                Debug.Log(player.name + " is being revived\nTimer_To_Revive: " + ReviveTimer + "\nDown_Timer: " + player.DownStateTimer);
                player.IsBeingRevived = true;

            }
            else
            {
                InteractionState = InteractionState.OPEN_STATE;
                Debug.Log(this.name + " revived player successfully");
                player.OnReviveCompleted();
                ReviveTimer = 0;
            }
        }

        if (Input.GetButtonUp(myControllerInput.LeftButton))
        {
            if (ReviveTimer < MAX_REVIVE_TIME)
            {
                Debug.Log("You didn't finish reviving and let go");
                InteractionState = InteractionState.OPEN_STATE;
                ReviveTimer = 0; // reset as player let go of the button
                player.OnReviveCancel();
            }
        }

    }

    /// <summary>
    /// Change the player's sprite and current weapon to the Weapon passed in. If the weapon is null, the original player image is used instead.
    /// </summary>
    public void UpdatePlayerCurrentWeapon(Weapon currentWeapon)
    {
        if (currentWeapon == null)
        {
            CurrentWeapon = null;
            GetComponent<SpriteRenderer>().sprite = PlayerOriginalImage;
        }
        else
        {
            CurrentWeapon = currentWeapon;
            GetComponent<SpriteRenderer>().sprite = currentWeapon.PlayerImage;
            if(CurrentWeapon is RangedWeapon)
            {
                AudioManager.Play(((RangedWeapon)CurrentWeapon).ReloadFinishSound);
            }
        }
        EventAggregator.GetInstance().Publish(new OnPlayerWeaponChangedEvent(playerNumber, currentWeapon, Ammunition));
    }

    public virtual void OnReviveCompleted() 
    {
        fHP = 50f;
        PlayerState = PlayerState.ALIVE;
        reviveBarHandler.OnReviveFinishHandler();
        DownStateTimer = MAX_DOWN_TIME; // reset down timer
        EventAggregator.GetInstance().Publish(new PlayerHealedEvent(fHP, playerNumber));
        IsBeingRevived = false;
    }

    public void OnReviveStart(InputType inputType) // input type to show the controller type that is healing this player
    {
        reviveBarHandler.SetPlatformButtonImage(inputType);
    }

    public void OnReviveCancel()
    {
        IsBeingRevived = false;
        reviveBarHandler.OnReviveCancelHandler();
    }

}

public enum PlayerState { ALIVE, DOWN, KILLED } 

public enum InteractionState { OPEN_STATE, INVENTORY_STATE, LOOTING_STATE, REVIVING_STATE}  // Player action state, determines what actions the player can do