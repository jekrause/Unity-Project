using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour, ISubscriber<OnBulletCollisionEvent>, ISubscriber<OnEnemyKilledEvent>
{

    private int NumOfPlayers = 1;

    // 0 - player 1 | 1 - player 2 | 2 - player 3 | 3 - player 4
    public int[] KillCounters { get; private set; } = new int[4];
    public int[] ScoreCounters { get; private set; } = new int[4];
    public int[] ShotsTotal { get; private set; } = new int[4];
    public int[] ShotsHit { get; private set; } = new int[4];
    public float[] Accuracy { get; private set; } = new float[4];

    //Text fields
    public GameObject[] ScoreTexts = new GameObject[4];
    public GameObject[] KillsTexts = new GameObject[4];
    public GameObject[] AccuracyTexts = new GameObject[4];

    // UI
    public GameObject LeaderboardPanel;
    public GameObject LeaderboardTitlePanel;


    // Start is called before the first frame update
    void Start()
    {
        NumOfPlayers = Settings.NumOfPlayers;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            UpdateLeaderboard();
        }
    }

    public void OnEventHandler(OnBulletCollisionEvent eventData)
    {
        int playerNum = eventData.PlayerNum - 1;
        ShotsTotal[playerNum] += 1;
        switch (eventData.CollidedTag)
        {
            case "Enemy":
                ScoreCounters[playerNum] += 10;
                ShotsHit[playerNum] += 1;
                break;
            default:
                
                break;
        }
        Accuracy[playerNum] = ((1.0f * ShotsHit[playerNum]) / (1.0f * ShotsTotal[playerNum])) * 100.0f;
    }

    public void OnEventHandler(OnEnemyKilledEvent eventData)
    {
        KillCounters[eventData.PlayerNumber - 1] += 1;
        ScoreCounters[eventData.PlayerNumber - 1] += 100;
        //UpdateLeaderboard();
    }


    private void UpdateLeaderboard()
    {
        for(int playerNum = 0; playerNum < Settings.NumOfPlayers; playerNum++)
        {
            ScoreTexts[playerNum].GetComponent<Text>().text = ScoreCounters[playerNum] + "";
            KillsTexts[playerNum].GetComponent<Text>().text = KillCounters[playerNum] + "";
            AccuracyTexts[playerNum].GetComponent<Text>().text = Accuracy[playerNum].ToString("0.00") + "%";
        }
        LeaderboardPanel.SetActive(!LeaderboardPanel.activeSelf);
        LeaderboardTitlePanel.SetActive(!LeaderboardTitlePanel.activeSelf);
        Debug.Log("Leaderboard Updated");
    }

    private void OnEnable()
    {
        EventAggregator.GetInstance().Register<OnBulletCollisionEvent>(this);
        EventAggregator.GetInstance().Register<OnEnemyKilledEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnBulletCollisionEvent>(this);
        EventAggregator.GetInstance().Unregister<OnEnemyKilledEvent>(this);
    }
}
