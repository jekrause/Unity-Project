using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item
{
    public override Type GetItemType()
    {
        return Type.QUEST_ITEM;
    }

    public override int GetMaxStackSize()
    {
        return 1;
    }

    public override bool UseItem(Player player)
    {
        // Can't Use
        return false;
    }
}
