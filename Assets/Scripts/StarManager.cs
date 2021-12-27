using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StarManager : MonoBehaviour
{
    [SerializeField] private int _maxStar;
    private int _starQuantity;
    private MainMenu _mainMenu;
    private SaveData _saveData;
    private AdMobManager _adMob;
    private int _timeToStar;

    public void SetStartParameters(SaveData saveData, MainMenu mainMenu, AdMobManager adMob)
    {
        _saveData = saveData;
        _mainMenu = mainMenu;
        _adMob = adMob;
        _starQuantity = _saveData.StarQuantity;
        _mainMenu.AddStarButton += AddStarButton;
        _adMob.AddRewardedAdListener(RewardeStatus);
        _mainMenu.StarQuantityChanged(_starQuantity);
        StartCoroutine(WaitingTimeToStar());
    }
    public void StarChanged(int changed)
    {
        _starQuantity+= changed;
        if (_starQuantity > _maxStar) _starQuantity = _maxStar;
        else if (_starQuantity<0) _starQuantity = 0;
        _mainMenu.StarQuantityChanged(_starQuantity);
    }

    private void AddStarButton()
    {
        _adMob.ShowRewardAd(RewardEvent);
    }

    private void RewardeStatus(bool rewardeAdLoad)
    {
        _mainMenu.AddStarButtonActive(rewardeAdLoad);
    }

    private void TimeToStarChanged()
    {
        if (_timeToStar > 0) _timeToStar--;
        else {
            _timeToStar = 60;
            StarChanged(1);
        }

        _mainMenu.TimeToStar(_timeToStar);
    }

    private IEnumerator WaitingTimeToStar()
    {
        while (_starQuantity<_maxStar)
        {
        yield return new WaitForSeconds(60);
        TimeToStarChanged();
        }
    }

    private void RewardEvent()
    {
        StarChanged(1);
        _adMob.RewardEvent -= RewardEvent;
    }
   
    private void OnDisable()
    {
        _mainMenu.AddStarButton -= AddStarButton;
        _adMob.RewardeStatus -= RewardeStatus;
        _saveData.StarQuantity = _starQuantity;
    }
}
