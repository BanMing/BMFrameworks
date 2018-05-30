
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SpritePackerEditorTool
{
    // public 
    [MenuItem("Assets/Create/Create Current Select Sprite Packer")]
    private static void CreateSpritePacker()
    {
        foreach (var item in Selection.instanceIDs)
        {
            var path = AssetDatabase.GetAssetPath(item);
            var packingTag = Path.GetFileName(path);
            List<string> fileList = new List<string>(); ;
            MyFileUtil.GetFileList(path, ref fileList, null, ".meta");
            List<Sprite> sprites = new List<Sprite>();
            foreach (var itemPath in fileList)
            {
                var newPath = itemPath.Substring(itemPath.IndexOf("Assets"));
                var improter = AssetImporter.GetAtPath(newPath) as TextureImporter;
                improter.textureType = TextureImporterType.Sprite;
                improter.mipmapEnabled = false;
                improter.spritePackingTag = packingTag;
                improter.SaveAndReimport();
                sprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(newPath));
            }
            GeneratePacker(sprites, path + "/" + packingTag + "SpritePacker.asset");
        }
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

        if (packer.SpriteList == null)
        {
            packer.SpriteList = new List<Sprite>();
        }
        packer.SpriteList.Clear();
        packer.SpriteList.AddRange(sprites);

        return false;
    }
}