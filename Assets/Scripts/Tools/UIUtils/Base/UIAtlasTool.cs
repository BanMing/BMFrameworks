/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/10/17
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class AtlasConfig : GameData<AtlasConfig>
{
    public string texturePath { set; get; }         //图集名

    public int alignment { set; get; }              //
    public string spriteName { set; get; }          //图片名
    public Vector4 border { set; get; }
    public Vector2 pivot { set; get; }
    public Rect rect { set; get; }

    static public string fileName = "AtlasInfo.xml";
}

public class UIAtlasTool : SingletonMonoBehaviour<UIAtlasTool>
{
    private bool IsInit = false;

    void Start()
    {
        
    }

    public void Init(Action callback = null)
    {
        if(IsInit)
        {
            if(callback != null)
            {
                callback();
            }
            return;
        }
        IsInit = true;

#if UNITY_EDITOR
        //建立缓存
        SaveConfig();
#endif
        //加载缓存
        LoadConfig(callback);
    }

    static public void SaveConfig()
    {
#if UNITY_EDITOR

        Dictionary<string, string> dict = new Dictionary<string, string>();

        int index = 0;
        var root = new System.Security.SecurityElement("root");

        string rootDir = "Assets/Resources/" + ResourcesManager.DirNameForAssetBundlesBuildFrom + "/";
        string dir1 = rootDir + "Common";
        string dir2 = rootDir + SDKConfig.GetCurrentVersionResPath();

        string[] guidArray = AssetDatabase.FindAssets("t:Sprite");
        foreach (string guid in guidArray)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
            if(importer.spriteImportMode != SpriteImportMode.Multiple)
            {
                continue;
            }
            
            //版本过滤
            if (path.Contains(dir1) == false && path.Contains(dir2) == false)
            {
                continue;
            }

            if (dict.ContainsKey(path))
            {
                continue;
            }
            dict.Add(path, "");

            //string str = string.Format("Atlas path {0}, sprite count {1}", path, importer.spritesheet.Length);
            //Debug.Log(str);

            foreach (SpriteMetaData data in importer.spritesheet)
            {
                var record = new System.Security.SecurityElement("Record");
                root.AddChild(record);

                record.AddChild(new System.Security.SecurityElement("id", index.ToString()));
                ++index;

                //记录图片路径
                string texPath = path;
                if(path.Contains(ResourcesManager.DirNameForAssetBundlesBuildFrom))
                {
                    int charIndex = path.IndexOf(ResourcesManager.DirNameForAssetBundlesBuildFrom) + ResourcesManager.DirNameForAssetBundlesBuildFrom.Length + 1;
                    texPath = path.Substring(charIndex);
                }

                if(SystemConfig.Instance.IsUseAssetBundle)
                {
                    texPath = texPath.ToLower();
                }
                
                record.AddChild(new System.Security.SecurityElement("texturePath", texPath));
                record.AddChild(new System.Security.SecurityElement("alignment", data.alignment.ToString()));
                record.AddChild(new System.Security.SecurityElement("spriteName", data.name));

                string border = string.Format("{0},{1},{2},{3}", data.border.x, data.border.y, data.border.z, data.border.w);
                record.AddChild(new System.Security.SecurityElement("border", border));

                string rect = string.Format("{0},{1},{2},{3}", data.rect.x, data.rect.y, data.rect.width, data.rect.height);
                record.AddChild(new System.Security.SecurityElement("rect", rect));
            }
        }
        
        if(root.Children == null || root.Children.Count == 0)
        {
            string str = string.Format("UIAtlasTool.SaveConfig:没有能生成图集信息列表");
            Debug.LogError(str);
            return;
        }

        MyFileUtil.WriteConfigDataInStreamingAssets(AtlasConfig.fileName, root.ToString());
#endif
    }

    Dictionary<string, List<AtlasConfig>> dictCacheConfig = new Dictionary<string, List<AtlasConfig>>();

    void LoadConfig(Action callback = null)
    {
        MyFileUtil.ReadConfigDataAsync(AtlasConfig.fileName, (data) =>
        {
            AtlasConfig.LoadFromText(data);

            //缓存配置文件
            foreach(var item in AtlasConfig.dataMap)
            {
                string texPath = item.Value.texturePath;
                if (SystemConfig.Instance.IsUseAssetBundle)
                {
                    texPath = texPath.ToLower();
                }

                if (dictCacheConfig.ContainsKey(texPath))
                {
                    dictCacheConfig[texPath].Add(item.Value);
                }
                else
                {
                    List<AtlasConfig> list = new List<AtlasConfig>();
                    list.Add(item.Value);
                    dictCacheConfig.Add(texPath, list);
                }
            }
            if(callback != null)
            {
                callback();
            }
        });
    }

    private AtlasConfig GetAtlasConfig(string assetBundleName, string spriteName)
    {
        if(dictCacheConfig.ContainsKey(assetBundleName))
        {
            for(int i = 0; i < dictCacheConfig[assetBundleName].Count; ++i)
            {
                AtlasConfig config = dictCacheConfig[assetBundleName][i];
                if(config.spriteName == spriteName)
                {
                    return config;
                }
            }
        }

        string str = string.Format("UIAtlasTool.GetAtlasConfig:加载AssetBundle名为{0},Sprite名为{1}的图片失败", assetBundleName, spriteName);
        Debug.LogError(str);
        return null;
    }

    public Sprite GetSpriteSync(string assetBundleName, string spriteName, System.Action<Sprite> onLoad)
    {
        UnityEngine.Object obj = ResourcesManager.Instance.GetPrefabSync(assetBundleName, "");

        UnityEngine.Texture2D tex = (UnityEngine.Texture2D)obj;
        if (tex == null)
        {
            string str = string.Format("UIAtlasTool.GetSprite:加载资源{0} 图片名{1}失败", assetBundleName, spriteName);
            Debug.LogError(str);
            onLoad(null);
            return null;
        }

        //缓存的Atlas图片列表名字不带后缀
        assetBundleName = GetAssetBundleName(assetBundleName);

        AtlasConfig config = GetAtlasConfig(assetBundleName, spriteName);
        Sprite sprite = Sprite.Create(tex, config.rect, config.pivot, 100, 0, SpriteMeshType.Tight, config.border);
        sprite.name = spriteName;

        if (onLoad != null)
        {
            onLoad(sprite);
        }
        return sprite;
    }

    public void GetSprite(string assetBundleName, string spriteName, System.Action<Sprite> onLoad)
    {
        System.Action<UnityEngine.Object> onLoadRes = delegate (UnityEngine.Object obj)
        {
            UnityEngine.Texture2D tex = (UnityEngine.Texture2D)obj;
            if (tex == null)
            {
                string str = string.Format("UIAtlasTool.GetSprite:加载资源{0} 图片名{1}失败", assetBundleName, spriteName);
                Debug.LogError(str);
                onLoad(null);
                return;
            }

            //缓存的Atlas图片列表名字不带后缀
            assetBundleName = GetAssetBundleName(assetBundleName);

            AtlasConfig config = GetAtlasConfig(assetBundleName, spriteName);
            Sprite sprite = Sprite.Create(tex, config.rect, config.pivot, 100, 0, SpriteMeshType.Tight, config.border);
            sprite.name = spriteName;
            
            onLoad(sprite);
        };
        ResourcesManager.Instance.GetPrefab(assetBundleName, "", onLoadRes);
    }

    public Texture2D GetTextureSync(string assetBundleName, System.Action<Texture> onLoad)
    {
        UnityEngine.Object obj = ResourcesManager.Instance.GetPrefabSync(assetBundleName, "");
        UnityEngine.Texture2D tex = (UnityEngine.Texture2D)obj;
        if (tex == null)
        {
            string str = string.Format("UIAtlasTool.GetTexture:加载资源{0} 图片失败", assetBundleName);
            Debug.LogError(str);
            onLoad(null);
            return null;
        }
        if (onLoad != null)
        {
            onLoad(tex);
        }
        return tex;
    }

    public void GetTexture(string assetBundleName, System.Action<Texture> onLoad)
    {
        System.Action<UnityEngine.Object> onLoadRes = delegate (UnityEngine.Object obj)
        {
            UnityEngine.Texture2D tex = (UnityEngine.Texture2D)obj;
            if (tex == null)
            {
                string str = string.Format("UIAtlasTool.GetTexture:加载资源{0} 图片失败", assetBundleName);
                Debug.LogError(str);
                onLoad(null);
                return;
            }

            onLoad(tex);
        };

        ResourcesManager.Instance.GetPrefab(assetBundleName, "", onLoadRes);        
    }

    public Rect GetTextureRect(string assetBundleName, string spriteName)
    {
        assetBundleName = GetAssetBundleName(assetBundleName);
        AtlasConfig config = GetAtlasConfig(assetBundleName, spriteName);
        return config.rect;
    }

    private string GetAssetBundleName(string assetBundleName)
    {
        //缓存的Atlas图片列表名字不带后缀
        assetBundleName = ResourcesManager.GetFilePathByFileName(assetBundleName);
        if (assetBundleName.EndsWith(ResourcesManager.mAssetBundleSuffix))
        {
            assetBundleName = assetBundleName.Replace(ResourcesManager.mAssetBundleSuffix, "");
        }

        return assetBundleName;
    }
}
