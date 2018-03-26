/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:	2015.4.28
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager
{
    static private UIManager mInstance = null;
    static public UIManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new UIManager();
            }

            return mInstance;
        }
    }

    private RectTransform m_UIRootRectTransform = null;
    public RectTransform UIRoot
    {
        get
        {
            if (m_UIRootRectTransform == null)
            {
                InitRootNode();
            }

            return m_UIRootRectTransform;
        }
    }

    private RectTransform m_UITopRoot = null;
    public RectTransform UITopRoot
    {
        get
        {
            if(m_UITopRoot == null)
            {
                InitRootNode();
            }
            return m_UITopRoot;
        }
    }
    private Canvas rootCanvas;
    public Canvas RootCanvas
    {
        get
        {
            if (rootCanvas == null)
            {
                InitRootNode();
            }

            return rootCanvas;
        }
    }   

    private void InitRootNode()
    {
        RectTransform tran = null;
        Canvas[] canvas = GameObject.FindObjectsOfType<Canvas>();
        if (canvas.Length == 0)
        {
            string str = string.Format("UIManager.InitRootNode:获取Canvas失败");
            Debug.LogError(str);
            return;
        }

        if (canvas.Length == 1)
        {
            tran = canvas[0].GetComponent<RectTransform>();
            rootCanvas = canvas[0];
        }
        else
        {
            foreach(Canvas can in canvas)
            {
                if(can.name == "UIRoot")
                {
                    tran = (RectTransform)can.transform;
                    rootCanvas = can;
                    break;
                }
            }
        }

        m_UIRootRectTransform = (RectTransform)MyUnityTool.FindChild(tran, "UILayer5");
        m_UITopRoot = (RectTransform)MyUnityTool.FindChild(tran, "UILayer10");
    }

    //-----------------------------------------------------------------------------//

    public LuaInterface.LuaFunction m_CreateWindow = null;
    public LuaInterface.LuaFunction m_DestroyWindow = null;
    public LuaInterface.LuaFunction m_ShowWindow = null;
    public LuaInterface.LuaFunction m_HideWindow = null;

    //窗口开始事件 --暂时没用
    public void TriggerWindowCreateEvent(string windowsName, UIWindows window)
    {
        if (m_CreateWindow != null)
        {
            m_CreateWindow.Call(windowsName, window);
        }
    }

    //窗口销毁消息事件 --暂时没用
    public void TriggerWindowDestroyEvent(string windowsName, UIWindows window)
    {
        if (m_DestroyWindow != null)
        {
            m_DestroyWindow.Call(windowsName, window);
        }
    }

    //窗口显示
    public void TriggerWindowShowEvent(string windowsName, UIWindows window)
    {
        if (m_ShowWindow != null)
        {
            m_ShowWindow.Call(windowsName, window);
        }
    }

    //窗口隐藏
    public void TriggerWindowHideEvent(string windowsName, UIWindows window)
    {
        if (m_HideWindow != null)
        {
            m_HideWindow.Call(windowsName, window);
        }
    }

    //-----------------------------------------------------------------------------//

    public void OpenWindow(string windowName)
    {
        OpenWindow(windowName, false);
    }

    public void OpenWindow(string windowName, bool isTopRoot)
    {
        if(isTopRoot)
        {
            OpenWindow(windowName, UITopRoot);
        }
        else
        {
            OpenWindow(windowName, UIRoot);
        }
    }

    public void OpenWindow(string windowName, RectTransform parent)
    {
        Transform windowTran = FindChild(parent, windowName);
        if (windowTran != null)
        {
            windowTran.gameObject.SetActive(true);
            return;
        }

        string prefabName = windowName + ".prefab";
        UnityEngine.Object obj = ResourcesManager.Instance.GetUIInstanceSync(prefabName, windowName);
        
        GameObject go = (GameObject)obj;
        RectTransform tran = go.GetComponent<RectTransform>();
        MyUnityTool.SetUIParentWithLocalInfo(tran, parent);
        go.name = windowName;
        go.SetActive(true);

        UIWindows window = go.GetComponent<UIWindows>();
        if (window == null)
        {
            window = go.AddComponent<UIWindows>();
        }

        if (string.IsNullOrEmpty(window.m_WindowName))
        {
            window.m_WindowName = windowName;
            window.GenerateBindList();
        }
        if (window.IsTriggerShowEvent == false)
        {
            window.TriggerWindowShowEvent();
        }
    }

    public void CloseWindow(GameObject go)
    {
        go.transform.SetParent(null);
        ResourcesManager.Instance.ReleaseUIInstance(go);
    }

    public void CloseWindow(string windowName)
    {
        CloseWindow(windowName, UIRoot);
    }

    public void CloseWindow(string windowName, RectTransform parent)
    {
        Transform tran = FindChild(parent, windowName);
        if (tran != null)
        {
            CloseWindow(tran.gameObject);
            return;
        }

        //
        UIWindows[] windowsArray = GameObject.FindObjectsOfType<UIWindows>();
        for (int i = 0; i < windowsArray.Length; ++i)
        {
            UIWindows window = windowsArray[i];
            if (window.name == windowName)
            {
                CloseWindow(window.gameObject);
                return;
            }
        }

        //
        string str = string.Format("UIManager.CloseWindow:Close Window [{0:s}] Fail", windowName);
        Debug.LogWarning(str);
    }

    public void HideWindow(GameObject go)
    {
        go.SetActive(false);
    }

    public void HideWindow(string windowName)
    {
        HideWindow(windowName, UIRoot);
    }

    public void HideWindow(string windowName, RectTransform parent)
    {
        Transform tran = FindChild(parent, windowName);
        if (tran != null)
        {
            tran.gameObject.SetActive(false);
            return;
        }

        //
        UIWindows[] windowsArray = GameObject.FindObjectsOfType<UIWindows>();
        for (int i = 0; i < windowsArray.Length; ++i)
        {
            UIWindows window = windowsArray[i];
            if (window.name == windowName)
            {
                window.gameObject.SetActive(false);
                return;
            }
        }

        //
        string str = string.Format("UIManager.HideWindow:Hide Window [{0:s}] Fail", windowName);
        Debug.LogWarning(str);
    }

    public void CloseAllWindow()
    {
        CloseAllWindowInRoot();
        CloseAllWindowInTopRoot();
    }

    public void CloseAllWindowInRoot()
    {
        for (int i = 0; i < UIRoot.childCount; ++i)
        {
            Transform tran = UIRoot.GetChild(0);
            CloseWindow(tran.gameObject);
        }
    }

    public void CloseAllWindowInTopRoot()
    {
        for (int i = 0; i < UITopRoot.childCount; ++i)
        {
            Transform tran = UITopRoot.GetChild(0);
            CloseWindow(tran.gameObject);
        }
    }

    private Transform FindChild(Transform parent, string childName)
    {
        if(string.IsNullOrEmpty(childName))
        {
            string str = string.Format("UIManager.FindChild:查找节点{0}的子节点错误,子节点名为空", parent.name);
            Debug.LogError(str);
            return null;
        }

        return parent.Find(childName);
    }

    //-----------------------------------------------------------------------------//

    public bool IsInit()
    {
        if(m_ShowWindow == null)
        {
            return false;
        }

        return true;
    }

    public Camera GetCamera()
    {
        var uiroot = MyUnityTool.Find("UIRoot");
        return MyUnityTool.FindChild(uiroot.transform, "UICamera").GetComponent<Camera>();
    }
    //-----------------------------------------------------------------------------//
}
