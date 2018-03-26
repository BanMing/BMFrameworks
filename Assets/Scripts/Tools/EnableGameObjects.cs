/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/09/27
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System.Collections;

public class EnableGameObjects : MonoBehaviour
{
    public GameObject m_Target = null;
    public float m_Time = 1f;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Active());
	}

    IEnumerator Active()
    {
        yield return new WaitForSeconds(m_Time);
        if(m_Target != null)
        {
            m_Target.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
