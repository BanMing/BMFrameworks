/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:	2014.2
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/
using UnityEngine;
using System;
using System.Collections.Generic;

public delegate void EventCallback(object obj);

public class EventServer
{
	private Dictionary<string, EventCallback> m_DictEvent = new Dictionary<string, EventCallback>();

	public void Register(string id, EventCallback cb)
	{
		try
		{
			if (this.m_DictEvent.ContainsKey(id))
			{
                m_DictEvent[id] += cb;
			}
			else
			{
				this.m_DictEvent.Add(id, cb);
			}
		}
		catch (Exception ex)
		{
			string text = string.Format("EventServer.Register: EventID {0} {1:s}", id, ex.Message);
			Debug.LogError(text);
            Debug.LogException(ex);
		}
	}

	public void UnRegister(string id, EventCallback cb)
	{
		try
		{
			if (this.m_DictEvent.ContainsKey(id))
			{
                m_DictEvent[id] -= cb;
			}
		}
		catch (Exception ex)
		{
			string text = string.Format("EventServer.UnRegister: EventID {0} {1:s}", id, ex.Message);
			Debug.LogError(text);
            Debug.LogException(ex);
		}
	}

	public void Fire(string id, object obj)
	{
		try
		{
			if (this.m_DictEvent.ContainsKey(id))
			{
                EventCallback cb = m_DictEvent[id];
				if (cb != null)
				{
                    cb(obj);
				}
                else
                {
                    string message = string.Format("EventServer.Fire: Fire But No Register, EventID {0}", id);
                    Debug.LogWarning(message);
                }
			}
            else
            {
                string message = string.Format("EventServer.Fire: Fire But No Register, EventID {0}", id);
                Debug.LogWarning(message);
            }
		}
		catch (Exception ex)
		{
			string text = string.Format("EventServer.Fire: EventID {0} {1:s}", id, ex.Message);
			Debug.LogError(text);
            Debug.LogException(ex);
		}
	}

    public void Clear()
    {
        m_DictEvent.Clear();
    }
}

public class GlobalEventServer
{
    public EventServer mEventServer = new EventServer();

    private static GlobalEventServer mInstance = null;
    public static GlobalEventServer Instance
    {
        get 
        {
            if (mInstance == null)
            {
                mInstance = new GlobalEventServer();
            }

            return mInstance;
        }
    }

    private GlobalEventServer()
    {
        
    }
}
