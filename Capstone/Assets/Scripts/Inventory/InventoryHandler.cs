using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{ 
    private Inventory MainInventory;
    private Inventory WeaponInventory;
    public GameObject MessagePanel;

 

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowPickUpItemMsg()
    {
        MessagePanel.SetActive(true);
    }

    public void RemovePickUpItemMsg()
    {
        MessagePanel.SetActive(false);
    }
}
