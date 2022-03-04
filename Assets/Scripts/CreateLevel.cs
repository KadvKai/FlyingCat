using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreateLevel : MonoBehaviour
{
    [SerializeField] LevelParameters[] _typeLevelParameters;
    [SerializeField] GameObject _createLevelPanel;
    [SerializeField] private TMP_Dropdown _timesDayDropdown;
    [SerializeField] private TMP_Dropdown _typeLevelDropdown;
    [SerializeField] private Slider _levelLengthSlider;
    [SerializeField] private Slider _windForceSlider;
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
        var options = new List<string>
        {
            "Random"
        };
        foreach (var item in Enum.GetValues(typeof(LevelParameters.TimesDay)))
        {
            options.Add(item.ToString());
        }
        return options;
    }

    private List<string> TypeLeveOptions()
    {
        var options = new List<string>
        {
            "Random"
        };
        foreach (var item in _typeLevelParameters)
        {
            options.Add(item.name);
        }
        return options;
    }
    public void OKButton()
    {
        if (_typeLevelDropdown.value==0)
        {
            _typeLevelDropdown.value = Random.Range(1,_typeLevelDropdown.options.Count);
        }
        if (_timesDayDropdown.value == 0)
        {
            _timesDayDropdown.value = Random.Range(1, _timesDayDropdown.options.Count);
        }
        _levelParameters = _typeLevelParameters[_typeLevelDropdown.value-1];
        _levelParameters.SetParameters((LevelParameters.TimesDay)(_timesDayDropdown.value-1), (int)_levelLengthSlider.value, (int)_windForceSlider.value);
       CancelButton();
    }

    public void CancelButton()
    {
        _createLevelPanel.SetActive(false);
        EventCreateLevel?.Invoke(_levelParameters);
    }

    public void GetElements(out TMP_Dropdown timesDayDropdown, out TMP_Dropdown typeLevelDropdown)
    {
        timesDayDropdown = _timesDayDropdown;
        typeLevelDropdown = _typeLevelDropdown;
    }

}
