using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MenuUserNameAge : MonoBehaviour
{
    [SerializeField] GameObject _menyUserNameAge;
    [SerializeField] InputField _userName;
    [SerializeField] InputField _userAge;
    public event UnityAction<string, int> UserNameAgeSet;
    public void StartMenuUserNameAge()
    {
        _menyUserNameAge.SetActive(true);
    }

    public void ButtonOK()
    {
        UserNameAgeSet?.Invoke(_userName.text, int.Parse(_userAge.text));
        _menyUserNameAge.SetActive(false);
    }
}
