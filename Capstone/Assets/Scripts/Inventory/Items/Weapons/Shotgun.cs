using UnityEngine;

public class Shotgun : RangedWeapon
{
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
        projDamage = 10f;
        projSpeed = 350;
        AmmoClip = new AmmoClip(25, 5);
        ReloadTime = 4;
    }

    public override void Fire(Player player)
    {
        if (AmmoClip.EnoughAmmoToFire())
        {
            Bullet[] bulletArr = new Bullet[5];

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
                Debug.Log("Not enough ammo in clip to fire, need to reload " + this.name + "\nAmmoUsePerBullet: " + AmmoClip.AMMO_USE_PER_BULLET + ", but in clip: " + AmmoClip.CurrentAmmo);
            }
        }
        
    }

}
