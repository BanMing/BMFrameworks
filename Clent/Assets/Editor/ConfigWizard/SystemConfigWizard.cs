using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Security.Cryptography;
using System.Xml;
using System.Collections.Generic;

public class SystemConfigWizard : ScriptableWizard
{
    public static SystemConfigWizard m_wizard;

    public bool IsUseAssetBundle;
    public bool IsLuaUseZip;
    public bool IsEncryptLuaCode;
    public bool IsUseLuaBytecode;
    public bool IsAutoUpdate;
    public bool IsEncryptConfigFile;
    public bool IsShowLog;
    public bool IsLog;
    public bool IsCanLoginByNormalUser;


    [MenuItem("Config/SystemConfig")]
    static void CreateWindow()
    {
        m_wizard = ScriptableWizard.DisplayWizard<SystemConfigWizard>("Systemconfig", "Apply");

        m_wizard.IsUseAssetBundle = SystemConfig.Instance.IsUseAssetBundle;
        m_wizard.IsLuaUseZip = SystemConfig.Instance.IsLuaUseZip;
        m_wizard.IsEncryptLuaCode = SystemConfig.Instance.IsEncryptLuaCode;
        m_wizard.IsUseLuaBytecode = SystemConfig.Instance.IsUseLuaBytecode;
        m_wizard.IsAutoUpdate = SystemConfig.Instance.IsAutoUpdate;
        m_wizard.IsEncryptConfigFile = SystemConfig.Instance.IsEncryptConfigFile;
        m_wizard.IsShowLog = SystemConfig.Instance.IsShowLog;
        m_wizard.IsLog = SystemConfig.Instance.IsLog;
        m_wizard.IsCanLoginByNormalUser = SystemConfig.Instance.IsCanLoginByNormalUser;
    }


    void OnWizardCreate()
    {
        XmlDocument xmlDoc = new XmlDocument();

        //root
        XmlElement root = xmlDoc.CreateElement("root");
        xmlDoc.AppendChild(root);

        //record
        XmlElement record = xmlDoc.CreateElement("record");
        root.AppendChild(record);


        //content
        XmlElement id = xmlDoc.CreateElement("id");
        id.InnerText = "0";
        record.AppendChild(id);

        XmlElement IsUseAssetBundle = xmlDoc.CreateElement("IsUseAssetBundle");
        IsUseAssetBundle.InnerText = m_wizard.IsUseAssetBundle.ToString();
        record.AppendChild(IsUseAssetBundle);
        SystemConfig.Instance.IsUseAssetBundle = m_wizard.IsUseAssetBundle;

        XmlElement IsLuaUseZip = xmlDoc.CreateElement("IsLuaUseZip");
        IsLuaUseZip.InnerText = m_wizard.IsLuaUseZip.ToString();
        record.AppendChild(IsLuaUseZip);
        SystemConfig.Instance.IsLuaUseZip = m_wizard.IsLuaUseZip;

        XmlElement IsEncryptLuaCode = xmlDoc.CreateElement("IsEncryptLuaCode");
        IsEncryptLuaCode.InnerText = m_wizard.IsEncryptLuaCode.ToString();
        record.AppendChild(IsEncryptLuaCode);
        SystemConfig.Instance.IsEncryptLuaCode = m_wizard.IsEncryptLuaCode;

        XmlElement IsUseLuaBytecode = xmlDoc.CreateElement("IsUseLuaBytecode");
        IsUseLuaBytecode.InnerText = m_wizard.IsUseLuaBytecode.ToString();
        record.AppendChild(IsUseLuaBytecode);
        SystemConfig.Instance.IsUseLuaBytecode = m_wizard.IsUseLuaBytecode;


        XmlElement IsAutoUpdate = xmlDoc.CreateElement("IsAutoUpdate");
        IsAutoUpdate.InnerText = m_wizard.IsAutoUpdate.ToString();
        record.AppendChild(IsAutoUpdate);
        SystemConfig.Instance.IsAutoUpdate = m_wizard.IsAutoUpdate;

        XmlElement IsEncryptConfigFile = xmlDoc.CreateElement("IsEncryptConfigFile");
        IsEncryptConfigFile.InnerText = m_wizard.IsEncryptConfigFile.ToString();
        record.AppendChild(IsEncryptConfigFile);
        SystemConfig.Instance.IsEncryptConfigFile = m_wizard.IsEncryptConfigFile;

        XmlElement IsShowLog = xmlDoc.CreateElement("IsShowLog");
        IsEncryptConfigFile.InnerText = m_wizard.IsShowLog.ToString();
        record.AppendChild(IsShowLog);
        SystemConfig.Instance.IsShowLog = m_wizard.IsShowLog;

        XmlElement IsLog = xmlDoc.CreateElement("IsLog");
        IsEncryptConfigFile.InnerText = m_wizard.IsLog.ToString();
        record.AppendChild(IsLog);
        SystemConfig.Instance.IsLog = m_wizard.IsLog;

        XmlElement IsCanLoginByNormalUser = xmlDoc.CreateElement("IsCanLoginByNormalUser");
        IsEncryptConfigFile.InnerText = m_wizard.IsCanLoginByNormalUser.ToString();
        record.AppendChild(IsCanLoginByNormalUser);
        SystemConfig.Instance.IsEncryptConfigFile = m_wizard.IsCanLoginByNormalUser;

        string outputPath = MyFileUtil.InnerConfigDir + SystemConfig.fileName;
        xmlDoc.Save(outputPath);
        Debug.Log("systemconfig修改完成");
    }
}
