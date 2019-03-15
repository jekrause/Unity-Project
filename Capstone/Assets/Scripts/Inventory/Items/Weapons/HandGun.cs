using UnityEngine;

public class HandGun : RangedWeapon
{
    public HandGun() {
        projDamage = 5f;
        projSpeed = 500;
        weight = 0;
        ReloadTime = 2;
        AmmoClip = new AmmoClip(12, 1);
    }

}
