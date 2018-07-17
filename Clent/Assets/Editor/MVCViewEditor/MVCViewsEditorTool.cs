using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class MVCViewsEditorTool : EditorWindow
{
    private static MVCViewsEditorTool window;
    private string generatePath;
    private string viewName;
    private int layerIndex;

    const string TemplateHeadLuaFilePath = "Assets/Editor/MVCViewEditor/TemplateHeadLuaFile.txt";
    const string TemplateViewLuaFilePath = "Assets/Editor/MVCViewEditor/TemplateViewLuaFile.txt";
    const string TemplateControllerLuaFilePath = "Assets/Editor/MVCViewEditor/TemplateControllerLuaFile.txt";
    const string TemplateModelLuaFilePath = "Assets/Editor/MVCViewEditor/TemplateModelLuaFile.txt";

    const string NAME = "#NAME#";
    const string PATH = "#PATH#";
    const string SNAME = "#SNAME#";
    const string UILAYER = "#UILAYER#";

    static string[] UILAYERS = new string[] { "UIManager.Instance.UIRoot", "UIManager.Instance.UITopRoot" };

    [MenuItem("Tools/MVC Eidtor")]
    static void OpenMVCEditorViews()
    {
        window = EditorWindow.CreateInstance<MVCViewsEditorTool>();
        window.Show();
        window.generatePath = PlayerPrefs.GetString("MVC View Code Path", Application.streamingAssetsPath);
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label(generatePath, EditorStyles.numberField);
            if (GUILayout.Button("select", GUILayout.Width(window.position.width / 10)))
            {
                generatePath = EditorUtility.OpenFolderPanel("MVC View Code Path", generatePath, "");
                generatePath = string.IsNullOrEmpty(generatePath) ? Application.streamingAssetsPath : generatePath;
                PlayerPrefs.SetString("MVC View Code Path", generatePath);
            }

        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Name:", GUILayout.Width(window.position.width / 10));
            viewName = EditorGUILayout.TextField(viewName, GUILayout.Width(window.position.width * 4 / 10));
            GUILayout.Label("Layer:", GUILayout.Width(window.position.width / 10));
            layerIndex = EditorGUILayout.Popup(layerIndex, new string[] { "Noraml	Layer", "Top	Layer" }, GUILayout.Width(window.position.width * 4 / 10));
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Generate Code"))
        {
            if (string.IsNullOrEmpty(viewName))
            {
                EditorUtility.DisplayDialog("Tips", "No View Name", "OK");
                return;
            }
            var dir = generatePath + "/" + viewName + "/";
            MyFileUtil.CreateDir(dir);
            var newPath = dir.Substring(dir.IndexOf("Assets")).Replace( LuaConst.luaDir.Substring(LuaConst.luaDir.IndexOf("Assets"))+"/", "");
            Debug.Log("dir:" + dir.Substring(dir.IndexOf("Assets")));

            Debug.Log("LuaConst.luaDir:" + LuaConst.luaDir.Substring(LuaConst.luaDir.IndexOf("Assets")));
            Debug.Log("newPath:" + newPath);
            //head 
            var headFilePath = dir + viewName + "Head.lua";
            MyFileUtil.CopyFile(TemplateHeadLuaFilePath, headFilePath);
            var headLua = MyFileUtil.ReadFileText(headFilePath);
            headLua = headLua.Replace(PATH, newPath);
            headLua = headLua.Replace(NAME, viewName);
            headLua = headLua.Replace(SNAME, viewName.ToLower());
            headLua = headLua.Replace(UILAYER, UILAYERS[layerIndex]);
            MyFileUtil.WriteFile(headFilePath, headLua);

            //view
            var viewFilePath = dir + viewName + "View.lua";
            MyFileUtil.CopyFile(TemplateViewLuaFilePath, viewFilePath);
            var viewLua = MyFileUtil.ReadFileText(viewFilePath);
            viewLua = viewLua.Replace(NAME, viewName);
            viewLua = viewLua.Replace(SNAME, viewName.ToLower());
            MyFileUtil.WriteFile(viewFilePath, headLua);

            //controller
            var controllerFilePath = dir + viewName + "Controller.lua";
            MyFileUtil.CopyFile(TemplateControllerLuaFilePath, controllerFilePath);
            var controllerLua = MyFileUtil.ReadFileText(controllerFilePath);
            controllerLua = controllerLua.Replace(NAME, viewName);
            controllerLua = controllerLua.Replace(SNAME, viewName.ToLower());
            MyFileUtil.WriteFile(controllerFilePath, controllerLua);

            //model
            var modelFilePath = dir + viewName + "Model.lua";
            MyFileUtil.CopyFile(TemplateModelLuaFilePath, modelFilePath);
            var modelLua = MyFileUtil.ReadFileText(modelFilePath);
            modelLua = modelLua.Replace(NAME, viewName);
            modelLua = modelLua.Replace(SNAME, viewName.ToLower());
            MyFileUtil.WriteFile(modelFilePath, modelLua);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Tips", "Generate Code Over!", "OK");
        }
    }
}
