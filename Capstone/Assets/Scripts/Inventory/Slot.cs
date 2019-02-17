
public class Slot {

    private Item CurrentItem;
    public int CurrentQuantity;

    public Slot(Item item, int quantity)
    {
        CurrentItem = item;
        CurrentQuantity = quantity;
    }

    public Slot() { Clear(); }

    public bool IsAvailable() { return CurrentItem == null; }

    public bool CanAddMore() { return CurrentItem != null && CurrentQuantity < CurrentItem.GetMaxStackSize();  }

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
