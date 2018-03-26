using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class BatteryLevel
{
#if UNITY_IPHONE
	[DllImport ("__Internal")]
	static extern float _GetBatteryLevel ();
#endif
	
	public static int GetBatteryLevel ()
    {
#if UNITY_IPHONE
        float battery = _GetBatteryLevel();
        return (int)(battery * 100);

#elif UNITY_ANDROID
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        using (var androidPlugin = new AndroidJavaObject("com.RSG.AndroidPlugin.AndroidPlugin", currentActivity))
                        {
                            return (int)(androidPlugin.Call<float>("GetBatteryPct") * 100);
                        }
                    }
                }
            }
            return 100;
        }
        catch (Exception e)
        {
            Debug.Log("Failed to read battery power; " + e.Message);
            return 100;
        }
#else
        return 100;
#endif
    }
}