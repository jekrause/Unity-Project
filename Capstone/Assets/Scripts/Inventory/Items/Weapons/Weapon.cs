using UnityEngine;

public abstract class Weapon : Item
{
    protected const int MAX_STACK_SIZE = 1;
    protected float weight;
    protected bool IsEquipped;

    public Sprite PlayerImage;
    public Bullet bullet;
    public Transform ShootPosition;

    public override Type GetItemType() { return Type.WEAPON; }

    public override int GetMaxStackSize() { return MAX_STACK_SIZE; }

    public override bool UseItem(Player player)
    {
        if(this is IRangedWeapon)
        {
            ((IRangedWeapon)this).Fire();
        }
        else
        {
            ((IMeleeWeapon)this).Strike();
        }
        return true;
    }

}

interface IRangedWeapon
{

    void Fire();

    void Reload();
}

interface IMeleeWeapon
{

    void Strike();

}

