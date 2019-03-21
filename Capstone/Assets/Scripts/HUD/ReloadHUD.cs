using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadHUD : MonoBehaviour, ISubscriber<OnWeaponReloadEvent>, ISubscriber<OnWeaponReloadCancelEvent>
{
    private Slider ReloadBar;
    public int playerNum;
    private float maxReloadTime;
    private bool stillReloading;
    private Image FillColor;

    // Start is called before the first frame update
    void Start()
    {
        ReloadBar = transform.GetChild(0).GetComponent<Slider>();
        ReloadBar.value = 0;
        EventAggregator.GetInstance().Register<OnWeaponReloadEvent>(this);
        EventAggregator.GetInstance().Register<OnWeaponReloadCancelEvent>(this);
        FillColor = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator OnReloadStart()
    {
        ReloadBar.gameObject.SetActive(true);
        stillReloading = true;
        while (stillReloading && ReloadBar.value < 1)
        {
            ReloadBar.value += (0.25f / maxReloadTime);
            //Debug.Log(ReloadBar.value);

            if (!stillReloading)
            {
                OnReloadStart();
                break;
            }

            if (ReloadBar.value >= 0.5f && ReloadBar.value < 0.95f)
            {
                FillColor.color = Color.yellow;
            }
            else if (ReloadBar.value >= 0.95f && ReloadBar.value <= 1f)
            {
                FillColor.color = Color.green;
            }

          yield return new WaitForSeconds(0.25f);
        }

        OnReloadEnd();
    }

    private void OnReloadEnd()
    {
        ReloadBar.gameObject.SetActive(false);
        stillReloading = false;
        FillColor.color = Color.red;
        ReloadBar.value = 0;
    }

    public void OnEventHandler(OnWeaponReloadEvent eventData)
    {
        if(eventData.playerNum == playerNum)
        {
            maxReloadTime = ((RangedWeapon)eventData.Weapon).ReloadTime;
            StartCoroutine(OnReloadStart());
        }
    }

    private void OnDestroy()
    {
        EventAggregator.GetInstance().Unregister<OnWeaponReloadEvent>(this);
    }

    public void OnEventHandler(OnWeaponReloadCancelEvent eventData)
    {
        if(eventData.playerNum == playerNum)
        {
            OnReloadEnd();
        }
    }
}
