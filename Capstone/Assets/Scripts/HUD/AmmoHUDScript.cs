using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHUDScript : MonoBehaviour, ISubscriber<OnWeaponAmmoChangedEvent>, ISubscriber<OnPlayerWeaponChangedEvent>,
                                            ISubscriber<OnPlayerAmmoChangedEvent>
{
    public int playerNumber;
    private RangedWeapon CurrentWeapon;
    public GameObject WeaponCanvas;
    public GameObject AmmoClip;
    public GameObject PlayerAmmo;

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
        WeaponCanvas.gameObject.SetActive(true);
    }

    private void HideHUD()
    {
        WeaponCanvas.gameObject.SetActive(false);
    }

    public void OnEventHandler(OnWeaponAmmoChangedEvent eventData)
    {
       if(eventData.playerNum == playerNumber)
        {
            AmmoClip.transform.GetComponent<Text>().text = CurrentWeapon.AmmoClip.GetCurrentAmmo() + "";
        }
    }

    public void OnEventHandler(OnPlayerWeaponChangedEvent eventData)
    {
        if(eventData.playerNum == playerNumber)
        {
            if(eventData.Weapon != null && eventData.Weapon is RangedWeapon)
            {
                CurrentWeapon = ((RangedWeapon)eventData.Weapon);
                PlayerAmmo.transform.GetComponent<Text>().text = (eventData.currentAmmunition.Amount / CurrentWeapon.AmmoClip.AMMO_USE_PER_BULLET) + "";
                AmmoClip.transform.GetComponent<Text>().text = CurrentWeapon.AmmoClip.GetCurrentAmmo() + "";
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
            PlayerAmmo.transform.GetComponent<Text>().text = (eventData.currentAmmunition.Amount / CurrentWeapon.AmmoClip.AMMO_USE_PER_BULLET) + "";
        }
    }
}
