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

    }

}