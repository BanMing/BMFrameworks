using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameCenter : SingletonMonoBehaviour<GameCenter> {
    public GameObject debuger;
    void Awake () {
        // debuger.SetActive (true);
        //  HTTPTool.GetText("www.baidu.com",(str)=>{Debug.Log(str);});
        Debug.Log ("GameCenter Init!");
        ResourcesManager.Instance.MoveStreaming2Cache ();
        var obj = ResourcesManager.GetInstanceGameOject ("Perfabs/Text");
        obj.transform.SetParent (GameObject.Find ("Canvas").transform);
        obj.transform.localPosition = Vector3.zero;
        InitLua ();
    }
    private void InitLua () {
        Debug.Log ("lua Init!");
        GameObject luaGo = new GameObject ("LuaClent");
        luaGo.AddComponent<LuaClient> ();
    }
}