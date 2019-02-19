public class Inventory{

    private class Slot
    {

        private Item CurrentItem;
        public int CurrentQuantity;

        public Slot(Item item, int quantity)
        {
            CurrentItem = item;
            CurrentQuantity = quantity;
        }

        public Slot() { Clear(); }

        public bool HasItem() { return CurrentItem != null; }

        public bool IsFull() { return CurrentItem != null && CurrentQuantity >= CurrentItem.GetMaxStackSize(); }

        public void UpdateQuantity()
        {
            CurrentQuantity = ++CurrentQuantity > CurrentItem.GetMaxStackSize() ? CurrentItem.GetMaxStackSize() : CurrentQuantity;
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
    private Slot[] Slots;

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
                    Slots[i].UpdateQuantity();
                    return true;
                }
                
            }
        }

        int freeSlotIndex = FindEmptySlot();
        if (freeSlotIndex != -1)
        {
            Slots[freeSlotIndex] = new Slot(item, 1);
            IncrementSlotUsed();
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
        ItemCursor = ++ItemCursor >= MAX_SLOT_SIZE ? 0 : ItemCursor;
        return Slots[ItemCursor].GetItem();
    }

    public Item GetPrevItem()
    {
        ItemCursor = --ItemCursor < 0 ? MAX_SLOT_SIZE - 1 : ItemCursor;
        return Slots[ItemCursor].GetItem();
    }

    public int GetNumOfSlotUsed() { return slotUsed; }

    // Should call the methods for safe mutation
    private int IncrementSlotUsed() { return slotUsed = ++slotUsed >= MAX_SLOT_SIZE ? MAX_SLOT_SIZE - 1 : slotUsed; }

    private int DecrementSlotUsed() { return slotUsed = --slotUsed < 0 ? 0 : slotUsed; }


}
