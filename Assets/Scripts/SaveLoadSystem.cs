using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem
{
    private const string USER_NAME = "user_name";
    private const string USER_AGE = "user_age";

    public void Save(SaveData saveData)
    {
        PlayerPrefs.SetString(USER_NAME, saveData._userName);
        PlayerPrefs.SetInt(USER_AGE, saveData._userAge);
        PlayerPrefs.Save();
    }

    public SaveData Load()
    {
        var saveData = new SaveData();
        if (PlayerPrefs.HasKey(USER_NAME))
        {
            saveData._userName = PlayerPrefs.GetString(USER_NAME);
        }
        if (PlayerPrefs.HasKey(USER_AGE))
        {
            saveData._userAge = PlayerPrefs.GetInt(USER_AGE);
        }

        return saveData;
    }
}
