using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StarManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LevelMap _levelMap;
    [SerializeField] private CameraMover _camera;
    [SerializeField] private LevelParameters[] _levelParameters;
    [SerializeField] private Wind _wind;
    [SerializeField] private EndCanvas _endCanvas;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private string _bannerId;
    [SerializeField] private string _rewardId;
    private int _level;
    private SaveLoadSystem _saveLoadSystem;
    private SaveData _saveData;
    private AdMobManager _adMob;
    private StarManager _starManager;

    private void Awake()
    {
        _starManager = GetComponent<StarManager>();
        _saveLoadSystem = new SaveLoadSystem();
        _saveData = new SaveData();
        _saveData=_saveLoadSystem.Load();
        _adMob = new AdMobManager(personalization: _saveData.PersonalizationAds, adForChild: _saveData.AdForChild, bannerId:_bannerId,rewardId: _rewardId);
        _starManager.SetStartParameters(_saveData, _mainMenu, _adMob);
    }

    private void Start()
    {
        MainMenu();
    }

    private void MainMenu()
    {
        _mainMenu.gameObject.SetActive(true);
       //Time.timeScale = 0;
        _mainMenu.StartMainMenu(_saveData.UserName);
        _adMob.ShowBanner();
    }

    private void LoadingLevel(int level)
    {
        _starManager.StarChanged(-1);
        //Time.timeScale = 1;
        _level = level;
        SetCameraParametrs();
        SetWindParameters();
        SetLevelMapParameters();
        SetPlayerParameters();
        _adMob.HideBanner();
    }

    private void SetCameraParametrs()
    {
        _camera.SetStartParameters();
    }

    private void SetLevelMapParameters()
    {
        _levelMap.SetCamera(_camera);
        _levelMap.CreateNewLevel(_levelParameters[_level].NumberLevelParts, _levelParameters[_level].StartPartLevel, _levelParameters[_level].PartLevel, _levelParameters[_level].FinishPartLevel);
    }
    private void SetPlayerParameters()
    {
        _player.SetStartParameters();
        _player.gameObject.SetActive(true);
    }
    private void SetWindParameters()
    {
        _wind.SetStartParameters(_levelParameters[_level].MaxWindForce);
        _wind.enabled = true;
    }
    private void MainMenuStartingParametersSet(string userName, int userAge,bool personalizationAds)
    {
        _saveData.UserName=userName;
        if (userAge < 13) _saveData.AdForChild = true;
        else _saveData.AdForChild = false;
        _saveData.PersonalizationAds = personalizationAds;
    }

    private void PlayerExit()
    {
        MainMenu(); 
    }
    private void PlayerGameOver()
    {
        _endCanvas.gameObject.SetActive(true);
        _endCanvas.GameOver(_starManager.GetStarQuantity()>0);
        _levelMap.StopCamera();
        _adMob.ShowBanner();
    }

    private void PlayerEndLevel()
    {
        _endCanvas.gameObject.SetActive(true);
        _endCanvas.EndLevel(_starManager.GetStarQuantity() > 0);
        _adMob.ShowBanner();
    }

    private void EndCanvasRepeat()
    {
        LoadingLevel(_level);
        _adMob.HideBanner();
    }

    private void EndCanvasExit()
    {
        MainMenu();
        _adMob.HideBanner();
    }
   
    private void OnEnable()
    {
        _mainMenu.PlayLevel += LoadingLevel;
        _mainMenu.MainMenuStartingParametersSet += MainMenuStartingParametersSet;
        _player.EndLevel += PlayerEndLevel;
        _player.GameOver += PlayerGameOver;
        _player.Exit += PlayerExit;
        _endCanvas.EndCanvasExit += EndCanvasExit;
        _endCanvas.EndCanvasReiterate += EndCanvasRepeat;
    }


    private void OnDisable()
    {
        _mainMenu.PlayLevel -= LoadingLevel;
        _mainMenu.MainMenuStartingParametersSet -= MainMenuStartingParametersSet;
        _player.EndLevel -= PlayerEndLevel;
        _player.GameOver -= PlayerGameOver;
        _player.Exit -= PlayerExit;
        _endCanvas.EndCanvasExit -= EndCanvasExit;
        _endCanvas.EndCanvasReiterate -= EndCanvasRepeat;
    }

    private void OnDestroy()
    {
        _saveLoadSystem.Save(_saveData);
    }
}
