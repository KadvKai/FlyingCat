using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class PlayerOutsideCameraTextIndicator : MonoBehaviour
{
    [SerializeField] private Text _playerOutsideCameraText;
    [SerializeField] private PlayerOutsideCamera _playerOutsideCamera;
    [SerializeField] private LocalizeStringEvent localizeString;
    public int Time { get; private set; }

    private void OnEnable()
    {
        _playerOutsideCamera.TimeToDestruction += TimeToDestructionIndicator;
    }

    private void OnDisable()
    {
        _playerOutsideCamera.TimeToDestruction -= TimeToDestructionIndicator;
    }
    private void TimeToDestructionIndicator(int time)
    {
        Time = time;
        if (time<=0) _playerOutsideCameraText.gameObject.SetActive(false);
        else
        {
            _playerOutsideCameraText.gameObject.SetActive(true);
            localizeString.StringReference.RefreshString();
            //_playerOutsideCameraText.text= "Вернитесь на экран!!!\n"+time;
        }

        
    }

}
