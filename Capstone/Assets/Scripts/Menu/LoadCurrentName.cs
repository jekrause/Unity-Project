using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadCurrentName : MonoBehaviour
{
    public int index;
    public Text text;
    public GameObject selectScreen;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //string dummName = text.text;

        if (selectScreen.GetComponent<LoadMenuController>().getCurrentPage() != null)
        {
            text.text = selectScreen.GetComponent<LoadMenuController>().getCurrentPage()[index];
        }
        else
        {
            text.text = "";
            Debug.Log("LoadCurrentName PAGE IS NULL!!! so setting text to blank");
        }

    }

    /*
    private string[] getCurrentPage()
    {
        switch (selectScreen.GetComponent<LoadMenuController>().currentPage)
        {
            case 0:
                return LoadProfileList.page1;

            case 1:
                return LoadProfileList.page2;

            case 2:
                return LoadProfileList.page3;

            case 3:
                return LoadProfileList.page4;

            case 4:
                return LoadProfileList.page5;

            case 5:
                return LoadProfileList.page6;

            case 6:
                return LoadProfileList.page7;

            case 7:
                return LoadProfileList.page8;

            case 8:
                return LoadProfileList.page9;

            case 9:
                return LoadProfileList.page10;

        }

        return LoadProfileList.page1;
    }
    */
}
