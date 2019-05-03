using System.Collections;
using UnityEngine;

public class AssaultRifle : RangedWeapon
{
    public readonly float DEFAULT_ATTACK_RATE = 0.5f;
    public readonly float DEFAULT_RELOAD_TIME = 3f;
    public readonly float DEFAULT_PROJ_DAMAGE = 10f;

    public AssaultRifle() {
        ReloadTime = DEFAULT_ATTACK_RATE;
        AmmoClip = new AmmoClip(25, 1);
        weight = 2;
        projSpeed = 650f;
        attackRate = DEFAULT_ATTACK_RATE;
        projDamage = DEFAULT_PROJ_DAMAGE;
        ReloadSound = "Universal_Reload";
        ReloadFinishSound = "Universal_Reload_Finished";
    }

    public override void UpdateWeaponStats(Stats playerStats)
    {
        ReloadTime = DEFAULT_RELOAD_TIME - (DEFAULT_RELOAD_TIME * playerStats.ReloadMultiplier);
        attackRate = DEFAULT_ATTACK_RATE - (DEFAULT_ATTACK_RATE * playerStats.AttackRateMultiplier);
        projDamage = DEFAULT_PROJ_DAMAGE + playerStats.DamageMultiplier;
    }
}
