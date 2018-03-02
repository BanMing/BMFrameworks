using UnityEngine;
using System;
using System.Reflection;

public class Singleton<T> : MonoBehaviour where T : class
{
    protected static T instance;
    public static T Instance
    {
        get
       {
            if (null == instance)
            {
                var type = typeof (T);
                if (type.IsAbstract || type.IsInterface)
                {
                    throw (new Exception("Class type must could be instantiated. Don't use abstract or interface"));
                }
                if (!type.IsSealed)
                {
                    throw (new Exception("Class type must be a sealed. Use sealed"));
                }
                var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    Type.EmptyTypes,
                    null);
                if (null == constructor)
                {
                    throw (new Exception("Constructor must empty"));
                }
                if (!constructor.IsPrivate)
                {
                    throw (new Exception("Constructor must be a private function"));
                }
                instance = constructor.Invoke(null) as T;
            }
            return instance;
        }
    }
}