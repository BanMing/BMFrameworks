using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DebugTool : MonoBehaviour
{

    private static bool IsLog
    {
        get { return SystemConfig.Instance.IsLog; }
    }
    private static bool IsShowLog
    {
        get { return SystemConfig.Instance.IsLog; }
    }
    List<string> logText;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        logText = new List<string>();
        Application.logMessageReceived += Handheld;
    }
    private void Handheld(string condition, string stackTrace, LogType logType)
    {
        string str = "";
        if (logType == LogType.Error)
        {
            str += "【LOG Error】:";
        }
        str += condition;
        if (logType == LogType.Error)
        {
            str += "【Stack Trace】:" + stackTrace;
        }
        logText.Add(str);
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    // void Update()
    // {这里不使用update写入文件，不知道会不会有错

    // }
    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    void OnApplicationQuit()
    {

    }
    public static void Log(string msg)
    {
        if (IsLog)
        {
            Debug.Log(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + msg);
        }

    }
    public static void LogError(string msg)
    {
        if (IsLog)
        {
            Debug.LogError(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + " " + msg);
        }

    }
    public static void LogWarning(string msg)
    {
        if (IsLog)
        {
            Debug.LogWarning(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ") + " " + msg);
        }
    }
    public static void UpLoadLogFile()
    {
        //测试用地址
        string upLoadLogUrl = "http://192.168.4.85:8001/uploadLog";

    }
    static IEnumerator UpLoad(string upLoadLogUrl, Action<string> CallBack)
    {
        byte[] screenshotBytes;
        MyUnityTool.ScreenShotByReadPixels((data) => { screenshotBytes = data; });
        HTTPTool.UpLoadFiles(upLoadLogUrl, new List<string>() { "log", "logs" }, new List<byte[]>() { },
        new List<string>() { "image.jpg", "log.txt" }, new List<string>() { "uid" }, ne)

    }
}