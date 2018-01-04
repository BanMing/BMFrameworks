
using System;

public static class Utils
{
   
    /// <summary>
    /// 替换字符串中的子字符串。
    /// </summary>
    /// <param name="input">原字符串</param>
    /// <param name="oldValue">旧子字符串</param>
    /// <param name="newValue">新子字符串</param>
    /// <param name="count">替换数量</param>
    /// <param name="startAt">从第几个字符开始</param>
    /// <returns>替换后的字符串</returns>
    public static String ReplaceFirst(this string input, string oldValue, string newValue, int startAt = 0)
    {
        int pos = input.IndexOf(oldValue, startAt);
        if (pos < 0)
        {
            return input;
        }
        return string.Concat(input.Substring(0, pos), newValue, input.Substring(pos + oldValue.Length));
    }
}
