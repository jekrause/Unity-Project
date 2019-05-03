using UnityEngine;


public class Sniper : RangedWeapon
{

    public readonly float DEFAULT_ATTACK_RATE = 5f;
    public readonly float DEFAULT_RELOAD_TIME = 4f;
    public readonly float DEFAULT_PROJ_DAMAGE = 40f;

    public Sniper() {
        projDamage = DEFAULT_PROJ_DAMAGE;
        projSpeed = 1000;
        weight = 2;
        AmmoClip = new AmmoClip(200, 25);
        ReloadTime = DEFAULT_RELOAD_TIME;
        attackRate = DEFAULT_ATTACK_RATE;
        ReloadSound = "Sniper_Reload";
        ReloadFinishSound = "Sniper_Reload_Finished";
    }

    public override void UpdateWeaponStats(Stats playerStats)
    {
        ReloadTime = DEFAULT_RELOAD_TIME - (DEFAULT_RELOAD_TIME * playerStats.ReloadMultiplier);
        attackRate = DEFAULT_ATTACK_RATE - (DEFAULT_ATTACK_RATE * playerStats.AttackRateMultiplier);
        projDamage = DEFAULT_PROJ_DAMAGE + playerStats.DamageMultiplier;
    }

}
