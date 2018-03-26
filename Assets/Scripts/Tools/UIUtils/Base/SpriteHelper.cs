using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteHelper : MonoBehaviour
{
    public UnityEngine.Object mSpriteDirObj = null;
    [HideInInspector]
    public string mSpriteDir = null;

    public List<Sprite> mListSprite = new List<Sprite>();

    public static SpriteHelper mInstance = null;
    public static SpriteHelper Instance
    {
        get
        {
            return mInstance;
        }
    }

    void Awake()
    {
        mInstance = this;
    }

    void OnDestroy()
    {
        mInstance = null;
    }

    public Sprite GetSprite(string imageName)
    {
        for (int i = 0; i < mListSprite.Count; ++i)
        {
            Sprite sprite = mListSprite[i];
            if (sprite.name == imageName)
            {
                return sprite;
            }
        }

        return null;
    }

#if UNITY_EDITOR   
    [ContextMenu("RelateSprite")]
    void RelateSprite()
    {
        mListSprite.Clear();

        mSpriteDir = UnityEditor.AssetDatabase.GetAssetPath(mSpriteDirObj);
        string dir = mSpriteDir.Replace("Assets", "");

        string rootPath = Application.dataPath + dir;
        List<FileUnit> fileList = new List<FileUnit>();
        MyFileUtil.GetRelativeFileListWithSpecialFileType(rootPath, mSpriteDir, ref fileList, ".png");

        for (int i = 0; i < fileList.Count; ++i)
        {
            FileUnit unit = fileList[i];
            Object obj = AssetDatabase.LoadAssetAtPath(unit.relativePath, typeof(Sprite));
            Sprite sprite = obj as Sprite;
            if (sprite == null)
            {
                continue;
            }

            Sprite newSprite = Sprite.Create(sprite.texture, sprite.rect, new Vector2(0.5f, 0.5f));
            newSprite.name = sprite.name;
            mListSprite.Add(newSprite);
        }

        Debug.Log("RelateSprite Over");
    }
#endif
}
