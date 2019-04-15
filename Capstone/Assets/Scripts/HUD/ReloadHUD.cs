using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadHUD : MonoBehaviour, ISubscriber<OnWeaponReloadEvent>, ISubscriber<OnWeaponReloadCancelEvent>
{
    [SerializeField] private Slider ReloadBar;
    public int playerNum;
    private float maxReloadTime;
    private bool stillReloading;
    [SerializeField] private GameObject FillColor;
    [SerializeField] private GameObject ReloadCanvas;

    private void OnEnable()
    {
        EventAggregator.GetInstance().Register<OnWeaponReloadEvent>(this);
        EventAggregator.GetInstance().Register<OnWeaponReloadCancelEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnWeaponReloadEvent>(this);
        EventAggregator.GetInstance().Unregister<OnWeaponReloadCancelEvent>(this);
    }

    private void Start()
    {
        ReloadBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator OnReloadStart()
    {
        ReloadCanvas.SetActive(true);
        stillReloading = true;
        while (stillReloading && ReloadBar.value < 1)
        {
            ReloadBar.value += (0.25f / maxReloadTime);
            //Debug.Log(ReloadBar.value);

            if (!stillReloading)
            {
                OnReloadEnd();
                break;
            }

            if (ReloadBar.value >= 0.5f && ReloadBar.value < 0.95f)
            {
                FillColor.gameObject.GetComponent<Image>().color = Color.yellow;
            }
            else if (ReloadBar.value >= 0.95f && ReloadBar.value <= 1f)
            {
                FillColor.gameObject.GetComponent<Image>().color = Color.green;
            }

          yield return new WaitForSeconds(0.25f);
        }

        OnReloadEnd();
    }

    private void OnReloadEnd()
    {
        ReloadCanvas.SetActive(false);
        stillReloading = false;
        FillColor.gameObject.GetComponent<Image>().color = Color.red;
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
