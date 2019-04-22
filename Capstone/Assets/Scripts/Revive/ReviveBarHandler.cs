using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveBarHandler : MonoBehaviour
{
    public GameObject ReviveTimeBar;
    public Image ReviveLoadingBar;
    public Slider DownTimeBar;
    public GameObject[] PlatformButtonImages;
    public GameObject ReviveStatusText;

    // Start is called before the first frame update
    void Start()
    {
        ReviveLoadingBar.fillAmount = 0;
        DownTimeBar.value = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.identity; // don't follow the player's rotation
    }

    public void SetPlatformButtonImage(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.KEYBOARD:
                PlatformButtonImages[2].SetActive(true);
                break;

            case InputType.PS4_CONTROLLER:
                PlatformButtonImages[1].SetActive(true);
                break;

            case InputType.XBOX_CONTROLLER:
                PlatformButtonImages[0].SetActive(true);
                break;
        }
        ReviveTimeBar.gameObject.SetActive(true);
    }

    public void OnReviveHandler(float downTime, float reviveTime)
    {
        gameObject.SetActive(true);
        DownTimeBar.value = downTime / Player.MAX_DOWN_TIME;
        ReviveLoadingBar.fillAmount = reviveTime / Player.MAX_REVIVE_TIME;
        if(reviveTime > 0)
        {
            ReviveStatusText.GetComponent<Text>().text = "Reviving";
            ReviveStatusText.GetComponent<Text>().color = Color.blue;
        }
       

    }

    public void OnReviveCancelHandler()
    {
        ReviveTimeBar.gameObject.SetActive(false);
        ReviveLoadingBar.fillAmount = 0;
        ReviveStatusText.GetComponent<Text>().text = "Bleeding";
        ReviveStatusText.GetComponent<Text>().color = Color.red;
    }

    public void OnReviveFinishHandler()
    {
        gameObject.SetActive(false);
        ReviveTimeBar.gameObject.SetActive(false);
        ReviveLoadingBar.fillAmount = 0;
        DownTimeBar.value = 1;
        ReviveStatusText.GetComponent<Text>().text = "Bleeding";
        ReviveStatusText.GetComponent<Text>().color = Color.red;
    }

}
