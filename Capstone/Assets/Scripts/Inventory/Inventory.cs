using UnityEngine;
public class Inventory
{

    private class Slot
    {

        private Item CurrentItem;
        public int CurrentQuantity;

        public Slot(Item item, int quantity)
        {
            CurrentItem = item ?? throw new System.ArgumentNullException("Cannot have null item");

            if(quantity > CurrentItem.GetMaxStackSize())
                throw new System.ArgumentException("Quantity given is bigger than item's max stack size");
          
            CurrentQuantity = quantity;
            
        }

        public Slot() { Clear(); }

        public bool HasItem() { return CurrentItem != null; }

        public bool IsFull() { return CurrentItem != null && CurrentQuantity >= CurrentItem.GetMaxStackSize(); }

        public void IncrementQuantity()
        {
            CurrentQuantity = ++CurrentQuantity > CurrentItem.GetMaxStackSize() ? CurrentItem.GetMaxStackSize() : CurrentQuantity;
        }

        public void DecrementQuantity() {
            CurrentQuantity = --CurrentQuantity;
            if(CurrentQuantity <= 0)
            {
                Clear();
            }
        }

        public void Clear()
        {
            CurrentItem = null;
            CurrentQuantity = 0;
        }
        public Item GetItem() { return CurrentItem; }

    }


    public readonly int MAX_SLOT_SIZE;
    private int slotUsed = 0;
    private int ItemCursor = 0; // may be used for quick selection 
    private readonly Slot[] Slots;

    public Inventory(int maxSlotSize) {
        if (maxSlotSize <= 0) throw new System.ArgumentException("Can't have max size be lower than 1");
        MAX_SLOT_SIZE = maxSlotSize;
        Slots = new Slot[MAX_SLOT_SIZE];
        ClearInventory();
    }

    public Inventory(Item item, int maxSlotSize) : this(maxSlotSize)
    {
        Slots[0] = new Slot(item, 1);
    }

    public void ClearInventory() {
        for (int i = 0; i < MAX_SLOT_SIZE; i++) Slots[i] = new Slot();
    }

    public bool AddItem(Item item)
    {
        bool ret = false;

        for (int i = 0; i < MAX_SLOT_SIZE; i++)
        {
            // if same item, ex: potion so we will be adding more potion to the current quantity stack
            if (Slots[i].HasItem() && Slots[i].GetItem().GetType() == item.GetType())
            {
                if (!Slots[i].IsFull())
                {
                    Slots[i].IncrementQuantity();
                    item.PickUpItem();
                    return true;
                }
                
            }
        }

        int freeSlotIndex = FindEmptySlot();
        if (freeSlotIndex != -1)
        {
            Slots[freeSlotIndex] = new Slot(item, 1);
            IncrementSlotUsed();
            item.PickUpItem();
            ret = true;
        }

        return ret;
    }

    private int FindEmptySlot()
    {
        for(int i = 0; i < MAX_SLOT_SIZE; i++)
        {
            if (!Slots[i].HasItem()) return i;
        }
        return -1; // inventory is full
    }

    public bool RemoveItem(int index)
    {
        bool ret = false;
        if (Slots[index].HasItem())
        {
            ret = true;
            Slots[index].Clear();
            DecrementSlotUsed();
        }
        return ret;
    }

    public Item GetCurrentItem() { return Slots[ItemCursor].GetItem(); }

    public Item GetItemInSlot(int slot) {

        if (slot < 0 || slot >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Inventory consist of " + MAX_SLOT_SIZE + " slots only");

        return Slots[slot].GetItem();
    }

    //used for cycling through the quick inventory list that would be displayed at the bottom of the screen
    public Item GetNextItem()
    {
        ItemCursor = ++ItemCursor >= slotUsed ? 0 : ItemCursor;
        return Slots[ItemCursor].GetItem();
    }

    public Item GetPrevItem()
    {
        ItemCursor = --ItemCursor < 0 ? slotUsed - 1 : ItemCursor;
        return Slots[ItemCursor].GetItem();
    }

    public int GetNumOfSlotUsed() { return slotUsed; }

    public bool UseItem(Player player, int slot) {
        if (!Slots[slot].HasItem()) return false; // no item in slot

        bool ret = false; // used to determine if the item was used sucessfully
        Slot s = Slots[slot];
        ret = s.GetItem().UseItem(player);
        if (ret == true)
        {
            s.DecrementQuantity();
            if (!s.HasItem()) DecrementSlotUsed();
        }
        return ret;
    }

    // Should call the methods for safe mutation
    private int IncrementSlotUsed() { return slotUsed = ++slotUsed >= MAX_SLOT_SIZE ? MAX_SLOT_SIZE - 1 : slotUsed; }

    private int DecrementSlotUsed() { return slotUsed = --slotUsed < 0 ? 0 : slotUsed; }


}
