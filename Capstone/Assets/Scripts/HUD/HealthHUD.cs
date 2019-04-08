using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour, ISubscriber<PlayerHealedEvent>, ISubscriber<PlayerDamagedEvent>
{
    public int playerNumHUD;
    private Slider HealthBar;


    // Start is called before the first frame update
    void Start()
    {
        if (playerNumHUD <= 0 || playerNumHUD >= 5)
        {
            throw new System.Exception("Health HUD player number is out of bound: " + playerNumHUD + "\nMust be 1 - 4");
        }
        HealthBar = transform.GetComponent<Slider>();
        // listen for player hp events
        EventAggregator.GetInstance().Register<PlayerHealedEvent>(this);
        EventAggregator.GetInstance().Register<PlayerDamagedEvent>(this);

        HealthBar.value = 1; // full health
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDamaged(float hp)
    {
        Debug.Log("OnDamaged event triggered");
        HealthBar.value = CalculateHealth(hp);
    }

    private void OnHealed(float hp)
    {
        Debug.Log("OnHealed event triggered");
        HealthBar.value = CalculateHealth(hp);
    }

    private float CalculateHealth(float hp)
    {
        return hp / 100;
    }

    private void OnDeath(float amount)
    {
        OnDisable();
    }

    private void OnDisable()
    {
        // unsubscribe the events
        EventAggregator.GetInstance().Unregister<PlayerHealedEvent>(this);
        EventAggregator.GetInstance().Unregister<PlayerDamagedEvent>(this);
    }


    // when an event is fired, these will be the methods that are called
    public void OnEventHandler(PlayerHealedEvent eventData)
    {
        if(playerNumHUD == eventData.playerNum) // make sure we are updating the correct player HUD
        {
            OnHealed(eventData.Health);
        }
    }

    public void OnEventHandler(PlayerDamagedEvent eventData)
    {
        if (playerNumHUD == eventData.playerNum) // make sure we are updating the correct player HUD
        {
            OnDamaged(eventData.Health);
        }
    }
}
