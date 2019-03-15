using UnityEngine;

public class RocketLauncher : RangedWeapon
{

    public RocketLauncher() {
        projDamage = 50f;
        projSpeed = 300f;
        weight = 4;
        ReloadTime = 5;
        AmmoClip = new AmmoClip(100, 50);
    }

}
