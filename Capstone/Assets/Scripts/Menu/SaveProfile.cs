using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveProfile : MonoBehaviour
{

    //public PlayerProfile profile;


    static public void saveProfile(string s)
    {
        int totalNames = LoadProfileList.getTotalNames();
        PlayerPrefs.SetString("name"+totalNames,s);
        PlayerPrefs.Save();
        Debug.Log("created: name"+totalNames);
        PlayerPrefs.SetInt("TotalProfiles", totalNames + 1);
        //PlayerPrefs.DeleteAll();
    }

    /*
    static public bool loadProfile(string s)
    {
        string loadedName = PlayerPrefs.GetString(s);

        if (loadedName != s)
        {
            Debug.Log("Name Doesn't exist");
            return false;
        }
        else
        {
            Debug.Log("Name Exists!");
            return true;
        }
    }
    */

    public void loadProfiles()
    {

    }


}
