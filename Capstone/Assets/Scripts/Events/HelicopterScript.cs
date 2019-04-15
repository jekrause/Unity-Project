using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterScript : MonoBehaviour, ISubscriber<OnQuestItemPickUpEvent>, ISubscriber<OnQuestItemDroppedEvent>, ISubscriber<OnEnemyKilledEvent>
{
    private bool HaveHelicopterKey = false;
    private int EnemyKilledCounter;
    private const int ENEMY_KILLED_REQUIREMENT = 20;
    private bool PlayerCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        EventAggregator.GetInstance().Register<OnQuestItemDroppedEvent>(this);
        EventAggregator.GetInstance().Register<OnQuestItemPickUpEvent>(this);
        EventAggregator.GetInstance().Register<OnEnemyKilledEvent>(this);
    }

    private void OnDestroy()
    {
        EventAggregator.GetInstance().Unregister<OnQuestItemDroppedEvent>(this);
        EventAggregator.GetInstance().Unregister<OnQuestItemPickUpEvent>(this);
        EventAggregator.GetInstance().Unregister<OnEnemyKilledEvent>(this);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();
        if (!PlayerCollided && player != null)
        {
            PlayerCollided = true;
            if (HaveHelicopterKey && EnemyKilledCounter >= ENEMY_KILLED_REQUIREMENT)
            {
                //Load the next scene
                Debug.Log("HELLO WORLD");
            }
            else
            {
                Debug.Log("We need to have the helicopter key and kill atleast 20 enemies\nStatus: " + "Have Helicopter Key: " + HaveHelicopterKey +
                          ", Enemies Killed So Far: " + EnemyKilledCounter);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerCollided = false;
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
