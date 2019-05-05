using System.Collections;
using UnityEngine;

public class AssaultRifle : RangedWeapon
{
    public readonly float DEFAULT_ATTACK_RATE = 0.5f;
    public readonly float DEFAULT_RELOAD_TIME = 3f;
    public readonly float DEFAULT_PROJ_DAMAGE = 10f;
    private const int DEFAULT_MAX_CLIP_SIZE = 25;
    private const int AMMO_USED_PER_BULLET = 1;
    private int MaxAmmoClip = DEFAULT_MAX_CLIP_SIZE; // mutated value as level increases

    public AssaultRifle() {
        ReloadTime = DEFAULT_ATTACK_RATE;
        AmmoClip = new AmmoClip(DEFAULT_MAX_CLIP_SIZE, AMMO_USED_PER_BULLET);
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
        if(playerStats.Level > 1)
        {
            int ammoInClip = AmmoClip.CurrentAmmoRaw;
            MaxAmmoClip = DEFAULT_MAX_CLIP_SIZE + ((AMMO_USED_PER_BULLET * 2) * playerStats.Level);
            AmmoClip = new AmmoClip(ammoInClip, MaxAmmoClip, AMMO_USED_PER_BULLET);
        }
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
            Debug.Log(AmmoClip.CurrentAmmoRaw + ", " + AmmoClip.MAX_CLIP_SIZE);
            ammoInClip =  (1.0f * AmmoClip.CurrentAmmoRaw) / (1.0f * AmmoClip.MAX_CLIP_SIZE);
            ammoInClip = ammoInClip * DEFAULT_MAX_CLIP_SIZE * 1f;
        }
        AmmoClip = new AmmoClip((int)ammoInClip, DEFAULT_MAX_CLIP_SIZE, AMMO_USED_PER_BULLET);
    }

}
