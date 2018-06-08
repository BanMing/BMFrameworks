
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
public class SpritesPathData : ScriptableObject
{

    static string path = "Assets/Editor/SpritePacker/SpritesPathData.asset";
    public List<SpriteInfo> spriteInfos;

    public static SpritesPathData GetSpritesPathData()
    {
        var data = AssetDatabase.LoadAssetAtPath<SpritesPathData>(path);
        if (data == null)
        {
            data = SpritesPathData.CreateInstance<SpritesPathData>();
            AssetDatabase.CreateAsset(data, path);
            data.spriteInfos = new List<SpriteInfo>();
            AssetDatabase.Refresh();
        }
        data.RemoveNullOrEmpty();
        return data;
    }

    public void RemoveNullOrEmpty()
    {
        List<SpriteInfo> res = new List<SpriteInfo>();
        for (int i = 0; i < spriteInfos.Count; i++)
        {
            var data = spriteInfos[i];
            if (!string.IsNullOrEmpty(data.path) && !string.IsNullOrEmpty(data.tag))
            {
                res.Add(data);
            }
        }
        spriteInfos.Clear();
        spriteInfos.AddRange(res);
    }
}

[System.Serializable]
public class SpriteInfo
{
    public string path;
    public string tag;
}