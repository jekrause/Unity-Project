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
    private static int totalNames;

    private void Start()
    {

    }

    public static int LoadData()
    {
        bool loadedAll = false;
        int count = 0;
        string currentName = "";
        string savedName = "";

        while (loadedAll == false)
        {
            savedName = "name" + count.ToString();
            currentName = PlayerPrefs.GetString(savedName);
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
        int numOfNames = getTotalNames();
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

    public static int getTotalNames()
    {
        totalNames = PlayerPrefs.GetInt("TotalProfiles");

        return totalNames;
    }



    public static void MakePages()
    {
        int currentPage = 0;
        int index = 0;
        bool newPage = true;
        int numOfNames = getTotalNames();

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
