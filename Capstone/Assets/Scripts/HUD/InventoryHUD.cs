using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    public List<GameObject> MainInvSlots = new List<GameObject>(6);
    public List<GameObject> WeaponInvSlots = new List<GameObject>(3);
    [SerializeField] public GameObject MainInventoryPanel;
    [SerializeField] public GameObject WeaponInventoryPanel;
    [SerializeField] public GameObject MessagePanel;
    [SerializeField] public GameObject ActionPanel;

    private EventAggregator eventAggregator = EventAggregator.GetInstance();
    private bool IsInvToggled;
    private bool IteratingMainInv = true;
    private int MainInvIndex = 0;
    private int WeaponInvIndex = 0;
    private int WeaponEquippedIndex = -1; // keep track what weapon slot color is green (equipped)

    private void Start()
    {
        //initialize Inventory HUD objects
        for(int i = 0; i < MainInventoryPanel.transform.childCount; i++)
            MainInvSlots[i] = MainInventoryPanel.transform.GetChild(i).gameObject;

        for (int i = 0; i < WeaponInventoryPanel.transform.childCount; i++)
            WeaponInvSlots[i] = WeaponInventoryPanel.transform.GetChild(i).gameObject;

    }

    public void ShowPickUpItemMsg(string message)
    {
        MessagePanel.transform.Find("PickUpItemText").GetComponent<Text>().text = message;
        MessagePanel.SetActive(true);
    }

    public void RemovePickUpItemMsg()
    {
        MessagePanel.SetActive(false);
    }

    public void ShowActionPanel(string message)
    {
        ActionPanel.transform.GetComponentInChildren<Text>().text = message;
        ActionPanel.gameObject.SetActive(true);
    }

    public void RemoveActionPanel()
    {
        ActionPanel.gameObject.SetActive(false);
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
            if (WeaponEquippedIndex == WeaponInvIndex) WeaponEquippedIndex = -1;
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
            if (WeaponEquippedIndex != -1 && WeaponEquippedIndex == WeaponInvIndex)
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
            if (WeaponEquippedIndex != -1 && WeaponEquippedIndex == WeaponInvIndex) 
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
                if(WeaponEquippedIndex != -1 && WeaponEquippedIndex == WeaponInvIndex)
                {
                    WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).gameObject.SetActive(false);
                    WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                }
                
            }
            
        }
            
    }

    public void OnWeaponStow(Item item, int weaponSlot, int mainInvSlot)
    {
        if(mainInvSlot != -1)
        {
            // remove from main inventory hud
            mainInvSlot--;
            MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Item").GetComponent<Image>().sprite = null;
            MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Item").gameObject.SetActive(false);
            MainInvSlots[MainInvIndex].transform.Find("ItemSlot").Find("Background").gameObject.SetActive(false);
        }
        
        // add it to weapon hud
        if (WeaponEquippedIndex == -1) OnWeaponEquip(weaponSlot);
        WeaponInvSlots[weaponSlot].transform.Find("ItemSlot").Find("Item").GetComponent<Image>().sprite = item.Image;
        WeaponInvSlots[weaponSlot].transform.Find("ItemSlot").Find("Item").gameObject.SetActive(true);
        
    }

    public void OnWeaponEquip(int weaponSlot)
    {
        OnWeaponUnEquip(); // remove any slot that was equipped
        if(IsInvToggled && weaponSlot == WeaponEquippedIndex)
            WeaponInvSlots[weaponSlot].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        else
            WeaponInvSlots[weaponSlot].transform.GetChild(0).GetComponent<Image>().color = Color.green;

        WeaponInvSlots[weaponSlot].transform.GetChild(0).gameObject.SetActive(true);
        WeaponEquippedIndex = weaponSlot;

    }

    public void OnWeaponUnEquip()
    {
        if(WeaponEquippedIndex != -1)
        {
            if(WeaponEquippedIndex == WeaponInvIndex && IsInvToggled)
            {
                WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).gameObject.SetActive(false);
            }
            WeaponEquippedIndex = -1;
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
