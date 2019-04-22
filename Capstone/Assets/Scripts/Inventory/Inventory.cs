using UnityEngine;

/// <summary>
/// A Simple class that will keep track of how many slots has been used. Modification and most logic in a slot
/// are modified in the Slot class and not in Inventory class. Most of the methods in this class will have wrapper
/// method of what the Slot class will be performing.
/// </summary>
public class Inventory
{
    public readonly int MAX_SLOT_SIZE;
    [SerializeField] private int slotUsed = 0;
    [SerializeField] private readonly Slot[] Slots;

    public Inventory(int maxSlotSize)
    {
        if (maxSlotSize <= 0) throw new System.ArgumentException("Inventory Constuctor(): Can't have max size be lower than 1");
        MAX_SLOT_SIZE = maxSlotSize;
        Slots = new Slot[MAX_SLOT_SIZE];
        ClearInventory();
    }

    public Inventory(Item item, int maxSlotSize) : this(maxSlotSize)
    {
        Slots[0] = new Slot(item, 1);
    }

    public void ClearInventory()
    {
        for (int i = 0; i < MAX_SLOT_SIZE; i++) Slots[i] = new Slot();
        slotUsed = 0;
    }

    public bool AllSlotTaken() { return slotUsed >= MAX_SLOT_SIZE; }

    public bool UseItem(Player player, int slotNum)
    {
         bool useSuccessfully = Slots[slotNum].UseItem(player);

        if (useSuccessfully)
        {
            if (!Slots[slotNum].HasItem())
            {
                slotUsed--;
            }
        }

        return useSuccessfully;
    }

    public int AddItem(Item item)
    {
        if (slotUsed >= MAX_SLOT_SIZE) return -1;
        if (item == null) throw new System.ArgumentNullException("Inventory AddItem(): Attempting to add null item");

        int ret = -1;

        for (int i = 0; i < MAX_SLOT_SIZE; i++)
        {
            bool addSuccessfully = Slots[i].AddItem(item);
            if (addSuccessfully)
            {
                if(Slots[i].CurrentQuantity == 1) // added a new item
                {
                    slotUsed++;
                }
                return i;
            }
        }

        return ret;
    }

    /// <summary>
    /// Attempt to remove 1 Item in the inventory slot and returns true if at least 1 item in the slot got removed successfully regardless if
    /// there exist more items in the slot.
    /// </summary>
    /// <returns></returns>
    public bool RemoveItemInSlot(int index)
    {
        if (slotUsed <= 0) return false;
        if (index <= -1 || index >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Index out of bound, must be from 0-" + (MAX_SLOT_SIZE - 1));
        bool removeSuccessfully = Slots[index].RemoveItem();
        if (removeSuccessfully)
        {
            if (!Slots[index].HasItem())
            {
                slotUsed--;
            }
        }
        return removeSuccessfully;
    }

    /// <summary>
    /// Attempt to remove the full stack of items in the inventory slot and returns true if all items in the slot got removed successfully.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool RemoveAllItemInSlot(int index)
    {
        if (slotUsed <= 0) return false;
        if (index <= -1 || index >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Index out of bound, must be from 0-" + (MAX_SLOT_SIZE - 1));
        bool removeSuccessfully = Slots[index].RemoveAllItem();
        if (removeSuccessfully)
        {
            slotUsed--;
        }
        return removeSuccessfully;
    }

    public Item GetItemInSlot(int slot)
    {
        if (slotUsed <= 0) return null;
        if (slot <= -1 || slot >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Inventory GetItemInSlot(): Index out of bound, must be from 0-" + (MAX_SLOT_SIZE - 1));

        return Slots[slot].GetItem();
    }

    public int GetNumOfSlotUsed() { return slotUsed; }

    public int GetQuantityInSlot(int slotNum)
    {
        if (slotNum <= -1 || slotNum >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Inventory GetQuantityInSlot(): Index out of bound, must be from 0-" + (MAX_SLOT_SIZE - 1));
        return Slots[slotNum].CurrentQuantity;
    }

    /// <summary>
    /// Modifies the slot in this inventory by updating the item in this slot to the given item and quantity. 
    /// </summary>
    /// <param name="slotNum"></param>
    /// <param name="item"></param>
    /// <param name="quantity"></param>
    public void ModifySlot(int slotNum, Item item, int quantity)
    {
        if(item == null)
        {
            Slots[slotNum] = new Slot();
            slotUsed--;
        }
        else
        {
            Slots[slotNum] = new Slot(item, quantity);
        }
    }

    // Should call the methods for safe mutation
    private int IncrementSlotUsed() { return slotUsed = ++slotUsed >= MAX_SLOT_SIZE ? MAX_SLOT_SIZE - 1 : slotUsed; }

    private int DecrementSlotUsed() { return slotUsed = --slotUsed < 0 ? 0 : slotUsed; }
    

}
