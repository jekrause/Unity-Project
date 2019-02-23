public abstract class Potion : Item, IConsumableItem
{
    private const int MAX_STACK_SIZE = 6;
    protected float HP_Points;

    public override int GetMaxStackSize() { return MAX_STACK_SIZE; }

    public override void InteractWithItem()
    {
        throw new System.NotImplementedException();
    }

    public void Apply(Player player)
    {
        player.Healed(HP_Points);
    }
}

public class LowPotion : Potion
{
    private const float LOW_POTION = 30f;
    public LowPotion() { HP_Points = LOW_POTION; }
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