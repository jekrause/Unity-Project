using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavy : Player
{
    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        fMoveRate = 5f;
        fHP *= 1.5f;
        fAttackRadius = 5f;
        fProjSpeed = 10f;

        if (basicWeapon != null)
        {
            WeaponInventory.AddItem(basicWeapon);
        }

    }

    protected override void Update()
    {

    }

    public override bool CanEquipWeapon(Item item) { return item is HandGun || item is Dagger || item is RocketLauncher; }

}