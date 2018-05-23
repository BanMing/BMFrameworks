/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/10/19
** 描  述: 	每次直接下载最新资源包，先将资源按照一定规则划分为一定数量的小资源包(zip)，
*           客户端一个新版本的生成，则生成相应的资源包，旧版本的客户端直接到服务器上面下载需要更新的资源包即可
*           如资源包分为代码资源包、UI图片资源包、UI窗口资源包、场景资源包、声音资源包，新版本生成时，产生这些资源包，并记录到配置表中
*           客户端根据本地记录计算需要更新的资源包，进行更新

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Security;
using Mono.Xml;

public class ResInfo
{
    public string resName;
    public string resMD5;
    public int resSize; //byte为单位
    public string resURL;

    //true：本地没有这个资源包则会下载这个资源包，需要更新则更新
    //false:本地没有这个资源包则不会下载，如果本地有，需要更新则会更新
    public bool isResRequire = false;
}

public class VersionInfo2
{
    public float ProgramVersion { set; get; }   //C#代码版本号

    public string ApkUrl { set; get; }          //最新的apk安装包路径
    public string ApkMd5 { set; get; }          //最新的apk安装包md5
    
    public string IOSAppUrl { set; get; }       //IOS更新URL，企业版
    public string IOSAppStoreUrl { set; get; }  //IOS更新URL，商店版
    public bool IsAppleAppStore { set; get; }   //是否属于IOS App Store版本
    public bool IsOpenAutoUpdateInAppStore { set; get; } //是否开启App Store版本的自动更新，服务器开关，动态开启关闭自动更新

    public Dictionary<string, ResInfo> dictRes = new Dictionary<string, ResInfo>();

    //暴露给Lua使用
    public void AddRes(ResInfo info)
    {
        dictRes.Add(info.resName, info);
    } 

    //-----------------------------------------------------------------------------------

    //解析xml数据
    static public VersionInfo2 ParseData(string xmlContent)
    {
        VersionInfo2 versionInfo = new VersionInfo2();

        try
        {
            SecurityParser securityParser = new SecurityParser();
            securityParser.LoadXml(xmlContent);
            SecurityElement xml = securityParser.ToXml();

            if (xml == null)
            {
                Debug.LogError("VersionInfo2.ParseData:XML Data Error");
                return versionInfo;
            }

            if (xml.Children == null || xml.Children.Count == 0)
            {
                return versionInfo;
            }

            foreach (SecurityElement se in xml.Children)
            {
                string tag = se.Tag.ToLower();
                switch (tag)
                {
                    case "programversion": versionInfo.ProgramVersion = float.Parse(se.Text); break;
                    case "apkurl": versionInfo.ApkUrl = se.Text; break;
                    case "apkmd5": versionInfo.ApkMd5 = se.Text; break;
                    case "iosappurl": versionInfo.IOSAppUrl = se.Text; break;
                    case "iosappstoreurl": versionInfo.IOSAppStoreUrl = se.Text; break;
                    case "isappleappstore": versionInfo.IsAppleAppStore = StrBoolParse(se.Text); break;
                    case "isopenautoupdateinappstore": versionInfo.IsOpenAutoUpdateInAppStore = StrBoolParse(se.Text); break;
                    case "resinfo":
                        {
                            if (se.Children == null || se.Children.Count == 0)
                            {
                                continue;
                            }

                            foreach (SecurityElement record in se.Children)
                            {                               
                                if (record.Children == null || record.Children.Count == 0)
                                {
                                    continue;
                                }

                                ResInfo resInfo = new ResInfo();
                                foreach (SecurityElement node in record.Children)
                                {
                                    string resTag = node.Tag.ToLower();
                                    switch (resTag)
                                    {
                                        case "resname": resInfo.resName = node.Text; break;
                                        case "resmd5": resInfo.resMD5 = node.Text; break;
                                        case "ressize": resInfo.resSize = int.Parse(node.Text); break;
                                        case "resurl": resInfo.resURL = node.Text; break;
                                        case "resrequire":
                                            {
                                                if (node.Text == "0")
                                                    resInfo.isResRequire = false;
                                                else if (node.Text == "1")
                                                    resInfo.isResRequire = true;
                                                else
                                                    resInfo.isResRequire = bool.Parse(node.Text);
                                            }
                                            break;
                                    }
                                }

                                if (versionInfo.dictRes.ContainsKey(resInfo.resName) == false)
                                {
                                    versionInfo.dictRes.Add(resInfo.resName, resInfo);
                                }
                                else
                                {
                                    string strError = string.Format("VersionInfo2.ParseData:更新资源包名{0}重复", resInfo.resName);
                                    Debug.LogError(strError);
                                }
                            }
                        }
                        break;
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogException(ex);
        }

        return versionInfo;
    }

    static bool StrBoolParse(string str)
    {
        if (str == "0")
            return false;
        else if (str == "1")
            return true;
        else
            return bool.Parse(str);
    }

    //序列化为字符串
    static public string Serialize(VersionInfo2 versionInfo)
    {
        var root = new System.Security.SecurityElement("root");
        root.AddChild(new System.Security.SecurityElement("ProgramVersion", versionInfo.ProgramVersion.ToString()));
        var resInfoNode = new System.Security.SecurityElement("ResInfo");
        root.AddChild(resInfoNode);

        foreach (var item in versionInfo.dictRes)
        {
            var recordNode = new System.Security.SecurityElement("Record");
            resInfoNode.AddChild(recordNode);

            recordNode.AddChild(new System.Security.SecurityElement("ResName", item.Value.resName));
            recordNode.AddChild(new System.Security.SecurityElement("ResMD5", item.Value.resMD5));
        }

        return root.ToString();
    }

    //在生成资源包时使用
    static public string SerializeInEditor(List<ResInfo> listResInfo)
    {
        string innerText = MyFileUtil.ReadConfigDataInStreamingAssets(VersionManager2.VersionInfoFilePath);
        VersionInfo2 innerVersionInfo = VersionInfo2.ParseData(innerText);

        var root = new System.Security.SecurityElement("root");
        root.AddChild(new System.Security.SecurityElement("ProgramVersion", innerVersionInfo.ProgramVersion.ToString()));
        root.AddChild(new System.Security.SecurityElement("ApkUrl", innerVersionInfo.ApkUrl));
        root.AddChild(new System.Security.SecurityElement("ApkMd5"));
        root.AddChild(new System.Security.SecurityElement("IOSAppUrl", innerVersionInfo.IOSAppUrl));
        root.AddChild(new System.Security.SecurityElement("IOSAppStoreUrl", innerVersionInfo.IOSAppStoreUrl));
        root.AddChild(new System.Security.SecurityElement("IsAppleAppStore", innerVersionInfo.IsAppleAppStore.ToString()));
        root.AddChild(new System.Security.SecurityElement("IsOpenAutoUpdateInAppStore", innerVersionInfo.IsOpenAutoUpdateInAppStore.ToString()));
        var resInfoNode = new System.Security.SecurityElement("ResInfo");
        root.AddChild(resInfoNode);

        foreach (var item in listResInfo)
        {
            var recordNode = new System.Security.SecurityElement("Record");
            resInfoNode.AddChild(recordNode);

            recordNode.AddChild(new System.Security.SecurityElement("ResName", item.resName));
            recordNode.AddChild(new System.Security.SecurityElement("ResMD5", item.resMD5));
            recordNode.AddChild(new System.Security.SecurityElement("ResURL", item.resURL));
            recordNode.AddChild(new System.Security.SecurityElement("ResSize", item.resSize.ToString()));
            recordNode.AddChild(new System.Security.SecurityElement("ResRequire", "false"));
        }

        return root.ToString();
    }
}

public class VersionManager2 : Singleton<VersionManager2>
{
    static public string VersionInfoFilePath = "VersionInfo.xml"; //版本配置文件路径

    //获取本地版本信息
    public VersionInfo2 GetLocalVersionInfo()
    {
        //获取安装包中版本信息
        string innerText = MyFileUtil.ReadConfigDataInStreamingAssets(VersionInfoFilePath);
        VersionInfo2 innerVersionInfo = VersionInfo2.ParseData(innerText);

        //外部版本信息
        string outText = MyFileUtil.ReadConfigDataInCacheDir(VersionInfoFilePath);
        if (outText != null)
        {
            VersionInfo2 outVersionInfo = VersionInfo2.ParseData(outText);
            outVersionInfo.ProgramVersion = innerVersionInfo.ProgramVersion;
            return outVersionInfo;
        }

        return innerVersionInfo;
    }

    public VersionInfo2 GetInnerVersionInfo()
    {
        string innerText = MyFileUtil.ReadConfigDataInStreamingAssets(VersionInfoFilePath);
        VersionInfo2 innerVersionInfo = VersionInfo2.ParseData(innerText);
        return innerVersionInfo;
    }

    private VersionInfo2 m_InnerVersionInfo = null;

    public VersionInfo2 InnerVersionInfo
    {
        get
        {
            if (m_InnerVersionInfo == null)
            {
                m_InnerVersionInfo = GetInnerVersionInfo();
            }
            return m_InnerVersionInfo;
        }
    }

    //-----------------------------------------------------------------------------------//

    //检查安装包中版本号和本地版本号
    public void CheckInstallationPackageVersionWithLocal()
    {
        string outText = MyFileUtil.ReadConfigDataInCacheDir(VersionInfoFilePath);
        if (outText == null)
        {
            return;
        }

        //判断本地版本号和包体内部版本号
        string innerText = MyFileUtil.ReadConfigDataInStreamingAssets(VersionInfoFilePath);
        VersionInfo2 innerVersionInfo = VersionInfo2.ParseData(innerText);

        VersionInfo2 outVersionInfo = VersionInfo2.ParseData(outText);

        if (innerVersionInfo.ProgramVersion > outVersionInfo.ProgramVersion)
        {
            //清空本地资源
            MyFileUtil.DeleteDir(MyFileUtil.CacheDir);
            MyFileUtil.CreateDir(MyFileUtil.CacheDir);
        }

        /*
        foreach(var item in innerVersionInfo.dictRes)
        {
            if(outVersionInfo.dictRes.ContainsKey(item.Key))
            {
                ResInfo outResInfo = outVersionInfo.dictRes[item.Key];
            }
        }
        */

        Debug.Log("VersionManager.CheckLocalLuaCodeVersion");
    }

    //检查服务器和本地版本号
    public void CheckLocalVersionInfoWithServer(System.Action<bool> updateFinish)
    {
        System.Action<string> getServerVersionFinish = delegate (string data)
        {
            //服务器版本
            VersionInfo2 serverVersionInfo = VersionInfo2.ParseData(data);

            //本地版本
            VersionInfo2 localVersionInfo = GetLocalVersionInfo();

            //苹果商店版本
            if (InnerVersionInfo.IsAppleAppStore && serverVersionInfo.IsOpenAutoUpdateInAppStore == false)
            {
                updateFinish(true);
                return;
            }

            if (localVersionInfo.ProgramVersion < serverVersionInfo.ProgramVersion)
            {
                //整个客户端需要更新
                System.Action clickAction = delegate ()
                {
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        if (InnerVersionInfo.IsAppleAppStore)
                        {
                            Application.OpenURL(serverVersionInfo.IOSAppStoreUrl);
                        }
                        else
                        {
                            Application.OpenURL(serverVersionInfo.IOSAppUrl);
                        }
                    }
                    else
                    {
                        Application.OpenURL(serverVersionInfo.ApkUrl);
                    }
                };

                //UIMsgBox.Instance.ShowMsgBoxOK(LanguageConfig.WordUpdate, "有新客户端发布了，点击按钮进行更新", "更新", clickAction, false);
                UIMsgBox.Instance.ShowMsgBoxOK(LanguageConfig.WordUpdate, LanguageConfig.GetText(3), LanguageConfig.WordUpdate, clickAction, false);
                Debug.Log("提示更新整包");
                return;
            }

            //计算需要更新的资源
            List<ResInfo> listResInfo = new List<ResInfo>();
            foreach(var item in serverVersionInfo.dictRes)
            {
                if(localVersionInfo.dictRes.ContainsKey(item.Key))
                {
                    ResInfo localResInfo = localVersionInfo.dictRes[item.Key];
                    Debug.Log("local" + localResInfo.resMD5 + "server" + item.Value.resMD5);
                    if(string.Compare(item.Value.resMD5, localResInfo.resMD5, true) != 0)
                    {
                        listResInfo.Add(item.Value);
                    }
                }
                else
                {
                    //本地没有
                    if(item.Value.isResRequire)
                    {
                        listResInfo.Add(item.Value);
                    }
                }
            }

            if (listResInfo.Count != 0)
            {
                //更新Lua脚本和资源
                DownLoadRes(listResInfo, updateFinish);
            }
            else
            {
                updateFinish(true);
            }
        };

        //获取服务器版本信息
        ServerURLManager.GetVersionData(getServerVersionFinish);
        Debug.Log("获取服务器版本信息");
    }

    //资源下载
    public void DownLoadRes(List<ResInfo> listResInfo, System.Action<bool> updateFinish)
    {
        if(Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            //提示非wifi情况下更新 提示下载数据大小
            long totalSize = 0;
            foreach (var item in listResInfo)
            {
                totalSize += item.resSize;
            }

            System.Action<bool> onClick = delegate (bool result)
            {
                if(result)
                {
                    //下载更新
                    StartDownLoadResFile(listResInfo, updateFinish);
                }
                else
                {
                    //退出游戏
                    Application.Quit();
                }
            };

            //提示处于移动网络，是否继续更新
            float downSize = totalSize / (1024.0f * 1024.0f); //换算为mb
            string tips = string.Format(LanguageConfig.GetText(4), downSize);
            UIMsgBox.Instance.ShowMsgBoxOKCancel(LanguageConfig.WordUpdate, tips, LanguageConfig.WordUpdate, LanguageConfig.WordCancel, onClick);
        }
        else
        {
            //wifi下自动更新
            StartDownLoadResFile(listResInfo, updateFinish);
        }
    }

    private int m_Current = 0;  //当前下载项目
    private List<ResInfo> m_ListResInfo = null;
    private System.Action<bool> m_UpdateFinish = null;
    private VersionInfo2 m_LocalVersionInfo = null;

    void StartDownLoadResFile(List<ResInfo> listResInfo, System.Action<bool> updateFinish)
    {
        m_Current = 0;
        m_ListResInfo = listResInfo;
        m_UpdateFinish = updateFinish;

        //本地版本信息
        m_LocalVersionInfo = GetLocalVersionInfo();

        DownLoadResItem();
    }

    void DownLoadResItem()
    {
        if (m_Current < m_ListResInfo.Count)
        {
            ResInfo item = m_ListResInfo[m_Current];
            ScriptThread.Instance.StartCoroutine(DownLoadResItemImp(item, m_ListResInfo.Count, m_Current, DownLoadResItemFinish));
        }
        else
        {
            m_UpdateFinish(true);
        }
    }

    IEnumerator DownLoadResItemImp(ResInfo item, int totalCount, int current, System.Action<bool, string, ResInfo> updateFinish)
    {
        //WWW www = new WWW(item.resURL);
        WWW www = HTTPTool.GetWWW(item.resURL);

        //ui提示
        UIWindowUpdate.Instance.ShowDownloadTips(totalCount, current + 1, item.resName, www);

        yield return www;

        if (string.IsNullOrEmpty(www.error) == false)
        {
            //下载出错
            Debug.LogError(www.error + item.resURL);
            //updateFinish(false, "资源下载失败，请点击重试", item);
            updateFinish(false, LanguageConfig.GetText(5), item);
            yield break;
        }
        else
        {
            UIWindowUpdate.Instance.ShowVerifyTips();

            
            if (MD5Tool.Verify(www.bytes, item.resMD5))
            {
                //解压文件--下载成功
                UIWindowUpdate.Instance.ShowUnZipTips();
                ZIPTool.DecompressToDirectory(www.bytes, MyFileUtil.CacheDir);
                updateFinish(true, "", item);
            }
            else
            {
                //md5 匹配不上
                string str = string.Format("VersionManager.DownLoadResImp:资源{0} md5出错 severStrmd5:{1} severResmd5:{2}", item.resURL,item.resMD5, MD5Tool.Get(www.bytes));
                Debug.LogError(str);

                str = "details:" + str;
                //updateFinish(false, "资源校验失败，md5值不匹配，请点击重新下载", item);
                updateFinish(false, LanguageConfig.GetText(6) + str, item);
                yield break;
            }
        }
    }

    void DownLoadResItemFinish(bool result, string errorInfo, ResInfo item)
    {
        if (result)
        {
            //更新版本信息
            if (m_LocalVersionInfo.dictRes.ContainsKey(item.resName))
            {
                m_LocalVersionInfo.dictRes[item.resName].resMD5 = item.resMD5;
            }
            else
            {
                m_LocalVersionInfo.dictRes.Add(item.resName, item);
            }

            //保存版本信息
            SaveLocalVersionInfo(m_LocalVersionInfo);

            ++m_Current;
            if (m_Current == m_ListResInfo.Count)
            {
                m_UpdateFinish(true);
            }
            else
            {
                DownLoadResItem();
            }
        }
        else
        {
            System.Action<bool> onClick = delegate (bool isClickOK)
            {
                if(isClickOK)
                {
                    if (Application.internetReachability == NetworkReachability.NotReachable)
                    {
                        //更新失败，先检查网络
                        UIFloatingMsgBox.Instance.ShowText(LanguageConfig.GetText(12));
                    }
                    else
                    {
                        UIMsgBox.Instance.HideMsgBox();
                        DownLoadResItem();
                    }
                }
                else
                {
                    Application.Quit();
                }
            };

            //错误提示
            UIMsgBox.Instance.ShowMsgBoxOKCancel(LanguageConfig.WordUpdate, errorInfo, LanguageConfig.GetText(13), LanguageConfig.GetText(14), onClick, false);
        }
    }

    //保存版本信息
    public void SaveLocalVersionInfo(VersionInfo2 versionInfo)
    {
        string data = VersionInfo2.Serialize(versionInfo);
        MyFileUtil.WriteConfigDataInCacheDir(VersionInfoFilePath, data);
    }

    //检查是否需要更新--外部接口
    public void UpdateGame(System.Action<bool> updateFinish)
    {
        //先检查安装包中的版本号和本地版本号
        CheckInstallationPackageVersionWithLocal();

        //检查本地版本号和服务器版本号
        CheckLocalVersionInfoWithServer(updateFinish);
    }

    //-----------------------------------------------------------------------------------//

    public void GetServerVersionInfo(Action<VersionInfo2> onLoad)
    {
        System.Action<string> onLoadText = delegate (string data)
        {
            //服务器版本
            VersionInfo2 serverVersionInfo = VersionInfo2.ParseData(data);
            onLoad(serverVersionInfo);
        };
        ServerURLManager.GetVersionData(onLoadText);
    }

    public void DownLoadSingleResItem(ResInfo info, Action<bool> onLoad)
    {
        List<ResInfo> list = new List<ResInfo>();
        list.Add(info);
        DownLoadRes(list, onLoad);
    }
}
