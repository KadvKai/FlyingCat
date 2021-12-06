using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOutsideCameraTextIndicator : MonoBehaviour
{
    [SerializeField] private Text _playerOutsideCameraText;

    void Start()
    {
        _playerOutsideCameraText.gameObject.SetActive(false);   
    }

    private void OnEnable()
    {
        PlayerOutsideCamera.TimeToDestruction += TimeToDestructionIndicator;
    }

    private void TimeToDestructionIndicator(int time)
    {
        
        if (time<=0) _playerOutsideCameraText.gameObject.SetActive(false);
        else
        {
            _playerOutsideCameraText.gameObject.SetActive(true);
            _playerOutsideCameraText.text= "��������� �� �����!!!\n"+time;
        }

        
    }

    private void OnDisable()
    {
        PlayerOutsideCamera.TimeToDestruction -= TimeToDestructionIndicator;
    }


}