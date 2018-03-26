/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/10/25
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;

public class MyShareSDK : ShareSDK
{
    private static MyShareSDK m_Instance = null;

    public EventHandler authHandler2 = null;
    public EventHandler shareHandler2 = null;
    public EventHandler showUserHandler2 = null;
    public EventHandler getFriendsHandler2 = null;
    public EventHandler followFriendHandler2 = null;
    public Action<int, String, String> wechatPayHandler2 = null;

    public static MyShareSDK Instance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = FindObjectOfType<MyShareSDK>();
            }

            return m_Instance;
        }
    }

    new void Awake()
    {
        m_Instance = this;
        appKey = SDKConfig.Instance.ShareSDKAppKey;
        ReadWechatConfig();
        authHandler = OnAuthResultHandler;
        shareHandler = OnShareResultHandler;
        showUserHandler = OnGetUserInfoResultHandler;
        getFriendsHandler = OnGetFriendsResultHandler;
        followFriendHandler = OnFollowFriendResultHandler;
        base.Awake();
    }

    void OnDestroy()
    {
        m_Instance = null;
    }

    private void ReadWechatConfig()
    {
        string wechatAppID = SDKConfig.Instance.WechatAppID;
        string wechatAppSecret = SDKConfig.Instance.WechatAppSecret;

        //wechat
        devInfo.wechat.Enable = true;
#if UNITY_ANDROID
        devInfo.wechat.AppId = wechatAppID;
        devInfo.wechat.AppSecret = wechatAppSecret;
        devInfo.wechat.BypassApproval = false;
#elif UNITY_IPHONE
        devInfo.wechat.app_id = wechatAppID;
		devInfo.wechat.app_secret = wechatAppSecret;
#endif

        //WeChatMoments
        devInfo.wechatMoments.Enable = true;
#if UNITY_ANDROID
        devInfo.wechatMoments.AppId = wechatAppID;
        devInfo.wechatMoments.AppSecret = wechatAppSecret;
        devInfo.wechatMoments.BypassApproval = false;
#elif UNITY_IPHONE
        devInfo.wechatMoments.app_id = wechatAppID;
	    devInfo.wechatMoments.app_secret = wechatAppSecret;
#endif

        //WeChatFavorites
        devInfo.wechatFavorites.Enable = true;
#if UNITY_ANDROID
        devInfo.wechatFavorites.AppId = wechatAppID;
        devInfo.wechatFavorites.AppSecret = wechatAppSecret;
#elif UNITY_IPHONE
        devInfo.wechatFavorites.app_id = wechatAppID;
		devInfo.wechatFavorites.app_secret = wechatAppSecret;
#endif

        //WechatSeries
#if UNITY_IPHONE
        devInfo.wechatSeries.Enable = true;
        devInfo.wechatSeries.app_id = wechatAppID;
		devInfo.wechatSeries.app_secret = wechatAppSecret;
#endif

        string alipayAppID = SDKConfig.Instance.AlipayAppID;
        //Alipay
        devInfo.alipay.Enable = true;
#if UNITY_ANDROID
        devInfo.alipay.AppId = alipayAppID;
#elif UNITY_IPHONE
        devInfo.alipay.app_id = alipayAppID;
#endif
        //AlipayMoments
        devInfo.alipayMoments.Enable = true;
#if UNITY_ANDROID
        devInfo.alipayMoments.AppId = alipayAppID;
#elif UNITY_IPHONE
        devInfo.alipayMoments.app_id = alipayAppID;
#endif
    }
    JSONObject m_json;
    public void SetWXData(Hashtable param)
    {
        try
        {
            string strjson = MiniJSON.jsonEncode(param);
            m_json = new JSONObject(strjson);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.LogError("SetWXData parse err:" + e.Message);
        }

    }
    public string GetWXValue(string key,bool isString)
    {
        try
        {
            if (m_json != null)
            {
                if(isString)
                {
                    return m_json[key].str;
                }
                else
                {
                    return m_json[key].i.ToString();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.LogError("GetWXValue parse err:" + e.Message);
        }

        return "";
    }

    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (authHandler2 != null)
        {
            
            authHandler2(reqID, state, type, result==null?new Hashtable():result);
            return;
        }
        if (state == ResponseState.Success)
        {
            GetUserInfo(type);
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            Debug.Log("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			Debug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
            
        }
        else if (state == ResponseState.Cancel)
        {
            Debug.Log("cancel !");
           
        }
       
    }
    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (showUserHandler2 != null)
        {

            showUserHandler2(reqID, state, type, result == null ? new Hashtable() : result);
            return;
        }

        if (state == ResponseState.Success)
        {
            SetWXData(result);
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            Debug.Log("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			Debug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            Debug.Log("cancel !");
        }
    }
    void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (shareHandler2 != null)
        {

            shareHandler2(reqID, state, type, result == null ? new Hashtable() : result);
            return;
        }
        if (state == ResponseState.Success)
        {
            Debug.Log("share successfully - share result :");
            Debug.Log(MiniJSON.jsonEncode(result));
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            Debug.Log("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			Debug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            Debug.Log("cancel !");
        }
    }
    void OnGetFriendsResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (getFriendsHandler2 != null)
        {

            getFriendsHandler2(reqID, state, type, result == null ? new Hashtable() : result);
            return;
        }
        if (state == ResponseState.Success)
        {
            Debug.Log("get friend list result :");
            Debug.Log(MiniJSON.jsonEncode(result));
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            Debug.Log("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			Debug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            Debug.Log("cancel !");
        }
    }
    void OnFollowFriendResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (followFriendHandler2 != null)
        {

            followFriendHandler2(reqID, state, type, result == null ? new Hashtable() : result);
            return;
        }

        if (state == ResponseState.Success)
        {
            Debug.Log("Follow friend successfully !");
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            Debug.Log("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			Debug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            Debug.Log("cancel !");
        }
    }

    void OnWechatPayHandler(string resultStr)
    {
        if (wechatPayHandler2 != null)
        {   
            Debug.Log("resultStr = " + resultStr);
            string[] param = resultStr.Split('&');

            string errCode = param[0];
            string errDesc = param[1];
            string transaction = param[2];

            if (!String.IsNullOrEmpty(errCode))
            {
                wechatPayHandler2(int.Parse(errCode), errDesc, transaction);
            }
        }
        else
        {
            Debug.Log("wechatPayHandler2 is null");
        }
    }
}
