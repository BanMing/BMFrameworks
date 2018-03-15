using System.IO;
using UnityEngine;
public class PathManager {
    static string cacheDir = string.Empty;

    static string streamingAssetPath2WWW = string.Empty;

    static string luaFolderEditorPath = string.Empty;

    static string luaZipPath = string.Empty;

    static string assetsBunldePath = string.Empty;

    static string forAssetBunldePath = string.Empty;

    static string systemConfigPath=string.Empty;

    public static string StreamingAssetPath2WWW {
        get {
            if (streamingAssetPath2WWW == string.Empty) {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                streamingAssetPath2WWW = "file:///" + Application.streamingAssetsPath + "/";
#else
                streamingAssetPath2WWW = Application.streamingAssetsPath + "/";
#endif
            }
            return streamingAssetPath2WWW;
        }
    }

    public static string CacheDir {
        get {
            if (cacheDir == string.Empty) {
#if UNITY_EDITOR || UNITY_STANDALONE
                cacheDir = MyFileUtil.GetParentDir (Application.dataPath) + "/";
#else
                cacheDir = Application.persistentDataPath + "/";
#endif
                cacheDir += "Cache/";
                MyFileUtil.CreateDir (cacheDir);
            }
            return cacheDir;
        }
    }

    public static string LuaFolderEditorPath {
        get {
            if (luaFolderEditorPath == string.Empty) {
                luaFolderEditorPath = Application.streamingAssetsPath + "/Lua";
            }
            return luaFolderEditorPath;
        }
    }

    public static string LuaZipPath {
        get {
            if (luaZipPath == string.Empty) {
                luaZipPath = AssetsBunldePath + "Code.zip";
                File.Create (luaZipPath);
            }
            return luaZipPath;
        }
    }

    public static string AssetsBunldePath {
        get {
            if (assetsBunldePath == string.Empty) {
                assetsBunldePath = Application.streamingAssetsPath + "/AssetsBundles/" + LuaConst.osDir + "/";
                MyFileUtil.CreateDir (assetsBunldePath);
            }
            return assetsBunldePath;
        }
    }

    public static string ForAssetBunldePath {
        get {
            if (forAssetBunldePath == string.Empty) {
                forAssetBunldePath = Application.dataPath + "/Resoures/ForAssetBundle/";
            }
            return forAssetBunldePath;
        }
    }

    public static string SystemConfigPath { get {
        if (systemConfigPath==string.Empty)
        {
            systemConfigPath=Application.streamingAssetsPath+"/Config/SystemConfig.txt";
        }
        return systemConfigPath;}}
}