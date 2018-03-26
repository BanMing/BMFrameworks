/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:	2016.8.1
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LocalData
{
    private static LocalData mInstance = null;
    public static LocalData Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new LocalData();
            }
            return mInstance;
        }
    }

    private string mLocalDataFileName = MyFileUtil.CacheDir + "LocalData.csv";
    private Dictionary<string, string> mDictLocalData = new Dictionary<string, string>();

    private LocalData()
    {
        Read();
    }

    void Read()
    {
        try
        {
            if (File.Exists(mLocalDataFileName) == false)
            {
                return;
            }

            CSVParser csv = new CSVParser();
            csv.ParseFile(mLocalDataFileName);
            for (int i = 0; i < csv.GetRowSize(); ++i)
            {
                string key = csv.GetString(i, 0);
                string value = csv.GetString(i, 1);

                if(mDictLocalData.ContainsKey(key))
                {
                    string str = string.Format("LocalData.Read:Key {0} 已经存在", key);
                    Debug.LogError(str);
                    continue;
                }
                mDictLocalData.Add(key, value);
            }

        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void Save()
    {
        try
        {
            StringWriter writer = new StringWriter();
            writer.WriteLine("key,value");
            writer.WriteLine("string,string");
            foreach (string key in mDictLocalData.Keys)
            {
                string str = key + "," + mDictLocalData[key];
                writer.WriteLine(str);
            }

            File.WriteAllText(mLocalDataFileName, writer.ToString());
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    //*********************************************************************************************//

    public bool IsContainKey(string key)
    {
        return mDictLocalData.ContainsKey(key);
    }

    public void SetValue(string key, string value, bool autoSave = true)
    {
        mDictLocalData[key] = value;
        if (autoSave)
        {
            Save();
        }
    }

    public string GetValue(string key)
    {
        if (mDictLocalData.ContainsKey(key))
        {
            return mDictLocalData[key];
        }

        return null;
    }

    public int GetValueInt(string key)
    {
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return int.Parse(str);
    }

    public uint GetValueUInt(string key)
    {
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return uint.Parse(str);
    }

    public float GetValueFloat(string key)
    {
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return float.Parse(str);
    }

    public double GetValueDouble(string key)
    {
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }

        return double.Parse(str);
    }

    public bool GetValueBool(string key)
    {
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }

        if (str == "0")
        {
            return false;
        }

        if (string.Compare(str, "false", true) == 0)
        {
            return false;
        }

        return true;
    }

    //*********************************************************************************************//

    public bool IsContainKey(string roleName, string key)
    {
        key = roleName + "_" + key;
        return mDictLocalData.ContainsKey(key);
    }

    public void SetValue(string roleName, string key, string value, bool autoSave = true)
    {
        key = roleName + "_" + key;
        mDictLocalData[key] = value;

        if (autoSave)
        {
            Save();
        }
    }

    public string GetValue(string roleName, string key)
    {
        key = roleName + "_" + key;
        if (mDictLocalData.ContainsKey(key))
        {
            return mDictLocalData[key];
        }

        return null;
    }

    public int GetValueInt(string roleName, string key)
    {
        key = roleName + "_" + key;
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return int.Parse(str);
    }

    public uint GetValueUInt(string roleName, string key)
    {
        key = roleName + "_" + key;
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return uint.Parse(str);
    }

    public float GetValueFloat(string roleName, string key)
    {
        key = roleName + "_" + key;
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return float.Parse(str);
    }

    public double GetValueDouble(string roleName, string key)
    {
        key = roleName + "_" + key;
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }

        return double.Parse(str);
    }

    public bool GetValueBool(string roleName, string key)
    {
        key = roleName + "_" + key;
        string str = GetValue(key);
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }

        if (str == "0")
        {
            return false;
        }

        if (string.Compare(str, "false", true) == 0)
        {
            return false;
        }

        return true;
    }
}
