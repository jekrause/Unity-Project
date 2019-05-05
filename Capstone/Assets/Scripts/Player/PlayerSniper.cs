using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSniper : Player
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        fMoveRate = 7f;
        Stats.Health *= 0.75f;
        Stats.SetMaxHP(Stats.Health);
        MyHUD.transform.Find("HealthBarPanel").GetComponent<HealthHUD>().SetMaxHP(Stats);
        fAttackRadius = 10f;
        fProjSpeed = 20f;
     
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnReviveCompleted()
    {
        fMoveRate = 7f;
        base.OnReviveCompleted();
    }

    public override bool CanEquipWeapon(Item item) { return item is HandGun || item is Dagger || item is Sniper; }
}