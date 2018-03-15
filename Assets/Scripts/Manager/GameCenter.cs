using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameCenter : SingletonMonoBehaviour<GameCenter> {
    public GameObject debuger;
    void Awake () {
        ConfigManager.Instance.InitConfig ();
        debuger.SetActive(ConfigManager.Instance.SystemConfig.IsShowDebug);
    }
    private void InitLua () {
        Debug.Log ("lua Init!");
        GameObject luaGo = new GameObject ("LuaClent");
        luaGo.AddComponent<LuaClient> ();
    }
    private void LuaCodeTest () {
        debuger.SetActive (true);
        Debug.Log ("GameCenter Init!");
        ResourcesManager.Instance.MoveStreaming2Cache (InitLua);
        var obj = ResourcesManager.GetInstanceGameOject ("Perfabs/Text");
        obj.transform.SetParent (GameObject.Find ("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
    }
}