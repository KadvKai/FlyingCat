using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LogViewer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _infoText;

    private void OnEnable()
    {
        LogUpdate();
        MyLogger.AddLoggListener(LogUpdate);
    }

    private void OnDisable()
    {
        MyLogger.RemoveLoggListener(LogUpdate);
    }

    private void OnDestroy()
    {
        MyLogger.RemoveLoggListener(LogUpdate);
    }

    public void LogUpdate()
    {
        _infoText.text = MyLogger.GetLog();
    }

    public void SetReturnLogType(bool state)
    {
        //true - return Unity log
        //false - return Game log
        MyLogger.SetLogType(state);

        LogUpdate();
    }
}
