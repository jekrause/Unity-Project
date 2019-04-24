using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InventoryHandler : MonoBehaviour, ISubscriber<OnMainInvChangedEvent>, ISubscriber<OnWeaponInvChangedEvent>
{
    private int playerNumber;
    private Player player;
    private bool actionInProgress; // to elminate multiple calls on a long button press
    private bool InventoryHUDFocused;
    private Item itemOnGround;
    public static List<GameObject> ObjectsPickedUp = new List<GameObject>();
    private bool ItemFocused;
    private EventAggregator eventAggregator;

    [SerializeField] private InventoryHUD InventoryHUD;
    private Inventory MainInventory;
    private Inventory WeaponInventory;
    private Sprite PlayerOriginalImage;
    private bool IteratingMainInv = true;
    private int EquippedWeaponSlot; // index of the current weapon equipped
    private int WeaponSlotIndex; // Used for iterating through weapon inventory when inventory is toggled only!
    private int MainSlotIndex;


    // Inventory HUD messages
    private string PickUpItemMessage = "-Press (N/A): Pick Up-";

    // Inventory HUD action panel messages
    private string ItemTypeMessage = "Item";
    private string DefaultActionMessage;
    private string SalvageWeaponMessage;
    private string EquipOrSalvageMessage;


    // item pick up timer
    private float timerButtonHeldDown;
    private const float BUTTON_HELD_DOWN_TIME = 0.75f;
    private bool buttonHeldDown = false;

    // Use this for initialization
    void Start()
    {
        // get player info

        player = GetComponent<Player>();
        playerNumber = player.playerNumber;
        InitializeInputMessages();
        MainInventory = player.MainInventory;
        WeaponInventory = player.WeaponInventory;
        if (MainInventory == null || WeaponInventory == null)
        {
            throw new System.MissingFieldException("Inventory Handler: Player should have Inventory as a field");
        }
        eventAggregator = EventAggregator.GetInstance();
    }

    private void OnEnable()
    {
        EventAggregator.GetInstance().Register<OnMainInvChangedEvent>(this);
        EventAggregator.GetInstance().Register<OnWeaponInvChangedEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnMainInvChangedEvent>(this);
        EventAggregator.GetInstance().Unregister<OnWeaponInvChangedEvent>(this);
    }

    public void OnEventHandler(OnMainInvChangedEvent eventData)
    {
        if(eventData.playerNumber == player.playerNumber)
        {
            InventoryHUD.OnItemAdd(eventData.item, eventData.slotNum, eventData.Quantity);
        }
    }

    public void OnEventHandler(OnWeaponInvChangedEvent eventData)
    {
        if (eventData.playerNumber == player.playerNumber)
        {
            if (eventData.Equipped == true)
            {
                InventoryHUD.OnWeaponEquip(eventData.slotNum);
                EquippedWeaponSlot = eventData.slotNum;
            }
            InventoryHUD.OnWeaponStow(eventData.item, eventData.slotNum, -1);
        }
    }

    public void ClearInventoryHUD()
    {
        InventoryHUD.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.PlayerState == PlayerState.ALIVE)
            ReadControllerInput();
    }

    public void InitializeInputMessages()
    {
        if (player.myControllerInput != null && player.myControllerInput.inputType != InputType.NONE)
        {
            //Initialize action panel messages
            DefaultActionMessage = "Item: " + ItemTypeMessage + "\nPress '" + player.DownPlatformButton + "' : Use\nPress '" + player.DownPlatformButton + "' : Drop";
            SalvageWeaponMessage = "Item: " + ItemTypeMessage + "\nHold '" + player.DownPlatformButton + "' : Salvage For Ammo\nPress '" + player.RightPlatformButton + "' : Drop";
            EquipOrSalvageMessage = "Item: " + ItemTypeMessage + "\nPress '" + player.DownPlatformButton + "' : Equip" +
                                    "\nHold '" + player.DownPlatformButton + "' : Salvage For Ammo" +
                                    "\nPress '" + player.RightPlatformButton + "' : Drop";

  
            DefaultActionMessage = "Item: " + ItemTypeMessage;
        }

    }

    public GameObject GetHUD() => InventoryHUD.gameObject;

    public void AssignInventoryHUD(GameObject HUD)
    {
        if (HUD.GetComponent<InventoryHUD>() != null)
        {
            InventoryHUD = HUD.GetComponent<InventoryHUD>();
            InventoryHUD.gameObject.SetActive(true);
        }

    }

    private void ReadControllerInput()
    {
        if (player.myControllerInput != null && player.myControllerInput.inputType != InputType.NONE
            && (player.InteractionState == InteractionState.INVENTORY_STATE || player.InteractionState == InteractionState.OPEN_STATE) )
        {

            if (Input.GetButtonDown(player.myControllerInput.UpButton))
            {
                InventoryHUDFocused = !InventoryHUDFocused; // toggle inventory selection
                InventoryHUD.InventoryToggled(InventoryHUDFocused);
                if (InventoryHUDFocused)
                {
                    AudioManager.Play("Open_Inventory");
                    UpdateAndDisplayInteractionPanel();
                    player.InteractionState = InteractionState.INVENTORY_STATE;
                }
                else
                {
                    AudioManager.Play("Close_Inventory");
                    player.InteractionState = InteractionState.OPEN_STATE;
                    player.InteractionPanel.RemoveInteractivePanel();
                    if (ItemFocused)
                    {
                        if (itemOnGround != null)
                            player.InteractionPanel.ShowInteractionPanel(itemOnGround.GetType() + "", PickUpItemMessage);
                    }
                    else
                    {
                        itemOnGround = null;
                    }
                        
                    
                }
            }

            if (InventoryHUDFocused) // user has inventory toggled on
            {
                if (Settings.OS == "Windows")
                {
                    if (Input.GetAxis(player.myControllerInput.DPadX_Windows) > 0) // D-Pad right, iterate throught item inventory
                    {
                        IterateRightList();
                    }
                    else if (Input.GetAxis(player.myControllerInput.DPadX_Windows) < 0) // D-Pad left, iterate throught item inventory
                    {
                        IterateLeftList();
                    }
                    
                    if (Input.GetAxis(player.myControllerInput.DPadX_Windows) == 0 && Input.GetAxis(player.myControllerInput.DPadY_Windows) == 0)
                    {
                        actionInProgress = false; // user let go of button, so action is no longer in progress
                    }
                }
                else
                {
                    if (player.myControllerInput.inputType == InputType.KEYBOARD)
                    {
                        if (Input.GetKeyDown(KeyCode.RightArrow)) // D-Pad right, iterate throught item inventory
                        {
                            IterateRightList();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow)) // D-Pad left, iterate throught item inventory
                        {
                            IterateLeftList();
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown(player.myControllerInput.DPadRight_Mac))
                        {
                            IterateRightList();
                        }
                        else if (Input.GetButtonDown(player.myControllerInput.DPadLeft_Mac))
                        {
                            IterateLeftList();
                        }
                    }
                    
                }

                if (Input.GetButtonDown(player.myControllerInput.RightButton)) // item remove
                {
                    RemoveItemFromInv();
                }
                else if (Input.GetButton(player.myControllerInput.DownButton))
                {
                    Item itemToSalvage = null;
                    if (IteratingMainInv)
                    {
                        itemToSalvage = MainInventory.GetItemInSlot(MainSlotIndex);
                    }
                    else
                    {
                        itemToSalvage = WeaponInventory.GetItemInSlot(WeaponSlotIndex);
                    }

                    if (timerButtonHeldDown > BUTTON_HELD_DOWN_TIME)
                    {
                        if (itemToSalvage != null && itemToSalvage is RangedWeapon)
                        {
                            if (IteratingMainInv)
                            {
                                if (SalvageWeaponForAmmo((RangedWeapon)itemToSalvage) == true)
                                {
                                    MainInventory.RemoveAllItemInSlot(MainSlotIndex);
                                    UpdateAndDisplayInteractionPanel();
                                    timerButtonHeldDown = 0;
                                }
                            }
                            else
                            {
                                if (SalvageWeaponForAmmo((RangedWeapon)itemToSalvage) == true)
                                {
                                    WeaponInventory.RemoveAllItemInSlot(WeaponSlotIndex);
                                    UpdateAndDisplayInteractionPanel();
                                    timerButtonHeldDown = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        timerButtonHeldDown += Time.deltaTime;
                        if (itemToSalvage != null && itemToSalvage.GetItemType() == Item.Type.WEAPON && timerButtonHeldDown <= BUTTON_HELD_DOWN_TIME)
                            player.InteractionPanel.ShowLoadBar(timerButtonHeldDown, BUTTON_HELD_DOWN_TIME);
                    }

                }
                else if (Input.GetButtonUp(player.myControllerInput.DownButton))
                {
                    player.InteractionPanel.RemoveLoadBar();
                    if (timerButtonHeldDown < 0.15f)
                    {
                        if (IteratingMainInv)
                        {
                            UseItemFromMainInv();
                        }
                        else
                        {
                            UseItemFromWeaponInv(WeaponSlotIndex);
                        }
                    }
                    timerButtonHeldDown = 0;
                }
            }
            else // Inventory HUD is no longer focused
            {
                if (Input.GetButton(player.myControllerInput.DownButton)) // add item
                {
                    if (timerButtonHeldDown > BUTTON_HELD_DOWN_TIME)
                    {
                        buttonHeldDown = true;
                        AddItem();
                    }
                    else
                    {
                        timerButtonHeldDown += Time.deltaTime;
                        if (itemOnGround != null && itemOnGround.GetItemType() == Item.Type.WEAPON && timerButtonHeldDown < BUTTON_HELD_DOWN_TIME)
                            player.InteractionPanel.ShowLoadBar(timerButtonHeldDown, BUTTON_HELD_DOWN_TIME);

                    }
                }
                else if (Input.GetButtonUp(player.myControllerInput.DownButton))
                {
                    player.InteractionPanel.RemoveLoadBar();

                    if (timerButtonHeldDown < BUTTON_HELD_DOWN_TIME)
                    {
                        buttonHeldDown = false;
                        AddItem();
                    }
                    timerButtonHeldDown = 0;
                }
                else if (Input.GetButtonDown(player.myControllerInput.RBumper)) // equip weapon equipment from the next right slot
                {
                    EquipNextWeapon();
                }
                else if (Input.GetButtonDown(player.myControllerInput.LBumper)) // equip weapon equipment from the next left slot
                {
                    EquipPrevWeapon();
                }
            }
        }

    }

    public void OnRayCastItemEnter(Collider2D collider)
    {
        if (collider != null && !InventoryHUDFocused)
        {
            itemOnGround = collider.GetComponent<Item>();
            if (itemOnGround != null)
            {
                ItemFocused = true;
                ItemTypeMessage = itemOnGround.GetType() + "";
                if (player.CanEquipWeapon(itemOnGround))
                {
                    PickUpItemMessage = "-Press '" + player.DownPlatformButton + "' : Pick Up-\n-Hold '" + player.DownPlatformButton + "' : Equip- ";
                }
                else
                {
                    if (itemOnGround.GetItemType() == Item.Type.WEAPON)
                    {
                        PickUpItemMessage = "-Press '" + player.DownPlatformButton + "' : Pick Up-\n-Hold '" + player.DownPlatformButton + "' : Salvage For Ammo- ";
                    }
                    else
                    {
                        PickUpItemMessage = "-Press '" + player.DownPlatformButton + "' : Pick Up-";
                    }
                }

                player.InteractionPanel.ShowInteractionPanel(ItemTypeMessage, PickUpItemMessage);
            }
        }
    }

    public void OnRayCastItemExit()
    {
        itemOnGround = null;
        if (ItemFocused)
            ItemFocused = false;
        if (!InventoryHUDFocused)
            player.InteractionPanel.RemoveInteractivePanel();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        


    }

    /// <summary>
    /// Simply Update the Inventory Action Panel message and send the message so it can be Displayed on Inventory HUD
    /// </summary>
    private void UpdateAndDisplayInteractionPanel()
    {
        player.InteractionPanel.RemoveInteractivePanel();
        Item item = null;
        string actionMessageToShow = "";
        if (IteratingMainInv)
        {
            item = MainInventory.GetItemInSlot(MainSlotIndex);
        }
        else
        {
            item = WeaponInventory.GetItemInSlot(WeaponSlotIndex);
        }

        if (item != null)
        {
            ItemTypeMessage = item.GetType() + "";
            if (item.GetItemType() == Item.Type.WEAPON)
            {
                if (player.CanEquipWeapon(item))
                {
                    string isEquipped = item == player.CurrentWeapon ? "Unequip" : "Equip";
                    EquipOrSalvageMessage = "Press '" + player.DownPlatformButton + "' : " + isEquipped +
                                "\nHold '" + player.DownPlatformButton + "' : Salvage For Ammo" +
                                "\nPress '" + player.RightPlatformButton + "' : Drop ";
                    actionMessageToShow = EquipOrSalvageMessage;
                }
                else
                {
                    SalvageWeaponMessage = "Hold '" + player.DownPlatformButton + "' : Salvage For Ammo\nPress '" + player.RightPlatformButton + "' : Drop";
                    actionMessageToShow = SalvageWeaponMessage;
                }
            }
            else
            {
                DefaultActionMessage = "Press '" + player.DownPlatformButton + "' : Use\nPress '" + player.RightPlatformButton + "' : Drop";
                actionMessageToShow = DefaultActionMessage;
            }
        }
        else
        {
            ItemTypeMessage = "N/A";
            DefaultActionMessage = "-Empty-";
            actionMessageToShow = DefaultActionMessage;
        }
        player.InteractionPanel.ShowInteractionPanel(ItemTypeMessage,actionMessageToShow);
    }

    private void EquipNextWeapon()
    {
        InterruptWeaponReload();
        EquippedWeaponSlot = ++EquippedWeaponSlot >= WeaponInventory.MAX_SLOT_SIZE ? 0 : EquippedWeaponSlot;
        UseItemFromWeaponInv(EquippedWeaponSlot);
    }

    private void EquipPrevWeapon()
    {
        InterruptWeaponReload();
        EquippedWeaponSlot = --EquippedWeaponSlot < 0 ? WeaponInventory.MAX_SLOT_SIZE - 1 : EquippedWeaponSlot;
        UseItemFromWeaponInv(EquippedWeaponSlot);
    }

    private void AddItem()
    {
        if (ItemFocused)
        {
            if (itemOnGround != null)
            {
                int slot = -1;

                if (itemOnGround.GetItemType() == Item.Type.WEAPON && buttonHeldDown) // attempt equip weapon on the spot
                {
                    if (player.CanEquipWeapon(itemOnGround))
                    {
                        slot = WeaponInventory.AddItem(itemOnGround);
                    }
                    else
                    {
                        if (itemOnGround is RangedWeapon)
                        {
                            SalvageWeaponForAmmo((RangedWeapon)itemOnGround);
                        }
                    }
                }
                else // it is either a quest item or healing item
                    slot = MainInventory.AddItem(itemOnGround);

                if (slot != -1) // if item added succesfully to inventory
                {
                    if (itemOnGround.GetItemType() == Item.Type.WEAPON && buttonHeldDown)
                    {
                        InventoryHUD.OnWeaponStow(itemOnGround, slot, -1);
                        if (player.CurrentWeapon == null)
                        {
                            InventoryHUD.OnWeaponEquip(slot);
                            EquippedWeaponSlot = slot;
                            player.UpdatePlayerCurrentWeapon((Weapon)itemOnGround);
                        }
                    }
                    else
                    {
                        InventoryHUD.OnItemAdd(itemOnGround, slot, MainInventory.GetQuantityInSlot(slot));
                        if (itemOnGround is RangedWeapon)
                        {
                            AudioManager.Play(((RangedWeapon)itemOnGround).ReloadFinishSound);
                        }
                        else
                        {
                            if (itemOnGround is QuestItem)
                            {
                                EventAggregator.GetInstance().Publish<OnQuestItemPickUpEvent>(new OnQuestItemPickUpEvent((QuestItem)itemOnGround));
                            }
                            AudioManager.Play("PickUpItem");
                        }
                    }

                    // disable game object
                    itemOnGround.gameObject.SetActive(false);
                    ObjectsPickedUp.Add(itemOnGround.gameObject);
                    ItemFocused = false;
                    player.InteractionPanel.RemoveInteractivePanel();
                }
                else
                {
                    Debug.Log("Couldnt add");
                }
            }

        }
    }

    /// <summary>
    /// Attempt to use an item from main inventory. Using a weapon will simply stow it to the weapon inventory,
    /// while other items will vary depending on what type the item is.
    /// </summary>
    private void UseItemFromMainInv()
    {
        Item itemToUse = MainInventory.GetItemInSlot(MainSlotIndex);
        if (itemToUse != null)
        {
            if (itemToUse.GetItemType() == Item.Type.WEAPON)
            {
                // equip weapon
                if (player.CanEquipWeapon(itemToUse))
                {
                    int weaponSlot = WeaponInventory.AddItem(itemToUse);
                    if (weaponSlot != -1)
                    {
                        MainInventory.RemoveAllItemInSlot(MainSlotIndex);
                        InventoryHUD.OnWeaponStow(itemToUse, weaponSlot, MainSlotIndex);
                        if (player.CurrentWeapon == null)
                        {
                            InventoryHUD.OnWeaponEquip(weaponSlot);
                            EquippedWeaponSlot = weaponSlot;
                            player.UpdatePlayerCurrentWeapon((Weapon)itemToUse);
                        }
                    }
                    else
                    {
                        // equip failed due to full weapon inventory, do nothing
                        Debug.Log("InventoryHandler: Weapon inventory full, could not store weapon into weapon inventory");
                    }
                }
                else
                {
                    Debug.Log("Player class: " + player.GetType() + " cannot equip " + itemToUse.GetType() + " weapon type.");
                }
            }
            else
            {
                if (MainInventory.UseItem(GetComponent<Player>(), MainSlotIndex))
                {
                    InventoryHUD.OnItemRemove(MainInventory.GetQuantityInSlot(MainSlotIndex));
                    ObjectsPickedUp.Remove(itemToUse.gameObject);
                    if (MainInventory.GetItemInSlot(MainSlotIndex) == null)
                        Destroy(itemToUse.gameObject);
                }

            }
            UpdateAndDisplayInteractionPanel();
        }
    }

    /// <summary>
    /// Attempt to equip a weapon from weapon inventory slot. If the slot is empty, the player will be replaced
    /// by the original sprite (no weapon). Otherwise, it will equip or unequip the weapon currently selected.
    /// </summary>
    private void UseItemFromWeaponInv(int weaponSlot)
    {
        Weapon weaponToUse = (Weapon)WeaponInventory.GetItemInSlot(weaponSlot);

        if (weaponToUse != null)
        {
            if (player.CurrentWeapon != null && player.CurrentWeapon == weaponToUse) // unequip their weapon
            {
                InterruptWeaponReload();
                player.UpdatePlayerCurrentWeapon(null);
                InventoryHUD.OnWeaponUnEquip();
            }
            else // equip weapon
            {
                InventoryHUD.OnWeaponEquip(weaponSlot);
                player.UpdatePlayerCurrentWeapon(weaponToUse);
                if (weaponToUse is RangedWeapon)
                    AudioManager.Play(((RangedWeapon)weaponToUse).ReloadFinishSound);
            }

            if (InventoryHUDFocused)
                UpdateAndDisplayInteractionPanel();
        }
        else // no weapon in slot, so use player orginal image
        {
            InterruptWeaponReload();
            player.UpdatePlayerCurrentWeapon(null);
            InventoryHUD.OnWeaponEquip(weaponSlot);
        }
    }

    private bool SalvageWeaponForAmmo(RangedWeapon itemToSalvage)
    {
        bool salvagedSuccess = false;

        if (itemToSalvage != null)
        {
            salvagedSuccess = player.Ammunition.Add((itemToSalvage).AmmoClip.CurrentAmmoRaw);
            if (salvagedSuccess)
            {
                if (itemToSalvage == player.CurrentWeapon)
                {
                    player.UpdatePlayerCurrentWeapon(null);
                }

                EventAggregator.GetInstance().Publish<OnPlayerAmmoChangedEvent>(new OnPlayerAmmoChangedEvent(player.playerNumber, player.Ammunition));
                AudioManager.Play("SalvagedWeapon");
                ItemFocused = false;
                player.InteractionPanel.RemoveInteractivePanel();
                Debug.Log("InventoryHandler: " + itemToSalvage.GetType() + " weapon salvaged for ammo.");
                Destroy(itemToSalvage.gameObject);

                if (InventoryHUDFocused)
                {
                    InventoryHUD.OnItemRemove(0);
                    player.InteractionPanel.RemoveLoadBar();
                }
            }
            else
            {
                Debug.Log("InventoryHandler: Cannot salvage weapon, Im already maxed out on ammo");
                player.InteractionPanel.ShowRejectedLoadBar();
            }
        }
        return salvagedSuccess;
    }

    

    /// <summary>
    /// Called when player attempts to drop, swap or unequip their current weapon in the case
    /// that the gun is currently reloading
    /// </summary>
    private void InterruptWeaponReload()
    {
        Weapon playerWeapon = (Weapon)player.CurrentWeapon;
        if (playerWeapon != null && playerWeapon is RangedWeapon)
        {
            ((RangedWeapon)playerWeapon).ReloadingInterrupted(player.playerNumber);
        }
    }

    /// <summary>
    /// Remove the selected item from the main inventory or weapon inventory and update the inventory HUD accordingly
    /// </summary>
    private void RemoveItemFromInv()
    {
        if (!actionInProgress)
        {
            actionInProgress = true;
            if (IteratingMainInv)
            {
                Item itemToRemove = MainInventory.GetItemInSlot(MainSlotIndex);
                if (itemToRemove != null)
                {
                    int itemIndex = 0;
                    for (int i = 0; i < ObjectsPickedUp.Count; i++)
                    {
                        if (ObjectsPickedUp[i].GetComponent<Item>().GetType() == itemToRemove.GetType())
                        {
                            Vector3 itemDropPosition = new Vector3(transform.position.x + 1f, transform.position.y - 1f);
                            ObjectsPickedUp[i].transform.position = itemDropPosition;
                            ObjectsPickedUp[i].SetActive(true);
                            AudioManager.Play("ItemDropped");
                            MainInventory.RemoveItemInSlot(MainSlotIndex);
                            itemIndex = i;
                            break;
                        }
                    }
                    InventoryHUD.OnItemRemove(MainInventory.GetQuantityInSlot(MainSlotIndex));
                    ObjectsPickedUp.RemoveAt(itemIndex);
                }
            }
            else
            {
                Weapon weaponToRemove = (Weapon)WeaponInventory.GetItemInSlot(WeaponSlotIndex);
                if (weaponToRemove != null)
                {
                    if (player.CurrentWeapon != null && player.CurrentWeapon == weaponToRemove)
                    {
                        InterruptWeaponReload();
                        player.UpdatePlayerCurrentWeapon(null);
                        InventoryHUD.OnWeaponUnEquip();
                    }
                    WeaponInventory.RemoveAllItemInSlot(WeaponSlotIndex);
                    int itemIndex = 0;
                    for (int i = 0; i < ObjectsPickedUp.Count; i++)
                    {
                        if (ObjectsPickedUp[i].GetComponent<Weapon>() == weaponToRemove)
                        {
                            ObjectsPickedUp[i].transform.position = transform.position;
                            ObjectsPickedUp[i].SetActive(true);
                            AudioManager.Play("ItemDropped");
                            WeaponInventory.RemoveAllItemInSlot(WeaponSlotIndex);
                            itemIndex = i;
                            break;
                        }
                    }
                    InventoryHUD.OnItemRemove(WeaponInventory.GetQuantityInSlot(WeaponSlotIndex));
                    ObjectsPickedUp.RemoveAt(itemIndex);
                }

            }
            UpdateAndDisplayInteractionPanel();
        }
    }

    /// <summary>
    /// Iterate to the next item. It will iterate through the main and weapon inventory
    /// </summary>
    private void IterateRightList()
    {
        if (!actionInProgress)
        {
            bool oldIteringMainInv = IteratingMainInv;
            IteratingMainInv = InventoryHUD.IterateRight(); // return true if iterating main inv
            actionInProgress = true;

            if (oldIteringMainInv == IteratingMainInv)
            {
                if (IteratingMainInv)
                {
                    MainSlotIndex = ++MainSlotIndex >= MainInventory.MAX_SLOT_SIZE ? 0 : MainSlotIndex;
                }
                else
                {
                    WeaponSlotIndex = ++WeaponSlotIndex >= WeaponInventory.MAX_SLOT_SIZE ? 0 : WeaponSlotIndex;
                }
            }
            else
            {
                if (IteratingMainInv)
                {
                    MainSlotIndex = 0;
                }
                else
                {
                    WeaponSlotIndex = 0;
                }
            }

            if (InventoryHUDFocused)
                UpdateAndDisplayInteractionPanel();

        }
    }

    /// <summary>
    /// Iterate to the previous item. It will iterate through the main and weapon inventory
    /// </summary>
    private void IterateLeftList()
    {
        if (!actionInProgress)
        {
            bool oldIteringMainInv = IteratingMainInv;
            IteratingMainInv = InventoryHUD.IterateLeft(); // return true if iterating main inv
            actionInProgress = true;

            if (oldIteringMainInv == IteratingMainInv)
            {
                if (IteratingMainInv)
                {
                    MainSlotIndex = --MainSlotIndex < 0 ? MainInventory.MAX_SLOT_SIZE - 1 : MainSlotIndex;
                }
                else
                {
                    WeaponSlotIndex = --WeaponSlotIndex < 0 ? WeaponInventory.MAX_SLOT_SIZE - 1 : WeaponSlotIndex;
                }
            }
            else
            {
                if (IteratingMainInv)
                {
                    MainSlotIndex = MainInventory.MAX_SLOT_SIZE - 1;
                }
                else
                {
                    WeaponSlotIndex = WeaponInventory.MAX_SLOT_SIZE - 1;
                }
            }

            if (InventoryHUDFocused)
                UpdateAndDisplayInteractionPanel();
        }
    }

    
}
