using System.Collections;
using UnityEngine;

public class AssaultRifle : RangedWeapon
{
    public AssaultRifle() {
        ReloadTime = 3f;
        AmmoClip = new AmmoClip(25, 1);
        weight = 2;
        projDamage = 10f;
        projSpeed = 650f;
        attackRate = 0.5f;
        ReloadSound = "Universal_Reload";
        ReloadFinishSound = "Universal_Reload_Finished";
    }

    
}
