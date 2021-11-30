using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(GameParameters))]
public class MenuUserNameAge : MonoBehaviour
{
    [SerializeField] GameObject _menyUserNameAge;
    [SerializeField] InputField _userName;
    [SerializeField] InputField _userAge;
    private GameParameters _gameParameters;
    public void Start()
    {
        _gameParameters = GetComponent<GameParameters>();
    }

    public void ButtonOK()
    {
        _gameParameters.SetUserNameAge(_userName.text, int.Parse(_userAge.text));
        _gameParameters.SaveParameters();
        _menyUserNameAge.SetActive(false);
    }
}
