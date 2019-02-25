using UnityEngine;
using UnityEngine.UI;
public abstract class Item : MonoBehaviour{

    public Sprite Image { get; }

    /// <summary>
    /// <ret>Returns if this item is a quest item.</ret>
    /// </summary>
    public bool IsQuestItem { get; private set; }

    /// <summary>
    /// <ret>Returns the max stack size of this item.</ret>
    /// </summary>
    public abstract int GetMaxStackSize();

    /// <summary>
    /// Use the item on the player. Result will vary depending on what type the item is.
    /// <para>Player: the player that is using the item.</para>
    /// </summary>
    public abstract bool UseItem(Player player);

    public void PickUpItem()
    {
        gameObject.SetActive(false);
    }

}
