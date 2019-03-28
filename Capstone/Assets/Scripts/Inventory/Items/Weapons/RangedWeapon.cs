using UnityEngine;
using System.Collections;

public abstract class RangedWeapon : Weapon
{
    protected float projDamage;
    protected float projSpeed;
    public float ReloadTime { get; protected set; }
    public bool IsReloading { get; protected set; }
    protected bool ReloadCancel;
    public AmmoClip AmmoClip { get; protected set; }


    public virtual void Fire(Player player)
    {
        if (AmmoClip.EnoughAmmoToFire())
        {
            if (IsReloading) ReloadingInterrupted(player.playerNumber);
            
            var x = Instantiate(bullet, player.shootPosition.position, player.shootPosition.rotation);
            x.SetDamage(projDamage);
            x.GetComponent<Rigidbody2D>().AddForce(x.transform.right * projSpeed);
            AmmoClip.Decrement();
            Debug.Log("Current ammo:" + AmmoClip.CurrentAmmo);
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
                Debug.Log("Not enough ammo in clip to fire, need to reload " + this.name + "\nAmmoUsePerBullet: " + AmmoClip.AMMO_USE_PER_BULLET + ", but in clip: " + AmmoClip.CurrentAmmo );
            }
            
        }

    }

    public IEnumerator Reload(int playerNumber, Ammunition ammunition)
    {
        if (AmmoClip.IsFull() || IsReloading) yield break; // if already reloading, then return
        else
        {
            IsReloading = true;
            Debug.Log(this.name + ": Reloading...");
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

            AmmoClip.LoadAmmunition(ammunition);
            Debug.Log(this.name + ": Reload finished.");
            IsReloading = false;
        }
        
    }

    // In the case the player switch or drop the weapon while it is reloading
    public void ReloadingInterrupted(int playerNumber)
    {
        if (IsReloading && ReloadCancel == false)
        {
            EventAggregator.GetInstance().Publish<OnWeaponReloadCancelEvent>(new OnWeaponReloadCancelEvent(playerNumber, true));
            ReloadCancel = true;
            Debug.Log(this.name + ": Reload interrupted.");
        }
    }
}

public class AmmoClip
{
    public readonly int MAX_CLIP_SIZE;
    public readonly int AMMO_USE_PER_BULLET;
    public int CurrentAmmo { get; private set; }

    public AmmoClip(int maxClipSize, int ammoPerBullet)
    {
        if (maxClipSize >= 1 && ammoPerBullet > 0 && ammoPerBullet <= maxClipSize)
        {
            MAX_CLIP_SIZE = maxClipSize;
            AMMO_USE_PER_BULLET = ammoPerBullet;
            CurrentAmmo = MAX_CLIP_SIZE;
        }
        else
        {
            throw new System.ArgumentException("AmmoClip() Constructor: Invalid argument given.");
        }
    }

    public void LoadAmmunition(Ammunition ammunition)
    {
        if(CurrentAmmo + ammunition.Amount > MAX_CLIP_SIZE)
        {
            int amountLeft = (CurrentAmmo + ammunition.Amount) - MAX_CLIP_SIZE;
            CurrentAmmo = MAX_CLIP_SIZE;
            ammunition.Amount = amountLeft;
        }
        else
        {
            CurrentAmmo += ammunition.Amount;
            ammunition.Amount = 0;
        }
        Debug.Log("Ammunition left on player: " + ammunition.Amount);
    }

    public void Decrement()
    {
        CurrentAmmo = CurrentAmmo - AMMO_USE_PER_BULLET < 0 ? 0 : CurrentAmmo - AMMO_USE_PER_BULLET;
    }

    public bool IsEmpty() { return this.CurrentAmmo <= 0; }

    public bool IsFull() { return this.CurrentAmmo == MAX_CLIP_SIZE; }

    public bool EnoughAmmoToFire() { return this.CurrentAmmo >= AMMO_USE_PER_BULLET; }
}
