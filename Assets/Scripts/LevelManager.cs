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
    private int _level;
    public void LoadingLevel(int level)
    {
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
    }

    private void OnDisable()
    {
        _player.EndLevel -= EndLevel;
        _player.GameOver -= GameOver;
    }
}
