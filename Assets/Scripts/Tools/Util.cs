using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.IO;

public class Util{
	public static Vector3 ParseVector(JSONObject json)
	{		
		float x = 0;
		float y = 0;
		float z = 0;
		if (json.HasField("x"))
		{
			x= json["x"].n;
		}
		if (json.HasField("y"))
		{
			y= json["y"].n;
		}
		if (json.HasField("z"))
		{
			z = json["z"].n;
		}
		return new Vector3(x, y, z);
	}

	public static void SetActive(Transform trans, bool visible/*, bool recursive = false*/)
	{
        if (trans != null)
        {
            trans.gameObject.SetActive(visible);
            //if (recursive)
            //{
            //    for (int i = 0, imax = trans.childCount; i < imax; ++i)
            //    {
            //        Transform child = trans.GetChild(i);
            //        SetActive(child, visible, recursive);
            //    }
            //}
        }
	}

    public static void SetPosition(Transform trans, Vector3 pos)
    {
        if(trans != null)
        {
            trans.position = pos;
        }
    }

    public static uint GetVersion(uint first_version, uint main_version, uint sub_version, uint build_version)
    {
        uint version = (uint)((first_version << 24) + (main_version << 16) + (sub_version << 8) + build_version);
        return version;
    }

    static StringBuilder md5_builder = new StringBuilder();

    public static string MD5Encrypt(string strText)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strText));
        md5_builder.Length = 0;
        for (int i = 0; i < result.Length; i++)
        {
            md5_builder.Append(result[i].ToString("x"));
        }
        //string str_result = System.Text.Encoding.Default.GetString(result);
        return md5_builder.ToString().ToUpper();
    }

    public static void Assert(bool bTrue)
    {
        if (!bTrue)
            Debug.LogError("Assert failed!");
    }

    public static void FillArray<T>(ref T[][] array, T value)
    {
        for(int i = 0; i < array.Length; ++i)
        {
            for(int j = 0; j < array[i].Length; ++j)
            {
                array[i][j] = value;
            }
        }
    }

    public static string UnicodeToString(string sIn)
    {
        

        if (!sIn.Contains("\\u"))
        {
            return sIn;
        }

        try
        {
            //string reg_str = @"\\ud83c[\\udf00-\udfff]|\\ud83d[\\udc00-\\ude4f]";
            //Regex emoji_regex = new Regex(reg_str, RegexOptions.IgnoreCase);
            //MatchCollection results = emoji_regex.Matches(sIn);
            //sIn = emoji_regex.Replace(sIn, "");

            StringBuilder sOut = new StringBuilder();
            string[] arr = sIn.Split(new string[] { "\\u" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < arr.Length; ++i)
            {
                string s = arr[i];
                if (i == 0 && !sIn.StartsWith("\\u"))
                {
                    sOut.Append(s);
                }
                else
                {
                    sOut.Append((char)Convert.ToInt32(s.Substring(0, 4), 16) + s.Substring(4));
                }
            }
            return sOut.ToString();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
            return "特殊字符无法显示";
        }
    }

    public static DateTime GetTime(long timeStamp)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); //得到1970年的时间戳 
        TimeSpan span = new TimeSpan(timeStamp * 10000000);
        DateTime result = startTime + span;
        return result;
    }

    public static float GetNowTimeStamp()
    {
        return DateTime.Now.Millisecond;
    }

    static public void Capture(string out_path, int quality = 28)
    {
        int width = Screen.width;

        int height = Screen.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);

        byte[] imagebytes = tex.EncodeToJPG(quality);//转化为png图

        tex.Compress(true);//对屏幕缓存进行压缩

        //image.mainTexture = tex;//对屏幕缓存进行显示（缩略图）

        File.WriteAllBytes(out_path, imagebytes);//存储png图
    }
    static HashSet<Type> m_action_set;
    public static bool IsTypeAction(Type type)
    {
        System.Func<bool> fu = delegate ()
        {
            return true;
        };

        if (type.FullName.Contains("System.Action") || type.FullName.Contains("System.Func") || type.FullName.Contains("System.Predicate")
            || type.FullName.Contains("System.Comparison") || type.FullName.Contains("UnityEngine.Events.UnityAction"))
        {
            return true;
        }
        return false;

        /*
        if (m_action_set == null)
        {
            m_action_set = new HashSet<Type>();
            m_action_set.Add(typeof(Action));
            m_action_set.Add(typeof(Action<bool>));
            m_action_set.Add(typeof(Action<int>));
            m_action_set.Add(typeof(Action<uint>));
            m_action_set.Add(typeof(Action<System.Object>));
            m_action_set.Add(typeof(Action<System.Object, System.Object>));
            m_action_set.Add(typeof(Action<System.Object, System.Object, System.Object>));
        }

        if(m_action_set.Contains(type))
        {
            return true;
        }

        else
        {
            return false;
        }
        */
    }


    public static void Vibrate()
    {
#if !UNITY_STANDALONE
            Handheld.Vibrate();
#endif
    }
}
