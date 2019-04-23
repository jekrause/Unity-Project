using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LootBagHandler : MonoBehaviour, ISubscriber<OnLootBagChangedEvent>
{
    // HUD componenets
    public GameObject LootBagHUD;        // The parent game object, will be used to simply toggle On and Off when interacting (Loot bag)
    private GameObject[] LootBagSlots;   // The slots to update item images Loot bag slots)
    private GameObject LootBagSlotPanel; // The game object panel that contains all the Inventory Slot game objects


    public GameObject MyInvBagHUD;       // The parent game object, will be used to simply toggle On and Off when interacting (My inventory)
    private GameObject[] MyInvSlots;     // The slots to update item images (My Inventory slots)
    private GameObject MainInvPanel;
    private GameObject WeaponInvPanel;
    
    private int SlotSize;                // Tow many slots we have in the bag

    // Data
    private LootBag CurrentLootBag;      // The current loot bag the player is looting
    private Player player; 
    private string AcionMessage = "-No Action-";
    private readonly string ItemTypeMessage = "Bag";
    private const float BUTTON_HELD_DOWN_TIME = 0.5f;
    private float ButtonHeldTimer = 0f;
    private bool BusyLooting = false;
    private bool IteratingMyInv = false;  // True - we now should select a slot in our inventory to swap, False - select a slot in loot bag first

    // index pointing to what slots the player want to swap slot with
    private int LootBagSlotIndex = 0; // [0 - 9]
    private int MyInvSlotIndex = 0;   // [0 - 9] where index 6,7,8 are weapon inventory

    //keep track what section we are iterating in for player's inventory
    private bool IteratingMainInv = true; // True - Main Inventory, False - Weapon Inventory

    // Const to reduce misspelling
    public const string ITEM_SELECTED = "ItemSelected";
    public const string ITEM_SLOT = "ItemSlot";
    public const string BACKGROUND = "Background";
    public const string QUANTITY = "Quantity";
    public const string ITEM = "Item";

    private bool axisInProgress = false;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        EventAggregator.GetInstance().Register<OnLootBagChangedEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnLootBagChangedEvent>(this);
    }

    public void InitializeLootBagHandler(GameObject HUD)
    {
        player = GetComponent<Player>();
        LootBagHUD = HUD.transform.Find("LootBag").gameObject;
        MyInvBagHUD = HUD.transform.Find("InventoryBag").gameObject;
        LootBagSlotPanel = LootBagHUD.transform.Find("LootBagSlots").gameObject;
        SlotSize = LootBagSlotPanel.transform.childCount;
        LootBagSlots = new GameObject[SlotSize];
        MyInvSlots = new GameObject[SlotSize];
        MainInvPanel = MyInvBagHUD.transform.Find("MainInventoryPanel").gameObject;
        WeaponInvPanel = MyInvBagHUD.transform.Find("WeaponInventoryPanel").gameObject;

        // Both loot bag and Main Inv slots
        for (int i = 0; i < SlotSize; i++)
        {
            LootBagSlots[i] = LootBagSlotPanel.transform.GetChild(i).gameObject;
            if (i <= 5)
                MyInvSlots[i] = MainInvPanel.transform.GetChild(i).gameObject;
        }

        // Weapon slots
        for (int i = 0; i < 3; i++)
            MyInvSlots[6 + i] = WeaponInvPanel.transform.GetChild(i).gameObject;


    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentLootBag != null && player.InteractionState == InteractionState.OPEN_STATE)
        {
            if (Input.GetButton(player.myControllerInput.DownButton))
            {
                
                if(ButtonHeldTimer < BUTTON_HELD_DOWN_TIME)
                {
                    ButtonHeldTimer += Time.deltaTime;
                    player.InteractionPanel.ShowLoadBar(ButtonHeldTimer, BUTTON_HELD_DOWN_TIME);
                }
              
            }

            if (Input.GetButtonUp(player.myControllerInput.DownButton))
            {
                if(ButtonHeldTimer >= BUTTON_HELD_DOWN_TIME)
                {
                    player.InteractionPanel.RemoveLoadBar();
                    player.InteractionPanel.RemoveInteractivePanel();
                    OpenBag();
                }
                ButtonHeldTimer = 0;
            }
        }
        if(player.InteractionState == InteractionState.LOOTING_STATE)
        {
            if(CurrentLootBag != null)
            {
                if (Input.GetButtonDown(player.myControllerInput.RightButton))
                {
                    if (!BusyLooting)
                        CloseBag();
                }

                ListenForUserInput();

            }
            else
            {
                CloseBag(); // special case if CurrentBag is somehow null and we are still in looting state
            }
        }
    }

    public void OnRayCastLootBagEnter(Collider2D collider)
    {
        if(collider != null && CurrentLootBag == null)
        {
            CurrentLootBag = collider.GetComponent<LootBag>();
            AcionMessage = "Hold " + player.DownPlatformButton + " : Loot Bag";
            player.InteractionPanel.ShowInteractionPanel(ItemTypeMessage, AcionMessage);
        }
    }

    public void OnRayCastLootBagExit()
    {
        CloseBag();
    }

    

    private void CloseBag()
    {
        CurrentLootBag = null;
        player.InteractionPanel.RemoveInteractivePanel();
        player.InteractionState = InteractionState.OPEN_STATE;
        LootBagHUD.SetActive(false);
        MyInvBagHUD.SetActive(false);
        IteratingMyInv = false;
        IteratingMainInv = true;
        LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SELECTED).gameObject.SetActive(false);
        MyInvSlots[MyInvSlotIndex].transform.Find(ITEM_SELECTED).gameObject.SetActive(false);
        MyInvSlotIndex = 0;
        LootBagSlotIndex = 0;
    }

    public void OnEventHandler(OnLootBagChangedEvent eventData)
    {
        if (CurrentLootBag != null && CurrentLootBag.ID == eventData.LootBag.ID) // only update HUD if more than 2 player are accessing the same loot
        {
            CurrentLootBag = eventData.LootBag;
            UpdateLootBagHUD();
        }
        
    }

    private void OpenBag()
    {
        if(CurrentLootBag != null)
        {
            // open bag
            player.InteractionState = InteractionState.LOOTING_STATE;
            IteratingMyInv = false;
            UpdateLootBagHUD();
            OpenMyInvBagHUD();
            LootBagHUD.SetActive(true);
            MyInvBagHUD.SetActive(true);
            LootBagSlots[0].transform.Find(ITEM_SELECTED).gameObject.SetActive(true);
        }
    }


    private void UpdateLootBagHUD()
    {
        if (CurrentLootBag == null) return;

        if(CurrentLootBag.Inventory.GetNumOfSlotUsed() == 0)
        {
            for (int i = 0; i < SlotSize; i++)
                LootBagSlots[i].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(false);
        }
        else
        {
            Item item;
            for (int i = 0; i < SlotSize; i++)
            {
                item = CurrentLootBag.Inventory.GetItemInSlot(i);
                if (item != null)
                {
                    LootBagSlots[i].transform.Find(ITEM_SLOT).Find(ITEM).GetComponent<Image>().sprite = item.Image;
                    if (!(item is Weapon || item is QuestItem))
                    {
                        LootBagSlots[i].transform.Find(ITEM_SLOT).Find(BACKGROUND).Find(QUANTITY).GetComponent<Text>().text = CurrentLootBag.Inventory.GetQuantityInSlot(i) + "";
                    }
                    LootBagSlots[i].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(true);
                }
                else
                {
                    LootBagSlots[i].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(false);
                }
            }
        }

        
        
    }

    private void OpenMyInvBagHUD()
    {
        if (player.MainInventory.GetNumOfSlotUsed() == 0 && player.WeaponInventory.GetNumOfSlotUsed() == 0) return;
        
        Item item;
        int index = 0;
        int size = 9;
        for (; index < 6; index++)
        {
            item = player.MainInventory.GetItemInSlot(index);
            if (item != null)
            {
                MyInvSlots[index].transform.Find(ITEM_SLOT).Find(ITEM).GetComponent<Image>().sprite = item.Image;
                if (item is FirstAid)
                {
                    MyInvSlots[index].transform.Find(ITEM_SLOT).Find(BACKGROUND).Find(QUANTITY).GetComponent<Text>().text = player.MainInventory.GetQuantityInSlot(index) + "";
                    MyInvSlots[index].transform.Find(ITEM_SLOT).Find(BACKGROUND).gameObject.SetActive(true);
                }
                else
                {
                    MyInvSlots[index].transform.Find(ITEM_SLOT).Find(BACKGROUND).gameObject.SetActive(false);
                }
                MyInvSlots[index].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(true);
            }
            else
            {
                MyInvSlots[index].transform.Find(ITEM_SLOT).Find(BACKGROUND).gameObject.SetActive(false);
                MyInvSlots[index].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(false);
            }

        }

        for (int weaponIndex = 0; index < size; index++)
        {
            item = player.WeaponInventory.GetItemInSlot(weaponIndex++);
            if (item != null)
            {
                MyInvSlots[index].transform.Find(ITEM_SLOT).Find(ITEM).GetComponent<Image>().sprite = item.Image;
                MyInvSlots[index].transform.Find(ITEM_SLOT).Find(BACKGROUND).gameObject.SetActive(false);
                MyInvSlots[index].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(true);
            }
            else
            {
                MyInvSlots[index].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(false);
            }

        }
     }

    private void SwapSlots()
    {

        int normalizedIndex = NormalizedIndex(); // normalize the myInvIndex since Main Inv is [0-5] and Weapon Inv is [0-2]

        // Loot bag slot
        Item itemToCopy = CurrentLootBag.Inventory.GetItemInSlot(LootBagSlotIndex);
        int quantityToCopy = CurrentLootBag.Inventory.GetQuantityInSlot(LootBagSlotIndex);
        if (itemToCopy == null) return;

        BusyLooting = true;

        // My slot
        Item myItem;
        int myItemQuantity;

        if (IteratingMainInv)
        {
            myItem = player.MainInventory.GetItemInSlot(normalizedIndex);
            myItemQuantity = player.MainInventory.GetQuantityInSlot(normalizedIndex);

            // check if same item to stack the items accordingly
            if(myItem != null && myItem.GetMaxStackSize() > 1 && myItem.GetType() == itemToCopy.GetType())
            {
                int leftOver = myItem.GetMaxStackSize() - (myItemQuantity + quantityToCopy);
                if(leftOver <= -1)
                {
                    myItemQuantity = (myItemQuantity + quantityToCopy) - myItem.GetMaxStackSize();
                    quantityToCopy = myItem.GetMaxStackSize();
                }
                else
                {
                    quantityToCopy = myItemQuantity + quantityToCopy;
                }
            }
            // update my slot
            player.MainInventory.ModifySlot(normalizedIndex, itemToCopy, quantityToCopy);
            Debug.Log(quantityToCopy);
            EventAggregator.GetInstance().Publish(new OnMainInvChangedEvent(player.playerNumber, normalizedIndex, itemToCopy, quantityToCopy));
        }
        else
        {
            myItem = player.WeaponInventory.GetItemInSlot(normalizedIndex);
            myItemQuantity = player.WeaponInventory.GetQuantityInSlot(normalizedIndex);

            // update my slot
            if (itemToCopy is Weapon)
            {
                bool equipped = false;
                if (player.CanEquipWeapon((Weapon)itemToCopy))
                {
                    player.UpdatePlayerCurrentWeapon((Weapon)itemToCopy);
                    equipped = true;
                }
                else
                {
                    Debug.Log("Cannot add it to weapon slot");
                    BusyLooting = false;
                    return;

                }
                player.WeaponInventory.ModifySlot(normalizedIndex, itemToCopy, quantityToCopy);
                Debug.Log(player.WeaponInventory.GetNumOfSlotUsed());
                EventAggregator.GetInstance().Publish(new OnWeaponInvChangedEvent(player.playerNumber, normalizedIndex, itemToCopy, quantityToCopy, equipped));
            }
            else
            {
                Debug.Log("Cannot add it to weapon slot");
                BusyLooting = false;
                return;
            }
           
        }
         
        CurrentLootBag.Inventory.ModifySlot(LootBagSlotIndex, myItem, myItemQuantity);  // update Loot bag slot
        EventAggregator.GetInstance().Publish(new OnLootBagChangedEvent(CurrentLootBag)); // announce that the loot bag have been altered

        //Update HUD bags
        if(myItem == null)
        {
            LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(false);
        }
        else
        {   
            if(myItem.GetItemType() == Item.Type.HEALING_ITEM)
            {
                LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SLOT).Find(BACKGROUND).Find(QUANTITY).GetComponent<Text>().text = myItemQuantity + "";
                LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SLOT).Find(BACKGROUND).gameObject.SetActive(true);
            }
            else
            {
                LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SLOT).Find(BACKGROUND).gameObject.SetActive(false);
            }
            LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SLOT).Find(ITEM).GetComponent<Image>().sprite = myItem.Image;
            LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(true);
        }

        if(itemToCopy.GetItemType() == Item.Type.HEALING_ITEM)
        {
            MyInvSlots[MyInvSlotIndex].transform.Find(ITEM_SLOT).Find(BACKGROUND).Find(QUANTITY).GetComponent<Text>().text = quantityToCopy + "";
            MyInvSlots[MyInvSlotIndex].transform.Find(ITEM_SLOT).Find(BACKGROUND).gameObject.SetActive(true);
        }
        else
        {
            MyInvSlots[MyInvSlotIndex].transform.Find(ITEM_SLOT).Find(BACKGROUND).gameObject.SetActive(false);
        }

        MyInvSlots[MyInvSlotIndex].transform.Find(ITEM_SLOT).Find(ITEM).GetComponent<Image>().sprite = itemToCopy.Image;
        MyInvSlots[MyInvSlotIndex].transform.Find(ITEM_SLOT).Find(ITEM).gameObject.SetActive(true);
        
        IteratingMyInv = false;
        BusyLooting = false;
        SwapSelectionHUD();
    }

    private void SwapSelectionHUD()
    {

        if (IteratingMyInv)
        {
            LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SELECTED).gameObject.SetActive(false);
            MyInvSlots[MyInvSlotIndex].transform.Find(ITEM_SELECTED).gameObject.SetActive(true);
        }
        else
        {
            MyInvSlots[MyInvSlotIndex].transform.Find(ITEM_SELECTED).gameObject.SetActive(false);
            LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SELECTED).gameObject.SetActive(true);
        }
    }


    private int NormalizedIndex()
    {
        int normalizedIndex = MyInvSlotIndex;
        if (!IteratingMainInv)
        {
            normalizedIndex = normalizedIndex == 6 ? 0
                            : normalizedIndex == 7 ? 1
                            : 2;
            
        }

        return normalizedIndex;
    }

    private void UpdateSelectionInHUD(int previous)
    {
        if (IteratingMyInv)
        {
            MyInvSlots[previous].transform.Find(ITEM_SELECTED).gameObject.SetActive(false);
            MyInvSlots[MyInvSlotIndex].transform.Find(ITEM_SELECTED).gameObject.SetActive(true);
        }
        else
        {
            LootBagSlots[previous].transform.Find(ITEM_SELECTED).gameObject.SetActive(false);
            LootBagSlots[LootBagSlotIndex].transform.Find(ITEM_SELECTED).gameObject.SetActive(true);
        }
    }

    private void OnIterateDown(int index)
    {
        if (index <= -1 || index >= 9) throw new System.ArgumentException("Index is out of range before iterating");
        int previous = index;
        switch (index)
        {                               // Iterating the slot below, we increment index accordingly shown below
            case 0: index = 3; break;                         // |0 1 2 |
            case 1: index = 4; break;                         // |3 4 5 |
            case 2: index = 5; break;                         // |6 7 8 |
            case 3: index = 6; break;
            case 4: index = 7; break;
            case 5: index = 8; break;
        }
        if (IteratingMyInv)
        {
            if (index >= 6)
                IteratingMainInv = false;
            
            MyInvSlotIndex = index;
        }
        else
        {
            LootBagSlotIndex = index;
        }
        UpdateSelectionInHUD(previous);
    }

    private void OnIterateUp(int index)
    {
        if (index <= -1 || index >= 9) throw new System.ArgumentException("Index is out of range before iterating");
        int previous = index;
        switch (index)
        {                               // Iterating the slot above, we increment index accordingly shown below
            case 3: index = 0; break;                   // |0 1 2 |
            case 4: index = 1; break;                   // |3 4 5 |
            case 5: index = 2; break;                   // |6 7 8 |
            case 6: index = 3; break;
            case 7: index = 4; break;
            case 8: index = 5; break;
        }

        if (IteratingMyInv)
        {
            MyInvSlotIndex = index;
            if(index <= 5)
               IteratingMyInv = true;
        }
        else
        {
            LootBagSlotIndex = index;
        }
        
        UpdateSelectionInHUD(previous);
    }

    private void OnIterateRight(int index)
    {
        if (index <= -1 || index >= 9)
            throw new System.ArgumentException("Index is out of range before iterating");

        int previous = index;

        if(index != 2 && index != 5 && index != 8)
            index++;

        if (IteratingMyInv)
        {
           MyInvSlotIndex = index;
        }
        else
        {
            LootBagSlotIndex = index;
        }

        UpdateSelectionInHUD(previous);
        
    }

    private void OnIterateLeft(int index)
    {
        if (index <= -1 || index >= 9)
            throw new System.ArgumentException("Index is out of range before iterating");

        int previous = index;

        if (index != 0 && index != 3 && index != 6)
            index--;

        if (IteratingMyInv)
        {
            MyInvSlotIndex = index;
        }
        else
        {
            LootBagSlotIndex = index;
        }

        UpdateSelectionInHUD(previous);
    }


    private void ListenForUserInput()
    {

        if (Input.GetButtonDown(player.myControllerInput.DownButton))
        {
            if (IteratingMyInv)
            {
                SwapSlots();
            }
            else
            {
                if (CurrentLootBag != null && CurrentLootBag.Inventory.GetItemInSlot(LootBagSlotIndex) != null)
                {
                    IteratingMyInv = !IteratingMyInv;
                    SwapSelectionHUD();
                }
            }
            
        }

        if (Settings.OS == "Windows" || (player.myControllerInput.inputType == InputType.KEYBOARD))
        {
            if (!axisInProgress)
            {
                axisInProgress = true;

                if (Input.GetAxis(player.myControllerInput.DPadX_Windows) > 0) // D-Pad right, iterate throught item inventory
                {
                    if (IteratingMyInv)
                    {
                        OnIterateRight(MyInvSlotIndex);
                    }
                    else
                    {
                        OnIterateRight(LootBagSlotIndex);
                    }
                }
                else if (Input.GetAxis(player.myControllerInput.DPadX_Windows) < 0) // D-Pad left, iterate throught item inventory
                {
                    if (IteratingMyInv)
                    {
                        OnIterateLeft(MyInvSlotIndex);
                    }
                    else
                    {
                        OnIterateLeft(LootBagSlotIndex);
                    }
                }
                else if (Input.GetAxis(player.myControllerInput.DPadY_Windows) < 0) // D-Pad down, iterate throught item inventory
                {
                    if (IteratingMyInv)
                    {
                        OnIterateDown(MyInvSlotIndex);
                    }
                    else
                    {
                        OnIterateDown(LootBagSlotIndex);
                    }
                }
                else if (Input.GetAxis(player.myControllerInput.DPadY_Windows) > 0) // D-Pad up, iterate throught item inventory
                {
                    if (IteratingMyInv)
                    {
                        OnIterateUp(MyInvSlotIndex);
                    }
                    else
                    {
                        OnIterateUp(LootBagSlotIndex);
                    }
                }
            }
           

            if(Input.GetAxis(player.myControllerInput.DPadX_Windows) == 0 && Input.GetAxis(player.myControllerInput.DPadY_Windows) == 0)
            {
                axisInProgress = false; // user let go of button, so action is no longer in progress
            }
        }
        else
        {
            if (Input.GetButtonDown(player.myControllerInput.DPadRight_Mac))
            {
                if (IteratingMyInv)
                {
                   OnIterateRight(MyInvSlotIndex);
                }
                else
                {
                    OnIterateLeft(LootBagSlotIndex);
                }
            }
            else if (Input.GetButtonDown(player.myControllerInput.DPadLeft_Mac))
            {
                if (IteratingMyInv)
                {
                    OnIterateLeft(MyInvSlotIndex);
                }
                else
                {
                    OnIterateLeft(LootBagSlotIndex);
                }
            }
            else if (Input.GetButtonDown(player.myControllerInput.DPadUp_Mac))
            {
                if (IteratingMyInv)
                {
                    OnIterateUp(MyInvSlotIndex);
                }
                else
                {
                    OnIterateUp(LootBagSlotIndex);
                }
            }
            else if (Input.GetButtonDown(player.myControllerInput.DPadDown_Mac))
            {
                if (IteratingMyInv)
                {
                     OnIterateDown(MyInvSlotIndex);
                }
                else
                {
                   OnIterateDown(LootBagSlotIndex);
                }
            }

        }
    }

}
