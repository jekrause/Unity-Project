using UnityEngine;
using UnityEngine.UI;


public abstract class Item : MonoBehaviour
{

    [SerializeField] public Sprite Image;

    public enum Type { HEALING_ITEM, WEAPON, ARMOR, QUEST_ITEM };

    /// <summary>
    /// <ret>Returns the max stack size of this item.</ret>
    /// </summary>
    public abstract int GetMaxStackSize();

    /// <summary>
    /// Use the item on the player. Result will vary depending on what type the item is.
    /// <para>Player: the player that is using the item.</para>
    /// </summary>
    public abstract bool UseItem(Player player);

    public abstract Type GetItemType();


}
