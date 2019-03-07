using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InventoryHandler : MonoBehaviour
{
    private int playerNumber;
    private MyControllerInput myControllerInput;
    private bool actionInProgress; // to elminate multiple button press
    private bool InventoryHUDFocused;
    private Item itemOnGround;
    private List<GameObject> ObjectsPickedUp = new List<GameObject>();
    private bool ItemFocused;
    private EventAggregator eventAggregator;

    [SerializeField] private InventoryHUD InventoryHUD;
    private Inventory MainInventory;
    private Inventory WeaponInventory;

    // Use this for initialization
    void Start()
    {
        // get player info
        playerNumber = GetComponent<Player>().playerNumber;
        myControllerInput = GetComponent<Player>().myControllerInput;
        MainInventory = GetComponent<Player>().MainInventory;
        WeaponInventory = GetComponent<Player>().WeaponInventory;
        if(MainInventory == null || WeaponInventory == null)
        {
            throw new System.MissingFieldException("Inventory Handler: Player should have Inventory as a field");
        }
        eventAggregator = EventAggregator.GetInstance();
       
    }

    // Update is called once per frame
    void Update()
    {
        ReadControllerInput();
    }

    private void ReadControllerInput()
    {
        if (myControllerInput.inputType != InputType.NONE)
        {

            if (Input.GetButtonDown(myControllerInput.UpButton) || Input.GetButton(myControllerInput.UpButton))
            {
                if (actionInProgress == false)
                {
                    InventoryHUDFocused = !InventoryHUDFocused; // toggle inventory selection
                    InventoryHUD.InventoryToggled(InventoryHUDFocused);
                    Debug.Log("Inventory toggled");
                    actionInProgress = true;
                    StartCoroutine(DelayReadingInput());
                }

            }

            if (InventoryHUDFocused)
            {
                if (Input.GetAxis(myControllerInput.DPadX_Windows) > 0) // D-Pad right
                {
                    if (actionInProgress == false)
                    {
                        MainInventory.GetNextItem();
                        InventoryHUD.IterateRight();
                        actionInProgress = true;
                        StartCoroutine(DelayReadingInput());
                    }

                }
                else if (Input.GetAxis(myControllerInput.DPadX_Windows) < 0) // D-Pad left
                {
                    if (actionInProgress == false)
                    {
                        MainInventory.GetPrevItem();
                        InventoryHUD.IterateLeft();
                        actionInProgress = true;
                        StartCoroutine(DelayReadingInput());
                    }
                }
                else if (Input.GetButtonDown(myControllerInput.LeftButton) || Input.GetButton(myControllerInput.LeftButton))
                {
                    if (actionInProgress == false)
                    {
                        int slotNum = MainInventory.GetCurrentSlotNum();
                        Item itemToDrop = MainInventory.GetCurrentItem();
                        if (itemToDrop != null)
                        {
                            int itemIndex = 0;
                            for (int i = 0; i < ObjectsPickedUp.Count; i++)
                            {
                                if (ObjectsPickedUp[i].GetComponent<Item>().GetType() == itemToDrop.GetType())
                                {
                                    ObjectsPickedUp[i].transform.position = transform.position;
                                    ObjectsPickedUp[i].SetActive(true);
                                    MainInventory.RemoveItem(slotNum, false);
                                    actionInProgress = true;
                                    StartCoroutine(DelayReadingInput());
                                    itemIndex = i;
                                    break;
                                }
                            }
                            InventoryHUD.OnItemRemove(MainInventory.GetQuantityInSlot(slotNum));
                            ObjectsPickedUp.RemoveAt(itemIndex);
                        }

                    }

                }
                else if (Input.GetButtonDown(myControllerInput.DownButton) || Input.GetButton(myControllerInput.DownButton))
                {
                    Item itemToUse = MainInventory.GetCurrentItem();
                    if (itemToUse != null)
                    {
                        if (MainInventory.UseItem(GetComponent<Player>(), MainInventory.GetCurrentSlotNum()))
                        {
                            InventoryHUD.OnItemRemove(MainInventory.GetQuantityInSlot(MainInventory.GetCurrentSlotNum()));
                            ObjectsPickedUp.Remove(itemToUse.gameObject);
                            if (MainInventory.GetCurrentItem() == null)
                                Destroy(itemToUse.gameObject);
                        }

                    }

                }
            }
            else
            {
                if (Input.GetButtonDown(myControllerInput.DownButton) || Input.GetButton(myControllerInput.DownButton))
                {
                    if (ItemFocused)
                    {
                        if (itemOnGround != null)
                        {
                            int slot = MainInventory.AddItem(itemOnGround);
                            if (slot != -1)
                            {
                                InventoryHUD.OnItemAdd(itemOnGround, slot, MainInventory.GetQuantityInSlot(slot));
                       
                                // disable game object
                                itemOnGround.gameObject.SetActive(false);
                                ObjectsPickedUp.Add(itemOnGround.gameObject);
                                ItemFocused = false;
                                InventoryHUD.RemovePickUpItemMsg();
                            }

                        }

                    }

                }
            }
        }
        else
        {
            //check if user has bind controller
            myControllerInput = GetComponent<Player>().myControllerInput;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        itemOnGround = collision.collider.GetComponent<Item>();
        if (itemOnGround != null)
        {
            ItemFocused = true;
            InventoryHUD.ShowPickUpItemMsg(myControllerInput.inputType);
        }
 
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        itemOnGround = collision.collider.GetComponent<Item>();

        if (itemOnGround != null)
        {
            ItemFocused = false;
            InventoryHUD.RemovePickUpItemMsg();
        }
    }

    private IEnumerator DelayReadingInput()
    {

        yield return new WaitForSeconds(.25f);
        actionInProgress = false;

    }
}
