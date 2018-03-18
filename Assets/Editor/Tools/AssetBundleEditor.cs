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
    static string LocalLuaFilePath = Application.streamingAssetsPath + "/Lua";
    static string LuaFilePath = Application.streamingAssetsPath + "/" + LuaConst.osDir;
    [MenuItem ("Bulid/TestBuildAssetBundle")]
    static void TestBuildAssetBundle () {
        if (!File.Exists (LuaFilePath)) {
            MyFileUtil.CreateDir (LuaFilePath);
        }
        BuildPipeline.BuildAssetBundles (LuaFilePath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh ();
    }

    [MenuItem ("Bulid/TestBuildLuaBundle")]
    static void TestBuildLuaBundle () {
        if (!File.Exists (Application.streamingAssetsPath + "/" + LuaConst.osDir)) {
            MyFileUtil.CreateDir (Application.streamingAssetsPath + "/" + LuaConst.osDir);
            var stream = File.Create (LuaFilePath);
            stream.Close ();
        }
        ZIPTool.CompressDirectory (LocalLuaFilePath, LuaFilePath);
        AssetDatabase.Refresh ();
    }

    [MenuItem ("Bulid/BuildLuaAssetBundle")]
    static void BuildLuaAssetBundle () {

        List<string> fileList = new List<string> ();
        MyFileUtil.GetFileList (LocalLuaFilePath, ref fileList, "lua");
        foreach (var item in fileList) {
            var str = MyFileUtil.ReadFileText (item);
            var data= Encoding.Unicode.GetBytes(str);
            var newName = item + ".bytes";
            MyFileUtil.WriteFile (newName, data);

        }
        Debug.Log("Create Lua Bytes Files Over!");
        AssetDatabase.Refresh ();
    }

    [MenuItem ("Bulid/SetLuaAssetBundleName")]
    static void SetLuaAssetBundleName () {

        List<string> fileList = new List<string> ();
        MyFileUtil.GetFileList (LocalLuaFilePath, ref fileList, "lua");
        foreach (var item in fileList) {
            var data = MyFileUtil.ReadFileBytes (item);
            var newName = item + ".bytes";
            var newPath = newName.Substring (newName.IndexOf ("Assets"));
            AssetImporter.GetAtPath (newPath).assetBundleName = "unity3d";
        }
        AssetDatabase.Refresh ();
    }

    [MenuItem ("Bulid/ClearEditorLuaBytesFiles")]
    static void ClearEditorLuaBytesFiles () {
        string luaFolderPath = Application.streamingAssetsPath + "/Lua";
        List<string> fileList = new List<string> ();
        MyFileUtil.GetFileList (luaFolderPath, ref fileList, "lua.bytes");
        foreach (var item in fileList) {
            MyFileUtil.DeleteFile (item);
        }
        Debug.Log ("Clear LuaBytes Files Over!");
        AssetDatabase.Refresh ();
    }
}