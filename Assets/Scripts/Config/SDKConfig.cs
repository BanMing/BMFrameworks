/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/10/21
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System.Collections;

public class SDKConfig : GameData<SDKConfig>
{
    static public readonly string fileName = "SDKConfig.xml";

    public string VersionName { set; get; }
    public string VersionResPath { set; get; }
    public string ShareSDKAppKey { set; get; }
    public string ShareSDKAppSecret { set; get; }
    public string WechatAppID { set; get; }
    public string WechatAppSecret { set; get; }
    public uint VoiceSDKAppID { set; get; }
    public string AlipayAppID { set; get; }

    static public SDKConfig Instance
    {
        get
        {
#if UNITY_EDITOR
            SDKConfig.Load(fileName);
#endif
            return dataMap[0];
        }
    }

    static public SDKConfig OriginalInstance
    {
        get
        {
            return dataMap[0];
        }
    }

    static public void SaveConfig()
    {
        SaveConfig(OriginalInstance);
    }

    static public void SaveConfig(SDKConfig sdkConfig)
    {
        var root = new System.Security.SecurityElement("Root");
        var record = new System.Security.SecurityElement("Record");
        root.AddChild(record);

        record.AddChild(new System.Security.SecurityElement("id", "0"));
        record.AddChild(new System.Security.SecurityElement("VersionName", sdkConfig.VersionName));
        record.AddChild(new System.Security.SecurityElement("VersionResPath", sdkConfig.VersionResPath));
        record.AddChild(new System.Security.SecurityElement("ShareSDKAppKey", sdkConfig.ShareSDKAppKey));
        record.AddChild(new System.Security.SecurityElement("ShareSDKAppSecret", sdkConfig.ShareSDKAppSecret));
        record.AddChild(new System.Security.SecurityElement("WechatAppID", sdkConfig.WechatAppID));
        record.AddChild(new System.Security.SecurityElement("WechatAppSecret", sdkConfig.WechatAppSecret));
        record.AddChild(new System.Security.SecurityElement("VoiceSDKAppID", sdkConfig.VoiceSDKAppID.ToString()));
        record.AddChild(new System.Security.SecurityElement("AlipayAppID", sdkConfig.AlipayAppID.ToString()));

        if (SystemConfig.Instance.IsEncryptConfigFile)
        {
            MyFileUtil.WriteUnEncryptConfigDataInStreamingAssets(fileName, root.ToString());
        }
        MyFileUtil.WriteConfigDataInStreamingAssets(fileName, root.ToString());
        
    }

    //获取当前版本资源路径
    static public string GetCurrentVersionResPath()
    {
        return Instance.VersionResPath;
    }

    static public string GetCurrentVersionName()
    {
        return Instance.VersionName;
    }
}
