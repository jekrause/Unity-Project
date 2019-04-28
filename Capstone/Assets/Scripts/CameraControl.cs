using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;
    private int playerNum;
    private Camera thisCam;
    private int numOfPlayers;
    public static GameObject HUD;

    // HUD gameobject name
    public static readonly string HUD_SINGLEPLAYER_MODE = "HUD_SinglePlayer_Mode";
    public static readonly string HUD_2PLAYERS_MODE = "HUD_2Players_Mode";
    public static readonly string HUD_3PLAYERS_MODE = "HUD_3Players_Mode";
    public static readonly string HUD_4PLAYERS_MODE = "HUD_4Players_Mode";
    public static readonly string P1_HUD = "P1_HUD";
    public static readonly string P2_HUD = "P2_HUD";
    public static readonly string P3_HUD = "P3_HUD";
    public static readonly string P4_HUD = "P4_HUD";

    private void Awake()
    {
        if(HUD == null)
            HUD = GameObject.FindGameObjectWithTag("HUD");
    }

    // Use this for initialization
    void Start () {

        thisCam = gameObject.GetComponent<Camera>();


        //for testing
        numOfPlayers = Settings.NumOfPlayers;
   
        switch (numOfPlayers)
        {
            case 1:
                if (player.name == "Player1")
                {
                    thisCam.rect = new Rect(0, 0, 1, 1);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_SINGLEPLAYER_MODE).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_SINGLEPLAYER_MODE).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_SINGLEPLAYER_MODE).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_SINGLEPLAYER_MODE).gameObject);
                    GameObject.FindWithTag("Camera2").SetActive(false);
                    GameObject.FindWithTag("Camera3").SetActive(false);
                    GameObject.FindWithTag("Camera4").SetActive(false);
                }
                break;
            case 2:
                if (player.name == "Player1")
                {
                    thisCam.rect = new Rect(0, 0.5f, 1, 0.5f);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_2PLAYERS_MODE).Find(P1_HUD).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_2PLAYERS_MODE).Find(P1_HUD).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_2PLAYERS_MODE).Find(P1_HUD).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_2PLAYERS_MODE).Find(P1_HUD).gameObject);
                    GameObject.FindWithTag("Camera3").SetActive(false);
                    GameObject.FindWithTag("Camera4").SetActive(false);
                }
                else if (player.name == "Player2")
                {
                    thisCam.rect = new Rect(0, 0, 1, 0.5f);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_2PLAYERS_MODE).Find(P2_HUD).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_2PLAYERS_MODE).Find(P2_HUD).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_2PLAYERS_MODE).Find(P2_HUD).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_2PLAYERS_MODE).Find(P2_HUD).gameObject);
                }
                break;
            case 3:
                if (player.name == "Player1")
                {
                    thisCam.rect = new Rect(0, 0.5f, 1f, 0.5f);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P1_HUD).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P1_HUD).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P1_HUD).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P1_HUD).gameObject);
                    GameObject.FindWithTag("Camera4").SetActive(false);
                }
                else if (player.name == "Player2")
                {
                    thisCam.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P2_HUD).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P2_HUD).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P2_HUD).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P2_HUD).gameObject);
                }
                else if (player.name == "Player3")
                {
                    thisCam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P3_HUD).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P3_HUD).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P3_HUD).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_3PLAYERS_MODE).Find(P3_HUD).gameObject);
                }
                break;
            case 4:
                if (player.name == "Player1")
                {
                    thisCam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P1_HUD).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P1_HUD).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P1_HUD).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P1_HUD).gameObject);
                }
                else if (player.name == "Player2")
                {
                    thisCam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P2_HUD).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P2_HUD).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P2_HUD).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P2_HUD).gameObject);
                }
                else if (player.name == "Player3")
                {
                    thisCam.rect = new Rect(0, 0, 0.5f, 0.5f);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P3_HUD).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P3_HUD).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P3_HUD).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P3_HUD).gameObject);
                }
                else if (player.name == "Player4")
                {
                    thisCam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    player.GetComponent<Player>().MyHUD = HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P4_HUD).gameObject;
                    player.GetComponent<InventoryHandler>().InitInventoryHandler(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P4_HUD).gameObject);
                    player.GetComponent<LootBagHandler>().InitLootBagHandler(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P4_HUD).gameObject);
                    player.transform.Find("InteractionCollider").GetComponent<InteractionHandler>().InitInteractionPanel(HUD.transform.Find(HUD_4PLAYERS_MODE).Find(P4_HUD).gameObject);
                }
                break;
        }

        //old
        //offset = transform.position - player.transform.position;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        offset = transform.position - player.transform.position;
        //transform.position = player.transform.position;

    }

    // LateUpdate is called once per frame, but after Update
    void LateUpdate () {
    
        transform.position = player.transform.position + offset;


    }
}
