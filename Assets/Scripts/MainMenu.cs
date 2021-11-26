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
    [SerializeField] TMP_Text userName;
    [SerializeField] GameObject menyUserNameAge;

    private GameParameters gameParameters;
    void Start()
    {
        menyUserNameAge.SetActive(false);
        gameParameters= GetComponent<GameParameters>();
        if (gameParameters.HaveSaveFile())
        {
            UserNameChanged();
        }
        else UserNameAge();

    }
    public void UserNameChanged()
    {
        userName.text = gameParameters.GetUserName();
    }
    private void UserNameAge()
    {
        menyUserNameAge.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
