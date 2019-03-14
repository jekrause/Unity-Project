using UnityEngine;

public class Inventory
{

    private class Slot
    {

        private Item CurrentItem;
        public int CurrentQuantity;

        public Slot(Item item, int quantity)
        {
            CurrentItem = item ?? throw new System.ArgumentNullException("Attempting to add null item");

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
            --CurrentQuantity;
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
    [SerializeField] private int slotUsed = 0;
    private int ItemCursor = 0; // may be used for quick selection 
    [SerializeField] private readonly Slot[] Slots;

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

    public bool AllSlotTaken() { return slotUsed >= MAX_SLOT_SIZE; }

    public int AddItem(Item item)
    {
        int ret = -1;
        if (item == null) throw new System.ArgumentNullException("Attempting to add null item");

        for (int i = 0; i < MAX_SLOT_SIZE; i++)
        {
            // if same item, ex: potion so we will be adding more potion to the current quantity stack
            if (Slots[i].HasItem())
            {
                if (!Slots[i].IsFull() && Slots[i].GetItem().GetType() == item.GetType())
                {
                    Slots[i].IncrementQuantity();
                    Debug.Log("Inventory AddItem(): Slot no: " + (i+1) + ", Item stack Incremented");
                    return i;
                }
                
            }
        }

        int freeSlotIndex = FindEmptySlot();
        if (freeSlotIndex != -1)
        {
            Slots[freeSlotIndex] = new Slot(item, 1);
            IncrementSlotUsed();
            ret = freeSlotIndex;
        }

        if(ret != -1)
        {
            Debug.Log("Inventory AddItem(): Item added to Slot: " + (ret + 1));
        }
        else
        {
            Debug.Log("Inventory AddItem(): Inventory Full, size: " + slotUsed);
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

    public bool RemoveItem(int index, bool removeFullStack)
    {
        bool ret = false;
        if (Slots[index].HasItem())
        {
            if (removeFullStack)
            {
                ret = true;
                Slots[index].Clear();
                DecrementSlotUsed();
                Debug.Log("Inventory RemoveItem(): Slot no: " + (index+1) + ", Full Stack removed");
            }
            else
            {
                ret = true;
                Slots[index].DecrementQuantity();
                if (!Slots[index].HasItem()) DecrementSlotUsed();
                Debug.Log("Inventory RemoveItem(): Slot no: " + (index+1) + ", 1 removed from stack");
            }
            
        }
        return ret;
    }

    public Item GetCurrentItem() { return Slots[ItemCursor].GetItem(); }

    public Item GetFirstItem() { return Slots[ItemCursor = 0].GetItem();  }

    public Item GetLastItem() { return Slots[ItemCursor = Slots.Length - 1].GetItem(); }

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

    public Item GetItemInSlot(int slot) {

        if (slot < 0 || slot >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Inventory consist of " + MAX_SLOT_SIZE + " slots only");

        return Slots[slot].GetItem();
    }

   

    public int GetNumOfSlotUsed() { return slotUsed; }

    public bool UseItem(Player player, int slot) {
        if (!Slots[slot].HasItem()) return false; // no item in slot

        bool ret = false; // used to determine if the item was used sucessfully
        Slot s = Slots[slot];
        ret = s.GetItem().UseItem(player);
        if (ret == true)
        {
            Debug.Log("Inventory UseItem(): Used item successfully");
            s.DecrementQuantity();
            if (!s.HasItem()) DecrementSlotUsed();
        }
        else
        {
            Debug.Log("Inventory UseItem(): Use item " + s.GetItem().name +  " unsuccessful");
        }
        return ret;
    }

    public int GetQuantityInSlot(int slot) {
        if (Slots[slot].HasItem())
        {
            return Slots[slot].CurrentQuantity;
        }
        else
        {
            return -1;
        }
    }

    public int GetCurrentSlotNum() { return ItemCursor; }

    public bool SlotIsFull(int slot) { return Slots[slot] != null && Slots[slot].IsFull(); }

    // Should call the methods for safe mutation
    private int IncrementSlotUsed() { return slotUsed = ++slotUsed >= MAX_SLOT_SIZE ? MAX_SLOT_SIZE - 1 : slotUsed; }

    private int DecrementSlotUsed() { return slotUsed = --slotUsed < 0 ? 0 : slotUsed; }


}
