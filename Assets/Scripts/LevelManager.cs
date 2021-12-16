using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppodealAds.Unity.Api;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LevelMap _levelMap;
    [SerializeField] private CameraMover _camera;
    [SerializeField] private LevelParameters[] _levelParameters;
    [SerializeField] private Wind _wind;
    [SerializeField] private EndCanvas _endCanvas;
    [SerializeField] private MainMenu _mainMenu;
    private int _level;
    private SaveLoadSystem _saveLoadSystem;
    private SaveData _saveData;
    private const string APPODEAL_APP_KEY = "9f1fcdef38d53a7a17ea35b5b0249b46b439473e47d6f052";

    private void Start()
    {
        Appodeal.initialize(APPODEAL_APP_KEY, Appodeal.BANNER, true);
        _saveLoadSystem = new SaveLoadSystem();
        _saveData = new SaveData();
        _saveData=_saveLoadSystem.Load();
        MainMenu();
    }

    private void MainMenu()
    {
        Appodeal.show(Appodeal.BANNER_TOP);
        Time.timeScale = 0;
        _mainMenu.PlayLevel += LoadingLevel;
        _mainMenu.MainMenuUserNameAgeSet += MainMenuUserNameAgeSet;
        _mainMenu.StartMainMenu(_saveData._userName);

    }

    private void MainMenuUserNameAgeSet(string userName, int userAge)
    {
        _saveData._userName=userName;
        _saveData._userAge = userAge;
        _saveLoadSystem.Save(_saveData);
    }

    private void LoadingLevel(int level)
    {
        Appodeal.hide(Appodeal.BANNER);
        _mainMenu.PlayLevel -= LoadingLevel;
        Time.timeScale = 1;
        _level = level;
        SetLevelMapParameters();
        SetPlayerParameters();
        SetWindParameters();
        SetCameraParametrs();
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
        _player.EndLevel += PlayerEndLevel;
        _player.GameOver += PlayerGameOver;
        _player.Exit += PlayerExit;
        _player.gameObject.SetActive(true);
    }

    private void PlayerExit()
    {
        MainMenu(); 
    }

    private void SetWindParameters()
    {
        _wind.SetStartParameters(_levelParameters[_level].MaxWindForce);
        _wind.enabled = true;
    }

    private void PlayerGameOver()
    {
        Appodeal.show(Appodeal.BANNER_TOP);
        StartCoroutine(_endCanvas.GameOver());
        _endCanvas.EndCanvasExit += EndCanvasExit;
        _endCanvas.EndCanvasReiterate += EndCanvasReiterate;
        _levelMap.StopCamera();
    }

    private void PlayerEndLevel()
    {
        Appodeal.show(Appodeal.BANNER_TOP);
        StartCoroutine(_endCanvas.EndLevel());
        _endCanvas.EndCanvasExit += EndCanvasExit;
        _endCanvas.EndCanvasReiterate += EndCanvasReiterate;
    }

    private void EndCanvasReiterate()
    {

        _endCanvas.EndCanvasExit -= EndCanvasExit;
        _endCanvas.EndCanvasReiterate -= EndCanvasReiterate;
        LoadingLevel(_level);
    }

    private void EndCanvasExit()
    {
        _endCanvas.EndCanvasExit -= EndCanvasExit;
        _endCanvas.EndCanvasReiterate -= EndCanvasReiterate;
        MainMenu();
    }

    private void OnDisable()
    {
        _player.EndLevel -= PlayerEndLevel;
        _player.GameOver -= PlayerGameOver;
        _player.Exit -= PlayerExit;
    }
}
