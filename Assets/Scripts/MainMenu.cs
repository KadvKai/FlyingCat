using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameParameters))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text _userName;
    [SerializeField] GameObject _menyUserNameAge;
    [SerializeField] LevelManager _levelManager;
    private GameParameters _gameParameters;
    void Start()
    {
        Time.timeScale = 0;
        _menyUserNameAge.SetActive(false);
        _gameParameters= GetComponent<GameParameters>();
        if (_gameParameters.HaveSaveFile())
        {
            UserNameChanged();
        }
        else UserNameAge();

    }
    public void UserNameChanged()
    {
        _userName.text = _gameParameters.GetUserName();
    }
    private void UserNameAge()
    {
        _menyUserNameAge.SetActive(true);
    }

    public void Play()
    {
        gameObject.SetActive(false);
        _levelManager.LoadingLevel(0);
        Time.timeScale = 1;
    }
}
