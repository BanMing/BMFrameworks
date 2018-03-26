/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:	2015.5.14
** 描  述: 	UIEventListener继承了多个接口，在滑动列表中一个列表项如果通过UIEventListener绑定了点击事件
            则会造成滑动失败，所以单独提供接口UIClickListener接口

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIClickListener : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
{
    public delegate void VoidDelegate(GameObject go, PointerEventData eventData);
    public VoidDelegate mOnPointerClick = null; //单击
    public VoidDelegate mOnDoublePointerClick = null;  //双击

    public void OnPointerClick(PointerEventData eventData)
    {
        if (mOnPointerClick != null && eventData.clickCount == 1)
        {
            mOnPointerClick(gameObject, eventData);
        }

        if (mOnDoublePointerClick != null && eventData.clickCount == 2)
        {
            mOnDoublePointerClick(gameObject, eventData);
        }
    }

    static public UIClickListener Get(GameObject go)
    {
        UIClickListener listener = go.GetComponent<UIClickListener>();
        if (listener == null) listener = go.AddComponent<UIClickListener>();
        return listener;
    }
}
