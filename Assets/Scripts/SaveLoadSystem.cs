using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem
{
    private const string USER_NAME = "user_name";
    private const string USER_AGE = "user_age";
    private const string STAR_QUANTITY = "star_quantity";

    public void Save(SaveData saveData)
    {
        PlayerPrefs.SetString(USER_NAME, saveData.UserName);
        PlayerPrefs.SetInt(USER_AGE, saveData.UserAge);
        PlayerPrefs.SetInt(STAR_QUANTITY, saveData.StarQuantity);
        PlayerPrefs.Save();
    }

    public SaveData Load()
    {
        var saveData = new SaveData();
        if (PlayerPrefs.HasKey(USER_NAME))
        {
            saveData.UserName = PlayerPrefs.GetString(USER_NAME);
        }
        else saveData.UserName = null;
        if (PlayerPrefs.HasKey(USER_AGE))
        {
            saveData.UserAge = PlayerPrefs.GetInt(USER_AGE);
        }
        if (PlayerPrefs.HasKey(STAR_QUANTITY))
        {
            saveData.StarQuantity = PlayerPrefs.GetInt(STAR_QUANTITY);
        }

        return saveData;
    }
}
