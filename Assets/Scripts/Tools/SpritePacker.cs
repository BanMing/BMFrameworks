using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
public class SpritePacker : ScriptableObject
{
    public List<Sprite> spriteList;
    private Dictionary<string, Sprite> spriteDic;
    void OnEnable()
    {
        if (spriteDic == null)
        {
            spriteDic = new Dictionary<string, Sprite>();
        }

        for (int i = 0; i < spriteList.Count; i++)
        {
            spriteDic.Add(spriteList[i].name, spriteList[i]);
        }
    }
    //get sprite by name
    public Sprite GetSprite(string name)
    {
        if (spriteDic == null)
        {
            return null;
        }
        Sprite res;
        spriteDic.TryGetValue(name, out res);
        return res;
    }
    void OnDestroy()
    {
        spriteDic.Clear();
    }
}