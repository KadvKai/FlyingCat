using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class GameParameters : MonoBehaviour
{
    private readonly string _directory = "/SaveData/";
    private readonly string _fileName = "SaveGame.txt";
    [SerializeField]  private string _userName;
    [SerializeField]  private int _userAge;

    private void Awake()
    {
        LoadParameters();

    }

    private void LoadParameters()
    {
        if (HaveSaveFile())
        {
            var json = File.ReadAllText(Application.persistentDataPath + _directory + _fileName);
            JsonUtility.FromJsonOverwrite(json,this);
        }
    }

    public void SaveParameters()
    {
        var json = JsonUtility.ToJson(this);
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + _directory);
        File.WriteAllText(Application.persistentDataPath + _directory + _fileName, json);
    }
    public bool HaveSaveFile()
    {
        if (File.Exists(Application.persistentDataPath + _directory + _fileName))
        {
            return true;
        }
        else return false;
    }
    public string GetUserName()
    {
        return _userName;
    }
    public void SetUserNameAge(string name, int age)
    {
        _userName = name;
        _userAge = age;
    }
}
