using UnityEngine;

public class Shotgun : RangedWeapon
{

    public readonly float DEFAULT_ATTACK_RATE = 2f;
    public readonly float DEFAULT_RELOAD_TIME = 4f;
    public readonly float DEFAULT_PROJ_DAMAGE = 10f;
    private const int DEFAULT_MAX_CLIP_SIZE = 50;
    private const int AMMO_USED_PER_BULLET = 10;
    private int MaxAmmoClip = DEFAULT_MAX_CLIP_SIZE; // mutated value as level increases

    private readonly Vector3[] projAngles = new Vector3[]
    {
        new Vector3(0,0,-5),
        new Vector3(0,0,-2.5f),
        new Vector3(0,0,0),
        new Vector3(0,0,2.5f),
        new Vector3(0,0,5),
    };
    
    public Shotgun() {
        weight = 3;
        projDamage = DEFAULT_PROJ_DAMAGE;
        projSpeed = 350;
        AmmoClip = new AmmoClip(DEFAULT_MAX_CLIP_SIZE, AMMO_USED_PER_BULLET);
        ReloadTime = DEFAULT_RELOAD_TIME;
        attackRate = DEFAULT_ATTACK_RATE;
        ReloadSound = "Shotgun_Reload";
        ReloadFinishSound = "Shotgun_Reload_Finished";
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

    public override void Fire(Player player)
    {
        if (AmmoClip.EnoughAmmoToFire())
        {
            Bullet[] bulletArr = new Bullet[5];

            //currentPlayer = player;
            PlayFireAnimation(player);

            for (int i = 0; i < bulletArr.Length; i++)
            {
                bulletArr[i] = Instantiate(bullet, player.shootPosition.position, player.shootPosition.rotation * Quaternion.Euler(projAngles[i]));
            }
            foreach (Bullet b in bulletArr)
            {
                b.SetDamage(projDamage);
                b.SetDistance(20f);
                b.GetComponent<Rigidbody2D>().AddForce(b.transform.right * projSpeed);
            }
            AmmoClip.Decrement();
            EventAggregator.GetInstance().Publish<OnWeaponAmmoChangedEvent>(new OnWeaponAmmoChangedEvent(player.playerNumber, AmmoClip.GetCurrentAmmo()));
        }
        else
        {
            // Fire OnReloadMessage Event
            if (IsReloading && !ReloadCancel)
            {
                Debug.Log(this.name + " is still reloading");
            }
            else
            {
                AudioManager.Play("No_Ammo_Sound");
                Debug.Log("Not enough ammo in clip to fire, need to reload " + this.name + "\nAmmoUsePerBullet: " + AmmoClip.AMMO_USE_PER_BULLET + ", but in clip: " + AmmoClip.GetCurrentAmmo());
            }
        }
        
    }

}
