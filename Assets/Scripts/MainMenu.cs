using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(MenuUserNameAge))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text _userName;
    [SerializeField] TMP_Text _starQuantity;
    [SerializeField] TMP_Text _versionText;
    [SerializeField] GameObject _mainMenuPanel;
    private MenuUserNameAge _menyUserNameAge;
    public event UnityAction<int> PlayLevel;
    public event UnityAction<string, int> MainMenuUserNameAgeSet;
    public void StartMainMenu(string userName, int starQuantity)
    {
        _versionText.text= Application.version;
        if (userName!=null)
        {
            _userName.text = userName;
            _starQuantity.text = starQuantity.ToString();
        }
        else UserNameAge();

    }
    private void UserNameAge()
    {
        _menyUserNameAge = GetComponent<MenuUserNameAge>();
        _menyUserNameAge.UserNameAgeSet += UserNameAgeSet;
        _menyUserNameAge.StartMenuUserNameAge();
    }

    private void UserNameAgeSet(string name, int age)
    {
        _userName.text = name;
        _menyUserNameAge.UserNameAgeSet -= UserNameAgeSet;
        MainMenuUserNameAgeSet?.Invoke(name,age);
    }

    public void Play()
    {
        //_mainMenuPanel.SetActive(false);
        gameObject.SetActive(false);
        PlayLevel?.Invoke(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
