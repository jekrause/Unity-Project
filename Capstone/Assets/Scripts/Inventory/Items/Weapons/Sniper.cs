using UnityEngine;


public class Sniper : RangedWeapon
{

    public readonly float DEFAULT_ATTACK_RATE = 5f;
    public readonly float DEFAULT_RELOAD_TIME = 4f;
    public readonly float DEFAULT_PROJ_DAMAGE = 40f;
    private const int DEFAULT_MAX_CLIP_SIZE = 200;
    private const int AMMO_USED_PER_BULLET = 25;
    private int MaxAmmoClip = DEFAULT_MAX_CLIP_SIZE; // mutated value as level increases

    public Sniper() {
        projDamage = DEFAULT_PROJ_DAMAGE;
        projSpeed = 1000;
        weight = 2;
        AmmoClip = new AmmoClip(DEFAULT_MAX_CLIP_SIZE, AMMO_USED_PER_BULLET);
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
        if (playerStats.Level > 1)
        {
            MaxAmmoClip = DEFAULT_MAX_CLIP_SIZE + (AMMO_USED_PER_BULLET * playerStats.Level);
            int ammoInClip = (int)(AmmoClip.GetPercentageInClip() * MaxAmmoClip);
            AmmoClip = new AmmoClip(ammoInClip, MaxAmmoClip, AMMO_USED_PER_BULLET);
        }
    }

    public override void ResetWeaponStats()
    {
        ReloadTime = DEFAULT_RELOAD_TIME;
        attackRate = DEFAULT_ATTACK_RATE;
        projDamage = DEFAULT_PROJ_DAMAGE;
        float ammoInClip = AmmoClip.GetPercentageInClip() * DEFAULT_MAX_CLIP_SIZE * 1f;
        AmmoClip = new AmmoClip((int)ammoInClip, DEFAULT_MAX_CLIP_SIZE, AMMO_USED_PER_BULLET);
    }

}
