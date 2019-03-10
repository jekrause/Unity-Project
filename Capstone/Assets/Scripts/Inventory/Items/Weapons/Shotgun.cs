using UnityEngine;

public class Shotgun : Weapon, IRangedWeapon
{

    protected float projDamage = 10f;
    protected float projSpeed = 1000;

    private readonly Vector3[] projAngles = new Vector3[]
    {
        new Vector3(0,-5,0),
        new Vector3(0,-2.5f,0),
        new Vector3(0,0,0),
        new Vector3(0,2.5f,0),
        new Vector3(0,5,0),
    };
    
    public Shotgun() { weight = 3; }

    public void Fire()
    {
        Bullet[] bulletArr = new Bullet[5];
       
        for (int i = 0; i < bulletArr.Length; i++)
        {
            bulletArr[i] = Instantiate(bullet, this.ShootPosition.position, ShootPosition.rotation * Quaternion.Euler(projAngles[i]));
        }
        foreach(Bullet b in bulletArr)
        {
            b.SetDamage(projDamage);
            b.GetComponent<Rigidbody2D>().AddForce(b.transform.right * projSpeed);
        }
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }
}
