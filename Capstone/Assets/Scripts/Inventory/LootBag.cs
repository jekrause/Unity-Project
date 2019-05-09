using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    private static int BagCounter = 0;
    public int ID { get; private set; }
    public List<Slot> Inventory = new List<Slot>(9);

    private void Start()
    {
        for (int i = 0; i < Inventory.Capacity; i++)
            Inventory.Add(new Slot());
            
        ID = BagCounter++;
    }
    
}
