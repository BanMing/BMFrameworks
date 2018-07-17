using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

public class WeChatManager : MonoBehaviour
{
    private bool isGetUserInfoNow;
    private static GameObject mInstanceGameObject;
    private static WeChatManager mInstance;
    public static WeChatManager Instance
    {
        get
        {
            if (WeChatManager.mInstance == null)
            {
                WeChatManager.CreateInstance();
            }
            return WeChatManager.mInstance;
        }
    }

    private static WeChatManager CreateInstance()
    {
        if (WeChatManager.mInstance == null)
        {
            WeChatManager.mInstanceGameObject = new GameObject(typeof(WeChatManager).Name);
            WeChatManager.mInstance = WeChatManager.mInstanceGameObject.AddComponent<WeChatManager>();
        }
        return WeChatManager.mInstance;
    }

    private void Awake()
    {
        if (WeChatManager.mInstance != null)
        {
            UnityEngine.Object.Destroy(base.gameObject);
            return;
        }
        UnityEngine.Object.DontDestroyOnLoad(this);
        WeChatManager.mInstance = this;
        RegisterWeChat();
    }

    public void Init()
    {
        DebugTool.Log("WeChatManager Init");
        isGetUserInfoNow = false;
    }


    //////////////////req/////////////////

    //手动注册微信
    public void RegisterWeChat()
    {
#if UNITY_ANDROID || UNITY_IOS
        RegisterWeChatApp(WeChatConstants.WeChatAppId);
#endif
    }
    //微信登陆
    public void LogInWeChat()
    {
#if UNITY_ANDROID || UNITY_IOS
        LoginWeChat();
#endif
    }
    //分享图文
    public void ShareWebpage(bool isFriend, string title, string url, string contentStr, string iconUrl)
    {
#if UNITY_ANDROID || UNITY_IOS
        HTTPTool.GetBytes(iconUrl,(bytes)=>{
            if (File.Exists(WeChatConstants.WebpageIconPath))
            {
                File.Delete(WeChatConstants.WebpageIconPath);
            }
            File.Create(WeChatConstants.WebpageIconPath);
            ShareContent(isFriend,title,url,contentStr,WeChatConstants.WebpageIconPath);
        });     
#endif
    }
    //分享截图
    public void ShareShotPic(bool isFriend)
    {
#if UNITY_ANDROID || UNITY_IOS
        MyUnityTool.ScreenShotByReadPixels((bytes) =>
        {
            if (File.Exists(WeChatConstants.ShotPicPath))
            {
                File.Delete(WeChatConstants.ShotPicPath);
            }
            File.Create(WeChatConstants.ShotPicPath);

        SharePic(isFriend,WeChatConstants.ShotPicPath);

        });
#endif
    }
    public void WxPay()
    {
#if UNITY_ANDROID || UNITY_IOS
#endif
    }


#if UNITY_ANDROID
    private AndroidJavaObject activity;
    private AndroidJavaObject wxSender;
    private AndroidJavaObject Activity
    {
        get
        {
            if (activity == null)
            {
                AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return activity;
        }
    }
    private AndroidJavaObject WXSender
    {
        get
        {
            if (wxSender == null)
            {
                AndroidJavaClass sender = new AndroidJavaClass("com.suixinplay.base.WX.WXSender");
                wxSender = sender.CallStatic<AndroidJavaObject>("Instance");
                wxSender.Call("Init", Activity);
            }
            return wxSender;
        }
    }
    private static void RegisterWeChatApp(string appId){
    
    private static void 
    }
#elif UNITY_IOS
    [DllImport("__Internal")]
    private static extern bool RegisterWeChatApp(string appId);
    [DllImport("__Internal")]
    private static extern void LoginWeChat();
    [DllImport("__Internal")]
    private static extern void ShareContent(bool isFriend,string title,string url,string contentStr,string iconPath);
    [DllImport("__Internal")]
    private static extern void WxPay();
    [DllImport("__Internal")]
    private static extern void SharePic(bool isFriend,string imgPath);
#endif



    /////////////call back/////////////////
    public void LogInCallBack()
    {

    }
    public void ShareCallBack()
    {

    }
    public void WxPayCallBack()
    {

    }
    public void GetAccessToken(string code)
    {
        if (!isGetUserInfoNow)
        {
            isGetUserInfoNow = true;
            Debug.Log("Code:" + code);
            var url = WeChatConstants.AccessTokenUrl.Replace("#CODE#", code);
            StartCoroutine(getAccessToken(url));
        }

    }
    IEnumerator getAccessToken(string accessUrl)
    {
        var webRequest = UnityWebRequest.Get(accessUrl);
        yield return webRequest.Send();
        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.LogError("getAccessToken error:" + webRequest.error);
        }
        else
        {
            Debug.Log("webRequest getAccessToken:" + webRequest.downloadHandler.text);
            var jsonObj = JSONObject.Create(webRequest.downloadHandler.text);
            if (jsonObj["errcode"] == null)
            {
                var openId = jsonObj["openid"].ToString();
                // Debug.LogWarning("openId:" + openId);
                var accessToken = jsonObj["access_token"].ToString();
                // Debug.LogWarning("accessToken:" + accessToken);
                var newUrl = WeChatConstants.UserInfoUrl.Replace("#OPENID#", openId);
                newUrl = newUrl.Replace("#ACCESS_TOKEN#", accessToken);
                // Debug.LogWarning("newUrl:" + newUrl);
                newUrl = newUrl.Replace("\"", "");
                // Debug.LogWarning("@@@@@newUrl:" + newUrl);
                var webRequest1 = UnityWebRequest.Get(newUrl);
                yield return webRequest1.Send();
                if (webRequest1.isHttpError || webRequest1.isNetworkError)
                {
                    Debug.LogError("getUserinfo error:" + webRequest1.error);
                }
                else
                {
                    Debug.Log("webRequest1:" + webRequest1.downloadHandler.text);
                    if (JSONObject.Create(webRequest1.downloadHandler.text)["errcode"] == null)
                    {
                        var userInfo = JsonUtility.FromJson<WeChatUserInfo>(webRequest1.downloadHandler.text);
                        Debug.Log("userInfo.openid:" + userInfo.openid);
                        Debug.Log("userInfo.nickname:" + userInfo.nickname);
                    }
                }
            }
        }
        isGetUserInfoNow = false;
    }
}