using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Первый");
        Debug.Log(LocalizationSettings.AvailableLocales.Locales.Count);
    }
    /*static List<string> GeTListLanguages()
    {
         var stri= LocalizationSettings.AvailableLocales.Locales[1].ToString();
    }*/
    public void LoadLocale(string languageIdentifier)
    {
        LocalizationSettings settings = LocalizationSettings.Instance;
        LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);//can be "en" "de" "ja" etc.
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            Locale aLocale = LocalizationSettings.AvailableLocales.Locales[i];
            LocaleIdentifier anIdentifier = aLocale.Identifier;
            if (anIdentifier == localeCode)
            {
                LocalizationSettings.SelectedLocale = aLocale;
            }
        }
    }
}
