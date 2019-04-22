
public class Slot 
{
        private Item CurrentItem;
        public int CurrentQuantity { get; private set; } = 0;

        public Slot(Item item, int quantity)
        {
            if (item == null)
            {
                if (quantity <= -1 || quantity >= 1) Settings.PrintDebugMsg("Careful there, item is null, quantity given should be 0");
            }
            else
            {
                if (quantity > CurrentItem.GetMaxStackSize())
                    throw new System.ArgumentException("Quantity given is bigger than item's max stack size");
                else if(quantity <= 0)
                    throw new System.ArgumentException("Quantity needs to be at least 1 or greater");

                CurrentQuantity = quantity;
            }
        }

        public Slot() { }

        /// <summary>
        /// Return true if this slot consist a item
        /// </summary>
        /// <returns></returns>
        public bool HasItem() { return CurrentItem != null; }

        /// <summary>
        /// Return true if this slot has an item and the current quantity is equal to the item max stack size
        /// </summary>
        /// <returns></returns>
        public bool IsFull() { return CurrentItem != null && CurrentQuantity >= CurrentItem.GetMaxStackSize(); }

        private void IncrementQuantity()
        {
            CurrentQuantity = ++CurrentQuantity > CurrentItem.GetMaxStackSize() ? CurrentItem.GetMaxStackSize() : CurrentQuantity;
        }

        private void DecrementQuantity()
        {
            --CurrentQuantity;
            if (CurrentQuantity <= 0)
            {
                Clear();
            }
        }

    /// <summary>
    /// Attempt to use the item in this slot on the given player. Returns true if used successful 
    /// and update quantity in the slot accordingly.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public bool UseItem(Player player)
    {
        if (player == null) throw new System.ArgumentNullException("Slot UseItem(): Player is null");
       
        if (CurrentItem == null) return false; // no item in slot

        bool ret = false; // used to determine if the item was used sucessfully
        ret = CurrentItem.UseItem(player);
        if (ret == true)
        {
            Settings.PrintDebugMsg("Slot UseItem(): Used item successfully");
            if(CurrentItem.GetMaxStackSize() > 1)
                DecrementQuantity();
        }
        else
        {
            Settings.PrintDebugMsg("Slot UseItem(): Use item " + CurrentItem.name + " unsuccessful");
        }
        return ret;
    }

    /// <summary>
    /// Attempt to add a item to this slot. Returns true if added successfully (stacked item included)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        bool ret = false;

        if (item == null) return ret;

        if (CurrentItem == null)
        {
            CurrentItem = item;
            IncrementQuantity();
            ret = true;
        }
        else
        {
            if(CurrentItem.GetType() == item.GetType() && !IsFull())
            {
                IncrementQuantity();
                ret = true;
            }
        }

        if(ret == true)
        {
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

    /// <summary>
    /// Attempt to remove 1 Item in the slot and returns true if at least 1 item in the slot got removed successfully regardless if
    /// there exist more items in the slot.
    /// </summary>
    /// <returns></returns>
    public bool RemoveItem()
    {
        bool ret = false;
        if (CurrentItem != null)
        {
            ret = true;
            if (CurrentItem is QuestItem)
            {
                EventAggregator.GetInstance().Publish<OnQuestItemDroppedEvent>(new OnQuestItemDroppedEvent((QuestItem)CurrentItem));
            }
            DecrementQuantity();

            if (CurrentQuantity <= 0)
                Clear();
        }

        return ret;
    }

    /// <summary>
    /// Attempt to remove the full stack of items in the slot and returns true if all items in the slot got removed successfully.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool RemoveAllItem()
    {
        bool ret = false;
        if (CurrentItem != null)
        {
            ret = true;
            if (CurrentItem is QuestItem)
            {
                EventAggregator.GetInstance().Publish<OnQuestItemDroppedEvent>(new OnQuestItemDroppedEvent((QuestItem)CurrentItem));
            }
            Clear();
        }
        return ret;
    }

    /// <summary>
    /// Remove everything in this slot
    /// </summary>
    public void Clear()
    {
        CurrentItem = null;
        CurrentQuantity = 0;
    }

    /// <summary>
    /// Return the item in this slot, may be empty (null)
    /// </summary>
    /// <returns></returns>
    public Item GetItem() => CurrentItem;

}