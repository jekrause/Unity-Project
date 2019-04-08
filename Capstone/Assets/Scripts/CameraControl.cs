using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;
    private int playerNum;
    private Camera thisCam;
    private int numOfPlayers;
    public static GameObject HUD;

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
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_SinglePlayer_Mode").gameObject);
                    GameObject.FindWithTag("Camera2").SetActive(false);
                    GameObject.FindWithTag("Camera3").SetActive(false);
                    GameObject.FindWithTag("Camera4").SetActive(false);
                }
                break;
            case 2:
                if (player.name == "Player1")
                {
                    thisCam.rect = new Rect(0, 0.5f, 1, 0.5f);
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_2Players_Mode").Find("P1_HUD").gameObject);
                    GameObject.FindWithTag("Camera3").SetActive(false);
                    GameObject.FindWithTag("Camera4").SetActive(false);
                }
                else if (player.name == "Player2")
                {
                    thisCam.rect = new Rect(0, 0, 1, 0.5f);
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_2Players_Mode").Find("P2_HUD").gameObject);
                }
                break;
            case 3:
                if (player.name == "Player1")
                {
                    thisCam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_3Or4Players_Mode").Find("P1_HUD").gameObject);
                    GameObject.FindWithTag("Camera4").SetActive(false);
                }
                else if (player.name == "Player2")
                {
                    thisCam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_3Or4Players_Mode").Find("P2_HUD").gameObject);
                }
                else if (player.name == "Player3")
                {
                    thisCam.rect = new Rect(0, 0, 0.5f, 0.5f);
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_3Or4Players_Mode").Find("P3_HUD").gameObject);
                }
                break;
            case 4:
                if (player.name == "Player1")
                {
                    thisCam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_3Or4Players_Mode").Find("P1_HUD").gameObject);
                }
                else if (player.name == "Player2")
                {
                    thisCam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_3Or4Players_Mode").Find("P2_HUD").gameObject);
                }
                else if (player.name == "Player3")
                {
                    thisCam.rect = new Rect(0, 0, 0.5f, 0.5f);
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_3Or4Players_Mode").Find("P3_HUD").gameObject);
                }
                else if (player.name == "Player4")
                {
                    thisCam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                    player.GetComponent<InventoryHandler>().AssignInventoryHUD(HUD.transform.Find("HUD_3Or4Players_Mode").Find("P4_HUD").gameObject);
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
