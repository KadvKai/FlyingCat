using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelParameters", menuName = "LevelParameters", order = 51)]

public class LevelParameters : ScriptableObject
{
    [SerializeField] private int _numberLevelParts;
    [SerializeField] private GameObject _startPartLevel;
    [SerializeField] private GameObject[] _partLevel;
    [SerializeField] private GameObject _finishPartLevel;


    public int NumberLevelParts => _numberLevelParts;
    public GameObject StartPartLevel =>_startPartLevel;
    public GameObject[] PartLevel => _partLevel;
    public GameObject FinishPartLevel =>_finishPartLevel;
}
