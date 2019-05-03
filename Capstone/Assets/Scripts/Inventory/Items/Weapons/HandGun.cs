using UnityEngine;

public class HandGun : RangedWeapon
{

    public readonly float DEFAULT_ATTACK_RATE = 0.7f;
    public readonly float DEFAULT_RELOAD_TIME = 2;
    public readonly float DEFAULT_PROJ_DAMAGE = 5f;

    public HandGun() {
        projDamage = DEFAULT_PROJ_DAMAGE;
        projSpeed = 500;
        weight = 0;
        ReloadTime = DEFAULT_RELOAD_TIME;
        attackRate = DEFAULT_ATTACK_RATE;
        AmmoClip = new AmmoClip(12, 1);
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
