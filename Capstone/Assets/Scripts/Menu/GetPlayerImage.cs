using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerImage : MonoBehaviour
{
    public GameObject playerMenu;
    private int playerIndex;
    public Sprite assault;
    public Sprite heavy;
    public Sprite shotgun;
    public Sprite sniper;
    // Start is called before the first frame update
    void Start()
    {
        playerIndex = playerMenu.GetComponent<PlayerMenuScript>().playerNum - 1;

        switch (MenuInputSelector.PlayerClasses[playerIndex])
        {
            case 0:
                GetComponent<Image>().sprite = assault;
                break;
            case 1:
                GetComponent<Image>().sprite = heavy;
                break;
            case 2:
                GetComponent<Image>().sprite = shotgun;
                break;
            case 3:
                GetComponent<Image>().sprite = sniper;
                break;
        }
    }

    private void OnEnable()
    {
        Start();
    }


}
