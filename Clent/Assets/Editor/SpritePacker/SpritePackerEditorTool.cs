
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SpritePackerEditorTool : EditorWindow
{

    static SpritePackerEditorTool window;

    SpritesPathData spritesPathData;
    private Vector2 scrollVector;
    [MenuItem("Tools/Sprite Packer Editor")]
    static void OpenSpritePackerEditor()
    {
        window = SpritePackerEditorTool.GetWindow<SpritePackerEditorTool>();
        window.spritesPathData = SpritesPathData.GetSpritesPathData();
        window.Show();
    }
    void OnGUI()
    {
        var pathWidth = window.position.width * 2 / 3;
        var normalWidth = window.position.width / 9;
        GUILayout.BeginHorizontal("box");
        {
            GUILayout.Label("Path", GUILayout.Width(pathWidth));
            GUILayout.Label("Tag", GUILayout.Width(normalWidth));
            GUILayout.Label("Packer", GUILayout.Width(normalWidth));
            GUILayout.Label("Del", GUILayout.Width(normalWidth));
        }
        GUILayout.EndHorizontal();
        scrollVector = GUILayout.BeginScrollView(scrollVector);
        {
            for (int i = 0; i < window.spritesPathData.spriteInfos.Count; i++)
            {
                var data = window.spritesPathData.spriteInfos[i];
                GUILayout.BeginHorizontal();
                data.path = EditorGUILayout.TextField(data.path, GUILayout.Width(pathWidth));
                GUILayout.Label(data.tag, EditorStyles.numberField);
                if (GUILayout.Button("Packer", GUILayout.Width(normalWidth)))
                {
                    if (!string.IsNullOrEmpty(data.path))
                    {
                        CreateSpritePacker(ref data);
                    }
                    else
                    {
                        window.spritesPathData.spriteInfos.RemoveAt(i);
                    }
                }
                if (GUILayout.Button("Del", GUILayout.Width(normalWidth)))
                {
                    window.spritesPathData.spriteInfos.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Add Sprites Path"))
            {
                window.spritesPathData.spriteInfos.Add(new SpriteInfo() { tag = "", path = "" });
            }
            if (GUILayout.Button("Pack All Sprites"))
            {
                window.spritesPathData.RemoveNullOrEmpty();
                for (int i = 0; i < window.spritesPathData.spriteInfos.Count; i++)
                {
                    var data = window.spritesPathData.spriteInfos[i];
                    CreateSpritePacker(ref data);
                }
            }
        }
        GUILayout.EndHorizontal();
    }
    private static void CreateSpritePacker(ref SpriteInfo data)
    {
        var path = data.path.Substring(data.path.IndexOf("Assets"));
        var packingTag = Path.GetFileName(path);
        data.tag = packingTag;
        data.path = path;
        List<string> fileList = new List<string>(); ;
        MyFileUtil.GetFileList(path, ref fileList, null, ".meta");
        List<Sprite> sprites = new List<Sprite>();
        foreach (var itemPath in fileList)
        {
            var newPath = itemPath.Substring(itemPath.IndexOf("Assets"));
            var extension = Path.GetExtension(newPath);
            if (extension.Equals(".asset"))
            {
                continue;
            }
            var improter = AssetImporter.GetAtPath(newPath) as TextureImporter;
            improter.textureType = TextureImporterType.Sprite;
            improter.mipmapEnabled = false;
            improter.spritePackingTag = packingTag;
            improter.SaveAndReimport();
            sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(newPath));
        }
        GeneratePacker(sprites, path + "/" + packingTag + "SpritePacker.asset");

        AssetDatabase.Refresh();
    }

    private static bool GeneratePacker(List<Sprite> sprites, string path)
    {
        var packer = AssetDatabase.LoadAssetAtPath<SpritePacker>(path);
        if (packer == null)
        {
            packer = SpritePacker.CreateInstance<SpritePacker>();
            UnityEditor.AssetDatabase.CreateAsset(packer, path);
        }
        //no eidtable
        packer.hideFlags=HideFlags.NotEditable;
        if (packer.spriteList == null)
        {
            packer.spriteList = new List<Sprite>();
        }
        packer.spriteList.Clear();
        packer.spriteList.AddRange(sprites);

        return false;
    }
}
