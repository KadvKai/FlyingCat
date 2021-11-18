using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOutsideCameraTextIndicator : MonoBehaviour
{
    [SerializeField] private Text playerOutsideCameraText;

    void Start()
    {
        playerOutsideCameraText.gameObject.SetActive(false);   
    }

    private void OnEnable()
    {
        PlayerOutsideCamera.TimeToDestruction += TimeToDestructionIndicator;
    }

    private void TimeToDestructionIndicator(int time)
    {
        
        if (time<=0) playerOutsideCameraText.gameObject.SetActive(false);
        else
        {
            playerOutsideCameraText.gameObject.SetActive(true);
            playerOutsideCameraText.text= "Вернитесь на экран!!!\n"+time;
        }

        
    }

    private void OnDisable()
    {
        PlayerOutsideCamera.TimeToDestruction -= TimeToDestructionIndicator;
    }


}
