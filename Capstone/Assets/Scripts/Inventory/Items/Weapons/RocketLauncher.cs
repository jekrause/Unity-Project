using UnityEngine;

public class RocketLauncher : RangedWeapon
{

    public readonly float DEFAULT_ATTACK_RATE = 2f;
    public readonly float DEFAULT_RELOAD_TIME = 5;
    public readonly float DEFAULT_PROJ_DAMAGE = 50f;

    public RocketLauncher() {
        projDamage = DEFAULT_PROJ_DAMAGE;
        projSpeed = 300f;
        weight = 4;
        ReloadTime = DEFAULT_RELOAD_TIME;
        attackRate = DEFAULT_ATTACK_RATE;
        AmmoClip = new AmmoClip(50, 25);
        // Will need to be updated to a better sound
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
