using UnityEngine;

public class RocketLauncher : Weapon, IRangedWeapon
{
    protected float projDamage = 50f;
    protected float projSpeed = 300f;

    public RocketLauncher() { weight = 4; }

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
