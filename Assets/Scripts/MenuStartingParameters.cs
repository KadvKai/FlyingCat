using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MenuStartingParameters : MonoBehaviour
{
    [SerializeField] GameObject _menyUserNameAge;
    [SerializeField] GameObject _menuPersonalizationAds;
    [SerializeField] InputField _userName;
    [SerializeField] InputField _userAge;
    public event UnityAction<string, int,bool> UserNameAgeSet;
    public void StartMenuUserNameAge()
    {
        _menyUserNameAge.SetActive(true);
    }

    public void ButtonUserNameAgeOK()
    {
        _menyUserNameAge.SetActive(false);
        _menuPersonalizationAds.SetActive(true);
    }

    public void ButtonPersonalizationAdsOK()
    {
        UserNameAgeSet?.Invoke(_userName.text, int.Parse(_userAge.text),true);
        _menuPersonalizationAds.SetActive(false);
    }
    public void ButtonPersonalizationAdsNO()
    {
        UserNameAgeSet?.Invoke(_userName.text, int.Parse(_userAge.text), true);
        _menuPersonalizationAds.SetActive(false);
    }
    public void ButtonPrivacyPolicy()
    {
        Application.OpenURL("https://kadvkai.github.io/KadvKai/PrivacyPolicy.html");
    }

}
