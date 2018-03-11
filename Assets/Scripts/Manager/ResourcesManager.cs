using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
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
        // #if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //         return;
        // #endif
        // string dir = MyFileUtil.CacheDir + LuaConst.osDir + "/" + MyFileUtil.LuaZipFileName;
        // if (File.Exists (dir)) {
        //     return;
        // }
        // if (File.Exists (MyFileUtil.StreamingAssetsPath + "/Lua/Main.lua")) {
        //     Debug.Log ("@@@@@@@@@@@@@@@@@@@@@@@");
        // }
        // if (File.Exists (LuaConst.luaDir)) {
        //     Debug.Log ("22222@@@@@");
        // }
        // if (File.Exists (MyFileUtil.StreamingAssetsPath)) {
        //     Debug.Log ("@@333333@@@@@@@@@@");
        // }
        // if (File.Exists (Application.streamingAssetsPath)) {
        //     Debug.Log ("5555555555555@@@@@@@@@@");
        // }
        // Debug.Log ("MyFileUtil.StreamingAssetsPath" + MyFileUtil.StreamingAssetsPath);
        // Debug.Log ("LuaConst.luaDir" + LuaConst.luaDir);
        // Debug.Log ("MyFileUtil.CacheDir" + MyFileUtil.CacheDir);
        // Debug.Log ("Application.streamingAssetsPath" + Application.streamingAssetsPath);
        // MyFileUtil.CopyFile (LuaConst.luaDir+"/Main.lua", LuaConst.luaResDir+"/Main.lua");
        // MyFileUtil.CopyFile (MyFileUtil.StreamingAssetsPath, MyFileUtil.CacheDir);

        // HTTPTool.GetText (MyFileUtil.StreamingAssetsPath + "Lua/mmm.txt", (str) => { Debug.Log ("mmm.txt:" + str);  });
        // GameCenter.Instance.StartCoroutine (GetMainLua (MyFileUtil.StreamingAssetsPath + "Lua/mmm.txt"));
        // GameCenter.Instance.StartCoroutine (GetMainLua (MyFileUtil.StreamingAssetsPath + "Lua/Main.lua"));
        // Debug.Log ("CopyFile Over!");
        // string strPath=(MyFileUtil.StreamingAssetsPath + "Lua/Main.lua").Replace("/",@"\");
        // GameCenter.Instance.StartCoroutine(GetMainLua(strPath));
        // if (File.Exists (MyFileUtil.CacheDir + "Lua/mmm.txt")) { Debug.Log ("@@###cacheDir:" + File.ReadAllText (MyFileUtil.CacheDir + "Lua/mmm.txt")); }

        // GameCenter.Instance.StartCoroutine(GetMainLua(MyFileUtil.CacheDir + "Lua/mmm.txt"));
        GameCenter.Instance.StartCoroutine (GetLuaCodeZip (Application.streamingAssetsPath + "/" + LuaConst.osDir + "/code.zip",callback));
    }
    IEnumerator GetMainLua (string path) {
        Debug.Log ("GetMainLuapath:" + path);
        WWW www = new WWW (path);
        yield return www;
        Debug.Log ("Main.Lua:" + www.text);
        yield break;
    }
    IEnumerator GetLuaCodeZip (string path,Action callback) {
        Debug.Log ("GetLuaCodeZippath:" + path);
        WWW www = new WWW (path);
        yield return www;
        // Debug.Log ("GetLuaCodeZip.byte:" + www.bytes);
        ZIPTool.DecompressToDirectory (www.bytes, MyFileUtil.CacheDir);
        if (File.Exists (MyFileUtil.CacheDir + "Lua/mmm.txt")) {
            //  Debug.Log ("555#cacheDir:" + File.ReadAllText (MyFileUtil.CacheDir + "Lua/mmm.txt"));
            Debug.Log ("MyFileUtil.CacheDir Tolua.lua path:" + MyFileUtil.CacheDir + "Lua/ToLua/tolua.lua");
            Debug.Log ("MyFileUtil.CacheDir Tolua.lua path:" + File.ReadAllText (MyFileUtil.CacheDir + "Lua/ToLua/tolua.lua"));
            if (callback!=null){
                callback();
            }
            
        }
        yield break;
    }
}