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
    [SerializeField] private GlobalLight _globalLight;
    [SerializeField] private EndCanvas _endCanvas;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private string _bannerId;
    [SerializeField] private string _rewardId;
    private LevelParameters[] _currentLevelParameters;
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
        //_adMob = new AdMobManager(/*personalization: _saveData.PersonalizationAds, adForChild: _saveData.AdForChild, bannerId:_bannerId,rewardId: _rewardId*/);
        _adMob = null;
        //_mainMenu.gameObject.SetActive(true);
        _starManager.SetStartParameters(_saveData, _mainMenu, _adMob);
    }

    private void Start()
    {
        MainMenu();
    }

    private void MainMenu()
    {
        //_mainMenu.gameObject.SetActive(true);
       //Time.timeScale = 0;
        _mainMenu.StartMainMenu(_saveData.UserName);
        if (_adMob!=null) _adMob.ShowBanner();
        _currentLevelParameters = _levelParameters;
    }

    private void LoadingLevel(int level)
    {
        _starManager.StarChanged(-1);
        //Time.timeScale = 1;
        _level = level;
        SetCameraParametrs();
        SetWindParameters();
        SetGlobalLight();
        SetLevelMapParameters();
        SetPlayerParameters();
        if (_adMob != null)  _adMob.HideBanner();
    }


    private void SetCameraParametrs()
    {
        _camera.SetStartParameters();
    }

    private void SetLevelMapParameters()
    {
        _levelMap.SetCamera(_camera);
        _levelMap.CreateNewLevel(_currentLevelParameters[_level].NumberLevelParts, _currentLevelParameters[_level].StartPartLevel, _currentLevelParameters[_level].PartLevel, _currentLevelParameters[_level].FinishPartLevel);
    }
    private void SetPlayerParameters()
    {
        _player.SetStartParameters(_currentLevelParameters[_level].CurrentTimesDay);
        _player.gameObject.SetActive(true);
    }
    private void SetWindParameters()
    {
        _wind.SetStartParameters(_currentLevelParameters[_level].MaxWindForce);
        _wind.enabled = true;
    }
    private void SetGlobalLight()
    {
        _globalLight.SetLight(_currentLevelParameters[_level].CurrentTimesDay);
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
        _levelMap.DestroyLevel();
        MainMenu(); 
    }
    private void PlayerGameOver()
    {
        _endCanvas.gameObject.SetActive(true);
        _endCanvas.GameOver(_starManager.GetStarQuantity()>0);
        _levelMap.StopCamera();
        if (_adMob != null) _adMob.ShowBanner();
    }

    private void PlayerEndLevel()
    {
        _endCanvas.gameObject.SetActive(true);
        _endCanvas.EndLevel(_starManager.GetStarQuantity() > 0,_level< _currentLevelParameters.Length-1);
        if (_adMob != null) _adMob.ShowBanner();
    }

    private void EndCanvasRepeat()
    {
        LoadingLevel(_level);
        if (_adMob != null) _adMob.HideBanner();
    }

    private void EndCanvasExit()
    {
        MainMenu();
        if (_adMob != null) _adMob.HideBanner();
    }
    private void EndCanvasNextLevel()
    {
        _level++;
        LoadingLevel(_level);
        if (_adMob != null) _adMob.HideBanner();

    }
    private void CreateLevel(LevelParameters levelParameters)
    {
        _currentLevelParameters = new LevelParameters[] {levelParameters };
        LoadingLevel(0);

    }

    private void OnEnable()
    {
        _mainMenu.PlayLevel += LoadingLevel;
        _mainMenu.MainMenuStartingParametersSet += MainMenuStartingParametersSet;
        _mainMenu.EventCreateLevel += CreateLevel;
        _player.EndLevel += PlayerEndLevel;
        _player.GameOver += PlayerGameOver;
        _player.Exit += PlayerExit;
        _endCanvas.EndCanvasExit += EndCanvasExit;
        _endCanvas.EndCanvasReiterate += EndCanvasRepeat;
        _endCanvas.EndCanvasNextLevel += EndCanvasNextLevel;
    }


    private void OnDisable()
    {
        _mainMenu.PlayLevel -= LoadingLevel;
        _mainMenu.MainMenuStartingParametersSet -= MainMenuStartingParametersSet;
        _mainMenu.EventCreateLevel -= CreateLevel;
        _player.EndLevel -= PlayerEndLevel;
        _player.GameOver -= PlayerGameOver;
        _player.Exit -= PlayerExit;
        _endCanvas.EndCanvasExit -= EndCanvasExit;
        _endCanvas.EndCanvasReiterate -= EndCanvasRepeat;
        _endCanvas.EndCanvasNextLevel -= EndCanvasNextLevel;
    }

    private void OnDestroy()
    {
        _saveLoadSystem.Save(_saveData);
    }
}
