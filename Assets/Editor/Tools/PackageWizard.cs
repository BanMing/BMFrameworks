using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

public class PackageWizard : ScriptableWizard
{
    public string serverAddress;
    public string version;
    public static PackageWizard m_wizard;
    [MenuItem("Build/打包向导")]
    static void CreateWindow()
    {
        m_wizard = ScriptableWizard.DisplayWizard<PackageWizard>("打包向导", "打包");

        //version
        string version;
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                version = Consts.AndroidVersion;
                break;
            case BuildTarget.iOS:
                version = Consts.IOSVersion;
                break;
            default:
                version = Consts.AndroidVersion;
                break;
        }
        version += "." + SVNUtils.GetSVNRevision();
        m_wizard.version = version;

        //address
        string url= ServerConfig.Instance.CfgMapURL;
        string domainRegex = @"http\:\/\/(.*?)\/";
        Match match = Regex.Match(url, domainRegex);
        string domain = match.Groups[1].Value;
        Debug.Log("domain:" + domain);
		m_wizard.serverAddress = "http://" + domain + "/GameResources/" + MyUnityEditorTool.GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget) + "/" + m_wizard.version + "/";
    }

    void OnWizardUpdate()
    {
    }

    void OnWizardCreate()
    {
        AssetBundlesEditorTools.PrepareForBuildImp();
    }
}
