using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(GameParameters))]
public class MenuUserNameAge : MonoBehaviour
{
    [SerializeField] GameObject menyUserNameAge;
    [SerializeField] InputField userName;
    [SerializeField] InputField userAge;
    private GameParameters gameParameters;
    public void Start()
    {
        gameParameters = GetComponent<GameParameters>();
    }

    public void ButtonOK()
    {
        gameParameters.SetUserNameAge(userName.text, int.Parse(userAge.text));
        gameParameters.SaveParameters();
        menyUserNameAge.SetActive(false);
    }
}
