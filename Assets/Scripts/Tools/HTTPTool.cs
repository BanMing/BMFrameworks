/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/09/13
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HTTPTool {
    static public void GetTexture (string url, System.Action<UnityEngine.Texture> onLoad) {
        ScriptThread.Instance.StartCoroutine (GetTextureImp (url, onLoad));
    }

    static public void GetSprite (string url, System.Action<Sprite> onLoad) {
        System.Action<UnityEngine.Texture> loadTexture = delegate (UnityEngine.Texture tex) {
            UnityEngine.Texture2D tex2d = (UnityEngine.Texture2D) tex;
            Rect rect = new Rect (0, 0, tex.width, tex.height);
            Sprite sprite = Sprite.Create (tex2d, rect, new Vector2 (0.5f, 0.5f));

            onLoad (sprite);
        };

        ScriptThread.Instance.StartCoroutine (GetTextureImp (url, loadTexture));
    }

    static private IEnumerator GetTextureImp (string url, System.Action<UnityEngine.Texture> onLoad) {
        WWW www = GetWWW (url);
        yield return www;

        try {
            if (string.IsNullOrEmpty (www.error) == false) {
                string str = string.Format ("HTTPTool.GetTextureImp:访问URL {0} 失败，错误信息{1}", url, www.error);
                Debug.LogError (str);
                onLoad (null);
            } else {
                onLoad (www.texture);
            }
        } catch (Exception ex) {
            Debug.LogException (ex);
        }
    }

    //--------------------------------------------------------------------------//

    static public void GetText (string url, System.Action<string> onLoad) {
        ScriptThread.Instance.StartCoroutine (GetTextImp (url, onLoad));
    }

    static private IEnumerator GetTextImp (string url, System.Action<string> onLoad) {
        WWW www = GetWWW (url);
        yield return www;

        try {
            if (string.IsNullOrEmpty (www.error) == false) {
                string str = string.Format ("HTTPTool.GetTextImp:访问URL {0} 失败，错误信息{1}", url, www.error);
                Debug.LogError (str);
                onLoad (null);
            } else {
                onLoad (www.text);
            }
        } catch (Exception ex) {
            Debug.LogException (ex);
        }
    }

    //--------------------------------------------------------------------------//

    static public void GetBytes (string url, System.Action<byte[]> onLoad) {
        ScriptThread.Instance.StartCoroutine (GetBytesImp (url, onLoad));
    }

    static private IEnumerator GetBytesImp (string url, System.Action<byte[]> onLoad) {
        WWW www = GetWWW (url);
        yield return www;

        try {
            if (string.IsNullOrEmpty (www.error) == false) {
                string str = string.Format ("HTTPTool.GetBytesImp:访问URL {0} 失败，错误信息{1}", url, www.error);
                Debug.LogError (str);
                onLoad (null);
            } else {
                onLoad (www.bytes);
            }
        } catch (Exception ex) {
            Debug.LogException (ex);
        }
    }

    //--------------------------------------------------------------------------//

    static public void DownLoadFile (string url, string localFilePath, bool isAbsolutePath, Action<bool> finishAction) {
        try {
            System.Action<byte[]> onLoad = delegate (byte[] data) {
                if (data != null) {
                    MyFileUtil.WriteFile (localFilePath, data, isAbsolutePath);
                    finishAction (true);
                } else {
                    finishAction (false);
                }
            };
            GetBytes (url, onLoad);
        } catch (Exception ex) {
            Debug.LogException (ex);
            finishAction (false);
        }
    }

    //--------------------------------------------------------------------------//

    static public string GetURL (string url) {
        string str = System.DateTime.Now.ToString ("yyyy_MM_dd_hh_mm_ss_fff_");
        str = str.Substring (10);
        long tick = System.DateTime.Now.Ticks;
        System.Random random = new System.Random ((int) tick);
        int num = random.Next (0, 10000);
        if (url.Contains ("?")) {
            url = string.Concat (url, "&p=", str, num.ToString ());
        } else {
            url = string.Concat (url, "?p=", str, num.ToString ());
        }

        Debug.Log("HTTPTool.GetURL:" + url);
        return url;
    }

    static public WWW GetWWW (string url) {
        url = GetURL (url);
        WWW www = new WWW (url);
        return www;
    }
}