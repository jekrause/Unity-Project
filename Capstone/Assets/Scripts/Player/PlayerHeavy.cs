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
        Stats.Health *= 1.5f;
        Stats.SetMaxHP(Stats.Health);
        MyHUD.transform.Find("HealthBarPanel").GetComponent<HealthHUD>().SetMaxHP(Stats);
        fAttackRadius = 5f;
        fProjSpeed = 10f;

        if (basicWeapon != null)
        {
            WeaponInventory.AddItem(basicWeapon);
        }
    }

    public override void OnReviveCompleted()
    {
        fMoveRate = 5f;
        base.OnReviveCompleted();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanEquipWeapon(Item item) { return item is HandGun || item is Dagger || item is RocketLauncher; }

}