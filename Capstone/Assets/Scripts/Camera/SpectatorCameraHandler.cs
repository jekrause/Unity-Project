using UnityEngine;
using UnityEngine.UI;

public class SpectatorCameraHandler : MonoBehaviour, ISubscriber<OnPlayerDeathEvent>
{

    private CameraControl[] CameraControls;
    private bool[] PlayersSpectating = { false, false, false, false };
    public GameObject[] SpectatingPanel;

    // Start is called before the first frame update
    void Start()
    {
        CameraControls = new CameraControl[Settings.NumOfPlayers];
        SpectatingPanel = new GameObject[Settings.NumOfPlayers];

        for(int i = 0; i < CameraControls.Length; i++)
        {
            CameraControls[i] = transform.GetChild(i).GetComponent<CameraControl>();
            SpectatingPanel[i] = CameraControls[i].transform.Find("SpectatingCanvas").Find("Panel").gameObject;
        }
            
        
    }

    
    public void OnEventHandler(OnPlayerDeathEvent eventData)
    {
        if (eventData == null || eventData.playerNum <= 0 || eventData.playerNum > 4)
            throw new System.ArgumentException("Invalid event data");

        // find a player who is alive that we can spectate
        GameObject playerToSpectate = null;
        for(int i = 0; i < CameraControls.Length; i++)
        {
            if(CameraControls[i].player.GetComponent<Player>().Stats.Health > 0)
            {
                playerToSpectate = CameraControls[i].player;
                break;
            }
        }

        // all players died
        if (playerToSpectate == null)
        {
            PlayersSpectating[eventData.playerNum - 1] = true;
            for (int i = 0; i < CameraControls.Length; i++)
                SpectatingPanel[i].SetActive(false);

            return;
        }

        // update all players who is dead to spectate a player who is alive
        for (int i = 0; i < PlayersSpectating.Length; i++)
        {
            // if there exist a player who is already spectating the player who just died, update that player to spectate someone else who is alive
            if (PlayersSpectating[i] == true && CameraControls[i].player.GetComponent<Player>().playerNumber == eventData.playerNum)
            {
                CameraControls[i].AssignCameraToPlayer(playerToSpectate);
                SpectatingPanel[i].transform.GetChild(0).GetComponent<Text>().text = "Spectating Player " + playerToSpectate.GetComponent<Player>().playerNumber;
            }
        }

        // the player who just died now spectate someone else who is alive
        CameraControls[eventData.playerNum - 1].AssignCameraToPlayer(playerToSpectate);
        PlayersSpectating[eventData.playerNum - 1] = true;
        SpectatingPanel[eventData.playerNum - 1].transform.GetChild(0).GetComponent<Text>().text = "Spectating Player " + playerToSpectate.GetComponent<Player>().playerNumber;
        SpectatingPanel[eventData.playerNum - 1].SetActive(true);
    }

    private void OnEnable()
    {
        EventAggregator.GetInstance().Register<OnPlayerDeathEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnPlayerDeathEvent>(this);
    }
}
