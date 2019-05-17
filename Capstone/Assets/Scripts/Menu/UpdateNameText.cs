using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class UpdateNameText : MonoBehaviour
{

    public Text sampleText;
    //public GameObject nameText;

    // Start is called before the first frame update
    void Start()
    {
        sampleText.text = "";

    }
    private void OnEnable()
    {
        Start();
    }


    public void addLetter(string letter)
    {
        if(sampleText.text.Length < 12)
        {
            sampleText.text = sampleText.text + letter;
            AudioManager.Play("Select");
        }
    }

    public void eraseLastLetter()
    {
        if (sampleText.text.Length > 0) {
            sampleText.text = sampleText.text.Remove(sampleText.text.Length - 1);
            AudioManager.Play("Wait");
        }

    }

    public bool checkNameIsValid()
    {

        if (sampleText.text == "")
        {
            Debug.Log("Name cannot be blank");
            AudioManager.Play("Wait");
            return false;
        }

        Debug.Log("sampleText.text = " + sampleText.text);

        if (LoadProfileList.checkForName(sampleText.text) == true)
        {
            Debug.Log("Name is not Valid");
            AudioManager.Play("Wait");
            return false;
        }
        else
        {   //file with this name doesnt exist yet, so create new file
            SaveProfile.saveProfile(sampleText.text);
            Debug.Log("Name is Good, created new profile!");
            
            return true;
        }


    }


    //following methods for loading profile screen
    /*
    public void loadName(int index, string[] page)
    {
        //sampleText.text = page[index];
        if (index > page.Length-1)
        {
            nameText.GetComponent<TextMeshPro>().text = "Empty";
        }
        else
        {
            nameText.GetComponent<TextMeshPro>().text = page[index];
        }

    }
    */  

}
