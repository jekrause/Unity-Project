using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{

    private Animator animator;
    private bool animationPlaying;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animationPlaying = true;
        AudioManager.Play("CreditMusic");
    }

    // Update is called once per frame
    void Update()
    {
        if(animationPlaying && animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            animationPlaying = false;
            StartCoroutine(GoToMainMenu());
            this.enabled = false;
        }
    }

    IEnumerator GoToMainMenu()
    {
        yield return null;

        AsyncOperation EndOfDemoScene = SceneManager.LoadSceneAsync("TitleScreen");
        EndOfDemoScene.allowSceneActivation = false;
        while (!EndOfDemoScene.isDone)
        {
            if (EndOfDemoScene.progress >= 0.9f)
            {
                EndOfDemoScene.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
