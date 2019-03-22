using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillReadyDescription : MonoBehaviour
{
    public int logo;
    public GameObject playerMenu;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        if (logo == 0)
        {
            text.text = playerMenu.GetComponent<PlayerMenuScript>().playerName;
        }
        else
        {
            text.text = playerMenu.GetComponent<PlayerMenuScript>().playerClass;
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
