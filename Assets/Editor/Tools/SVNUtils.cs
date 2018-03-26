using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// SVN工具
/// </summary>
public class SVNUtils
{
    private static List<string> drives = new List<string>() { "c:", "d:", "e:", "f:" };
    private static string svnPath = @"\Program Files\TortoiseSVN\bin\";
    private static string svnProc = @"TortoiseProc.exe";
    private static string svnProcPath = "";

    [MenuItem("SVNTool/SVN更新 %&e")]
    public static void UpdateFromSVN()
    {
        if (string.IsNullOrEmpty(svnProcPath))
            svnProcPath = GetSvnProcPath();
        var dir = new DirectoryInfo(Application.dataPath);
        var path = dir.Parent.FullName.Replace('/', '\\');
        var para = "/command:update /path:\"" + path + "\" /closeonend:0";
        System.Diagnostics.Process.Start(svnProcPath, para);
    }

    [MenuItem("SVNTool/SVN提交 %&r")]
    public static void CommitToSVN()
    {
        if (string.IsNullOrEmpty(svnProcPath))
            svnProcPath = GetSvnProcPath();
        var path = Application.dataPath.Replace('/', '\\');
        var para = "/command:commit /path:\"" + path + "\"";
        System.Diagnostics.Process.Start(svnProcPath, para);
    }

    [MenuItem("SVNTool/回滚本地修改（更新时看到红色字请点我！） %&t")]
    public static void RevertFromSVN()
    {
        if (string.IsNullOrEmpty(svnProcPath))
            svnProcPath = GetSvnProcPath();
        var path = Application.dataPath.Replace('/', '\\');
        var para = "/command:revert /path:\"" + path + "\"";
        System.Diagnostics.Process.Start(svnProcPath, para);
    }

    [MenuItem("SVNTool/SVN更新策划数据 %&i")]
    public static void UpdateDataFromSVN()
    {
        if (string.IsNullOrEmpty(svnProcPath))
            svnProcPath = GetSvnProcPath();
        var path = (Application.dataPath + "/Resources/data/xml").Replace('/', '\\');
        var para = "/command:update /path:\"" + path + "\" /closeonend:0";
        System.Diagnostics.Process.Start(svnProcPath, para);
    }

    [MenuItem("SVNTool/SVN添加 %&u")]
    public static void AddToSVN()
    {
        if (string.IsNullOrEmpty(svnProcPath))
            svnProcPath = GetSvnProcPath();
        var path = Application.dataPath.Replace('/', '\\');
        var para = "/command:add /path:\"" + path + "\"";
        System.Diagnostics.Process.Start(svnProcPath, para);
    }

    [MenuItem("SVNTool/清理SVN %&y")]
    public static void CleanUpFromSVN()
    {
        if (string.IsNullOrEmpty(svnProcPath))
            svnProcPath = GetSvnProcPath();
        var path = Application.dataPath.Replace('/', '\\');
        var para = "/command:cleanup /path:\"" + path + "\"";
        System.Diagnostics.Process.Start(svnProcPath, para);
    }

    [MenuItem("SVNTool/获取SVN Info")]
    public static string GetSVNInfo()
    {
        string info;
        string rootPath = MyFileUtil.GetParentDir(Application.dataPath);
        var output = CMDTool.ProcessCommand("svn", "info " + rootPath);
        return output;
    }

    [MenuItem("SVNTool/获取SVN Version")]
    public static string GetSVNRevision()
    {
        string RevisionRegexStr = @"Last\sChanged\sRev\:\s(.+)\n";
        string info = GetSVNInfo();
        Match match = Regex.Match(info, RevisionRegexStr);
        string version = match.Groups[1].Value;
        version = version.Replace("\r", "");
        return version;
    }

    private static string GetSvnProcPath()
    {
        foreach (var item in drives)
        {
            var path = string.Concat(item, svnPath, svnProc);
            if (File.Exists(path))
                return path;
        }
        return EditorUtility.OpenFilePanel("Select TortoiseProc.exe", "c:\\", "exe");
    }
}