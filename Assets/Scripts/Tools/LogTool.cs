/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/10/28
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/
using UnityEngine;
using System.Collections;

public class LogTool
{
    public static void Log(string message)
    {
        string str = System.DateTime.Now.ToString("[hh:mm:ss fff] ") + message;
        UnityEngine.Debug.Log(str);
    }

    public static void LogError(string message)
    {
        string str = System.DateTime.Now.ToString("[hh:mm:ss fff] ") + message;
        UnityEngine.Debug.LogError(str);
    }

    public static void LogWarning(object message)
    {
        string str = System.DateTime.Now.ToString("[hh:mm:ss fff] ") + message;
        UnityEngine.Debug.LogWarning(str);
    }

    public static void LogException(System.Exception exception)
    {
        string str = System.DateTime.Now.ToString("[hh:mm:ss fff] ");
        UnityEngine.Debug.LogError(str);
        UnityEngine.Debug.LogException(exception);
    }
}
