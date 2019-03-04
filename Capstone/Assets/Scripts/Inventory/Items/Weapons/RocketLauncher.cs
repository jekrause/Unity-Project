public class RocketLauncher : Weapon, IRangedWeapon
{
    public RocketLauncher() { weight = 4; }

    public void Fire()
    {
        var x = Instantiate(bullet, this.ShootPosition.position, ShootPosition.rotation);
        x.gameObject.SendMessage("SetTargetPosition", transform.position);
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }

}
