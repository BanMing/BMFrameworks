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

public class LocalDataV
{
    private static LocalDataV mInstance = null;
    public static LocalDataV Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new LocalDataV();
            }
            return mInstance;
        }
    }
				
	private string mLocalDataFileName = MyFileUtil.CacheDir;
	private string extendedName = ".csv";
    private Dictionary<string, string> mDictLocalData = new Dictionary<string, string>();

    private LocalDataV()
    {
        //Read();
    }

    void Read(string dataName)
    {
        try
        {
			if (File.Exists(mLocalDataFileName+dataName+extendedName) == false)
            {
                return;
            }

            CSVParser csv = new CSVParser();
			csv.ParseFile(mLocalDataFileName+dataName+extendedName);
			mDictLocalData.Clear();
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

	public void Save(string dataName)
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

			File.WriteAllText(mLocalDataFileName+dataName+extendedName, writer.ToString());
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

	public void SetValue(string dataName,string key, string value, bool autoSave = true)
    {
		Read (dataName);
        mDictLocalData[key] = value;
        if (autoSave)
        {
			Save(dataName);
        }
    }

	public string GetValue(string dataName,string key)
    {
		Read (dataName);
        if (mDictLocalData.ContainsKey(key))
        {
            return mDictLocalData[key];
        }

        return null;
    }

	public int GetValueInt(string dataName,string key)
    {
		string str = GetValue(dataName,key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return int.Parse(str);
    }

	public uint GetValueUInt(string dataName,string key)
    {
        string str = GetValue(dataName,key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return uint.Parse(str);
    }

	public float GetValueFloat(string dataName,string key)
    {
        string str = GetValue(dataName,key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return float.Parse(str);
    }

	public double GetValueDouble(string dataName,string key)
    {
        string str = GetValue(dataName,key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }

        return double.Parse(str);
    }

	public bool GetValueBool(string dataName,string key)
    {
        string str = GetValue(dataName,key);
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

	private string roleData = "RoleData";
    public bool IsContainKey(string roleName, string key)
    {
        key = roleName + "_" + key;
        return mDictLocalData.ContainsKey(key);
    }

    public void SetRoleValue(string roleName, string key, string value, bool autoSave = true)
    {
        key = roleName + "_" + key;
        mDictLocalData[key] = value;

        if (autoSave)
        {
			Save(roleData);
        }
    }

	public string GetRoleValue(string roleName, string key)
    {
        key = roleName + "_" + key;
        if (mDictLocalData.ContainsKey(key))
        {
            return mDictLocalData[key];
        }

        return null;
    }

	public int GetRoleValueInt(string roleName, string key)
    {
        key = roleName + "_" + key;
		string str = GetValue(roleData,key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return int.Parse(str);
    }

	public uint GetRoleValueUInt(string roleName, string key)
    {
        key = roleName + "_" + key;
		string str = GetValue(roleData,key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return uint.Parse(str);
    }

	public float GetRoleValueFloat(string roleName, string key)
    {
        key = roleName + "_" + key;
		string str = GetValue(roleData,key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return float.Parse(str);
    }

	public double GetRoleValueDouble(string roleName, string key)
    {
        key = roleName + "_" + key;
		string str = GetValue(roleData,key);
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }

        return double.Parse(str);
    }

	public bool GetRoleValueBool(string roleName, string key)
    {
        key = roleName + "_" + key;
		string str = GetValue(roleData,key);
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
