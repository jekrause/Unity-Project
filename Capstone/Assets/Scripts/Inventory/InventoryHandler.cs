using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InventoryHandler : MonoBehaviour
{
    private int playerNumber;
    private MyControllerInput myControllerInput;
    private bool actionInProgress; // to elminate multiple button press
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
    private Item EquippedWeapon;

    // Use this for initialization
    void Start()
    {
        // get player info
        playerNumber = GetComponent<Player>().playerNumber;
        myControllerInput = GetComponent<Player>().myControllerInput;
        MainInventory = GetComponent<Player>().MainInventory;
        WeaponInventory = GetComponent<Player>().WeaponInventory;
        PlayerOriginalImage = GetComponent<SpriteRenderer>().sprite;
        if(MainInventory == null || WeaponInventory == null)
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

            if (Input.GetButtonDown(myControllerInput.UpButton) || Input.GetButton(myControllerInput.UpButton))
            {
                if (actionInProgress == false)
                {
                    InventoryHUDFocused = !InventoryHUDFocused; // toggle inventory selection
                    InventoryHUD.InventoryToggled(InventoryHUDFocused, myControllerInput.inputType);
                    if (InventoryHUDFocused) InventoryHUD.RemovePickUpItemMsg();
                    Debug.Log("Inventory toggled");
                    actionInProgress = true;
                    StartCoroutine(DelayReadingInput());
                }

            }

            if (InventoryHUDFocused) // user has inventory toggled on
            {

                if(Settings.OS == "Windows"){
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
               
                if (Input.GetButtonDown(myControllerInput.RightButton) || Input.GetButton(myControllerInput.RightButton)) // item remove
                {
                    RemoveItemFromInv();
                }
                else if (Input.GetButtonDown(myControllerInput.DownButton) || Input.GetButton(myControllerInput.DownButton)) // use item
                {
                    if (!actionInProgress)
                    {
                        if (IteratingMainInv)
                        {
                            UseItemFromMainInv();
                        }
                        else
                        {
                            UseItemFromWeaponInv();
                        }
                        actionInProgress = true;
                        StartCoroutine(DelayReadingInput());
                    }
                    
                }
            }
            else
            {
                if (Input.GetButtonDown(myControllerInput.DownButton) || Input.GetButton(myControllerInput.DownButton)) // add item
                {
                    AddItem();
                }
                else if (Input.GetButtonDown(myControllerInput.RBumper) || Input.GetButton(myControllerInput.RBumper)) // equip weapon equipment from the next right slot
                {
                    if (!actionInProgress)
                    {
                        EquipNextWeapon();
                        actionInProgress = true;
                        StartCoroutine(DelayReadingInput());
                    }
                    
                }
                else if (Input.GetButtonDown(myControllerInput.LBumper) || Input.GetButton(myControllerInput.LBumper)) // equip weapon equipment from the next left slot
                {
                    if (!actionInProgress)
                    {
                        EquipPrevWeapon();
                        actionInProgress = true;
                        StartCoroutine(DelayReadingInput());
                    }
                }
            }
        }
        else
        {
            //check if user has bind controller
            myControllerInput = GetComponent<Player>().myControllerInput;
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
                InventoryHUD.ShowPickUpItemMsg(myControllerInput.inputType);
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

    private IEnumerator DelayReadingInput()
    {

        yield return new WaitForSeconds(.25f);
        actionInProgress = false;

    }

    private void EquipNextWeapon()
    {
        WeaponReloadInterrupt();
        WeaponInventory.GetNextItem();
        UseItemFromWeaponInv();
    }

    private void EquipPrevWeapon()
    {
        WeaponReloadInterrupt();
        WeaponInventory.GetPrevItem();
        UseItemFromWeaponInv();
    }

    private void AddItem()
    {
        if (ItemFocused)
        {
            if (itemOnGround != null)
            {
                int slot = MainInventory.AddItem(itemOnGround);
                if (slot != -1)
                {
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
                    if (MainInventory.GetCurrentItem() == null)
                        Destroy(itemToUse.gameObject);
                }

            }

        }
    }

    /// <summary>
    /// Attempt to equip a weapon from weapon inventory slot. If the slot is empty, the player will be replaced
    /// by the original sprite (no weapon). Otherwise, it will equip or unequip the weapon currently selected.
    /// </summary>
    private void UseItemFromWeaponInv()
    {
        Weapon weaponToUse = (Weapon) WeaponInventory.GetCurrentItem();
        int weaponInvSlot = WeaponInventory.GetCurrentSlotNum();

        if(weaponToUse != null)
        {
            if(GetComponent<Player>().CurrentWeapon != null && GetComponent<Player>().CurrentWeapon == weaponToUse) // unequip their weapon
            {
                WeaponReloadInterrupt();
                GetComponent<SpriteRenderer>().sprite = PlayerOriginalImage;
                GetComponent<Player>().CurrentWeapon = null;
                InventoryHUD.OnWeaponUnEquip();
            }
            else // equip weapon
            {
                InventoryHUD.OnWeaponEquip(weaponInvSlot);
                GetComponent<SpriteRenderer>().sprite = weaponToUse.PlayerImage;
                GetComponent<Player>().CurrentWeapon = weaponToUse;
            }
            
        }
        else // no weapon in slot, so use player orginal image
        {
            WeaponReloadInterrupt();
            GetComponent<SpriteRenderer>().sprite = PlayerOriginalImage;
            GetComponent<Player>().CurrentWeapon = null;
            InventoryHUD.OnWeaponUnEquip();
        }
    }

    /// <summary>
    /// Called when player attempts to drop, swap or unequip their current weapon in the case
    /// that the gun is currently reloading
    /// </summary>
    private void WeaponReloadInterrupt()
    {
        Weapon playerWeapon = (Weapon)GetComponent<Player>().CurrentWeapon;
        if (playerWeapon != null && playerWeapon is RangedWeapon)
        {
            ((RangedWeapon)playerWeapon).ReloadingInterrupted();
        }
    }

    /// <summary>
    /// Remove the selected item from the main inventory or weapon inventory and update the inventory HUD accordingly
    /// </summary>
    private void RemoveItemFromInv()
    {
        if (actionInProgress == false)
        {
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
            else
            {
                int slotNum = WeaponInventory.GetCurrentSlotNum();
                Weapon weaponToRemove = (Weapon) WeaponInventory.GetCurrentItem();
                if (weaponToRemove != null)
                {
                    if(GetComponent<Player>().CurrentWeapon != null && GetComponent<Player>().CurrentWeapon == weaponToRemove)
                    {
                        WeaponReloadInterrupt();
                        GetComponent<SpriteRenderer>().sprite = PlayerOriginalImage;
                        GetComponent<Player>().CurrentWeapon = null;
                        InventoryHUD.OnWeaponUnEquip();
                    }
                    WeaponInventory.RemoveItem(slotNum, true);
                    int itemIndex = 0;
                    for (int i = 0; i < ObjectsPickedUp.Count; i++)
                    {
                        if (ObjectsPickedUp[i].GetComponent<Item>().name.Equals(weaponToRemove.name))
                        {
                            ObjectsPickedUp[i].transform.position = transform.position;
                            ObjectsPickedUp[i].SetActive(true);
                            WeaponInventory.RemoveItem(slotNum, false);
                            actionInProgress = true;
                            StartCoroutine(DelayReadingInput());
                            itemIndex = i;
                            break;
                        }
                    }
                    InventoryHUD.OnItemRemove(WeaponInventory.GetQuantityInSlot(slotNum));
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
        if (actionInProgress == false)
        {
            bool oldIteringMainInv = IteratingMainInv;
            IteratingMainInv = InventoryHUD.IterateRight(); // return true if iterating main inv
            actionInProgress = true;
            StartCoroutine(DelayReadingInput());

            if(oldIteringMainInv == IteratingMainInv)
            {
                if (IteratingMainInv)
                {
                    MainInventory.GetNextItem();
                }
                else
                {
                    WeaponInventory.GetNextItem();
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
                    WeaponInventory.GetFirstItem();
                }
            }

        }
    }

    /// <summary>
    /// Iterate to the previous item. It will iterate through the main and weapon inventory
    /// </summary>
    private void IterateLeftList()
    {
        if (actionInProgress == false)
        {
            bool oldIteringMainInv = IteratingMainInv;
            IteratingMainInv = InventoryHUD.IterateLeft(); // return true if iterating main inv
            actionInProgress = true;
            StartCoroutine(DelayReadingInput());

            if (oldIteringMainInv == IteratingMainInv)
            {
                if (IteratingMainInv)
                {
                    MainInventory.GetPrevItem();
                }
                else
                {
                    WeaponInventory.GetPrevItem();
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
                    WeaponInventory.GetLastItem();
                }
            }
        }
    }
}
