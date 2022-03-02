using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    [SerializeField] LevelParameters[] _typeLevelParameters;
    [SerializeField] GameObject _createLevelPanel;
    [SerializeField] private TMP_Dropdown _timesDayDropdown;
    [SerializeField] private TMP_Dropdown _typeLevelDropdown;
    private LevelParameters.TimesDay _timesDay;
    private int _numberLevelParts;
    [Range(0, 3)] private int _maxWindForce;
    private LevelParameters _levelParameters;

    public void StartCreateLevel()
    {
        _createLevelPanel.SetActive(true);

        _timesDayDropdown.AddOptions(TimesDayOptions());
        _typeLevelDropdown.AddOptions(TypeLeveOptions());
    }

    private List<string> TimesDayOptions()
    {
        var options= new List<string>();
        foreach (var item in Enum.GetValues(typeof(LevelParameters.TimesDay)))
        {
            options.Add(item.ToString());
        }
        
        return options;
    }

    private List<string> TypeLeveOptions()
    {
        var options = new List<string>();
        foreach (var item in _typeLevelParameters)
        {
            options.Add(item.name);
        }
        return options;
    }
}
