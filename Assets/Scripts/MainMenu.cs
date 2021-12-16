using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using AppodealAds.Unity.Api;

[RequireComponent(typeof(MenuUserNameAge))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text _userName;
    [SerializeField] GameObject _mainMenuPanel;
    private MenuUserNameAge _menyUserNameAge;
    public event UnityAction<int> PlayLevel;
    public event UnityAction<string, int> MainMenuUserNameAgeSet;
    public void StartMainMenu(string userName)
    {
        if (userName!=null)
        {
            MainMenuPanel(userName);
        }
        else UserNameAge();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }
    public void MainMenuPanel(string userName)
    {
        _mainMenuPanel.SetActive(true);
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
        _mainMenuPanel.SetActive(false);
        PlayLevel?.Invoke(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
