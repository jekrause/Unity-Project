using UnityEngine;

public class HandGun : Weapon, IRangedWeapon
{
    protected float projDamage = 5f;
    protected float projSpeed = 500;

    public HandGun() { weight = 0; }

    public void Fire()
    {
        var x = Instantiate(bullet, this.ShootPosition.position, ShootPosition.rotation);
        x.SetDamage(projDamage);
        x.GetComponent<Rigidbody2D>().AddForce(x.transform.right * projSpeed);
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }

}
