using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CreateLevel))]
public class MainMunuScreen : MonoBehaviour
{
    [SerializeField] GameObject[] _levelPart;
    [SerializeField] private GlobalLight _globalLight;
    private void Awake()
    {
        var createLevel = GetComponent<CreateLevel>();

        createLevel.GetElements(out TMP_Dropdown timesDayDropdown, out TMP_Dropdown typeLevelDropdown);
        timesDayDropdown.onValueChanged.AddListener(TimesDayChanged);
        typeLevelDropdown.onValueChanged.AddListener(TypeLevelChanged);
    }

    private void OnEnable()
    {
        _levelPart[0].SetActive(true);
    }

    private void OnDisable()
    {

        foreach (var part in _levelPart)
        {
            if (part != null)
            {
                part.SetActive(false);
            }
        }
    }


    private void TimesDayChanged(int index)
    {
        if (index != 0)
        {
            _globalLight.SetLight((LevelParameters.TimesDay)(index - 1));
        };
    }
    private void TypeLevelChanged(int index)
    {
        if (index != 0)
        {

            for (int i = 0; i < _levelPart.Length; i++)
            {
                if (i == index - 1) _levelPart[i].SetActive(true);
                else _levelPart[i].SetActive(false);
            }
        }
    }
}
