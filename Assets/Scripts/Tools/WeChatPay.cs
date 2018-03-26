using UnityEngine;
using System;
using System.Collections.Generic;

public class WeChatPay
{

    private static AndroidJavaObject _wxApi = null;
	private static AndroidJavaObject WXApi {
		get {
			if (_wxApi == null) {
                Debug.Log("Get WXApi");
				AndroidJavaObject activity = GetCurrentActivity();
				AndroidJavaClass apiFactory = new AndroidJavaClass ("com.tencent.mm.opensdk.openapi.WXAPIFactory");

				_wxApi = apiFactory.CallStatic<AndroidJavaObject> ("createWXAPI", activity, SDKConfig.OriginalInstance.WechatAppID, false);
                // _wxApi.Call("registerApp", SDKConfig.OriginalInstance.WechatAppID);
                Debug.Log("Wechat appid = " + SDKConfig.OriginalInstance.WechatAppID);

            }
			return _wxApi;
		}
	}

    private static AndroidJavaObject GetCurrentActivity()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");

        return activity;
    }

	public static void SendWeChatPay(string appid, string partnerid, string prepayid, string package, string noncestr, string timestamp, string sign)
	{
        Debug.Log("start SendWeChatPay");
        Dictionary<string,string> paras = new Dictionary<string, string>();
        paras.Add ("appId", appid);
        paras.Add ("partnerId", partnerid); //商户号
        paras.Add ("prepayId", prepayid);
        paras.Add ("packageValue", package); //暂填写固定值Sign=WXPay
        paras.Add ("nonceStr", noncestr); //随机字符串
        paras.Add ("timeStamp", timestamp);
        paras.Add ("sign", sign); //应用签名

        var request = new AndroidJavaObject ("com.tencent.mm.opensdk.modelpay.PayReq");
        foreach(var kv in paras)
        {
            request.Set (kv.Key, kv.Value);
        }

        bool isValid = request.Call<bool>("checkArgs");
        if (!isValid) 
        {
            Debug.Log("微信支付参数不合法，请检查参数");
        }
        else
        {
            bool ret = WXApi.Call<bool>("sendReq", request);
            Debug.Log("SendWeChatPay  ret = " + ret.ToString());
        }
	}
}
