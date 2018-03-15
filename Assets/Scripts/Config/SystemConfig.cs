using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SystemConfig {
	public bool IsUseLuaZip;
	public bool IsShowDebug;

	public static SystemConfig ReadSystemConfig () {
		var jsonStr = ResourcesManager.Instance.ReadConfig (PathManager.SystemConfigPath);
		// Debug.Log ("SystemConfig JsonStr:" + jsonStr);
		return JsonUtility.FromJson<SystemConfig> (jsonStr);
	}
	public static void WriteSystemConfig (SystemConfig systemConfig) {
		var jsonStr = JsonUtility.ToJson (systemConfig);
		ResourcesManager.Instance.WriteConfig (PathManager.SystemConfigPath, jsonStr);
	}
}