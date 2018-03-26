/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:  2014.2
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 2016.6.14
** 描  述: 
*******************************************************************/

/**
 * 1.在csv文件的第一行有#include filename,则会读取宏定义文件，
 * 在csv文件字段类型定义中 如果有字段类型为macro，则会进行宏替换
 * 
 * 宏定义，每行如下定义
 * #define 被替换字符串 = 替换字符串
 * 
 * 2.字符串中 <br> 表示换行
 * 
 * 3.CSV文件格式如下
 * 第一行 引入宏定义文件(不是必须的)
 * 第二行 字段名称
 * 第三行 字段类型
 */

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_5 || UNITY_4
using UnityEngine;
#endif

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class CSVParser
{
    //宏是共用的
    private static Dictionary<string, string> s_DictMacro = new Dictionary<string, string>();
    private static List<string> s_ListMacroFileName = new List<string>();

    private const string m_MacroStr = "macro"; 

    //-----------------------------------------------------------------------------------//

    static System.Text.Encoding m_Encoding = Encoding.UTF8;

    private char m_SplitChar = ',';
    public char SplitChar
    {
        get
        {
            return m_SplitChar;
        }

        set
        {
            m_SplitChar = value;
        }
    }

    //-----------------------------------------------------------------------------------//

    private List<string[]> m_ListFileContent = new List<string[]>();

    private int m_RowSize = 0; //文件行数
    private int m_ColSize = 0; //文件列数

    private string[] m_TypeArray = null; //数据类型

    //-----------------------------------------------------------------------------------//
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="defaultCSVFormat">true时按照第一行宏定义，第二行字段名，第三行数据类型进行解析；false则从第一行就进行数据解析</param>
    public void ParseFile(string filePath, bool defaultCSVFormat = true)
    {
        StreamReader fileReader = new StreamReader(filePath, m_Encoding);
        ParseImp(fileReader, defaultCSVFormat);
        fileReader.Close();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="defaultCSVFormat">true时按照第一行宏定义，第二行字段名，第三行数据类型进行解析；false则从第一行就进行数据解析</param>
    public void ParseFromBuffer(byte[] data, bool defaultCSVFormat = true)
    {
        Stream stream = new MemoryStream(data);
        StreamReader fileReader = new StreamReader(stream, m_Encoding);
        ParseImp(fileReader, defaultCSVFormat);
        fileReader.Close();
        stream.Close();
    }

    //-----------------------------------------------------------------------------------//
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileReader"></param>
    /// <param name="defaultFormat">true时按照第一行宏定义，第二行字段名，第三行数据类型进行解析；false则从第一行就进行数据解析</param>
    private void ParseImp(StreamReader fileReader, bool defaultCSVFormat = true)
    {
        if (defaultCSVFormat)
        {
            //解析宏
            string lineContent = fileReader.ReadLine();
            if (lineContent.Contains("#include"))
            {
                string fileName = lineContent.Replace("#include", "");
                fileName = fileName.Replace(m_SplitChar, ' '); //去掉多余的分割符
                fileName = fileName.Trim();
                ParseMacroFile(fileName);

                lineContent = fileReader.ReadLine(); //字段名
            }
            
            //字段名 跳过

            //字段数据类型
            lineContent = fileReader.ReadLine();
            m_TypeArray = lineContent.Split(m_SplitChar);
        }
        
        //数据内容
        while (fileReader.EndOfStream == false)
        {
            string lineContent = fileReader.ReadLine();
            if(string.IsNullOrEmpty(lineContent))
            {
                continue;
            }

            m_RowSize++;
            string[] array = lineContent.Split(m_SplitChar);
            m_ListFileContent.Add(array);
        }

        //获取列数
        if (m_ListFileContent.Count > 0)
        {
            m_ColSize = m_ListFileContent[0].Length;
        }

        if (s_DictMacro.Count > 0 && m_TypeArray != null && m_TypeArray.Length > 0)
        {
            for (int i = 0; i < this.m_ColSize; i++)
            {
                if (m_TypeArray[i].ToLower() == m_MacroStr)
                {
                    for (int j = 0; j < this.m_RowSize; j++) 
                    {
                        string text = this.m_ListFileContent[j][i];
                        if (string.IsNullOrEmpty(text) == false)
                        {
                            if (s_DictMacro.ContainsKey(text))
                            {
                                this.m_ListFileContent[j][i] = s_DictMacro[text];
                            }
                            else
                            {
                                string str = string.Format("CsvParser.Parse: macro error, can not find macro {0}", text);
                                LogError(str);
                            }
                        }
                    }
                }
            }
        }
    }

    static public bool ParseMacroFile(string path)
    {
        foreach (string str in s_ListMacroFileName)
        {
            //已经解析过
            if (str == path)
            {
                return true;
            }
        }
        s_ListMacroFileName.Add(path);

        byte[] data = File.ReadAllBytes(path);
        return ParseMacroDataImp(data);
    }

    static public bool ParseMacroFileData(string text)
    {
        byte[] data = m_Encoding.GetBytes(text);
        return ParseMacroDataImp(data);
    }

    static private bool ParseMacroDataImp(byte[] data)
    {
        try
        {
            Stream stream = new MemoryStream(data);
            StreamReader streamReader = new StreamReader(stream, m_Encoding);

            while (streamReader.EndOfStream == false)
            {
                string text = streamReader.ReadLine();
                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }

                if (text.Contains("#define") == false)
                {
                    continue;
                }

                text = text.Replace("#define", "");
                string[] array = text.Split('=');
                if (array.Length != 2)
                {
                    string str = string.Format("CsvParser.ParseMacroDataImp: MacroData {0} error", text);
                    LogError(str);
                }
                else
                {
                    string key = array[0].Trim();
                    string value = array[1].Trim();

                    if (s_DictMacro.ContainsKey(key))
                    {
                        string str = string.Format("CsvParser.ParseMacroDataImp: {0} 宏定义重复", key);
                        LogError(str);
                    }
                    else
                    {
                        s_DictMacro.Add(key, value);
                    }
                }
            }

            return true;
        }
        catch (System.Exception ex)
        {
            LogException(ex);
            return false;
        }
    }

    //-----------------------------------------------------------------------------------//

    static private int GetCharNum(string str, char key)
    {
        int num = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Equals(key))
            {
                num++;
            }
        }
        return num;
    }

    //-----------------------------------------------------------------------------------//

    public int GetRowSize()
    {
        return this.m_RowSize;
    }

    public int GetColSize()
    {
        return this.m_ColSize;
    }

    //-----------------------------------------------------------------------------------//

    public string GetString(int x, int y)
    {
        return this.m_ListFileContent[x][y].Replace("<br>", "\n");
    }

    public int GetInt(int x, int y)
    {
        string str = m_ListFileContent[x][y];
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return int.Parse(str);
    }

    public uint GetUInt(int x, int y)
    {
        string str = m_ListFileContent[x][y];
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return uint.Parse(str);
    }

    public float GetFloat(int x, int y)
    {
        string str = m_ListFileContent[x][y];
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }
        return float.Parse(str);
    }

    public double GetDouble(int x, int y)
    {
        string str = m_ListFileContent[x][y];
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }

        return double.Parse(str);
    }

    public bool GetBool(int x, int y)
    {
        string str = m_ListFileContent[x][y];
        if(string.IsNullOrEmpty(str))
        {
            return false;
        }

        if(str == "0")
        {
            return false;
        }

        if(string.Compare(str, "false", true) == 0)
        {
            return false;
        }

        return true;
    }

    //-----------------------------------------------------------------------------------//

    public void Clear()
    {
        m_ListFileContent.Clear();

        m_RowSize = 0;
        m_ColSize = 0;

        m_TypeArray = null;
    }

    //-----------------------------------------------------------------------------------//

    static private void LogError(string info)
    {
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_5 || UNITY_4
        Debug.LogError(info);
#else
        Console.WriteLine(info);
#endif
    }

    static private void LogException(Exception ex)
    {
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_5 || UNITY_4
        Debug.LogException(ex);
#else
        Console.WriteLine(ex.Message);
#endif
    }
}
