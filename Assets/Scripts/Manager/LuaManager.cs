/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/09/08
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

public class LuaManager : LuaClient
{
	void Start ()
    {
        Debug.Log("LuaManager.Start");
	}

    static public System.Action m_InitFinishCB = null; //初始化结束

    protected override LuaFileUtils InitLoader()
    {
        return new MyLuaResLoader();
        //return new LuaResLoader();
    }

    protected override void OnLoadFinished()
    {
        /*
        //添加Lua脚本目录, Android不添加目录APK中的路径，否则会报错
        if(Application.platform != RuntimePlatform.Android)
        {
            luaState.AddSearchPath(LuaConst.luaDir);
            luaState.AddSearchPath(LuaConst.toluaDir);
        }
        */

        base.OnLoadFinished();
        Debug.Log("LuaManager.OnLoadFinished");
    }

    protected override void StartMain()
    {
        luaState.DoFile("Main.lua");

        InitUI();

        levelLoaded = luaState.GetFunction("OnLevelWasLoaded");
        DispatchSocketMsgAction = luaState.GetFunction("MsgManager.DispatchMsg");

        Debug.Log("LuaManager.StartMain:准备执行Lua主函数");
        CallMain();
        Debug.Log("LuaManager.StartMain:执行Lua主函数结束");

        if (m_InitFinishCB != null)
        {
            m_InitFinishCB();
        }

        Debug.Log("LuaManager.StartMain");
    }

    static private LuaInterface.LuaFunction DispatchSocketMsgAction = null;

    public LuaState GetLuaState()
    {
        return luaState;
    }

    /*
    static public void DispatchSocketMsg(NetManager netManager, byte[] data)
    {
        if(DispatchSocketMsgAction != null)
        {
            DispatchSocketMsgAction.Call(netManager, data);
        }
    }
    */

    // static public void DispatchSocketMsg(NetManager netManager, TCP_Buffer buffer)
    // {
    //     if (DispatchSocketMsgAction != null)
    //     {
    //         DispatchSocketMsgAction.Call(netManager, buffer);
    //     }
    // }

    #region Lua交互

    public void LoadLuaScriptFile(string fileName)
    {
        luaState.DoFile(fileName);
    }
    
    public void LoadLuaScriptText(string scriptText, string scriptableName = "LuaState.cs")
    {
        luaState.DoString(scriptText, scriptableName);
    }

    public void CallLuaFunction(string functionName)
    {
        LuaFunction func = luaState.GetFunction(functionName);
        if(func != null)
        {
            func.Call();
            func.Dispose();
            func = null;
        }
        else
        {
            string str = string.Format("LuaManager.CallLuaFunction:调用Lua函数{0}失败", functionName);
            Debug.LogError(str);
        }
    }

    public object[] CallLuaFunction(string functionName, params object[] args)
    {
        LuaFunction func = luaState.GetFunction(functionName);
        if(func != null)
        {
            object[] results = func.Call(args);
            func.Dispose();
            func = null;
            return results;
        }
        else
        {
            string str = string.Format("LuaManager.CallLuaFunction:调用Lua函数{0}失败", functionName);
            Debug.LogError(str);
        }

        return null;
    }

    #endregion

    #region UI

    public const string CreateWindowFunctionName = "UIMgr.CreateWindowEvent";
    public const string DestroyWindowFunctionName = "UIMgr.DestroyWindowEvent";
    public const string ShowWindowFunctionName = "UIMgr.ShowWindowEvent";
    public const string HideWindowFunctionName = "UIMgr.HideWindowEvent";

    void InitUI()
    {
        UIManager.Instance.m_ShowWindow = luaState.GetFunction(ShowWindowFunctionName);
        UIManager.Instance.m_HideWindow = luaState.GetFunction(HideWindowFunctionName);
    }

    void ReleaseUI()
    {
        if(UIManager.Instance.m_ShowWindow != null)
        {
            UIManager.Instance.m_ShowWindow.Dispose();
            UIManager.Instance.m_ShowWindow = null;
        }

        if (UIManager.Instance.m_HideWindow != null)
        {
            UIManager.Instance.m_HideWindow.Dispose();
            UIManager.Instance.m_HideWindow = null;
        }
    }

    #endregion
}

public class MyLuaResLoader : LuaResLoader
{
    static MyLuaResLoader()
    {
        IsArch64 = MyUnityTool.IsProcessorArch64();
    }

    static bool IsArch64 = false;

    public static string X32LuaByteCodeFileSuffix = ".bytes";
    public static string X64LuaByteCodeFileSuffix = ".64.bytes";
    public static string LuaByteCodeFileSuffix
    {
        get
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer && IsArch64)
            {
                return X64LuaByteCodeFileSuffix;
            }

            return X32LuaByteCodeFileSuffix;
        }
    }

    private string[] m_ListLuaSearchDir = new string[]
    {
        "Lua/",
        "Lua/ToLua/"
    };

    public override byte[] ReadFile(string fileName)
    {
        if(SystemConfig.Instance.IsUseLuaBytecode)
        {
            if (fileName.EndsWith(".lua"))
            {
                fileName = fileName.Replace(".lua", LuaByteCodeFileSuffix);
            }

            if(fileName.EndsWith(LuaByteCodeFileSuffix) == false)
            {
                fileName = fileName + LuaByteCodeFileSuffix;
            }
        }
        else
        {
            if (fileName.EndsWith(LuaByteCodeFileSuffix))
            {
                fileName = fileName.Replace(LuaByteCodeFileSuffix, ".lua");
            }

            if (fileName.EndsWith(".lua") == false)
            {
                fileName = fileName + ".lua";
            }
        }

        if (ResourcesManager.IsLuaUseZip)
        {
            return GetLuaFileDataFromZip(fileName);
        }
        else
        {
            return GetLuaFileDataFromStreamingAssetsPath(fileName);
        }
    }

    private byte[] GetLuaFileDataFromStreamingAssetsPath(string fileName)
    {
        for (int i = 0; i < m_ListLuaSearchDir.Length; ++i)
        {
            string filePath = Application.streamingAssetsPath + "/" + m_ListLuaSearchDir[i] + fileName;
            if (filePath.Contains("://"))
            {
                WWW www = new WWW(filePath);
                while (!www.isDone)
                {
                    //等待加载完成
                }
                if (string.IsNullOrEmpty(www.error))
                {
                    return www.bytes;
                }
            }
            else
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
            }
        }

        string str = string.Format("GetLuaFileDataFromStreamingAssetsPath:读取文件{0}文件失败", fileName);
        Debug.LogError(str);
        return null;
    }

    private byte[] GetLuaFileDataFromZip(string fileName)
    {
        for(int i = 0; i < m_ListLuaSearchDir.Length; ++i)
        {
            string path = m_ListLuaSearchDir[i] + fileName;
            byte[] data = ResourcesManager.Instance.GetLuaScriptDataFromZip(path);
            if(data != null)
            {
                return data;
            }
        }

        return null;
    }
}