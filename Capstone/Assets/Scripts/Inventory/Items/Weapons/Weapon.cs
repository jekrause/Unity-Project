using UnityEngine;

public abstract class Weapon : Item
{
    protected const int MAX_STACK_SIZE = 1;
    protected float weight;
    protected bool IsEquipped;

    public Sprite PlayerImage;
    private Sprite PlayerOldImage;
    public Bullet bullet;
    public Transform ShootPosition;

    public override Type GetItemType() { return Type.WEAPON; }

    public override int GetMaxStackSize() { return MAX_STACK_SIZE; }

    public override bool UseItem(Player player)
    {
        return Equip(player);
    }

    public bool Equip(Player player)
    {
        bool ret = false; // in the case that it is already equipped, return false
        if (!IsEquipped)
        {
            print(this.name + " has been equipped.");

            PlayerOldImage = player.gameObject.GetComponent<SpriteRenderer>().sprite;
            player.gameObject.GetComponent<SpriteRenderer>().sprite = PlayerImage;
            // equip weapon here
            ret = IsEquipped = true;
        }

        return ret;
    }

    public bool UnEquip(Player player)
    {
        if (!IsEquipped) return false;

        IsEquipped = !IsEquipped;
        print(this.name + " has been unequipped.");
        player.gameObject.GetComponent<SpriteRenderer>().sprite = PlayerOldImage;
        PlayerOldImage = null; // remove it in case player drop it and someone else pick it up

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

