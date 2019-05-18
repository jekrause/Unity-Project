using UnityEngine;

public class RocketLauncher : RangedWeapon
{

    public readonly float DEFAULT_ATTACK_RATE = 2f;
    public readonly float DEFAULT_RELOAD_TIME = 5;
    public readonly float DEFAULT_PROJ_DAMAGE = 20f; //should start pretty weak, the explosion damage does 1/10th this damage every frame
    private const int DEFAULT_MAX_CLIP_SIZE = 50;
    private const int AMMO_USED_PER_BULLET = 25;
    private int MaxAmmoClip = DEFAULT_MAX_CLIP_SIZE; // mutated value as level increases

    public RocketLauncher() {
        projDamage = DEFAULT_PROJ_DAMAGE;
        projSpeed = 1000f;//300f;
        weight = 4;
        ReloadTime = DEFAULT_RELOAD_TIME;
        attackRate = DEFAULT_ATTACK_RATE;
        AmmoClip = new AmmoClip(DEFAULT_MAX_CLIP_SIZE, AMMO_USED_PER_BULLET);
        // Will need to be updated to a better sound
        ReloadSound = "Universal_Reload";
        ReloadFinishSound = "Universal_Reload_Finished";
        FireSound = "RocketFire";
    }

    public override void UpdateWeaponStats(Stats playerStats)
    {
        ReloadTime = DEFAULT_RELOAD_TIME - (DEFAULT_RELOAD_TIME * playerStats.ReloadMultiplier);
        attackRate = DEFAULT_ATTACK_RATE - (DEFAULT_ATTACK_RATE * playerStats.AttackRateMultiplier);
        projDamage = DEFAULT_PROJ_DAMAGE + playerStats.DamageMultiplier;
        if(playerStats.Level % 2 == 0)
        {
            MaxAmmoClip = DEFAULT_MAX_CLIP_SIZE + (AMMO_USED_PER_BULLET * playerStats.Level);
            int ammoInClip = (int) (AmmoClip.GetPercentageInClip() * MaxAmmoClip);
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
