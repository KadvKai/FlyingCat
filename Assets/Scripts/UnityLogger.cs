using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLogger : MonoBehaviour
{
    private void Awake()
    {
            //Подписываем делегат LogHandler на событие вывода в консоль Unity
            Application.logMessageReceivedThreaded += LogHandler;
    }
    public void LogHandler(string logString, string stackTrace, LogType type)
    {
        MyLogger.PublishUnityLog("**** UNITY MESSAGE BEGINE ****"
            + "\nLOG STRING\n" + logString
            + "\nSTACK TRACE\n" + stackTrace
            + "\nLOG TYPE\n" + type.ToString()
            + "\n**** UNITY MESSAGE END ****\n");
    }

    private void OnDestroy()
    {
        Application.logMessageReceivedThreaded -= LogHandler;
    }
}

