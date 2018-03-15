using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ConfigManager : SingletonObject<ConfigManager> {

    public SystemConfig SystemConfig { get; private set; }

    public void InitConfig () {
        this.SystemConfig = SystemConfig.ReadSystemConfig ();
    }
    private ConfigManager () {

    }
}