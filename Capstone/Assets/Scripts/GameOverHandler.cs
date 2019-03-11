using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour, ISubscriber<OnPlayerDeathEvent>
{
    // Start is called before the first frame update
    private int NumberOfPlayer; 

    void Start()
    {
        EventAggregator.GetInstance().Register<OnPlayerDeathEvent>(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnPlayerDeathEvent>(this);
    }

    public void OnEventHandler(OnPlayerDeathEvent eventData)
    {
        NumberOfPlayer++;
        if (NumberOfPlayer == Settings.NumOfPlayers)
        {
            LoadGameOverScene();
        }
    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }
}
