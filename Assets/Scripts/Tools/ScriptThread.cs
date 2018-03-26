/******************************************************************
** �ļ���:	
** ��  Ȩ:	(C)  
** ������:  Liange
** ��  ��:	2015.5.11
** ��  ��: 	������2����ֲ����

**************************** �޸ļ�¼ ******************************
** �޸���: 
** ��  ��: 
** ��  ��: 
*******************************************************************/
using System;
using System.Collections;
using UnityEngine;
public class ScriptThread : MonoBehaviour
{
	private static GameObject mInstanceGameObject;
	private static ScriptThread mInstance;
	public static ScriptThread Instance
	{
		get
		{
			if (ScriptThread.mInstance == null)
			{
				ScriptThread.CreateInstance();
			}
			return ScriptThread.mInstance;
		}
	}

	public static ScriptThread CreateInstance()
	{
		if (ScriptThread.mInstance == null)
		{
			ScriptThread.mInstanceGameObject = new GameObject(typeof(ScriptThread).Name);
			ScriptThread.mInstance = ScriptThread.mInstanceGameObject.AddComponent<ScriptThread>();
		}
		return ScriptThread.mInstance;
	}

	private void Awake()
	{
		if (ScriptThread.mInstance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(this);
		ScriptThread.mInstance = this;
	}

	public static Coroutine Start(IEnumerator routine)
	{
		return ScriptThread.Instance.StartCoroutine(routine);
	}

	public static void StopAll()
	{
		ScriptThread.Instance.StopAllCoroutines();
	}

	public static void Stop(IEnumerator routine)
	{
		ScriptThread.Instance.StopCoroutine(routine);
	}

    //��ʱִ��
    public static void DoAction(Action action, float time)
    {
        Instance.StartCoroutine(DoActionImp(action, time));
    }

    private static IEnumerator DoActionImp(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    //add by liange@2015.10.23
    //����һִ֡�к���
    public static void DoActionInNextFrame(Action action)
    {
        Instance.StartCoroutine(DoActionInNextFrameImp(action));
    }

    private static IEnumerator DoActionInNextFrameImp(Action action)
    {
        yield return null;
        action();
    }

    //add by liange@2015.10.26
    //��ÿһ����ִ�к���Func����ִ�н��Ϊtrueʱ����
    public static void DoActionEverySecond(Func<bool> func)
    {
        Instance.StartCoroutine(DoActionEverySecondImp(func));
    }

    private static IEnumerator DoActionEverySecondImp(Func<bool> func)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            bool result = func();
            if (result)
            {
                break;
            }
        }
        
    }

    //add by liange@2015.11.10
    //ÿ�����룬ѭ��ִ��,��ִ�н��Ϊtrueʱ����
    public static void RepeatDoAction(Func<bool> func, float time)
    {
        Instance.StartCoroutine(RepeatDoActionImp(func, time));
    }

    private static IEnumerator RepeatDoActionImp(Func<bool> func, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            bool result = func();
            if (result)
            {
                break;
            }
        }

    }

    //add by liange@2015.12.15
    //ÿ�����룬ѭ��ִ��
    public static void RepeatDoAction(Action func, float time)
    {
        Instance.StartCoroutine(RepeatDoActionImp(func, time));
    }

    private static IEnumerator RepeatDoActionImp(Action func, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            func();
        }
    }
}