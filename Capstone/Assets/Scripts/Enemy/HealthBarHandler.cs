using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    private Slider HealthBar;
    private float maxHP = 100f; // by default
    private float timer = 0;
    private bool coroutineRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar = transform.GetChild(0).GetChild(0).GetComponent<Slider>(); // transform/Canvas/HealthBar
        HealthBar.value = 1; // full health    
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxHP(float maxHP)
    {
        this.maxHP = maxHP;
    }

    public void OnDamaged(float hp)
    {
        HealthBar.value = CalculateHealth(hp);
        timer = 0;
        if(!coroutineRunning)
            StartCoroutine(DisplayHealthBar());
    }

    public void OnHealed(float hp)
    {
        HealthBar.value = CalculateHealth(hp);
    }

    public float CalculateHealth(float hp)
    {
        return hp / maxHP;
    }

    public void OnDeath(float amount)
    {
        
    }

    private void SetHealthBarActive(bool active)
    {
        HealthBar.gameObject.SetActive(active);
        coroutineRunning = active;
    }

    private IEnumerator DisplayHealthBar()
    {
        // display health bar for 3 seconds
        SetHealthBarActive(true);
        while (timer < 3)
        {
            yield return null;
            timer += Time.deltaTime;
        }
        SetHealthBarActive(false);
    }
    
}
