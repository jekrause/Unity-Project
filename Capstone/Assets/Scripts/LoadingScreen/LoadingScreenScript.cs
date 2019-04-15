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
        AudioManager.Play("LoadingScreenMusic");
        StartCoroutine(LoadNextSceneAsync());
    }

    IEnumerator LoadNextSceneAsync()
    {
        yield return null;

        AsyncOperation level = SceneManager.LoadSceneAsync("Level01");
        level.allowSceneActivation = false;
        while (!level.isDone)
        {
            LoadingBar.fillAmount = (level.progress / 1);
            PercentageText.text = (level.progress * 100) + "%";

            if (level.progress >= 0.9f)
                level.allowSceneActivation = true;

            yield return null;

        }
        
        
    }
}
