using UnityEngine;

public abstract class FirstAid : Item, IHealingItem
{
    private const int MAX_STACK_SIZE = 6;
    [SerializeField] protected float HP_Points;

    public override int GetMaxStackSize() { return MAX_STACK_SIZE; }

    public bool Apply(Player player){ return player.Healed(HP_Points); }

    public override bool UseItem(Player player){ return Apply(player); }

    public Sprite GetImage() { return Image; }

}

interface IHealingItem
{

    bool Apply(Player player);

}

