using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using LuaInterface;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class AssetBundleEditor {
    struct MyAssetBundleName {
        public const string LuaCode = "Lua/luacode";
    }

    [MenuItem ("Build/BuildLuaTest")]
    static void BuildLuaTest () {

        ClearEditorLuaBytesFiles ();

        SetLuaBytesFile ();

        SetLuaAssetBundleName ();

        BuildAssetBundle ();

        BuildLocalLuaZip ();
    }

    [MenuItem ("Build/BuildManifestZip")]
    static void BuildManifestZip () {
        List<string> files=new List<string>(){PathManager.AssetsBunldePath+LuaConst.osDir,PathManager.AssetsBunldePath+LuaConst.osDir+".manifest"};
         ZIPTool.CompressFiles (files, Application.streamingAssetsPath,PathManager.AssetsBunldePath+"manifest.zip", 0, false, true);
        AssetDatabase.Refresh ();
    }

    [MenuItem ("Build/LocalBuild/BuildAssetBundle")]
    static void BuildAssetBundle () {
        if (!File.Exists (PathManager.AssetsBunldePath)) {
            MyFileUtil.CreateDir (PathManager.AssetsBunldePath);
        }
        BuildPipeline.BuildAssetBundles (PathManager.AssetsBunldePath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh ();
    }

    [MenuItem ("Build/LocalBuild/Lua/BuildLocalLuaZip")]
    static void BuildLocalLuaZip () {
        List<string> files = new List<string> ();
        MyFileUtil.GetFileList (PathManager.AssetsBunldePath + "Lua", ref files, null, ".meta");
        ZIPTool.CompressFiles (files, Application.streamingAssetsPath, PathManager.LuaZipPath, 0, false, true);
        AssetDatabase.Refresh ();
    }

    [MenuItem ("Build/LocalBuild/Lua/SetLuaBytesFile")]
    static void SetLuaBytesFile () {

        List<string> fileList = new List<string> ();
        MyFileUtil.GetFileList (PathManager.EditorLuaFilePath, ref fileList, "lua");
        foreach (var item in fileList) {
            var str = MyFileUtil.ReadFileText (item);
            var data = Encoding.Unicode.GetBytes (str);
            var newName = item + ".bytes";
            MyFileUtil.WriteFile (newName, data);
        }
        MyFileUtil.WriteFile (Application.dataPath + "/temp.txt", "ssss");
        Debug.Log ("Create Lua Bytes Files Over!");
        AssetDatabase.Refresh ();
    }

    [MenuItem ("Build/LocalBuild/Lua/SetLuaAssetBundleName")]
    static void SetLuaAssetBundleName () {

        List<string> fileList = new List<string> ();
        MyFileUtil.GetFileList (PathManager.EditorLuaFilePath, ref fileList, "lua");
        foreach (var item in fileList) {
            var data = MyFileUtil.ReadFileBytes (item);
            var newName = item + ".bytes";
            var newPath = newName.Substring (newName.IndexOf ("Assets"));
            AssetImporter.GetAtPath (newPath).assetBundleName = MyAssetBundleName.LuaCode;
        }
        AssetImporter.GetAtPath ((Application.dataPath + "/temp.txt").Substring ((Application.dataPath + "/temp.txt").IndexOf ("Assets"))).assetBundleName = MyAssetBundleName.LuaCode;
        Debug.Log ("Set LuaBytes AssetBundleName Over!");
        AssetDatabase.Refresh ();
    }

    [MenuItem ("Build/LocalBuild/Lua/ClearEditorLuaBytesFiles")]
    static void ClearEditorLuaBytesFiles () {
        List<string> fileList = new List<string> ();
        MyFileUtil.GetFileList (PathManager.EditorLuaFilePath, ref fileList, "lua.bytes");
        foreach (var item in fileList) {
            MyFileUtil.DeleteFile (item);
        }
        MyFileUtil.DeleteFile (Application.dataPath + "/temp.txt");
        Debug.Log ("Clear LuaBytes Files Over!");
        AssetDatabase.Refresh ();
    }
}