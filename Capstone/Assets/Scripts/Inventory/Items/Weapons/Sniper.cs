using UnityEngine;


public class Sniper : RangedWeapon
{
    public Sniper() {
        projDamage = 40f;
        projSpeed = 1000;
        weight = 2;
        AmmoClip = new AmmoClip(200, 25);
        ReloadTime = 4;
        attackRate = 5;
    }
}
