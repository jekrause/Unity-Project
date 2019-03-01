public class RocketLauncher : Weapon, IRangedWeapon
{
    public RocketLauncher() { weight = 4; }

    public void Fire()
    {
        Instantiate(bullet, ShootPosition.position, ShootPosition.rotation);
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }

}
