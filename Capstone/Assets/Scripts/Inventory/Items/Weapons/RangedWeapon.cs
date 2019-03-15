using UnityEngine;
using System.Collections;

public abstract class RangedWeapon : Weapon
{
    protected float projDamage;
    protected float projSpeed;
    protected float ReloadTime;
    public bool IsReloading { get; protected set; }
    protected bool CancelReload;
    protected AmmoClip AmmoClip;


    public virtual void Fire()
    {
        if (!AmmoClip.IsEmpty())
        {
            var x = Instantiate(bullet, this.ShootPosition.position, ShootPosition.rotation);
            x.SetDamage(projDamage);
            x.GetComponent<Rigidbody2D>().AddForce(x.transform.right * projSpeed);
            AmmoClip.Decrement();
            Debug.Log("Current ammo:" + AmmoClip.CurrentAmmo);
        }
        else
        {
            // Fire OnReloadMessage Event
            if (IsReloading && !CancelReload)
            {
                Debug.Log(this.name + " is still reloading");
            }
            else
            {
                Debug.Log("Empty clip, need to reload " + this.name + "\nAmmoUsePerBullet: " + AmmoClip.AMMO_USE_PER_BULLET + ", but in clip: " + AmmoClip.CurrentAmmo );
            }
            
        }

    }

    public IEnumerator Reload(Ammunition ammunition)
    {
        if (AmmoClip.IsFull() || IsReloading) yield return new WaitForSeconds(0f);
        else
        {
            IsReloading = true;
            Debug.Log(this.name + ": Reloading...");
            yield return new WaitForSeconds(ReloadTime);
            if (CancelReload)
            {
                Debug.Log(this.name + ": Reload interrupted.");
                CancelReload = false;
            }
            else
            {
                AmmoClip.LoadAmmunition(ammunition);
                Debug.Log(this.name + ": Reload finished.");
            
            }
            IsReloading = false;
        }
        
    }

    // In the case the player switch or drop the weapon while it is reloading
    public void ReloadingInterrupted()
    {
        if (IsReloading)
        {
            CancelReload = true;
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
        if (maxClipSize >= 1)
        {
            MAX_CLIP_SIZE = maxClipSize;
            AMMO_USE_PER_BULLET = ammoPerBullet;
            CurrentAmmo = MAX_CLIP_SIZE;
        }
        else
        {
            throw new System.ArgumentException("Cannot have negative clip");
        }
    }

    public void LoadAmmunition(Ammunition ammunition)
    {
        int amountLeft = ammunition.Amount - MAX_CLIP_SIZE;
        if (amountLeft < 0)
        {
            CurrentAmmo = amountLeft >= 0 ? amountLeft : 0;
            ammunition.Amount = 0;
        }
        else
        {
            CurrentAmmo = MAX_CLIP_SIZE;
            ammunition.Amount = amountLeft;
        }
        
    }

    public void Decrement()
    {
        CurrentAmmo = CurrentAmmo - AMMO_USE_PER_BULLET < 0 ? 0 : CurrentAmmo - AMMO_USE_PER_BULLET;
    }

    public bool IsEmpty() { return this.CurrentAmmo <= 0; }

    public bool IsFull() { return this.CurrentAmmo == MAX_CLIP_SIZE; }
}
