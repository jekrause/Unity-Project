using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour, ISubscriber<OnQuestItemPickUpEvent>, ISubscriber<OnQuestItemDroppedEvent>, ISubscriber<OnEnemyKilledEvent>
{
    private bool HaveHelicopterKey = false;
    private int EnemyKilledCounter;
    private const int ENEMY_KILLED_REQUIREMENT = 20;
    private string DefaultMessage = "We need to find the 'Helicopter Key' first";
    private string ActionMessage = "";
    private string platformButton = "";
    private int NumOfPlayersInRange = 0; // all players need to be at the helicopter
    private const float BUTTON_HELD_DOWN_TIME = 0.5f;
    private float ButtonHeldTimer = 0f;

    // Start is called before the first frame update
    void OnEnable()
    {
        EventAggregator.GetInstance().Register<OnQuestItemDroppedEvent>(this);
        EventAggregator.GetInstance().Register<OnQuestItemPickUpEvent>(this);
        EventAggregator.GetInstance().Register<OnEnemyKilledEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnQuestItemDroppedEvent>(this);
        EventAggregator.GetInstance().Unregister<OnQuestItemPickUpEvent>(this);
        EventAggregator.GetInstance().Unregister<OnEnemyKilledEvent>(this);
    }

    private void Update()
    {
        
    }

    public void OnHelicopterTriggerEnter(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            if (platformButton.Equals(""))
                platformButton = player.DownPlatformButton;

            if (HaveHelicopterKey && EnemyKilledCounter >= ENEMY_KILLED_REQUIREMENT)
            {
                ActionMessage = "-Hold " + platformButton + " to use 'Helicopter Key'";
                player.InteractionPanel.ShowInteractionPanel(this.name, ActionMessage);
                ListenForUserInput();
            }
            else
            {
                player.InteractionPanel.ShowInteractionPanel(this.name, DefaultMessage);
                Debug.Log("We need to have the helicopter key and kill atleast 20 enemies\nStatus: " + "Have Helicopter Key: " + HaveHelicopterKey +
                          ", Enemies Killed So Far: " + EnemyKilledCounter);
            }
        }
    }

    public void OnHelicopterTriggerExit(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if(player != null)
        {
            player.InteractionPanel.RemoveInteractionPanel();
        }
    }

    public void ListenForUserInput()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        NumOfPlayersInRange++;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        NumOfPlayersInRange--;
    }

    public void OnEventHandler(OnQuestItemPickUpEvent eventData)
    {
        Settings.PrintDebugMsg("Quest Item Picked Up");
        HaveHelicopterKey = true;
    }

    public void OnEventHandler(OnQuestItemDroppedEvent eventData)
    {
        Settings.PrintDebugMsg("Quest Item Dropped");
        HaveHelicopterKey = false;
    }

    public void OnEventHandler(OnEnemyKilledEvent eventData)
    {
        EnemyKilledCounter++;
    }
}
