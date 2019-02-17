// TODO - may need multiple tiers of Health Potion
public class HealthPotion : Item, IConsumableItem
{
    private const int MAX_STACK_SIZE = 6;
    private const int WEAK_POTION = 20;

    public override int GetMaxStackSize() { return MAX_STACK_SIZE; } // may be more?

    public override void InteractWithItem()
    {
        throw new System.NotImplementedException();
    }

    public void Apply(Player player)
    {
        //player.fFP += WEAK_POTION
    }
}
