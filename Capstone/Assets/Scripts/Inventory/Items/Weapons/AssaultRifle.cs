﻿using System.Collections;
using UnityEngine;

public class AssaultRifle : RangedWeapon
{
    public AssaultRifle() {
        ReloadTime = 3f;
        AmmoClip = new AmmoClip(25, 1);
        weight = 2;
        projDamage = 10f;
        projSpeed = 650f;
    }

    
}
