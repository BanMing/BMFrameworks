using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AssetBundleEditorView : EditorWindow {

	void OnGUI(){
		EditorGUILayout.LabelField("AssetBundle Step:");
		EditorGUILayout.SelectableLabel("1.ClearEditorLuaBytesFiles");
		EditorGUILayout.SelectableLabel("2.SetLuaBytesFile");
		EditorGUILayout.SelectableLabel("3.SetLuaAssetBundleName");
		EditorGUILayout.SelectableLabel("4.BuildAssetBundle");
		EditorGUILayout.SelectableLabel("5.BuildLocalLuaZip");
	}

	[MenuItem("Build/OpenEditorWindow")]
	static void OpenEditorWindow(){
		ScriptableObject.CreateInstance<AssetBundleEditorView>().Show();
	}
}
