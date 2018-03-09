using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LuaInterface;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class AssetBundleEditor {
    static string LocalLuaFilePath = Application.streamingAssetsPath + "/Lua";
    static string LuaFilePath = Application.streamingAssetsPath + "/" + LuaConst.osDir+"/code.zip";
    [MenuItem("Bulid/TestBuildAssetBundle")]
    static void TestBuildAssetBundle () {
        if (!File.Exists(LuaFilePath))
        {
            MyFileUtil.CreateDir(LuaFilePath);
        }
        BuildPipeline.BuildAssetBundles(LuaFilePath,BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }
    [MenuItem("Bulid/TestBuildLuaBundle")]
    static void TestBuildLuaBundle(){
        if (!File.Exists(Application.streamingAssetsPath + "/" + LuaConst.osDir))
        {
            MyFileUtil.CreateDir(Application.streamingAssetsPath + "/" + LuaConst.osDir);
            var stream= File.Create(LuaFilePath);
            stream.Close();
        }
        ZIPTool.CompressDirectory(LocalLuaFilePath,LuaFilePath);
        AssetDatabase.Refresh();
    }
}