using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadProfileList : MonoBehaviour
{

    public static string[] page1;
    public static string[] page2;
    public static string[] page3;
    public static string[] page4;
    public static string[] page5;
    public static string[] page6;
    public static string[] page7;
    public static string[] page8;
    public static string[] page9;
    public static string[] page10;
    public static string[] nameList = new string[100];
    public static int[] AssaultLvList = new int[100];
    public static int[] HeavyLvList = new int[100];
    public static int[] ShotgunLvList = new int[100];
    public static int[] SniperLvList = new int[100];
    public static int totalNames;

    /*
    private void Start()
    {

    }
    */

    public static int LoadData()
    {
        bool loadedAll = false;
        int count = 0;
        string currentName = "";
        string savedName = "";
        int currentAssaultLv = 1;
        int currentHeavyLv = 1;
        int currentShotgunLv = 1;
        int currentSniperLv = 1;

        while (loadedAll == false)
        {
            savedName = "name" + count.ToString();
            currentName = PlayerPrefs.GetString(savedName);
            currentAssaultLv = PlayerPrefs.GetInt("AssaultLv"+count.ToString());
            currentHeavyLv = PlayerPrefs.GetInt("HeavyLv" + count.ToString());
            currentShotgunLv = PlayerPrefs.GetInt("ShotgunLv" + count.ToString());
            currentSniperLv = PlayerPrefs.GetInt("SniperLv" + count.ToString());
            Debug.Log("savedName = "+savedName);
            Debug.Log("currentName = " + currentName);

            if (currentName.Equals(string.Empty))
            {
                Debug.Log("No More names to load");
                totalNames = count;
                Debug.Log("TotalNamesLoaded: "+totalNames);
                loadedAll = true;
            }
            else
            {
                Debug.Log(currentName + " has been loaded");
                nameList[count] = currentName;
                AssaultLvList[count] = currentAssaultLv;
                HeavyLvList[count] = currentHeavyLv;
                ShotgunLvList[count] = currentShotgunLv;
                SniperLvList[count] = currentSniperLv;
                count = count + 1;
            }
        }

        Debug.Log("List of names: " + ArrayToString(nameList, totalNames));
        return totalNames;

    }

    public static bool checkForName(string name)
    {
        LoadData();
        
        Debug.Log("name = " + name);
        int numOfNames = LoadData();  //getTotalNames();
        Debug.Log("numOfNames = " + numOfNames);

        if (numOfNames == -1)
        {
            Debug.Log("There are no profiles yet!");
            return false;
        }

        for (int i = 0; i < numOfNames; i += 1)
        {
            if (nameList[i] == null)
            {
                Debug.Log("Name doesn't exist!");
                return false;
            }
            if (nameList[i].Equals(name))
            {
                Debug.Log("Name already exists!");
                return true;
            }
        }

        return false;
    }

    public static int GetNameIndex(string name)
    {
        LoadData();

        Debug.Log("name = " + name);
        int numOfNames = LoadData();  //getTotalNames();
        Debug.Log("numOfNames = " + numOfNames);

        if (numOfNames == -1)
        {
            Debug.Log("There are no profiles saved!");
            return -1;
        }

        for (int i = 0; i < numOfNames; i += 1)
        {
            if (nameList[i] == null)
            {
                Debug.Log("Name doesn't exist! returning -1");
                return -1;
            }
            if (nameList[i].Equals(name))
            {
                Debug.Log("Name exists at index "+i);
                return i;
            }
        }

        return -1;
    }

    public static bool RemoveName(string name)
    {
        int oldTotalNames = LoadData();

        string[] oldNameList = nameList;
        int[] oldAssaultLvList = AssaultLvList;
        int[] oldHeavyLvList = HeavyLvList;
        int[] oldShotgunLvList = ShotgunLvList;
        int[] oldSniperLvList = SniperLvList;

        int index = 0;  //keeps track of new list index

        RemoveAllData();



        for (int i = 0; i < oldTotalNames; i += 1)
        {

            if (!name.Equals(oldNameList[i]))
            {
                //nameList[index] = oldNameList[i];
                //AssaultLvList[index] = oldAssaultLvList[i];
                //HeavyLvList[index] = oldHeavyLvList[i];
                //ShotgunLvList[index] = oldShotgunLvList[i];
                //SniperLvList[index] = oldSniperLvList[i];
                SaveProfile(oldNameList[i], index, oldAssaultLvList[i], oldHeavyLvList[i], oldShotgunLvList[i], oldSniperLvList[i]);
                Debug.Log("added " + oldNameList[i] + " to new name list");
                index = index + 1;
            }
            else
            {
                Debug.Log("removed " + name + " from name list");
            }


        }

        //totalNames = totalNames - 1;

        Debug.Log("oldTotalNames = " + oldTotalNames);
        Debug.Log("newTotalNames = " + LoadData());

        Debug.Log("List after removing name: " + ArrayToString(nameList, oldTotalNames-1));
        Debug.Log("List before removing name: " + ArrayToString(oldNameList, oldTotalNames));


        return false;
    }

    public static void RemoveAllData()
    {
        PlayerPrefs.DeleteAll();
        nameList = new string[100];
        AssaultLvList = new int[100];
        HeavyLvList = new int[100];
        ShotgunLvList = new int[100];
        SniperLvList = new int[100];
        page1 = null;
        page2 = null;
        page3 = null;
        page4 = null;
        page6 = null;
        page7 = null;
        page8 = null;
        page9 = null;
        page10 = null;
    }


    //If a level of a class is anything less than 1, it will be ignored
    static public void SaveProfile(string s, int nameIndex, int assaultLv, int heavyLv, int shotgunLv, int sniperLv)
    {
        //int totalNames = LoadProfileList.LoadData(); //.getTotalNames();

        PlayerPrefs.SetString("name" + nameIndex, s);
        if (assaultLv >= 1)
        {
            PlayerPrefs.SetInt("AssaultLv" + nameIndex, assaultLv);
        }
        if (heavyLv >= 1)
        {
            PlayerPrefs.SetInt("HeavyLv" + nameIndex, heavyLv);
        }
        if (shotgunLv >= 1)
        {
            PlayerPrefs.SetInt("ShotgunLv" + nameIndex, shotgunLv);
        }
        if (sniperLv >= 1)
        {
            PlayerPrefs.SetInt("SniperLv" + nameIndex, sniperLv);
        }

        PlayerPrefs.Save();
        Debug.Log("Saved: name" + nameIndex +
                    "\nName"+nameIndex+" = " + s +
                    "\nAssault" +nameIndex+"Lv = "+assaultLv+
                    "\nHeavy" + nameIndex + "Lv = " + heavyLv +
                    "\nShotgun" + nameIndex + "Lv = " + shotgunLv +
                    "\nSniper" + nameIndex + "Lv = " + sniperLv);
    }


    //only can be called when playing a level
    public static void SavePlayerProgress()
    {

        GameObject player = null;
        string playerName = null;
        int playerLv = 0;
        int playerClass = 0;
        int nameIndex = 0;

        for (int i = 0; i < Settings.NumOfPlayers; i += 1)
        {


            player = GameObject.Find("Player" + (i + 1));
            playerName = MenuInputSelector.PlayerNames[i];
            playerLv = player.GetComponent<Stats>().Level;
            playerClass = MenuInputSelector.PlayerClasses[i];
            nameIndex = GetNameIndex(playerName);

            if (playerClass == 0)
            {
                SaveProfile(playerName, nameIndex, playerLv, 0, 0, 0);
            }
            else if (playerClass == 1)
            {
                SaveProfile(playerName, nameIndex, 0, playerLv, 0, 0);
            }
            else if (playerClass == 2)
            {
                SaveProfile(playerName, nameIndex, 0, 0, playerLv, 0);
            }
            else if (playerClass == 3)
            {
                SaveProfile(playerName, nameIndex, 0, 0, 0, playerLv);
            }
        }

    }

    public static int GetAssaultLevel(string name)
    {
        int level = 0;
        int nameIndex = GetNameIndex(name);
        level = PlayerPrefs.GetInt("AssaultLv"+nameIndex);
        return level;
    }
    public static int GetHeavyLevel(string name)
    {
        int level = 0;
        int nameIndex = GetNameIndex(name);
        level = PlayerPrefs.GetInt("HeavyLv" + nameIndex);
        return level;
    }
    public static int GetShotgunLevel(string name)
    {
        int level = 0;
        int nameIndex = GetNameIndex(name);
        level = PlayerPrefs.GetInt("ShotgunLv" + nameIndex);
        return level;
    }
    public static int GetSniperLevel(string name)
    {
        int level = 0;
        int nameIndex = GetNameIndex(name);
        level = PlayerPrefs.GetInt("SniperLv" + nameIndex);
        return level;
    }

    /*
    public static int getTotalNames()
    {
        totalNames = PlayerPrefs.GetInt("TotalProfiles");

        return totalNames;
    }
    */




    public static void MakePages()
    {
        int currentPage = 0;
        int index = 0;
        bool newPage = true;
        int numOfNames = LoadData(); //getTotalNames();

        for (int i = 0; i < numOfNames; i += 1)
        {
            switch (currentPage)
            {
                case 0:
                    if(newPage == true)
                    {
                        Debug.Log("i = " + i);
                        index = 0;
                        newPage = false;
                        page1 = new string[10];
                        page1[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page1[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
                case 1:
                    if (newPage == true)
                    {
                        index = 0;
                        newPage = false;
                        page2 = new string[10];
                        page2[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page2[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
                case 2:
                    if (newPage == true)
                    {
                        index = 0;
                        newPage = false;
                        page3 = new string[10];
                        page3[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page3[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
                case 3:
                    if (newPage == true)
                    {
                        index = 0;
                        newPage = false;
                        page4 = new string[10];
                        page4[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page4[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
                case 4:
                    if (newPage == true)
                    {
                        index = 0;
                        newPage = false;
                        page5 = new string[10];
                        page5[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page5[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
                case 5:
                    if (newPage == true)
                    {
                        index = 0;
                        newPage = false;
                        page6 = new string[10];
                        page6[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page6[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
                case 6:
                    if (newPage == true)
                    {
                        index = 0;
                        newPage = false;
                        page7 = new string[10];
                        page7[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page7[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
                case 7:
                    if (newPage == true)
                    {
                        index = 0;
                        newPage = false;
                        page8 = new string[10];
                        page8[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page8[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
                case 8:
                    if (newPage == true)
                    {
                        index = 0;
                        newPage = false;
                        page9 = new string[10];
                        page9[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page9[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
                case 9:
                    if (newPage == true)
                    {
                        index = 0;
                        newPage = false;
                        page10 = new string[10];
                        page10[index] = nameList[i];
                        index = index + 1;
                    }
                    else
                    {
                        page10[index] = nameList[i];
                        index = index + 1;
                        if (index > 9)
                        {
                            currentPage = currentPage + 1;
                            newPage = true;
                        }
                    }
                    break;
            }
        }
    }











    private static string ArrayToString(string[] lst)
    {
        if (lst == null)
        {
            return "null list";
        }

        string output = "";
        for (int i = 0; i < lst.Length; i += 1)
        {
            if (i == 0)
            {
                output = "[" + lst[i] + ", ";
            }
            else
            {
                output = output + lst[i] + ", ";
            }
            if (i == lst.Length - 1)
            {
                output = output + "]";
            }
        }
        return output;
    }


    public static void printNamePages()
    {
        Debug.Log("page1 =" + ArrayToString(page1));
        Debug.Log("page2 =" + ArrayToString(page2));
        Debug.Log("page3 =" + ArrayToString(page3));
        Debug.Log("page4 =" + ArrayToString(page4));
        Debug.Log("page5 =" + ArrayToString(page5));
        Debug.Log("page6 =" + ArrayToString(page6));
        Debug.Log("page7 =" + ArrayToString(page7));
        Debug.Log("page8 =" + ArrayToString(page8));
        Debug.Log("page9 =" + ArrayToString(page9));
        Debug.Log("page10 =" + ArrayToString(page10));
    }

    private static string ArrayToString(string[] lst, int total)
    {
        if (lst == null)
        {
            return "null list";
        }

        string output = "";

        if (total > lst.Length)
        {
            total = lst.Length;
        }

        for (int i = 0; i < total; i += 1)
        {
            if (i == 0)
            {
                output = "[" + lst[i] + ", ";
            }
            else
            {
                if (i == total - 1)
                {
                    output = output + lst[i] + "]";
                }
                else
                {
                    output = output + lst[i] + ", ";
                }
               
            }

        }
        return output;
    }



}
