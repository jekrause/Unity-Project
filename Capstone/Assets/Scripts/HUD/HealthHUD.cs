using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Slider HealthBar;


    // Start is called before the first frame update
    void Start()
    {
        // listen for player hp events
        player.OnPlayerDamagedEvent += OnDamaged;
        player.OnPlayerHealedEvent += OnHealed;
        player.OnPlayerDeathEvent += OnDeath;
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
        player.OnPlayerDamagedEvent -= OnDamaged;
        player.OnPlayerHealedEvent -= OnHealed;
        player.OnPlayerDeathEvent -= OnDeath;
    }

}
