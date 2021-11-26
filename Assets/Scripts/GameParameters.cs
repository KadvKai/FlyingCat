using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class GameParameters : MonoBehaviour
{
    private readonly string directory = "/SaveData/";
    private readonly string fileName = "SaveGame.txt";
    [SerializeField]  private string userName;
    [SerializeField]  private int userAge;

    private void Awake()
    {
        LoadParameters();

    }

    private void LoadParameters()
    {
        if (HaveSaveFile())
        {
            var json = File.ReadAllText(Application.persistentDataPath + directory + fileName);
            JsonUtility.FromJsonOverwrite(json,this);
        }
    }

    public void SaveParameters()
    {
        var json = JsonUtility.ToJson(this);
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + directory);
        File.WriteAllText(Application.persistentDataPath + directory + fileName, json);
    }
    public bool HaveSaveFile()
    {
        if (File.Exists(Application.persistentDataPath + directory + fileName))
        {
            return true;
        }
        else return false;
    }
    public string GetUserName()
    {
        return userName;
    }
    public void SetUserNameAge(string name, int age)
    {
        userName = name;
        userAge = age;
    }
}
