using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SaveLoadSystem
{
    private const string USER_NAME = "user_name";
    private const string USER_AGE = "user_age";
    private const string STAR_QUANTITY = "star_quantity";
    private const string TIME_TO_STAR = "time_to_star";
    private const string EXIT_TIME = "exit_time";
    public void Save(SaveData saveData)
    {
        PlayerPrefs.SetString(USER_NAME, saveData.UserName);
        PlayerPrefs.SetInt(USER_AGE, saveData.UserAge);
        PlayerPrefs.SetInt(STAR_QUANTITY, saveData.StarQuantity);
        PlayerPrefs.SetInt(TIME_TO_STAR, saveData.TimeToStar);
        PlayerPrefs.SetString(EXIT_TIME, DateTime.UtcNow.ToString(format:"u",CultureInfo.InvariantCulture));
        PlayerPrefs.Save();
    }

    public SaveData Load()
    {
        var saveData = new SaveData();
        if (PlayerPrefs.HasKey(USER_NAME)) saveData.UserName = PlayerPrefs.GetString(USER_NAME);
        else saveData.UserName = null;
        if (PlayerPrefs.HasKey(USER_AGE)) saveData.UserAge = PlayerPrefs.GetInt(USER_AGE);
        if (PlayerPrefs.HasKey(STAR_QUANTITY)) saveData.StarQuantity = PlayerPrefs.GetInt(STAR_QUANTITY);
        if (PlayerPrefs.HasKey(TIME_TO_STAR)) saveData.TimeToStar = PlayerPrefs.GetInt(TIME_TO_STAR);
        if (PlayerPrefs.HasKey(EXIT_TIME)) saveData.ExitTime = DateTime.ParseExact(s: PlayerPrefs.GetString(EXIT_TIME), format: "u", CultureInfo.InvariantCulture);
        return saveData;
    }
}
