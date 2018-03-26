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
using System.Collections;

public class IsShowLog : MonoBehaviour
{
    public GameObject m_Target = null;

	// Use this for initialization
	void Start ()
    {
#if UNITY_EDITOR
        Destroy(m_Target);
#else
        if(SystemConfig.Instance.IsShowLog)
	    {
            m_Target.SetActive(true);
	    }
        else
        {
            Destroy(m_Target);
        }
#endif
    }
}
