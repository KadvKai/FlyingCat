using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MyLogger
{
    private static Action LogListeners;

    private static Queue<string> _unityDate = new Queue<string>();
    private static Queue<string> _gameDate = new Queue<string>();

    private static bool _returnUnityLog = false;

    public static void SetLogType(bool typeLog)
    {
        _returnUnityLog = typeLog;
    }

    public static void AddLoggListener(Action listener)
    {
        LogListeners += listener;
    }

    public static void RemoveLoggListener(Action listener)
    {
        LogListeners -= listener;
    }

    public static void PublishUnityLog(string data)
    {
        _unityDate.Enqueue($"{data}\n");

        if (_unityDate.Count == 10)
            _unityDate.Dequeue();

        if (_returnUnityLog == false)
            return;

        LogListeners?.Invoke();
    }

    public static void PublishMyLog(string data)
    {
        _gameDate.Enqueue($"{data}\n");

        if (_gameDate.Count == 10)
            _gameDate.Dequeue();

        if (_returnUnityLog)
            return;

        LogListeners?.Invoke();
    }

    public static string GetLog()
    {
        string data = "***LOG***\n";
        if (_returnUnityLog)
        {
            foreach (string _data in _unityDate)
            {
                data += _data;
            }

            return $"***UNITY CONSOLE LOG***\n{data}";
        }

        foreach (string _data in _gameDate)
        {
            data += _data;
        }

        return $"***GAME LOG***\n{data}";
    }
}
