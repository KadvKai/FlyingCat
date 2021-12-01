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
    private GameParameters gameParameters;
    void Start()
    {
        Time.timeScale = 0;
        _menyUserNameAge.SetActive(false);
        gameParameters= GetComponent<GameParameters>();
        if (gameParameters.HaveSaveFile())
        {
            UserNameChanged();
        }
        else UserNameAge();

    }
    public void UserNameChanged()
    {
        _userName.text = gameParameters.GetUserName();
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
