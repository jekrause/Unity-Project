using UnityEngine;
using System.Collections;

public abstract class RangedWeapon : Weapon
{
    public readonly string NO_AMMO_SOUND = "No_Ammo_Sound";
    protected float projDamage;
    protected float projSpeed;
    public float ReloadTime { get; protected set; }
    public bool IsReloading { get; protected set; }
    protected bool ReloadCancel;
    public AmmoClip AmmoClip { get; protected set; }
    public string ReloadSound { get; protected set; }
    public string ReloadFinishSound { get; protected set; }


    public virtual void Fire(Player player)
    {
        if (AmmoClip.EnoughAmmoToFire())
        {
            if (IsReloading) ReloadingInterrupted(player.playerNumber);
            
            PlayFireAnimation(player);
            var x = Instantiate(bullet, player.shootPosition.position, player.shootPosition.rotation);

            x.SetDamage(projDamage);
            x.setShooter(player.gameObject);
            x.GetComponent<Rigidbody2D>().AddForce(x.transform.right * projSpeed);
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
                AudioManager.Play(NO_AMMO_SOUND);
                Debug.Log("Not enough ammo in clip to fire, need to reload " + this.name + "\nAmmoUsePerBullet: " + AmmoClip.AMMO_USE_PER_BULLET + ", but in clip: " + AmmoClip.CurrentAmmoRaw);
            }
            
        }

    }

    public IEnumerator Reload(int playerNumber, Ammunition ammunition)
    {
        if (AmmoClip.IsFull() || IsReloading) yield break; // if already reloading, then return
        else
        {
            IsReloading = true;
            //Debug.Log(this.name + ": Reloading...");
            AudioManager.Play(ReloadSound);
            EventAggregator.GetInstance().Publish<OnWeaponReloadEvent>(new OnWeaponReloadEvent(playerNumber, this));
            for (float timer = ReloadTime; timer > 0; timer -= 0.25f)
            {
                if (ReloadCancel)
                {
                    IsReloading = ReloadCancel = false;
                    yield break;
                }
                yield return new WaitForSeconds(0.25f);
            }
            AudioManager.Stop(ReloadSound);
            AmmoClip.LoadAmmunition(ammunition);
            EventAggregator.GetInstance().Publish(new OnWeaponAmmoChangedEvent(playerNumber, AmmoClip.GetCurrentAmmo()));
            EventAggregator.GetInstance().Publish(new OnPlayerAmmoChangedEvent(playerNumber, ammunition));
            AudioManager.Play(ReloadFinishSound);
            IsReloading = false;
        }
        
    }

    public abstract void UpdateWeaponStats(Stats playerStats);
    public abstract void ResetWeaponStats();

    // In the case the player switch or drop the weapon while it is reloading
    public void ReloadingInterrupted(int playerNumber)
    {
        if (IsReloading && ReloadCancel == false)
        {
            AudioManager.Stop(ReloadSound);
            EventAggregator.GetInstance().Publish<OnWeaponReloadCancelEvent>(new OnWeaponReloadCancelEvent(playerNumber, true));
            ReloadCancel = true;
            Debug.Log(this.name + ": Reload interrupted.");
        }
    }
    
    public void PlayFireAnimation(Player player)
    {
        player.GetComponent<SpriteRenderer>().sprite = this.PlayerFireImage;   //change to firing sprite


        float fireRate = 0f;
        if (this.attackRate > 0.6f)
        {
            fireRate = 0.6f;      //0.6f will be max fire delay
        }
        else
        {
            fireRate = this.attackRate;
        }

        player.WaitForFireSprite(this.PlayerImage, fireRate / 2);    //change back to regular sprite
    }
}

public class AmmoClip
{
    public readonly int MAX_CLIP_SIZE;
    public readonly int AMMO_USE_PER_BULLET;
    public int CurrentAmmoRaw { get; private set; }

    public AmmoClip(int maxClipSize, int ammoPerBullet)
    {
        if (maxClipSize >= 1 && ammoPerBullet > 0 && ammoPerBullet <= maxClipSize)
        {
            MAX_CLIP_SIZE = maxClipSize;
            AMMO_USE_PER_BULLET = ammoPerBullet;
            CurrentAmmoRaw = MAX_CLIP_SIZE;
        }
        else
        {
            throw new System.ArgumentException("AmmoClip() Constructor: Invalid argument given.");
        }
    }

    public AmmoClip(int startAmmoClip, int maxClipSize, int ammoPerBullet) : this(maxClipSize, ammoPerBullet)
    { 
        CurrentAmmoRaw = startAmmoClip;
    }

    public void LoadAmmunition(Ammunition ammunition)
    {
        if(CurrentAmmoRaw + ammunition.Amount > MAX_CLIP_SIZE)
        {
            int amountLeft = (CurrentAmmoRaw + ammunition.Amount) - MAX_CLIP_SIZE;
            CurrentAmmoRaw = MAX_CLIP_SIZE;
            ammunition.Amount = amountLeft;
        }
        else
        {
            CurrentAmmoRaw += ammunition.Amount;
            ammunition.Amount = 0;
        }
        //Debug.Log("Ammunition left on player: " + ammunition.Amount);
    }

    public void Decrement()
    {
        CurrentAmmoRaw = CurrentAmmoRaw - AMMO_USE_PER_BULLET < 0 ? 0 : CurrentAmmoRaw - AMMO_USE_PER_BULLET;
    }

    public float GetPercentageInClip() => (1.0f * CurrentAmmoRaw) / (1.0f * MAX_CLIP_SIZE);

    public bool IsEmpty() { return this.CurrentAmmoRaw <= 0; }

    public bool IsFull() { return this.CurrentAmmoRaw == MAX_CLIP_SIZE; }

    public bool EnoughAmmoToFire() { return this.CurrentAmmoRaw >= AMMO_USE_PER_BULLET; }
    
    public int GetCurrentAmmo()
    {
        return CurrentAmmoRaw / AMMO_USE_PER_BULLET;
    }
}
