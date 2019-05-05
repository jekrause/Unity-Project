using UnityEngine;

public class RocketLauncher : RangedWeapon
{

    public readonly float DEFAULT_ATTACK_RATE = 2f;
    public readonly float DEFAULT_RELOAD_TIME = 5;
    public readonly float DEFAULT_PROJ_DAMAGE = 50f;
    private const int DEFAULT_MAX_CLIP_SIZE = 50;
    private const int AMMO_USED_PER_BULLET = 25;
    private int MaxAmmoClip = DEFAULT_MAX_CLIP_SIZE; // mutated value as level increases

    public RocketLauncher() {
        projDamage = DEFAULT_PROJ_DAMAGE;
        projSpeed = 300f;
        weight = 4;
        ReloadTime = DEFAULT_RELOAD_TIME;
        attackRate = DEFAULT_ATTACK_RATE;
        AmmoClip = new AmmoClip(DEFAULT_MAX_CLIP_SIZE, AMMO_USED_PER_BULLET);
        // Will need to be updated to a better sound
        ReloadSound = "Universal_Reload";
        ReloadFinishSound = "Universal_Reload_Finished";
    }

    public override void UpdateWeaponStats(Stats playerStats)
    {
        ReloadTime = DEFAULT_RELOAD_TIME - (DEFAULT_RELOAD_TIME * playerStats.ReloadMultiplier);
        attackRate = DEFAULT_ATTACK_RATE - (DEFAULT_ATTACK_RATE * playerStats.AttackRateMultiplier);
        projDamage = DEFAULT_PROJ_DAMAGE + playerStats.DamageMultiplier;
        if(playerStats.Level % 2 == 0)
        {
            int ammoInClip = AmmoClip.CurrentAmmoRaw;
            MaxAmmoClip = DEFAULT_MAX_CLIP_SIZE + (AMMO_USED_PER_BULLET * playerStats.Level);
            AmmoClip = new AmmoClip(ammoInClip, MaxAmmoClip, AMMO_USED_PER_BULLET);
        }

        AmmoClip = new AmmoClip(MaxAmmoClip, AMMO_USED_PER_BULLET);
    }

    public override void ResetWeaponStats()
    {
        ReloadTime = DEFAULT_RELOAD_TIME;
        attackRate = DEFAULT_ATTACK_RATE;
        projDamage = DEFAULT_PROJ_DAMAGE;
        float ammoInClip = AmmoClip.CurrentAmmoRaw;
        if (AmmoClip.CurrentAmmoRaw > DEFAULT_MAX_CLIP_SIZE)
        {
            // normalize the ammo clip back to default current ammo in clip
            ammoInClip = (1.0f * AmmoClip.CurrentAmmoRaw) / (1.0f * AmmoClip.MAX_CLIP_SIZE);
            ammoInClip = ammoInClip * DEFAULT_MAX_CLIP_SIZE;
        }
        AmmoClip = new AmmoClip((int)ammoInClip, DEFAULT_MAX_CLIP_SIZE, AMMO_USED_PER_BULLET);
    }

}
