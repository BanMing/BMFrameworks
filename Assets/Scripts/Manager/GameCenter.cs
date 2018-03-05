using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenter : Singleton<MonoBehaviour> {

    void Awake () {
        InitLua ();
    }
    private void InitLua () {
        Debug.Log("lua Init!");
        GameObject luaGo = new GameObject ("LuaClent");
        luaGo.AddComponent<LuaClient> ();
    }
}