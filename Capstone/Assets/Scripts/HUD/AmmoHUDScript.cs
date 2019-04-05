using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHUDScript : MonoBehaviour, ISubscriber<OnWeaponAmmoChangedEvent>, ISubscriber<OnPlayerWeaponChangedEvent>,
                                            ISubscriber<OnPlayerAmmoChangedEvent>
{

    private string AmmoCountText;
    private string PlayerAmmoText;
    public int playerNumber;
    public GameObject WeaponAmmoPanel;

    private void OnEnable()
    {
        EventAggregator.GetInstance().Register<OnWeaponAmmoChangedEvent>(this);
        EventAggregator.GetInstance().Register<OnPlayerWeaponChangedEvent>(this);
        EventAggregator.GetInstance().Register<OnPlayerAmmoChangedEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.GetInstance().Unregister<OnWeaponAmmoChangedEvent>(this);
        EventAggregator.GetInstance().Unregister<OnPlayerWeaponChangedEvent>(this);
        EventAggregator.GetInstance().Unregister<OnPlayerAmmoChangedEvent>(this);
    }

    private void ShowHUD()
    {
        WeaponAmmoPanel.transform.GetChild(0).GetComponent<Text>().text = AmmoCountText;
        WeaponAmmoPanel.transform.GetChild(1).GetComponent<Text>().text = PlayerAmmoText;
        WeaponAmmoPanel.gameObject.SetActive(true);
    }

    private void HideHUD()
    {
        WeaponAmmoPanel.gameObject.SetActive(false);
    }

    public void OnEventHandler(OnWeaponAmmoChangedEvent eventData)
    {
       if(eventData.playerNum == playerNumber)
        {
            AmmoCountText = eventData.currentAmmoClip + "";
            WeaponAmmoPanel.transform.GetChild(0).GetComponent<Text>().text = AmmoCountText;
        }
    }

    public void OnEventHandler(OnPlayerWeaponChangedEvent eventData)
    {
        if(eventData.playerNum == playerNumber)
        {
            if(eventData.Weapon != null && eventData.Weapon is RangedWeapon)
            {
                PlayerAmmoText = eventData.currentAmmunition.Amount + "";
                AmmoCountText = ((RangedWeapon)eventData.Weapon).AmmoClip.CurrentAmmo + "";
                ShowHUD();
            }
            else
            {
                HideHUD();
            }
        }
    }

    public void OnEventHandler(OnPlayerAmmoChangedEvent eventData)
    {
        if(eventData.playerNum == playerNumber)
        {
            PlayerAmmoText = eventData.currentAmmunition.Amount + "";
            WeaponAmmoPanel.transform.GetChild(1).GetComponent<Text>().text = PlayerAmmoText;
        }
    }
}
