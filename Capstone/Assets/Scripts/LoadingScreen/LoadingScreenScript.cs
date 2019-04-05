using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScreenScript : MonoBehaviour
{
    [SerializeField]private Image LoadingBar;
    [SerializeField] private Text PercentageText;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNextSceneAsync());
    }

    IEnumerator LoadNextSceneAsync()
    {
       // simply using this since it loads the next scene too quick
        while(timer < 4.5f)
        {
            timer += 0.25f;
            LoadingBar.fillAmount = (timer / 4.5f);
            PercentageText.text = ((int)((timer / 4.5) * 100)) + "%";
            yield return new WaitForSeconds(0.25f);
        }
        if(timer >= 4.5f)
        {
            AsyncOperation level = SceneManager.LoadSceneAsync(1);
        }
            
    }
}
