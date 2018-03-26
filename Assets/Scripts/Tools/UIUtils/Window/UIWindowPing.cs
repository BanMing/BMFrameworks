/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/10/25
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIWindowPing : SingletonMonoBehaviour<UIWindowPing>
{
    new static public string ResPath = "UI/PanelPing";

    override public void Init()
    {
        MyUnityTool.SetUIParentWithLocalInfo(transform, UIManager.Instance.UITopRoot);

        m_TextPing = MyUnityTool.FindChild(transform, "PingText").GetComponent<Text>();
        m_TextPing.text = "";
    }

    //------------------------------------------------------------------------//

    private Text m_TextPing = null;

    public void ShowPing(long ping)
    {
        m_TextPing.text = string.Format("Ping:{0}ms", ping);

        if (ping > 2000)
        {
            m_TextPing.color = Color.red;
        }
        else if (ping > 1000)
        {
            m_TextPing.color = Color.yellow;
        }
        else
        {
            m_TextPing.color = Color.green;
        }
    }
}
