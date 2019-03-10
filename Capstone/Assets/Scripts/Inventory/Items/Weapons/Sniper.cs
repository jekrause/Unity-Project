using UnityEngine;


public class Sniper : Weapon, IRangedWeapon
{

    protected float projDamage = 40f;
    protected float projSpeed = 1000;

    public Sniper() { weight = 2; }

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
