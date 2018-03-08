using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LuaInterface;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class AssetBundleEditor {
    // static string LuaFilePath = Application.streamingAssetsPath + "/Lua";
    static string LuaFilePath = Application.streamingAssetsPath + "/" + LuaConst.osDir;
    [MenuItem("Bulid/TestBuildAssetBundle")]
    static void TestBuildAssetBundle () {
        if (!File.Exists(LuaFilePath))
        {
            MyFileUtil.CreateDir(LuaFilePath);
        }
        BuildPipeline.BuildAssetBundles(LuaFilePath,BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}