using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageUpdater : MonoBehaviour
{
    public Text text;
    public GameObject selectScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = getPageNumber();
    }

    private string getPageNumber()
    {
        int pageNumber = selectScreen.GetComponent<LoadMenuController>().currentPage;
        string pageLogo = "Page ";

        switch (pageNumber)
        {
            case 0:
                pageLogo = pageLogo + "1";
                break;
            case 1:
                pageLogo = pageLogo + "2";
                break;
            case 2:
                pageLogo = pageLogo + "3";
                break;
            case 3:
                pageLogo = pageLogo + "4";
                break;
            case 4:
                pageLogo = pageLogo + "5";
                break;
            case 5:
                pageLogo = pageLogo + "6";
                break;
            case 6:
                pageLogo = pageLogo + "7";
                break;
            case 7:
                pageLogo = pageLogo + "8";
                break;
            case 8:
                pageLogo = pageLogo + "9";
                break;
            case 9:
                pageLogo = pageLogo + "10";
                break;
        }
        return pageLogo;
    }
}
