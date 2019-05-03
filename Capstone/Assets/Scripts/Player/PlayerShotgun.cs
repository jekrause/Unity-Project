using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotgun : Player
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        MyHUD.transform.Find("HealthBarPanel").GetComponent<HealthHUD>().SetMaxHP(Stats);
        fAttackRadius = 2f;
        fMoveRate = 6f;
        fProjSpeed = 20f;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnReviveCompleted()
    {
        fMoveRate = 6f;
        base.OnReviveCompleted();
    }

    public override bool CanEquipWeapon(Item item) { return item is HandGun || item is Dagger || item is Shotgun; }
}