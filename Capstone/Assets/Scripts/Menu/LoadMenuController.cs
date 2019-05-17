using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenuController : MonoBehaviour
{
    public GameObject parentPlayer;
    private int player;
    public int currentPage;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        player = parentPlayer.GetComponent<PlayerMenuScript>().playerNum;
        LoadProfileList.totalNames = LoadProfileList.LoadData();    //loads data and set totalNames to the total amount of names
        LoadProfileList.MakePages();
        LoadProfileList.printNamePages();
        Debug.Log("LoadProfileList.totalNames = " + LoadProfileList.totalNames);
    }

    private void OnEnable()
    {
        Start();
    }



    public void gotoNextPage()
    {
        currentPage = currentPage + 1;

        if (currentPage > 9)
        {
            currentPage = 0;
        }

        if (getCurrentPage() == null)
        {
            currentPage = 0;
            Debug.Log("Page is null");
            AudioManager.Play("Wait");
        }
    }

    public void gotoPrevPage()
    {
        if (currentPage == 0)
        {
            currentPage = 9;
            for (int i = currentPage; i > 0; i -= 1)
            {
                currentPage = i;
                Debug.Log("currentPage = " +currentPage);
                if (getCurrentPage() != null)
                {
                    return;
                }
            }
            Debug.Log("Page is null");
            AudioManager.Play("Wait");
            currentPage = 0;
        }
        else
        {
            currentPage = currentPage - 1;
        }


    }

    public string[] getCurrentPage()
    {
        switch (currentPage)
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
}
