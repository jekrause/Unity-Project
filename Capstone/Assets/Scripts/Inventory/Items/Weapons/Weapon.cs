using UnityEngine;

public abstract class Weapon : Item
{
    protected const int MAX_STACK_SIZE = 1;
    protected float weight;
    protected bool IsEquipped;

    public Bullet bullet;
    public Transform ShootPosition;
    

    public override int GetMaxStackSize() { return MAX_STACK_SIZE; }

    public override bool UseItem(Player player)
    {
        bool ret = false; // in the case that it is already equipped, return false
        if (!IsEquipped)
        {
            // equip weapon here
            ret = IsEquipped = true;
        }

        return ret;
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

