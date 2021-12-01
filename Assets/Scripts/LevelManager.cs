using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelMap _levelMap;
    [SerializeField] private CameraMover _camera;
    [SerializeField] private LevelParameters[] _levelParameters;
    public void LoadingLevel(int level)
    {
        _levelMap.gameObject.SetActive(true);
        _levelMap.SetCameraTransform(_camera);
        _levelMap.CreateNewLevel(_levelParameters[level].NumberLevelParts, _levelParameters[level].StartPartLevel, _levelParameters[level].PartLevel, _levelParameters[level].FinishPartLevel);
        StartCoroutine(_levelMap.LevelController());
    }
}
