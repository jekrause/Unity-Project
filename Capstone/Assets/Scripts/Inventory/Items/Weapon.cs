public abstract class Weapon : Item
{
    protected const int MAX_STACK_SIZE = 1;
    protected float weight;

    public override int GetMaxStackSize() { return MAX_STACK_SIZE; }

    public override void InteractWithItem()
    {
        throw new System.NotImplementedException();
    }
}

public class AssaultRifle : Weapon, IRangedWeapon
{
    public AssaultRifle() { weight = 2; }

    public void Fire()
    {
        throw new System.NotImplementedException();
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }
}

public class RocketLauncher : Weapon, IRangedWeapon
{
    public RocketLauncher() { weight = 4; }

    public void Fire()
    {
        throw new System.NotImplementedException();
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }

}

public class HandGun : Weapon, IRangedWeapon
{
    public HandGun() { weight = 0; }

    public void Fire()
    {
        throw new System.NotImplementedException();
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }

}

public class Sniper : Weapon, IRangedWeapon
{
    public Sniper() { weight = 2; }

    public void Fire()
    {
        throw new System.NotImplementedException();
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }

}


public class Dagger : Weapon, IMeleeWeapon
{
    public Dagger() { weight = 0; }

    public void Strike()
    {
        throw new System.NotImplementedException();
    }
}
