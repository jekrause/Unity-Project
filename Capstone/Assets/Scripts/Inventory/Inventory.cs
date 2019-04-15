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

            if (quantity > CurrentItem.GetMaxStackSize())
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

        public void DecrementQuantity()
        {
            --CurrentQuantity;
            if (CurrentQuantity <= 0)
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
    }

    public bool AllSlotTaken() { return slotUsed >= MAX_SLOT_SIZE; }

    public int AddItem(Item item)
    {
        if (slotUsed >= MAX_SLOT_SIZE) return -1;
        if (item == null) throw new System.ArgumentNullException("Inventory AddItem(): Attempting to add null item");

        int ret = -1;

        for (int i = 0; i < MAX_SLOT_SIZE; i++)
        {
            // if same item, ex: potion so we will be adding more potion to the current quantity stack
            if (Slots[i].HasItem())
            {
                if (!Slots[i].IsFull() && Slots[i].GetItem().GetType() == item.GetType())
                {
                    Slots[i].IncrementQuantity();
                    Settings.PrintDebugMsg("Inventory AddItem(): Slot no: " + (i + 1) + ", Item stack Incremented");
                    if (item is RangedWeapon)
                    {
                        AudioManager.Play(((RangedWeapon)item).ReloadFinishSound);
                    }
                    else
                    {
                        AudioManager.Play("PickUpItem");
                    }
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
            if (item is RangedWeapon)
            {
                AudioManager.Play(((RangedWeapon)item).ReloadFinishSound);
            }
            else
            {
                if (item is QuestItem)
                {
                    EventAggregator.GetInstance().Publish<OnQuestItemPickUpEvent>(new OnQuestItemPickUpEvent((QuestItem)item));
                }
                AudioManager.Play("PickUpItem");
            }

        }

        return ret;
    }

    private int FindEmptySlot()
    {
        if (slotUsed == 0) return 0;

        for (int i = 0; i < MAX_SLOT_SIZE; i++)
        {
            if (!Slots[i].HasItem()) return i;
        }
        return -1; // inventory is full
    }

    /// <summary>
    /// Attempt to remove 1 Item in the inventory slot and returns true if at least 1 item in the slot got removed successfully regardless if
    /// there exist more items in the slot.
    /// </summary>
    /// <returns></returns>
    public bool RemoveItemInSlot(int index)
    {
        if (index <= -1 || index >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Index out of bound, must be from 0-" + (MAX_SLOT_SIZE - 1));

        bool ret = false;
        if (Slots[index].HasItem())
        {
            ret = true;
            if (Slots[index].GetItem() is QuestItem)
            {
                EventAggregator.GetInstance().Publish<OnQuestItemDroppedEvent>(new OnQuestItemDroppedEvent((QuestItem)Slots[index].GetItem()));
            }
            Slots[index].DecrementQuantity();
            if (!Slots[index].HasItem()) DecrementSlotUsed();
            Settings.PrintDebugMsg("Inventory RemoveItem(): Slot no: " + (index + 1) + ", 1 removed from stack");
        }

        return ret;
    }

    /// <summary>
    /// Attempt to remove the full stack of items in the inventory slot and returns true if all items in the slot got removed successfully.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool RemoveAllItemInSlot(int index)
    {
        if (index <= -1 || index >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Index out of bound, must be from 0-" + (MAX_SLOT_SIZE - 1));

        bool ret = false;
        if (Slots[index].HasItem())
        {
            ret = true;
            if (Slots[index].GetItem() is QuestItem)
            {
                EventAggregator.GetInstance().Publish<OnQuestItemDroppedEvent>(new OnQuestItemDroppedEvent((QuestItem)Slots[index].GetItem()));
            }
            Slots[index].Clear();
            DecrementSlotUsed();
            Settings.PrintDebugMsg("Inventory RemoveItem(): Slot no: " + (index + 1) + ", Full Stack removed");
        }
        return ret;
    }

    public Item GetItemInSlot(int slot)
    {

        if (slot <= -1 || slot >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Inventory GetItemInSlot(): Index out of bound, must be from 0-" + (MAX_SLOT_SIZE - 1));

        return Slots[slot].GetItem();
    }

    public int GetNumOfSlotUsed() { return slotUsed; }

    public bool UseItem(Player player, int slot)
    {

        if (player == null) throw new System.ArgumentNullException("Inventory UseItem(): Player is null");
        if (slot <= -1 || slot >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Inventory UseItem(): Index out of bound, must be from 0-" + (MAX_SLOT_SIZE - 1));

        if (!Slots[slot].HasItem()) return false; // no item in slot

        bool ret = false; // used to determine if the item was used sucessfully
        Slot s = Slots[slot];
        ret = s.GetItem().UseItem(player);
        if (ret == true)
        {
            Settings.PrintDebugMsg("Inventory UseItem(): Used item successfully");
            s.DecrementQuantity();
            if (!s.HasItem()) DecrementSlotUsed();
        }
        else
        {
            Settings.PrintDebugMsg("Inventory UseItem(): Use item " + s.GetItem().name + " unsuccessful");
        }
        return ret;
    }

    public int GetQuantityInSlot(int slot)
    {

        if (slot <= -1 || slot >= MAX_SLOT_SIZE) throw new System.ArgumentOutOfRangeException("Index out of bound, must be from 0-" + (MAX_SLOT_SIZE - 1));

        if (Slots[slot].HasItem())
        {
            return Slots[slot].CurrentQuantity;
        }
        else
        {
            return -1;
        }
    }

    public bool SlotIsFull(int slot) { return Slots[slot] != null && Slots[slot].IsFull(); }

    // Should call the methods for safe mutation
    private int IncrementSlotUsed() { return slotUsed = ++slotUsed >= MAX_SLOT_SIZE ? MAX_SLOT_SIZE - 1 : slotUsed; }

    private int DecrementSlotUsed() { return slotUsed = --slotUsed < 0 ? 0 : slotUsed; }


}
