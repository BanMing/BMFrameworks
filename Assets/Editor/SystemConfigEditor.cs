using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SystemConfigEditor : EditorWindow {
	private SystemConfig systemConfig;
	void Show () {
		base.Show ();
		systemConfig = SystemConfig.ReadSystemConfig ();
	}
	void OnGUI () {
		systemConfig.IsUseLuaZip = GUILayout.Toggle (systemConfig.IsUseLuaZip, "IsUseLuaZip");
		systemConfig.IsShowDebug = GUILayout.Toggle (systemConfig.IsShowDebug, "IsShowDebug");

		if (GUILayout.Button ("Apply")) {
			SystemConfig.WriteSystemConfig (systemConfig);
			Close ();
			Debug.Log ("Fix System Config Complete!");
		}
		if (GUILayout.Button ("Close")) {
			Close ();
		}
	}

	[MenuItem ("ConfigEditor/System")]
	static void OpenSystemConfigEditor () {
		ScriptableObject.CreateInstance<SystemConfigEditor> ().Show ();
	}
}