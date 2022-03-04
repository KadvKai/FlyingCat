using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(MenuStartingParameters))]
[RequireComponent(typeof(CreateLevel))]
[RequireComponent(typeof(MainMunuScreen))]

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text _userName;
    [SerializeField] TMP_Text _starQuantity;
    [SerializeField] TMP_Text _timeToStar;
    [SerializeField] TMP_Text _versionText;
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _optionsPanel;
    [SerializeField] Button _addStar;
    [SerializeField] Button _playButton;
    private MenuStartingParameters _menyUserNameAge;
    private CreateLevel _createLevel;
    private MainMunuScreen _mainMunuScreen;
    public event UnityAction<int> PlayLevel;
    public event UnityAction<string, int,bool> MainMenuStartingParametersSet;
    public event UnityAction AddStarButton;
    public event UnityAction<LevelParameters> EventCreateLevel;

    public void StartMainMenu(string userName)
    {
        _mainMunuScreen=GetComponent<MainMunuScreen>();
        _mainMunuScreen.enabled = true;
        _versionText.text= Application.version;
        if (userName!=null)
        {
            _userName.text = userName;
        }
        else StartingParameters();
        StartCoroutine(GetComponent<LanguageManager>().StartLanguageManager());
        _createLevel = GetComponent<CreateLevel>();
    }

    public void StarQuantityChanged(int starQuantity)
    {
        if (starQuantity>0) _playButton.interactable = true;
        else _playButton.interactable = false;
        _starQuantity.text = starQuantity.ToString();
    }
    public void TimeToStar(int time)
    {
        if (time==0)
        {
            _timeToStar.gameObject.SetActive(false);
        }
        else
        {
            if (_timeToStar.gameObject.activeSelf == false)
            {
                _timeToStar.gameObject.SetActive(true);
            }
        _timeToStar.text = time + " min";
        }
    }

    public void AddStarButtonActive(bool active)
    {
        _addStar.interactable=active;
    }

    public void AddStarButtonPress()
    {
        AddStarButton?.Invoke();
    }
    private void StartingParameters()
    {
        _mainMenuPanel.SetActive(false);
        _menyUserNameAge = GetComponent<MenuStartingParameters>();
        _menyUserNameAge.StartingParametersSet += StartingParametersSet;
        _menyUserNameAge.StartMenuUserNameAge();
    }

    private void StartingParametersSet(string name, int age, bool personalizationAds)
    {
        _mainMenuPanel.SetActive(true);
        _userName.text = name;
        _menyUserNameAge.StartingParametersSet -= StartingParametersSet;
        MainMenuStartingParametersSet?.Invoke(name,age, personalizationAds);
    }

    public void PlayButton()
    {
        //_mainMenuPanel.SetActive(false);
        gameObject.SetActive(false);
        PlayLevel?.Invoke(0);
        _mainMunuScreen.enabled = false;
    }

    public void ÑreateButton()
    {
        _mainMenuPanel.SetActive(false);
        _createLevel.EventCreateLevel += CreateLevel;
        _createLevel.StartCreateLevel();
        //PlayLevel?.Invoke(0);
    }


    public void ExitButton()
    {
        Application.Quit();
    }

    public void Options()
    {
        _mainMenuPanel.SetActive(false);
        _optionsPanel.SetActive(true);
    }

    public void Reset()
    {
        _mainMenuPanel.SetActive(true);
        _optionsPanel.SetActive(false);
        StartMainMenu(null);
    }
    private void CreateLevel(LevelParameters levelParameters)
    {
        if (levelParameters==null)
        {
            _mainMenuPanel.SetActive(true);
        }
        else
        {
            EventCreateLevel?.Invoke(levelParameters);
            _mainMunuScreen.enabled = false;

        }
        _createLevel.EventCreateLevel -= CreateLevel;
    }

}
