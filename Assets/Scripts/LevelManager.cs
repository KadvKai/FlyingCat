using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameParameters))]
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
    private GameParameters _gameParameters;

    private void Start()
    {
        _gameParameters = GetComponent<GameParameters>();
        MainMenu();
    }

    private void MainMenu()
    {
        string UserName;
        if (_gameParameters.HaveSaveFile())
        {
            UserName= _gameParameters.GetUserName();
        }
        else UserName=null;
        Time.timeScale = 0;
        _mainMenu.PlayLevel += LoadingLevel;
        _mainMenu.StartMainMenu(UserName);

    }

    private void LoadingLevel(int level)
    {
        _mainMenu.PlayLevel -= LoadingLevel;
        Time.timeScale = 1;
        _level = level;
        SetLevelMapParameters();
        SetPlayerParameters();
        SetWindParameters();
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
        _player.EndLevel += EndLevel;
        _player.GameOver += GameOver;
        _player.gameObject.SetActive(true);
    }


    private void SetWindParameters()
    {
        _wind.SetStartParameters(_levelParameters[_level].MaxWindForce);
        _wind.enabled = true;
    }

    private void GameOver()
    {
        StartCoroutine(_endCanvas.GameOver());
        _levelMap.StopCamera();
    }
    private void EndLevel()
    {
        StartCoroutine(_endCanvas.EndLevel());
        _endCanvas.EndCanvasExit += EndCanvasExit;
        _endCanvas.EndCanvasReiterate += EndCanvasReiterate;
    }

    private void EndCanvasReiterate()
    {
        _endCanvas.EndCanvasReiterate -= EndCanvasReiterate;
    }

    private void EndCanvasExit()
    {
        _endCanvas.EndCanvasExit -= EndCanvasExit;
    }

    private void OnDisable()
    {
        _player.EndLevel -= EndLevel;
        _player.GameOver -= GameOver;
    }
}
