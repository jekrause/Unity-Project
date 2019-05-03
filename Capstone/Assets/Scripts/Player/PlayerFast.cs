using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFast : Player
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        fMoveRate = 10f;
        fProjSpeed = 20f;
        MyHUD.transform.Find("HealthBarPanel").GetComponent<HealthHUD>().SetMaxHP(Stats);
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            if(child.tag == "Weapon")
            {
                print(child.GetComponent<Item>().name);
                WeaponInventory.AddItem(child.GetComponent<Item>());
                break;
            }
        }
  
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnReviveCompleted()
    {
        fMoveRate = 10f;
        base.OnReviveCompleted();
    }

    public override bool CanEquipWeapon(Item item) { return item is HandGun || item is Dagger || item is AssaultRifle; }
}