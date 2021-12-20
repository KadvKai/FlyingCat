using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LevelMap _levelMap;
    [SerializeField] private CameraMover _camera;
    [SerializeField] private LevelParameters[] _levelParameters;
    [SerializeField] private Wind _wind;
    [SerializeField] private EndCanvas _endCanvas;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private string _bannerId;
    private int _level;
    private SaveLoadSystem _saveLoadSystem;
    private SaveData _saveData;
    private AdMobManager _adMob;

    private void Awake()
    {
        _adMob = new AdMobManager(personalization:false,adForChild:true,bannerId:_bannerId);
        _saveLoadSystem = new SaveLoadSystem();
        _saveData = new SaveData();
        _saveData=_saveLoadSystem.Load();
    }

    private void Start()
    {
        MainMenu();
    }

    private void MainMenu()
    {
        _mainMenu.gameObject.SetActive(true);
       Time.timeScale = 0;
        _mainMenu.StartMainMenu(_saveData._userName);
        _adMob.ShowBanner();
    }

    private void MainMenuUserNameAgeSet(string userName, int userAge)
    {
        _saveData._userName=userName;
        _saveData._userAge = userAge;
        _saveLoadSystem.Save(_saveData);
    }

    private void LoadingLevel(int level)
    {

        Time.timeScale = 1;
        _level = level;
        SetLevelMapParameters();
        SetPlayerParameters();
        SetWindParameters();
        SetCameraParametrs();
        _adMob.HideBanner();
    }

    private void SetCameraParametrs()
    {
        _camera.SetStartParameters();
    }

    private void SetLevelMapParameters()
    {
        _levelMap.SetCameraTransform(_camera);
        _levelMap.CreateNewLevel(_levelParameters[_level].NumberLevelParts, _levelParameters[_level].StartPartLevel, _levelParameters[_level].PartLevel, _levelParameters[_level].FinishPartLevel);
        StartCoroutine(_levelMap.LevelController());
        _levelMap.gameObject.SetActive(true);
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

    private void PlayerExit()
    {
        MainMenu(); 
    }
    private void PlayerGameOver()
    {
        _endCanvas.gameObject.SetActive(true);
        StartCoroutine(_endCanvas.GameOver());
        _levelMap.StopCamera();
        _adMob.ShowBanner();
    }

    private void PlayerEndLevel()
    {
        _endCanvas.gameObject.SetActive(true);
        StartCoroutine(_endCanvas.EndLevel());
        _adMob.ShowBanner();
    }

    private void EndCanvasReiterate()
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
        _mainMenu.MainMenuUserNameAgeSet += MainMenuUserNameAgeSet;
        _player.EndLevel += PlayerEndLevel;
        _player.GameOver += PlayerGameOver;
        _player.Exit += PlayerExit;
        _endCanvas.EndCanvasExit += EndCanvasExit;
        _endCanvas.EndCanvasReiterate += EndCanvasReiterate;
        
    }

    private void OnDisable()
    {
        _mainMenu.PlayLevel -= LoadingLevel;
        _mainMenu.MainMenuUserNameAgeSet -= MainMenuUserNameAgeSet;
        _player.EndLevel -= PlayerEndLevel;
        _player.GameOver -= PlayerGameOver;
        _player.Exit -= PlayerExit;
        _endCanvas.EndCanvasExit -= EndCanvasExit;
        _endCanvas.EndCanvasReiterate -= EndCanvasReiterate;
    }
}
