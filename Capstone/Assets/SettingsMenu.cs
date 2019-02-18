using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    int buttonNum;

    // Start is called before the first frame update
    void Start()
    {
        int.TryParse(GetComponentInChildren<Text>().ToString(), out buttonNum);
        Settings.NumOfPlayers = buttonNum;
    }

    // Update is called once per frame
    void Update()
    {
        if(Settings.NumOfPlayers == buttonNum)
        {
            GetComponent<Button>().image.color = Color.blue;
        }
        else
        {
            GetComponent<Button>().image.color = Color.red;
        }
    }
}
