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
    public string DownPlatformButton { get; private set; }
    public string RightPlatformButton { get; private set; }
    public MyControllerInput PlayerInput { get; private set; }
    public Collider2D MostRecentCollider { get; private set; }

    // Use this for initialization
    void Start()
    {
        Helicopter = GameObject.Find("Helicopter").GetComponent<Helicopter>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitInteraction(GameObject HUD)
    {
        player = transform.GetComponentInParent<Player>();
        DownPlatformButton = player.DownPlatformButton;
        RightPlatformButton = player.RightPlatformButton;
        PlayerInput = player.myControllerInput;
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
        RemoveLoadBar();
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
        if (player.PlayerState == PlayerState.ALIVE && collision != null)
        {
            MostRecentCollider = collision;
            Debug.Log(MostRecentCollider.tag);
            switch (collision.tag)
            {
                case ("Player"):
                    if(collision.GetComponent<Player>().PlayerState == PlayerState.DOWN)
                        player.OnPlayerTriggerEnter(collision);
                    break;

                case ("Item"):
                    player.InventoryHandler.OnItemTriggerEnter(collision);
                    break;

                case ("Weapon"):
                    player.InventoryHandler.OnItemTriggerEnter(collision);
                    break;

                case ("Helicopter"):
                    Debug.Log("Helicopter triggered");
                    Helicopter.OnHelicopterTriggerEnter(this);
                    break;

                case ("LootBag"):
                    player.LootBagHandler.OnLootBagTriggerEnter(collision);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MostRecentCollider = null;

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
                    Helicopter.OnHelicopterTriggerExit(this);
                    break;

                case ("LootBag"):
                    player.LootBagHandler.OnLootBagTriggerExit();
                    break;
            }
        }
    }

    public bool InteractionPanelIsActive() => InteractionPanel.gameObject.activeSelf == true;


}
