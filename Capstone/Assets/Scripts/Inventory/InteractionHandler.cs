using UnityEngine;
using UnityEngine.UI;

public class InteractionHandler : MonoBehaviour
{
    private GameObject InteractionPanel;
    private GameObject ItemTypeText;
    private Slider HoldButtonDownBar;
    private Image FillColor;
    private Color DefaultColor;
    private Player player;
    private Helicopter Helicopter;

    // Use this for initialization
    void Start()
    {
        GameObject.Find("Helicopter").GetComponent<Helicopter>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitInteractionPanel(GameObject HUD)
    {
        player = transform.GetComponentInParent<Player>();
        InteractionPanel = HUD.transform.Find("InteractionPanel").gameObject;
        HoldButtonDownBar = InteractionPanel.transform.Find("HoldButtonBar").GetComponent<Slider>();
        ItemTypeText = InteractionPanel.transform.Find("MainTextPanel").Find("ItemTypeText").gameObject;
        HoldButtonDownBar.value = 0;
        FillColor = HoldButtonDownBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        DefaultColor = FillColor.color;
    }

    public void ShowInteractionPanel(string item, string action)
    {
        InteractionPanel.transform.GetComponentInChildren<Text>().text = action;
        ItemTypeText.GetComponent<Text>().text = item;
        InteractionPanel.gameObject.SetActive(true);
    }

    public void RemoveInteractionPanel()
    {
        InteractionPanel.gameObject.SetActive(false);
    }

    public void ShowLoadBar(float time, float maxTime)
    {
        HoldButtonDownBar.value = time / maxTime;
        if (HoldButtonDownBar.value > .20f) HoldButtonDownBar.gameObject.SetActive(true);
        if (HoldButtonDownBar.value >= 1)
        {
            HoldButtonDownBar.value = 0;
            HoldButtonDownBar.gameObject.SetActive(false);
        }
    }

    public void RemoveLoadBar()
    {
        if (FillColor == null) return;
        HoldButtonDownBar.value = 0;
        FillColor.color = DefaultColor;
        HoldButtonDownBar.gameObject.SetActive(false);
    }

    public void ShowRejectedLoadBar()
    {
        HoldButtonDownBar.value = 1;
        FillColor.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            switch (collision.tag)
            {
                case ("Player"):
                    player.OnPlayerTriggerEnter(collision);
                    break;

                case ("Item"):
                    player.InventoryHandler.OnItemTriggerEnter(collision);
                    break;

                case ("Weapon"):
                    player.InventoryHandler.OnItemTriggerEnter(collision);
                    break;

                case ("Helicopter"):
                    Helicopter.OnHelicopterTriggerEnter(collision);
                    break;

                case ("LootBag"):
                    player.LootBagHandler.OnLootBagTriggerEnter(collision);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision != null)
        {
            switch (collision.tag)
            {
                case ("Player"):
                    player.OnPlayerTriggerExit();
                    break;

                case ("Item"):
                    player.InventoryHandler.OnItemTriggerExit();
                    break;

                case ("Weapon"):
                    player.InventoryHandler.OnItemTriggerExit();
                    break;

                case ("Helicopter"):
                    break;

                case ("LootBag"):
                    player.LootBagHandler.OnLootBagTriggerExit();
                    break;
            }
        }
    }


}
