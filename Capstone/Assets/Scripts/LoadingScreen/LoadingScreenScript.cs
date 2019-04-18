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
        while (timer < 2.5f)
        {
            timer += 0.25f;
            LoadingBar.fillAmount = timer / 3.0f;
            PercentageText.text = ((int)((timer/3.0f) * 100)) + "%";
            
            yield return new WaitForSeconds(0.25f);
        }

        while (!level.isDone)
        {
            if(level.progress >= 0.9f)
            {
                LoadingBar.fillAmount = 1;
                PercentageText.text = 100 + "%";
                level.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
