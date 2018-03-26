/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/09/28
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

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Atlas
{
    public class SpriteInfo
    {
        public string spriteName;
        public int x;           //x起点
        public int y;           //y起点
        public float width;
        public float height;

        public Vector2 pivot = Vector2.zero;
    }

    public class AtlasInfo
    {
        public int m_Width;
        public int m_Height;

        public Texture m_Texture = null;

        public List<SpriteInfo> m_ListSprite = new List<SpriteInfo>();

        public void ParseTPSheet(byte[] data)
        {
            m_ListSprite.Clear();
            Stream stream = new MemoryStream(data);
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            while (reader.EndOfStream == false)
            {
                string str = reader.ReadLine();
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }
                if (str.StartsWith("#"))
                {
                    continue;
                }

                //图片大小
                if (str.StartsWith(":size="))
                {
                    string[] sizeArray = str.Remove(0, 6).Split(new char[]
                    {
                        'x'
                    });
                    m_Width = int.Parse(sizeArray[0]);
                    m_Height = int.Parse(sizeArray[1]);
                    continue;
                }

                if(str.Contains(";")==false)
                {
                    continue;
                }

                string[] textArray = str.Split(';');
                int i = 0;

                SpriteInfo spriteInfo = new SpriteInfo();
                //名字
                spriteInfo.spriteName = textArray[i++];

                //大小
                spriteInfo.x = System.Convert.ToInt32(textArray[i++]);
                spriteInfo.y = System.Convert.ToInt32(textArray[i++]);
                spriteInfo.width = System.Convert.ToInt32(textArray[i++]);
                spriteInfo.height = System.Convert.ToInt32(textArray[i++]);

                //锚点
                spriteInfo.pivot.x = float.Parse(textArray[i++]);
                spriteInfo.pivot.y = float.Parse(textArray[i++]);

                m_ListSprite.Add(spriteInfo);
            }
        }
    }
}

[System.Serializable]
public class MahjongImageInfo
{
    public string spriteName;
    public float u;
    public float v;
    public float uSize;
    public float vSize;
}

public class MahjongAtlasTool : SingletonMonoBehaviour<MahjongAtlasTool>
{
    [SerializeField]
    private TextAsset m_MahjongAtlasText = null;

    public Texture2D m_MahjongTexture = null;

    public List<MahjongImageInfo> m_MahjongImageInfoList = new List<MahjongImageInfo>();

    public MahjongImageInfo GetMahjongImageInfo(string spriteName)
    {
        for(int i =0; i < m_MahjongImageInfoList.Count; ++i)
        {
            MahjongImageInfo info = m_MahjongImageInfoList[i];
            if(info.spriteName == spriteName)
            {
                return info;
            }
        }

        return null;
    }

    public void SetMahjongMaterialImage(ref Material material, string spriteName)
    {
        material.mainTexture = m_MahjongTexture;

        MahjongImageInfo info = GetMahjongImageInfo(spriteName);
        Vector2 offset = new Vector2(info.u, info.v);
        material.SetTextureOffset("_MainTex", offset);

        Vector2 size = new Vector2(info.uSize, info.vSize);
        material.SetTextureScale("_MainTex", size);
    }

#if UNITY_EDITOR
    [ContextMenu("导入麻将图集")]
    public void ParseTPSheet()
    {
        if (m_MahjongAtlasText == null)
        {
            Debug.LogError("MahjongAtlasTool.ParseTPSheet：导入麻将图集失败");
            return;
        }

        Atlas.AtlasInfo atlasInfo = new Atlas.AtlasInfo();
        atlasInfo.ParseTPSheet(m_MahjongAtlasText.bytes);

        m_MahjongImageInfoList.Clear();
        foreach(var item in atlasInfo.m_ListSprite)
        {
            MahjongImageInfo info = new MahjongImageInfo();
            info.spriteName = item.spriteName;
            info.u = (float)item.x / (float)atlasInfo.m_Width;
            info.v = (float)item.y / (float)atlasInfo.m_Height;
            info.uSize = item.width / (float)atlasInfo.m_Width;
            info.vSize = item.height / (float)atlasInfo.m_Height;

            m_MahjongImageInfoList.Add(info);
        }

        m_MahjongAtlasText = null;
        Debug.Log("MahjongAtlasTool.ParseTPSheet：导入麻将图集成功");
    }
#endif
}