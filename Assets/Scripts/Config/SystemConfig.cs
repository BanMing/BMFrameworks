/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/10/14
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System.Collections;

public class SystemConfig : GameData<SystemConfig>
{
    static public readonly string fileName = "SystemConfig.xml";

    public bool IsUseAssetBundle { set; get; }
    public bool IsLuaUseZip { set; get; }
    public bool IsEncryptLuaCode { set; get; }
    public bool IsUseLuaBytecode { set; get; }
    public bool IsAutoUpdate { set; get; }
    public bool IsEncryptConfigFile { set; get; }

    public bool IsShowLog { set; get; }

    public bool CanLoginManual { set; get; } //是否显示登陆注册对话框

    public bool IsOpenHeartBeatCheck { set; get; } //是否开启心态检测

    private static bool IsInit = false;

    public static SystemConfig Instance
    {
        get
        {
            if(IsInit == false)
            {
                byte[] data = MyFileUtil.ReadConfigDataInStreamingAssetsImp(fileName);
                if(data == null)
                {
                    //解密
                    data = MyFileUtil.ReadConfigDataInStreamingAssetsImp(fileName + MyFileUtil.EncryptXMLFileSuffix);
                    data = DESCrypto.Decrypt(data, MyFileUtil.EncryptKey);
                }

                string str = System.Text.UTF8Encoding.UTF8.GetString(data);
                SystemConfig.LoadFromText(str, fileName);

                IsInit = true;
            }

            return dataMap[0];
        }
    }
}
