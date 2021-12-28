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
    private Coroutine _coroutineWaitingTimeToStar;

    public void SetStartParameters(SaveData saveData, MainMenu mainMenu, AdMobManager adMob)
    {
        _saveData = saveData;
        _mainMenu = mainMenu;
        _adMob = adMob;
        _starQuantity = _saveData.StarQuantity;
        _timeToStar = _saveData.TimeToStar;
        _coroutineWaitingTimeToStar = StartCoroutine(WaitingTimeToStar());
        _mainMenu.AddStarButton += AddStarButton;
        _adMob.AddRewardedAdListener(RewardeStatus);
        CountingStarTimeToStar();
    }

    public void CountingStarTimeToStar()
    {
        var timeChange = DateTime.UtcNow - _saveData.ExitTime;
        int minutesChange = (int)timeChange.TotalMinutes;
        minutesChange = Mathf.Clamp(minutesChange,0,7*24*60);

        TimeToStarChanged(minutesChange % 60);
        StarChanged(minutesChange/60);
    }
    public void StarChanged(int change)
    {
        _starQuantity+= change;
        if (_starQuantity > _maxStar) _starQuantity = _maxStar;

        if (_starQuantity== _maxStar)
        {
            if (_coroutineWaitingTimeToStar != null) StopCoroutine(_coroutineWaitingTimeToStar);
            _timeToStar = 60;
            _mainMenu.TimeToStar(0);
        }
        else
        {
            if(_coroutineWaitingTimeToStar == null) _coroutineWaitingTimeToStar= StartCoroutine(WaitingTimeToStar());
        }
        if (_starQuantity < 0) _starQuantity = 0;
        _mainMenu.StarQuantityChanged(_starQuantity);
    }

    private void AddStarButton()
    {
        _adMob.ShowRewardAd(RewardEvent);
    }
    private void RewardEvent()
    {
        StarChanged(1);
        _adMob.RewardEvent -= RewardEvent;
    }

    private void RewardeStatus(bool rewardeAdLoad)
    {
        _mainMenu.AddStarButtonActive(rewardeAdLoad);
    }

    private void TimeToStarChanged(int change)
    {
        if (change < 0) change = 0;
        _timeToStar -= change;
        if (_timeToStar <= 0) 
         {
            _timeToStar += 60;
            StarChanged(1);
        }
        _mainMenu.TimeToStar(_timeToStar);
    }

    private IEnumerator WaitingTimeToStar()
    {
        while (_starQuantity<_maxStar)
        {
        yield return new WaitForSeconds(60);
        TimeToStarChanged(1);
        }
    }

   
    private void OnDisable()
    {
        _mainMenu.AddStarButton -= AddStarButton;
        _adMob.RewardeStatus -= RewardeStatus;
        _saveData.StarQuantity = _starQuantity;
        _saveData.TimeToStar = _timeToStar;
    }
}
