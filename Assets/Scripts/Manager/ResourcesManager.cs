using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public sealed class ResourcesManager : SingletonObject<ResourcesManager> {
    private ResourcesManager () { }
    public static UnityEngine.GameObject GetInstanceGameOject (string path) {
        // var obj= Resources.Load<GameObject>(path);
        // Debug.Log(obj.name);
        // GameObject gameObject= GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(path));

        return GameObject.Instantiate<GameObject> (Resources.Load<GameObject> (path));
    }

    /// <summary>
    /// 第一次运行把Streming中资源移动到沙盒中
    /// </summary>
    public void MoveStreaming2Cache (Action callback) {
        Debug.Log ("CopyFile start!");
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
        // GameCenter.Instance.StartCoroutine(GetMainLua(MyFileUtil.CacheDir + "Lua/mmm.txt"));
        GameCenter.Instance.StartCoroutine (GetLuaCodeZip (PathManager.LuaZipPath, callback));
    }
    IEnumerator GetMainLua (string path) {
        Debug.Log ("GetMainLuapath:" + path);
        WWW www = new WWW (path);
        yield return www;
        Debug.Log ("Main.Lua:" + www.text);
        yield break;
    }
    IEnumerator GetLuaCodeZip (string path, Action callback) {
        Debug.Log ("GetLuaCodeZippath:" + path);
        WWW www = new WWW (path);
        yield return www;
        // Debug.Log ("GetLuaCodeZip.byte:" + www.bytes);
        ZIPTool.DecompressToDirectory (www.bytes, MyFileUtil.CacheDir);
        WWW mm = new WWW (PathManager.AssetsBunldePath + "manifest.zip");
        yield return mm;
        ZIPTool.DecompressToDirectory (mm.bytes, MyFileUtil.CacheDir);
        yield return null;
        WWW cc = new WWW (PathManager.CacheDir + "AssetsBundles/Android/Android");
        yield return cc;
        AssetBundleManifest manifest = (AssetBundleManifest) cc.assetBundle.LoadAsset ("AssetBundleManifest");
        List<string> list = new List<string> (manifest.GetAllAssetBundles ());
        Debug.Log ("manifest AllAssetBundles count:" + list.Count);
        foreach (var str in list)
        {
            Debug.Log("AssetBundles Name:"+str);
            WWW dd=new WWW(PathManager.CacheDir + "AssetsBundles/Android/"+str);
            yield return dd;
            
        }
        yield return null;
        callback ();
        // if (File.Exists (MyFileUtil.CacheDir + "Lua/mmm.txt")) {
        //     //  Debug.Log ("555#cacheDir:" + File.ReadAllText (MyFileUtil.CacheDir + "Lua/mmm.txt"));
        //     Debug.Log ("MyFileUtil.CacheDir Tolua.lua path:" + MyFileUtil.CacheDir + "Lua/ToLua/tolua.lua");
        //     Debug.Log ("MyFileUtil.CacheDir Tolua.lua path:" + File.ReadAllText (MyFileUtil.CacheDir + "Lua/ToLua/tolua.lua"));
        //     if (callback != null) {
        //         callback ();
        //     }
        // }
        yield break;
    }

    public string ReadConfig (string path) {
        return MyFileUtil.ReadFileText (path);
    }

    public void WriteConfig (string path, string data) {
        MyFileUtil.WriteFile (path, data);
    }
}