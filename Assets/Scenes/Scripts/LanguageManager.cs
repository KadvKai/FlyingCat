using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

public class LanguageManager :  MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    public IEnumerator StartLanguageManager()
    {
        yield return LocalizationSettings.InitializationOperation;

        var options = new List<TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new TMP_Dropdown.OptionData(locale.LocaleName ));
        }

        _dropdown.options = options;

        _dropdown.value = selected;
        _dropdown.onValueChanged.AddListener(LocaleSelected);
    }

    private void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
