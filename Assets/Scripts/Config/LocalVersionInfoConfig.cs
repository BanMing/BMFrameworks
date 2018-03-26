using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Xml;
using Mono.Xml;

public class LocalVersionInfoConfig : GameData<LocalVersionInfoConfig>
{
    static public readonly string fileName = "LocalVersionInfoConfig.xml";

    public string VersionName { set; get; }
    public string CurrentVersionName { set; get; }  //当前版本名

    public string ResPath { set; get; }

    public string CompanyName { set; get; }
    public string ProductName { set; get; }

    public string VersionDifferenceFileDir { set; get; }
    public string VersionDirrerenceResourcesDir { set; get; }

    public string AppID { set; get; }
    public string Macro { set; get; }

    public string ShareSDKAppKey { set; get; }
    public string ShareSDKAppSecret { set; get; }
    public string ShareSDKJarPath { set; get; }

    public string WechatAppID { set; get; }
    public string WechatAppSecret { set; get; }

    public uint VoiceSDKAppID { set; get; }

    public string AlipayAppID { set; get; }

    public const string Current = "Current";

    static public LocalVersionInfoConfig GetLocalVersionInfoConfig(string versionName)
    {
        foreach (var item in LocalVersionInfoConfig.dataMap)
        {
            if (item.Value.VersionName == versionName)
            {
                return item.Value;
            }
        }

        return null;
    }

    //获取所有版本名
    static public List<string> GetVersionNameList()
    {
        List<string> listName = new List<string>();
        foreach (var item in LocalVersionInfoConfig.dataMap)
        {
            string versionName = item.Value.VersionName;
            if (versionName == LocalVersionInfoConfig.Current)
            {
                continue;
            }

            listName.Add(item.Value.VersionName);
        }

        return listName;
    }
}
