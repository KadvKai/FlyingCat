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
    [SerializeField] TMP_Text _versionText;
    [SerializeField] GameObject _mainMenuPanel;
    private MenuUserNameAge _menyUserNameAge;
    public event UnityAction<int> PlayLevel;
    public event UnityAction<string, int> MainMenuUserNameAgeSet;
    public void StartMainMenu(string userName)
    {
        _versionText.text= Application.version;
        if (userName!=null)
        {
            MainMenuPanel(userName);
        }
        else UserNameAge();

    }
  
    public void MainMenuPanel(string userName)
    {
        //_mainMenuPanel.SetActive(true);
        _userName.text = userName;
    }
    private void UserNameAge()
    {
        _menyUserNameAge = GetComponent<MenuUserNameAge>();
        _menyUserNameAge.UserNameAgeSet += UserNameAgeSet;
        _menyUserNameAge.StartMenuUserNameAge();
    }

    private void UserNameAgeSet(string name, int age)
    {
        MainMenuPanel(name);
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
