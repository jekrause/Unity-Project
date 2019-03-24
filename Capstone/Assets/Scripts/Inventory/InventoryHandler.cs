using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InventoryHandler : MonoBehaviour
{
    private int playerNumber;
    private Player player;
    private MyControllerInput myControllerInput;
    private bool actionInProgress; // to elminate multiple calls on a long button press
    private bool InventoryHUDFocused;
    private Item itemOnGround;
    private List<GameObject> ObjectsPickedUp = new List<GameObject>();
    private bool ItemFocused;
    private EventAggregator eventAggregator;

    [SerializeField] private InventoryHUD InventoryHUD;
    private Inventory MainInventory;
    private Inventory WeaponInventory;
    private Sprite PlayerOriginalImage;
    private bool IteratingMainInv = true;
    private int EquippedWeaponSlot; // index of the current weapon equipped
    private int WeaponSlotIndex; // Used for iterating through weapon inventory when inventory is toggled only!

    // item pick up timer
    private int timerButtonHeldDown;
    private bool buttonHeldDown = false;

    // Use this for initialization
    void Start()
    {
        // get player info
        player = GetComponent<Player>();
        playerNumber = player.playerNumber;
        myControllerInput = player.myControllerInput;
        MainInventory = player.MainInventory;
        WeaponInventory = player.WeaponInventory;
        PlayerOriginalImage = GetComponent<SpriteRenderer>().sprite;
        if (MainInventory == null || WeaponInventory == null)
        {
            throw new System.MissingFieldException("Inventory Handler: Player should have Inventory as a field");
        }
        eventAggregator = EventAggregator.GetInstance();


    }

    // Update is called once per frame
    void Update()
    {
        ReadControllerInput();
    }

    private void ReadControllerInput()
    {
        if (myControllerInput.inputType != InputType.NONE)
        {

            if (Input.GetButtonDown(myControllerInput.UpButton))
            {

                InventoryHUDFocused = !InventoryHUDFocused; // toggle inventory selection
                InventoryHUD.InventoryToggled(InventoryHUDFocused, myControllerInput.inputType);
                if (InventoryHUDFocused) InventoryHUD.RemovePickUpItemMsg();
                Debug.Log("Inventory toggled");

            }

            if (InventoryHUDFocused) // user has inventory toggled on
            {
                if (Settings.OS == "Windows")
                {
                    if (Input.GetAxis(myControllerInput.DPadX_Windows) > 0) // D-Pad right, iterate throught item inventory
                    {
                        IterateRightList();
                    }
                    else if (Input.GetAxis(myControllerInput.DPadX_Windows) < 0) // D-Pad left, iterate throught item inventory
                    {
                        IterateLeftList();
                    }
                }
                else
                {
                    if (Input.GetButton(myControllerInput.DPadRight_Mac))
                    {
                        IterateRightList();
                    }
                    else if (Input.GetButton(myControllerInput.DPadLeft_Mac))
                    {
                        IterateLeftList();
                    }
                }

                if (Input.GetButtonDown(myControllerInput.RightButton)) // item remove
                {
                    RemoveItemFromInv();
                }
                else if (Input.GetButtonDown(myControllerInput.DownButton)) // use item
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
            }
            else
            {
                if (Input.GetButton(myControllerInput.DownButton)) // add item
                {
                    if (timerButtonHeldDown >= 32)
                    {
                        timerButtonHeldDown = 0;
                        buttonHeldDown = true;
                        AddItem();
                    }
                    timerButtonHeldDown++;
                }
                else if (Input.GetButtonUp(myControllerInput.DownButton))
                {
                    if(timerButtonHeldDown < 32)
                    {
                        AddItem();
                        buttonHeldDown = false;
                        timerButtonHeldDown = 0;
                    }
                    
                }
                else if (Input.GetButtonDown(myControllerInput.RBumper)) // equip weapon equipment from the next right slot
                {
                    EquipNextWeapon();
                }
                else if (Input.GetButtonDown(myControllerInput.LBumper)) // equip weapon equipment from the next left slot
                {
                    EquipPrevWeapon();
                }
            }

            if (Input.GetAxis(myControllerInput.DPadX_Windows) == 0 && Input.GetAxis(myControllerInput.DPadY_Windows) == 0)
            {
                actionInProgress = false; // user let go of button, so action is no longer in progress
            }
        }
        else
        {
            //check if user has bind controller
            myControllerInput = player.myControllerInput;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!InventoryHUDFocused)
        {
            itemOnGround = collision.collider.GetComponent<Item>();
            if (itemOnGround != null)
            {
                ItemFocused = true;
                bool isWeapon = itemOnGround.GetItemType() == Item.Type.WEAPON;
                InventoryHUD.ShowPickUpItemMsg(myControllerInput.inputType, isWeapon);
            }
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
                int slot;

                if (itemOnGround.GetItemType() == Item.Type.WEAPON && buttonHeldDown) // attempt equip weapon on the spot
                    slot = WeaponInventory.AddItem(itemOnGround);
                else
                    slot = MainInventory.AddItem(itemOnGround);

                if (slot != -1)
                {
                    if (itemOnGround.GetItemType() == Item.Type.WEAPON && buttonHeldDown)
                    {
                        InventoryHUD.OnWeaponStow(itemOnGround, slot, -1);
                        if (player.CurrentWeapon == null)
                        {
                            GetComponent<SpriteRenderer>().sprite = ((Weapon)itemOnGround).PlayerImage;
                            player.CurrentWeapon = itemOnGround;
                        }
                    }
                    else
                        InventoryHUD.OnItemAdd(itemOnGround, slot, MainInventory.GetQuantityInSlot(slot));

                    // disable game object
                    itemOnGround.gameObject.SetActive(false);
                    ObjectsPickedUp.Add(itemOnGround.gameObject);
                    ItemFocused = false;
                    InventoryHUD.RemovePickUpItemMsg();
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
        Item itemToUse = MainInventory.GetCurrentItem();
        int mainInvSlot = MainInventory.GetCurrentSlotNum();
        if (itemToUse != null)
        {
            if (itemToUse.GetItemType() == Item.Type.WEAPON)
            {
                // equip weapon
                int weaponSlot = WeaponInventory.AddItem(itemToUse);
                if (weaponSlot != -1)
                {
                    MainInventory.RemoveItem(mainInvSlot, true);
                    InventoryHUD.OnWeaponStow(itemToUse, weaponSlot, mainInvSlot);
                    if (player.CurrentWeapon == null)
                    {
                        GetComponent<SpriteRenderer>().sprite = ((Weapon)itemOnGround).PlayerImage;
                        player.CurrentWeapon = itemOnGround;
                    }
                    eventAggregator.Publish(new OnWeaponEquipEvent(playerNumber, (Weapon)itemToUse));
                    Debug.Log("InventoryHandler: Weapon stowed Successfully");
                }
                else
                {
                    // equip failed due to full weapon inventory, do nothing
                    Debug.Log("InventoryHandler: Weapon inventory full, could not store weapon");
                }
            }
            else
            {
                if (MainInventory.UseItem(GetComponent<Player>(), mainInvSlot))
                {
                    InventoryHUD.OnItemRemove(MainInventory.GetQuantityInSlot(mainInvSlot));
                    ObjectsPickedUp.Remove(itemToUse.gameObject);
                    if (MainInventory.GetItemInSlot(mainInvSlot) == null)
                        Destroy(itemToUse.gameObject);
                }

            }

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
                GetComponent<SpriteRenderer>().sprite = PlayerOriginalImage;
                player.CurrentWeapon = null;
                InventoryHUD.OnWeaponUnEquip();
            }
            else // equip weapon
            {
                InventoryHUD.OnWeaponEquip(weaponSlot);
                GetComponent<SpriteRenderer>().sprite = weaponToUse.PlayerImage;
                player.CurrentWeapon = weaponToUse;
            }

        }
        else // no weapon in slot, so use player orginal image
        {
            InterruptWeaponReload();
            GetComponent<SpriteRenderer>().sprite = PlayerOriginalImage;
            player.CurrentWeapon = null;
            InventoryHUD.OnWeaponUnEquip();
        }
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
                int slotNum = MainInventory.GetCurrentSlotNum();
                Item itemToRemove = MainInventory.GetCurrentItem();
                if (itemToRemove != null)
                {
                    int itemIndex = 0;
                    for (int i = 0; i < ObjectsPickedUp.Count; i++)
                    {
                        if (ObjectsPickedUp[i].GetComponent<Item>().GetType() == itemToRemove.GetType())
                        {
                            ObjectsPickedUp[i].transform.position = transform.position;
                            ObjectsPickedUp[i].SetActive(true);
                            MainInventory.RemoveItem(slotNum, false);
                            itemIndex = i;
                            break;
                        }
                    }
                    InventoryHUD.OnItemRemove(MainInventory.GetQuantityInSlot(slotNum));
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
                        GetComponent<SpriteRenderer>().sprite = PlayerOriginalImage;
                        player.CurrentWeapon = null;
                        InventoryHUD.OnWeaponUnEquip();
                    }
                    WeaponInventory.RemoveItem(WeaponSlotIndex, true);
                    int itemIndex = 0;
                    for (int i = 0; i < ObjectsPickedUp.Count; i++)
                    {
                        if (ObjectsPickedUp[i].GetComponent<Item>().name.Equals(weaponToRemove.name))
                        {
                            ObjectsPickedUp[i].transform.position = transform.position;
                            ObjectsPickedUp[i].SetActive(true);
                            WeaponInventory.RemoveItem(WeaponSlotIndex, false);
                            itemIndex = i;
                            break;
                        }
                    }
                    InventoryHUD.OnItemRemove(WeaponInventory.GetQuantityInSlot(WeaponSlotIndex));
                    ObjectsPickedUp.RemoveAt(itemIndex);
                }

            }
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
                    MainInventory.GetNextItem();
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
                    MainInventory.GetFirstItem();
                }
                else
                {
                    WeaponSlotIndex = 0;
                }
            }

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
                    MainInventory.GetPrevItem();
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
                    MainInventory.GetLastItem();
                }
                else
                {
                    WeaponSlotIndex = WeaponInventory.MAX_SLOT_SIZE - 1;
                }
            }
        }
    }
}
