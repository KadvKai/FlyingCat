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
    [SerializeField] private Canvas _background;
    private int _level;
    public void LoadingLevel(int level)
    {
        _level = level;
        SetLevelMapParameters();
        SetPlayerParameters();
        SetWindParameters();
        SetBackgroundParameters();
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
        _wind.WindChanged += _player.WindChanged;
        _player.EndLevel += EndLevel;
        _player.gameObject.SetActive(true);
    }

    private void SetWindParameters()
    {
        _wind.SetStartParameters(_camera.GetComponent<Camera>(), _levelParameters[_level].MaxWindForce);
        _wind.gameObject.SetActive(true);
    }

    private void SetBackgroundParameters()
    {
        var clouds=_background.GetComponentsInChildren<ParallaxCloud>();
        foreach (var cloud in clouds)
        {
            _wind.WindChanged += cloud.WindChanged;
        }
    }

    private void EndLevel()
    {
        _wind.WindChanged -= _player.WindChanged;
        var clouds = _background.GetComponentsInChildren<ParallaxCloud>();
        foreach (var cloud in clouds)
        {
            _wind.WindChanged -= cloud.WindChanged;
        }
    }

}
