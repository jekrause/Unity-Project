using UnityEngine;

public class AssaultRifle : Weapon, IRangedWeapon
{

    protected float projDamage = 10f;
    protected float projSpeed = 500;

    public AssaultRifle() { weight = 2; }

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
