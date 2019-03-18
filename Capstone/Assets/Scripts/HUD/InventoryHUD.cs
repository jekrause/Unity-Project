using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    [SerializeField] public List<GameObject> MainInvSlots;
    [SerializeField] public List<GameObject> WeaponInvSlots;
    [SerializeField] public GameObject MainInventoryPanel;
    [SerializeField] public GameObject WeaponInventoryPanel;
    [SerializeField] public GameObject MessagePanel;
    [SerializeField] public GameObject ActionPanel;

    private InputType inputType = InputType.NONE;
    private EventAggregator eventAggregator = EventAggregator.GetInstance();
    private bool IsInvToggled;
    private string PickUpMessage;
    private string PickUpWepMessage;
    private string ActionMessage;
    private bool IteratingMainInv = true;
    private int MainInvIndex = 0;
    private int WeaponInvIndex = 0;
    private int MainInvSlotUsed = 0;
    private int WeaponInvSlotUsed = 0;
    private int WeaponEquippedIndex = -1; // keep track what weapon slot color is green (equipped)

    private void Start()
    {

    }

    public void ShowPickUpItemMsg(InputType input, bool isWeapon)
    {
        if(inputType == InputType.NONE) inputType = input;

        // initialize messages
        if (PickUpMessage == null && input != InputType.NONE)
        {
            string platformButton = input == InputType.KEYBOARD ? "E" : input == InputType.PS4_CONTROLLER ? "X" : "A";
            PickUpMessage = "-Press '" + platformButton + "' to pick up item-";
            PickUpWepMessage = "-Press '" + platformButton + "' to pick up weapon-\n-Hold '" + platformButton + "' to equip weapon - ";
        }

        if (isWeapon)
        {
            MessagePanel.transform.Find("PickUpItemText").GetComponent<Text>().text = PickUpWepMessage;
        }
        else
        {
            MessagePanel.transform.Find("PickUpItemText").GetComponent<Text>().text = PickUpMessage;
        }

        MessagePanel.SetActive(true);
    }

    public void RemovePickUpItemMsg()
    {
        MessagePanel.SetActive(false);
    }

    public void OnItemAdd(Item item, int slot, int Quantity)
    {
        int stackQuantity = Quantity;
        MainInvSlots[slot].transform.Find("ItemSelected").Find("Item").GetComponent<Image>().sprite = item.Image;
        MainInvSlots[slot].transform.Find("ItemSelected").Find("Background").gameObject.SetActive(true);
        if(item.GetItemType() != Item.Type.WEAPON)
        {
            MainInvSlots[slot].transform.Find("ItemSelected").Find("Background").Find("Quantity").GetComponent<Text>().text = stackQuantity + "";
            MainInvSlots[slot].transform.Find("ItemSelected").Find("Background").gameObject.SetActive(true);
        }
        else
        {
            MainInvSlots[slot].transform.Find("ItemSelected").Find("Background").gameObject.SetActive(false);
        }
            
       
    }

    public void OnItemRemove(int Quantity)
    {
        if (IteratingMainInv)
        {
            MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Background").Find("Quantity").GetComponent<Text>().text = Quantity + "";
            MainInvSlotUsed--;
            if (Quantity <= 0)
            {
                MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Item").GetComponent<Image>().sprite = null;
                MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Background").gameObject.SetActive(false);
            }
        }
        else
        {
            WeaponInvSlots[WeaponInvIndex].transform.Find("WeaponItemSelected").Find("WeaponItem").GetComponent<Image>().sprite = null;
            WeaponInvSlotUsed--;
        }
        
      
    }

    public bool IterateRight()
    {
        if (IteratingMainInv)
        {
            MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            MainInvIndex++;
            if(MainInvIndex >= MainInvSlots.Count)
            {
                IteratingMainInv = false;
                MainInvIndex = 0;
                WeaponInvSlots[WeaponInvIndex = 0].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
            
        }
        else
        {
            if (WeaponEquippedIndex != -1 && WeaponEquippedIndex == WeaponInvIndex)
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }

            WeaponInvIndex++;
            if(WeaponInvIndex >= WeaponInvSlots.Count)
            {
                IteratingMainInv = true;
                WeaponInvIndex = 0;
                MainInvSlots[MainInvIndex = 0].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
        }

        return IteratingMainInv;
        
    }

    public bool IterateLeft()
    {
        if (IteratingMainInv)
        {
            MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            MainInvIndex--;
            if (MainInvIndex < 0)
            {
                IteratingMainInv = false;
                MainInvIndex = MainInvSlots.Count;
                WeaponInvSlots[WeaponInvIndex = WeaponInvSlots.Count - 1].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }

        }
        else
        {
            if (WeaponEquippedIndex != -1 && WeaponEquippedIndex == WeaponInvIndex) 
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            WeaponInvIndex--;
            if (WeaponInvIndex < 0)
            {
                IteratingMainInv = true;
                WeaponInvIndex = WeaponInvSlots.Count;
                MainInvSlots[MainInvIndex = MainInvSlots.Count - 1].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
        }

        return IteratingMainInv;
    }

    public void InventoryToggled(bool InvToggled, InputType input)
    {
        IsInvToggled = InvToggled;
        if (InvToggled)
        {
            if (IteratingMainInv)
            {
                MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            }
            
            if(inputType == InputType.NONE) inputType = input;
            if (ActionMessage == null)
            {
                switch (inputType)
                {
                    case InputType.PS4_CONTROLLER:
                        ActionMessage = "'X' - Use Item\n'O' - Drop Item";
                        break;

                    case InputType.XBOX_CONTROLLER:
                        ActionMessage = "'A' - Use Item\n'B' - Drop Item";
                        break;

                    default:
                        ActionMessage = ActionPanel.transform.GetComponentInChildren<Text>().text;
                        break;
                }
                ActionPanel.transform.GetComponentInChildren<Text>().text = ActionMessage;
            }
             
             ActionPanel.gameObject.SetActive(true);
            
            
        }
        else
        {
            if (IteratingMainInv)
            {
                MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            else
            {
                if(WeaponEquippedIndex != -1 && WeaponEquippedIndex == WeaponInvIndex)
                {
                    WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                }
                else
                {
                    WeaponInvSlots[WeaponInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                }
                
            }
            
            ActionPanel.gameObject.SetActive(false);
        }
            
    }

    public void OnWeaponStow(Item item, int weaponSlot, int mainInvSlot)
    {
        if(mainInvSlot != -1)
        {
            // remove from main inventory hud
            mainInvSlot--;
            MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Item").GetComponent<Image>().sprite = null;
            MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Background").gameObject.SetActive(false);
        }
        
        // add it to weapon hud
        WeaponInvSlotUsed++;
        WeaponInvSlots[weaponSlot].transform.Find("WeaponItemSelected").Find("WeaponItem").GetComponent<Image>().sprite = item.Image;
        WeaponInvSlots[weaponSlot].transform.Find("WeaponItemSelected").Find("WeaponItem").gameObject.SetActive(true);
        
    }

    public void OnWeaponEquip(int weaponSlot)
    {
        OnWeaponUnEquip(); // remove any slot that was equipped
        if(IsInvToggled)
            WeaponInvSlots[weaponSlot].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        else
            WeaponInvSlots[weaponSlot].transform.GetChild(0).GetComponent<Image>().color = Color.green;

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
                WeaponInvSlots[WeaponEquippedIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            WeaponEquippedIndex = -1;
        }
        
        
    }

    public void OnDropWeapon(int slot)
    {
        WeaponInvSlotUsed--;
    }

    public void SetHUDActive(bool active)
    {
        gameObject.SetActive(active);
    }

    private void OnDisable()
    {

    }
}
