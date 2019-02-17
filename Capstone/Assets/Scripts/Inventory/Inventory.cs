

public class Inventory{

    private const int MAX_SLOT_SIZE = 6;
    private int slotUsed = 0;
    private int ItemCursor = -1; // may be used for quick selection 
    private Slot[] Slots;

    public Inventory() {
        Slots = new Slot[MAX_SLOT_SIZE];
        ClearInventory();
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
            if (Slots[i].GetItem().GetType() == item.GetType())
            {
                if (Slots[i].CanAddMore())
                {
                    Slots[i].UpdateQuantity();
                    ret = true;
                }
                else
                {
                    int freeSlotIndex = FindEmptySlot();
                    if(freeSlotIndex != -1)
                    {
                        Slots[freeSlotIndex] = new Slot(item, 1);
                        IncrementSlotUsed();
                        ret = true;
                    }
                }
            }
            else
            {
                if (Slots[i].IsAvailable())
                {
                    Slots[i] = new Slot(item, 1);
                    IncrementSlotUsed();
                    ret = true;
                    break;
                }
  
            }
            
        }
        return ret;
    }

    private int FindEmptySlot()
    {
        for(int i = 0; i < MAX_SLOT_SIZE; i++)
        {
            if (Slots[i].IsAvailable()) return i;
        }
        return -1; // inventory is full
    }

    public bool RemoveItem(int index)
    {
        bool ret = false;
        if (Slots[index].IsAvailable())
        {
            ret = true;
            Slots[index].Clear();
            DecrementSlotUsed();
        }

        return ret;
    }

    public Item GetCurrentItem()
    {
        ItemCursor = ItemCursor == -1 ? 0 : ItemCursor;
        return Slots[ItemCursor].GetItem();
    }

    public Item GetItemInSlot(int slot) {

        if (slot < 0 || slot >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Inventory consist of 6 slots only");

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

    public int GetSlotUsed() { return slotUsed; }

    // Should call the methods for safe mutation
    private int IncrementSlotUsed() { return slotUsed = ++slotUsed >= MAX_SLOT_SIZE ? MAX_SLOT_SIZE - 1 : slotUsed; }

    private int DecrementSlotUsed() { return slotUsed = --slotUsed < 0 ? 0 : slotUsed; }

}
