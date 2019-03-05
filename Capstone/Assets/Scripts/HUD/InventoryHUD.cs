using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    [SerializeField] private List<GameObject> MainInvSlots;
    [SerializeField] private List<GameObject> WeaponInvSlots;
    [SerializeField] private GameObject MainInventoryPanel;
    [SerializeField] private GameObject WeaponInventoryPanel;
    [SerializeField] private GameObject MessagePanel;
    private bool IsInvToggled;
    private string PickUpMessage;
    private int slotIndex = 0;

    private void Start()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPickUpItemMsg(InputType inputType)
    {
        if(PickUpMessage == null && inputType != InputType.NONE)
        {
            string platformButton = inputType == InputType.KEYBOARD ? "E" : inputType == InputType.PS4_CONTROLLER ? "X" : "A";
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
        MainInvSlots[slot].transform.Find("ItemSelected").Find("Background").Find("Quantity").GetComponent<Text>().text = stackQuantity + "";
       
    }

    public void OnItemRemove(int Quantity)
    {
        Debug.Log("Item removed");
        MainInvSlots[slotIndex].transform.Find("ItemSelected").Find("Background").Find("Quantity").GetComponent<Text>().text = Quantity + "";

        if(Quantity <= 0)
        {
            MainInvSlots[slotIndex].transform.Find("ItemSelected").Find("Item").GetComponent<Image>().sprite = null;
            MainInvSlots[slotIndex].transform.Find("ItemSelected").Find("Background").gameObject.SetActive(false);
        }
      
    }

    public void IterateRight()
    {
        MainInvSlots[slotIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        slotIndex = ++slotIndex >= MainInvSlots.Count ? 0 : slotIndex;
        MainInvSlots[slotIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
    }

    public void IterateLeft()
    {
        MainInvSlots[slotIndex].transform.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        slotIndex = --slotIndex < 0 ? MainInvSlots.Count - 1 : slotIndex;
        MainInvSlots[slotIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
    }

    public void InventoryToggled(bool InvToggled)
    {
        if(InvToggled)
            MainInvSlots[slotIndex].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        else
            MainInvSlots[slotIndex].transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }

    private void OnItemUse(Item item, int slot, int Quantity)
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
