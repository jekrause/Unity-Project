using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour, ISubscriber<OnPlayerDeathEvent>
{
    // Start is called before the first frame update
    public static int PlayersAlive { get; private set; }

    void Start()
    {
        PlayersAlive = Settings.NumOfPlayers;
        EventAggregator.GetInstance().Register<OnPlayerDeathEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnPlayerDeathEvent>(this);
    }

    public void OnEventHandler(OnPlayerDeathEvent eventData)
    {
        --PlayersAlive;
        if (PlayersAlive <= 0)
        {
            LoadGameOverScene();
        }
    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }
}
