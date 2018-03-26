/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:	2015.3.10
** 描  述: 	用于其他线程和主线程(UI线程)进行通信

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventParam
{
    public string id;
    public object actionCB; //action
    public object paramObj; //
}

public class ThreadCommunicationEventServer : SingletonMonoBehaviour <ThreadCommunicationEventServer>
{
    static private object mLock = new object();
    static private Queue<EventParam> mQueueEventParam = new Queue<EventParam>();
    static private EventServer mEventServer = new EventServer();

    override protected void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            if (m_Instance != this)
            {
                Debug.LogError("ThreadCommunicationServer.Awake:当前场景存在多个ThreadCommunicationServer脚本");
            }
        }
    }

    void OnDestroy()
    {       
        //mQueueEventParam.Clear();
    }

	void Update () 
    {
        lock (mLock)
        {
            while (mQueueEventParam.Count > 0)
            {
                EventParam param = mQueueEventParam.Dequeue();
                mEventServer.Fire(param.id, param.paramObj);
            }
        }
	}

    static public void Register(string id, EventCallback cb)
    {
        lock(mLock)
        {
            mEventServer.Register(id, cb);
        }
    }

    static public void UnRegister(string id, EventCallback cb)
    {
        lock(mLock)
        {
            mEventServer.UnRegister(id, cb);
        }
    }

    static public void Fire(string id, object obj)
    {
        lock (mLock)
        {
            EventParam param = new EventParam();
            param.id = id;
            param.paramObj = obj;

            mQueueEventParam.Enqueue(param);
        }
    }

    static public void Clear()
    {
        mEventServer.Clear();
    }

    //--------------------------------------------------------------------------//

    //在主线程中执行逻辑

    static public void DoAction(System.Action action)
    {
        string id = string.Format("{0}_{1}", action.ToString(), GetEventID());
        ThreadCommunicationEventServer.Register(id, DoActionImp0);

        EventParam parm = new EventParam();
        parm.id = id;
        parm.actionCB = action;
        Fire(id, parm);
    }

    //action:将要执行的函数 obj:传递给action的参数
    static public void DoAction(System.Action<System.Object> action, System.Object obj)
    {
        string id = string.Format("{0}_{1}", action.ToString(), GetEventID());
        ThreadCommunicationEventServer.Register(id, DoActionImp1);

        EventParam parm = new EventParam();
        parm.id = id;
        parm.actionCB = action;
        parm.paramObj = obj;
        Fire(id, parm);
    }

    //action:将要执行的函数 obj:传递给action的参数
    static public void DoAction(System.Action<bool> action, bool obj)
    {
        string id = string.Format("{0}_{1}", action.ToString(), GetEventID());
        ThreadCommunicationEventServer.Register(id, DoActionImp2);

        EventParam parm = new EventParam();
        parm.id = id;
        parm.actionCB = action;
        parm.paramObj = obj;
        Fire(id, parm);
    }

    static private void DoActionImp0(System.Object obj)
    {
        EventParam parm = (EventParam)obj;
        UnRegister(parm.id, DoActionImp0);
        System.Action action = (System.Action)parm.actionCB;
        if (action != null)
        {
            action();
        }
    }

    static private void DoActionImp1(System.Object obj)
    {
        EventParam parm = (EventParam)obj;
        UnRegister(parm.id, DoActionImp1);
        System.Action<System.Object> action = (System.Action<System.Object>)parm.actionCB;
        if(action != null)
        {
            action(parm.paramObj);
        }
    }

    static private void DoActionImp2(System.Object obj)
    {
        EventParam parm = (EventParam)obj;
        UnRegister(parm.id, DoActionImp2);
        System.Action<bool> action = (System.Action<bool>)parm.actionCB;
        if (action != null)
        {
            action((bool)parm.paramObj);
        }
    }

    static private string GetEventID()
    {
        long tick = System.DateTime.Now.Ticks;
        System.Random random = new System.Random((int)tick);
        int num = random.Next(0, 10000);

        string str = string.Format("{0}_{1}", tick, num);
        return str;
    }
}
