using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelParameters", menuName = "LevelParameters", order = 51)]

public class LevelParameters : ScriptableObject
{
    public enum TimesDay { Day, Evening, Night, Morning };
    [SerializeField] private TimesDay _currentTimesDay;
    [SerializeField] private int _numberLevelParts;
    [SerializeField] private GameObject _startPartLevel;
    [SerializeField] private GameObject[] _partLevel;
    [SerializeField] private GameObject _finishPartLevel;
    [SerializeField] private int _maxWindForce;


    public int NumberLevelParts => _numberLevelParts;
    public GameObject StartPartLevel =>_startPartLevel;
    public GameObject[] PartLevel => _partLevel;
    public GameObject FinishPartLevel =>_finishPartLevel;
    public int MaxWindForce => _maxWindForce;
    public TimesDay CurrentTimesDay => _currentTimesDay;
    public void SetParameters(TimesDay timesDay, int numberLevelParts, int maxWindForce)
    {
        _currentTimesDay = timesDay;
        _numberLevelParts = numberLevelParts;
        _maxWindForce = maxWindForce;
    }
}
