
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SpritePackerEditorTool : EditorWindow
{
    static SpritePackerEditorTool window;
    static string SpritesPath=ResourcesManager.DirForAssetBundlesBuildFrom;
    private Vector2 scrollVector;
    [MenuItem("Tools/Sprite Packer Editor")]
    static void OpenSpritePackerEditor()
    {
        window = SpritePackerEditorTool.GetWindow<SpritePackerEditorTool>();
        window.Show();
        PlayerPrefs.GetString("SpritePackerEditorToolSpritesPath",SpritesPath.Remove(SpritesPath.IndexOf("Asset")));
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

        }
        GUILayout.EndScrollView();
        SpritesPath=GUILayout.TextField(SpritesPath);
        if (GUILayout.Button("Pack All Sprites"))
        {

        }
    }
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
    class SpriteInfo
    {
        public string path;
        public string tag;
    }
}