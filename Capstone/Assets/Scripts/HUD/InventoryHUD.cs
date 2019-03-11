﻿using System.Collections.Generic;
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
    private string ActionMessage;
    private int MainInvIndex = 0;
    private int WeaponInvIndex = 0;

    private void Start()
    {

    }

    public void ShowPickUpItemMsg(InputType input)
    {
        if(inputType == InputType.NONE) inputType = input;
        if (PickUpMessage == null && input != InputType.NONE)
        {
            string platformButton = input == InputType.KEYBOARD ? "E" : input == InputType.PS4_CONTROLLER ? "X" : "A";
            PickUpMessage = "-Press '" + platformButton + "' to pick up item-";
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
        Debug.Log("InventoryHUD: Item removed");
        MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Background").Find("Quantity").GetComponent<Text>().text = Quantity + "";

        if(Quantity <= 0)
        {
            MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Item").GetComponent<Image>().sprite = null;
            MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Background").gameObject.SetActive(false);
        }
      
    }

    public void IterateRight()
    {
        MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        MainInvIndex = ++MainInvIndex >= MainInvSlots.Count ? 0 : MainInvIndex;
        MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
    }

    public void IterateLeft()
    {
        MainInvSlots[MainInvIndex].transform.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        MainInvIndex = --MainInvIndex < 0 ? MainInvSlots.Count - 1 : MainInvIndex;
        MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
    }

    public void InventoryToggled(bool InvToggled, InputType input)
    {
        if (InvToggled)
        {
            MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
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
            MainInvSlots[MainInvIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            ActionPanel.gameObject.SetActive(false);
        }
            
    }

    public void OnWeaponEquip(Item item, int weaponSlot, int mainInvSlot)
    {
        // remove from main inventory hud
        MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Item").GetComponent<Image>().sprite = null;
        MainInvSlots[MainInvIndex].transform.Find("ItemSelected").Find("Background").gameObject.SetActive(false);

        // add it to weapon hud
        WeaponInvSlots[weaponSlot].transform.Find("WeaponItemSelected").Find("WeaponItem").GetComponent<Image>().sprite = item.Image;
        WeaponInvSlots[weaponSlot].transform.Find("WeaponItemSelected").Find("WeaponItem").gameObject.SetActive(true);
        
    }

    public void OnWeaponUnEquip(Item item, int slot)
    {
        //TODO
    }

    public void SetHUDActive(bool active)
    {
        gameObject.SetActive(active);
    }

    private void OnDisable()
    {

    }
}
