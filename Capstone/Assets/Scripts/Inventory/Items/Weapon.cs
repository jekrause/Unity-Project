
//TODO
public abstract class Weapon : Item, IEquipableItem
{
    public override int GetMaxStackSize()
    {
        throw new System.NotImplementedException();
    }

    public override void InteractWithItem()
    {
        throw new System.NotImplementedException();
    }

    public void Equip(Player player)
    {
       
    }

}
