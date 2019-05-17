using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveProfile : MonoBehaviour
{

    //public PlayerProfile profile;


    static public void saveProfile(string s)
    {
        int totalNames = LoadProfileList.LoadData(); //.getTotalNames();
        PlayerPrefs.SetString("name"+totalNames,s);
        PlayerPrefs.SetInt("AssaultLv" + totalNames, 1);  //set to level 1 for now
        PlayerPrefs.SetInt("HeavyLv" + totalNames, 1);  //set to level 1 for now
        PlayerPrefs.SetInt("ShotgunLv" + totalNames, 1);  //set to level 1 for now
        PlayerPrefs.SetInt("SniperLv" + totalNames, 1);  //set to level 1 for now
        PlayerPrefs.Save();
        //Debug.Log("created: name"+totalNames);
        Debug.Log("Created: name" + totalNames +
                    "\nName" + totalNames + " = " + s +
                    "\nAssault" + totalNames + "Lv = " + 1 +
                    "\nHeavy" + totalNames + "Lv = " + 1 +
                    "\nShotgun" + totalNames + "Lv = " + 1 +
                    "\nSniper" + totalNames + "Lv = " + 1);
        //PlayerPrefs.SetInt("TotalProfiles", totalNames + 1);
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

    /*
    public void loadProfiles()
    {

    }
    */

}
