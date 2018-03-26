/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/11/03
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System.Collections;

public class SyncLoadTextureTool : MonoBehaviour
{
    public string m_TextureFileName = null;
    public UnityEngine.UI.RawImage m_RawImage = null;

	// Use this for initialization
	void Awake ()
    {
        if(m_RawImage == null || string.IsNullOrEmpty(m_TextureFileName))
        {
            return;
        }

        string filePath = MyFileUtil.CacheDir + m_TextureFileName;
        if(System.IO.File.Exists(filePath))
        {
            filePath = "file://" + filePath;
            WWW www = new WWW(filePath);

            while(www.isDone == false)
            {
                continue;
            }

            m_RawImage.texture = www.texture;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
