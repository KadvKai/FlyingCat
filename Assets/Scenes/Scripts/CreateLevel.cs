using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreateLevel : MonoBehaviour
{
    [SerializeField] LevelParameters[] _typeLevelParameters;
    [SerializeField] GameObject _createLevelPanel;
    [SerializeField] private TMP_Dropdown _timesDayDropdown;
    [SerializeField] private TMP_Dropdown _typeLevelDropdown;
    [SerializeField] private Slider _levelLengthSlider;
    [SerializeField] private Slider _windForceSlider;
    private LevelParameters.TimesDay _timesDay;
    private int _numberLevelParts;
    private int _maxWindForce;
    private LevelParameters _levelParameters;
    public event UnityAction<LevelParameters> EventCreateLevel;

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
    public void OKButton()
    {
        _levelParameters = _typeLevelParameters[_typeLevelDropdown.value];
        _levelParameters.SetParameters((LevelParameters.TimesDay)_timesDayDropdown.value, (int)_levelLengthSlider.value, (int)_windForceSlider.value);
       CancelButton();
    }

    public void CancelButton()
    {
        _createLevelPanel.SetActive(false);
        EventCreateLevel?.Invoke(_levelParameters);
    }


}
