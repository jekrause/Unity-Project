using UnityEngine;

public abstract class Potion : Item, IConsumableItem
{
    public Sprite _image;
    private const int MAX_STACK_SIZE = 6;
    protected float HP_Points;

    public override int GetMaxStackSize() { return MAX_STACK_SIZE; }

    public bool Consume(Player player){ return player.Healed(HP_Points); }

    public override bool UseItem(Player player){ return Consume(player); }
}


public class SuperPotion : Potion
{
    private const float SUPER_POTION = 50f;
    public SuperPotion() { HP_Points = SUPER_POTION; }
}

public class HighPotion : Potion
{
    private const float HIGH_POTION = 70f;
    public HighPotion() { HP_Points = HIGH_POTION; }
}

public class MaxPotion : Potion
{
    private const float MAX_POTION = 100f;
    public MaxPotion() { HP_Points = MAX_POTION; }
}