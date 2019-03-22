using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    private Slider HealthBar;
    private float maxHP = 100f; // by default
    private bool HealthBarActive;

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
        HealthBarActive = active;
    }

    private IEnumerator DisplayHealthBar()
    {
        // display health bar for 3 seconds
        SetHealthBarActive(true);
        int timer = 0;
        while (HealthBarActive && timer < 3)
        {
            timer++;
            yield return new WaitForSeconds(1);
        }
        SetHealthBarActive(false);
    }
    
}
