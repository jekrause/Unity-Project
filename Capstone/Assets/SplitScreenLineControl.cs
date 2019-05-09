using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenLineControl : MonoBehaviour
{

    public GameObject TopToBottomLine;
    public GameObject MiddleToBottomLine;
    public GameObject LeftToRightLine;



    // Start is called before the first frame update
    void Start()
    {
        switch (Settings.NumOfPlayers)
        {
            case 1:
                TopToBottomLine.SetActive(false);
                MiddleToBottomLine.SetActive(false);
                LeftToRightLine.SetActive(false);
                break;
            case 2:
                TopToBottomLine.SetActive(false);
                MiddleToBottomLine.SetActive(false);
                LeftToRightLine.SetActive(true);
                break;
            case 3:
                TopToBottomLine.SetActive(false);
                MiddleToBottomLine.SetActive(true);
                LeftToRightLine.SetActive(true);
                break;
            case 4:
                TopToBottomLine.SetActive(true);
                MiddleToBottomLine.SetActive(false);
                LeftToRightLine.SetActive(true);
                break;
        }

    }

}
