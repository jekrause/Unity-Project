using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodLoot : MonoBehaviour
{
    private Inventory Inventory = new Inventory(9);
    private GameObject[] items = new GameObject[9];
    // Start is called before the first frame update
    void Start()
    {
        items[0] = Instantiate(Resources.Load("Prefabs/Weapons/Sniper") as GameObject);
        items[1] = Instantiate(Resources.Load("Prefabs/Weapons/Shotgun") as GameObject);
        items[2] = Instantiate(Resources.Load("Prefabs/Weapons/Assault Rifle") as GameObject);
        items[3] = Instantiate(Resources.Load("Prefabs/Weapons/Assault Rifle") as GameObject);
        items[4] = Instantiate(Resources.Load("Prefabs/Weapons/Handgun") as GameObject);
        items[5] = Instantiate(Resources.Load("Prefabs/Weapons/RocketLauncher 1") as GameObject);
        items[6] = Instantiate(Resources.Load("Prefabs/Weapons/RocketLauncher 1") as GameObject);
        items[7] = Instantiate(Resources.Load("Prefabs/Medkit") as GameObject);
        items[8] = Instantiate(Resources.Load("Prefabs/StimShot") as GameObject);

        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(false);
            Inventory.AddItem(items[i].GetComponent<Item>());
        }

        GetComponent<LootBag>().Inventory = Inventory;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
