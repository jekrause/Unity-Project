using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    public List<GameObject> MainInvSlots = new List<GameObject>(6);
    public List<GameObject> WeaponInvSlots = new List<GameObject>(3);
    [SerializeField] public GameObject MainInventoryPanel;
    [SerializeField] public GameObject WeaponInventoryPanel;
    

    private EventAggregator eventAggregator = EventAggregator.GetInstance();
    private bool IsInvToggled;
    private bool IteratingMainInv = true;
    private int MainInvIndex = 0;
    private int WeaponInvIndex = 0;
    private int WeaponEquippedIndex = 0; // keep track what weapon slot color is green (equipped)

    private void Start()
    {
        //initialize Inventory HUD objects
        for(int i = 0; i < MainInventoryPanel.transform.childCount; i++)
            MainInvSlots[i] = MainInventoryPanel.transform.GetChild(i).gameObject;

        for (int i = 0; i < WeaponInventoryPanel.transform.childCount; i++)
            WeaponInvSlots[i] = WeaponInventoryPanel.transform.GetChild(i).gameObject;

        

        WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
        WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).gameObject.SetActive(true);
    }

    


    public void OnItemAdd(Item item, int slot, int Quantity)
    {
        int stackQuantity = Quantity;
        MainInvSlots[slot].transform.Find("ItemSlot").Find("Item").GetComponent<Image>().sprite = item.Image;
        MainInvSlots[slot].transform.Find("ItemSlot").Find("Item").gameObject.SetActive(true);
        if(item.GetItemType() != Item.Type.WEAPON)
        {
            MainInvSlots[slot].transform.Find("ItemSlot").Find("Background").Find("Quantity").GetComponent<Text>().text = stackQuantity + "";
            MainInvSlots[slot].transform.Find("ItemSlot").Find("Background").gameObject.SetActive(true);
        }

    }

    public void OnItemRemove(int CurrentQuantity)
    {
        if (IteratingMainInv)
        {
            MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Background").Find("Quantity").GetComponent<Text>().text = CurrentQuantity + "";
            if (CurrentQuantity <= 0)
            {
                MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Item").GetComponent<Image>().sprite = null;
                MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Item").gameObject.SetActive(false);
                MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Background").gameObject.SetActive(false);
            }
        }
        else
        {
            WeaponInvSlots[WeaponInvIndex].transform.Find("ItemSlot").Find("Item").gameObject.SetActive(false);
            WeaponInvSlots[WeaponInvIndex].transform.Find("ItemSlot").Find("Item").GetComponent<Image>().sprite = null;
        }
        
      
    }

    public bool IterateRight()
    {
        if (IteratingMainInv)
        {
            MainInvSlots[MainInvIndex].transform.GetChild(0).gameObject.SetActive(false);
            MainInvIndex++;
            if(MainInvIndex >= MainInvSlots.Count)
            {
                IteratingMainInv = false;
                MainInvIndex = 0;
                WeaponInvSlots[WeaponInvIndex = 0].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                WeaponInvSlots[WeaponInvIndex = 0].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                MainInvSlots[MainInvIndex].transform.GetChild(0).gameObject.SetActive(true);
            }
            
        }
        else
        {
            if (WeaponEquippedIndex == WeaponInvIndex)
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(false);
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
            }

            WeaponInvIndex++;
            if(WeaponInvIndex >= WeaponInvSlots.Count)
            {
                IteratingMainInv = true;
                WeaponInvIndex = 0;
                MainInvSlots[MainInvIndex = 0].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        return IteratingMainInv;
        
    }

    public bool IterateLeft()
    {
        if (IteratingMainInv)
        {
            MainInvSlots[MainInvIndex].transform.GetChild(0).gameObject.SetActive(false);
            MainInvIndex--;
            if (MainInvIndex < 0)
            {
                IteratingMainInv = false;
                MainInvIndex = MainInvSlots.Count;
                WeaponInvIndex = WeaponInvSlots.Count - 1;
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                MainInvSlots[MainInvIndex].transform.GetChild(0).gameObject.SetActive(true);
            }

        }
        else
        {
            if (WeaponEquippedIndex == WeaponInvIndex) 
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(false);
            }

            WeaponInvIndex--;
            if (WeaponInvIndex < 0)
            {
                IteratingMainInv = true;
                WeaponInvIndex = WeaponInvSlots.Count - 1 ;
                MainInvSlots[MainInvIndex = MainInvSlots.Count - 1].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        return IteratingMainInv;
    }

    public void InventoryToggled(bool InvToggled)
    {
        IsInvToggled = InvToggled;
        if (InvToggled)
        {
            if (IteratingMainInv)
            {
                MainInvSlots[MainInvIndex].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(true);
            }   
        }
        else
        {
            if (IteratingMainInv)
            {
                MainInvSlots[MainInvIndex].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(false);
            }

            WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
            WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).gameObject.SetActive(true);

        }
            
    }

    public void OnWeaponStow(Item item, int weaponSlot, int mainInvSlot)
    {
        if(mainInvSlot != -1)
        {
            // remove from main inventory hud
            MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Item").GetComponent<Image>().sprite = null;
            MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Item").gameObject.SetActive(false);
            MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Background").gameObject.SetActive(false);
        }
        
        // add it to weapon hud
        WeaponInvSlots[weaponSlot].transform.Find("ItemSlot").Find("Item").GetComponent<Image>().sprite = item.Image;
        WeaponInvSlots[weaponSlot].transform.Find("ItemSlot").Find("Item").gameObject.SetActive(true);
        
    }

    public void OnWeaponEquip(int weaponSlot)
    {

        WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).gameObject.SetActive(false); // turn off previous equipped slot

        if (IsInvToggled && !IteratingMainInv)
            WeaponInvSlots[weaponSlot].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        else
            WeaponInvSlots[weaponSlot].transform.GetChild(0).GetComponent<Image>().color = Color.green;

        WeaponInvSlots[weaponSlot].transform.GetChild(0).gameObject.SetActive(true);
        WeaponEquippedIndex = weaponSlot;
    }

    public void OnWeaponUnEquip()
    {
        
        if( IsInvToggled && WeaponEquippedIndex == WeaponInvIndex)
        {
            WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        }
        else if(WeaponEquippedIndex == WeaponInvIndex)
        {
            WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }
        
    }

    public void OnDropWeapon(int slot)
    {
        
    }

    public void SetHUDActive(bool active)
    {
        gameObject.SetActive(active);
    }

    private void OnDisable()
    {

    }
}
