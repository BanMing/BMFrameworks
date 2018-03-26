/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/11/01
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

/**
 * 目前 委托/事件 只支持参数是对象的函数 绑定，基本数值类型会报错
 */
class DelegateTool : LuaDelegate
{
    public static string FunctionName = "DelegateParam";
    public static int FunctionCount = 11;

    //-----------------------------------------------------------//

    public DelegateTool(LuaFunction func) : base(func) { }

    //-----------------------------------------------------------//

    public void DelegateParam0()
    {
        func.BeginPCall();
        func.PCall();
        func.EndPCall();
    }

    public void DelegateParam1(System.Object obj)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    public void DelegateParam2(System.Object obj1, System.Object obj2)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj1);
            func.Push(obj2);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    public void DelegateParam3(System.Object obj1, System.Object obj2, System.Object obj3)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj1);
            func.Push(obj2);
            func.Push(obj3);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    public void DelegateParam4(System.Object obj1, System.Object obj2, System.Object obj3, System.Object obj4)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj1);
            func.Push(obj2);
            func.Push(obj3);
            func.Push(obj4);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    public void DelegateParam5(System.Object obj1, System.Object obj2, System.Object obj3, System.Object obj4, System.Object obj5)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj1);
            func.Push(obj2);
            func.Push(obj3);
            func.Push(obj4);
            func.Push(obj5);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    public void DelegateParam6(System.Object obj1, System.Object obj2, System.Object obj3, System.Object obj4, System.Object obj5, System.Object obj6)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj1);
            func.Push(obj2);
            func.Push(obj3);
            func.Push(obj4);
            func.Push(obj5);
            func.Push(obj6);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    public void DelegateParam7(System.Object obj1, System.Object obj2, System.Object obj3, System.Object obj4, System.Object obj5, System.Object obj6,
        System.Object obj7)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj1);
            func.Push(obj2);
            func.Push(obj3);
            func.Push(obj4);
            func.Push(obj5);
            func.Push(obj6);
            func.Push(obj7);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    public void DelegateParam8(System.Object obj1, System.Object obj2, System.Object obj3, System.Object obj4, System.Object obj5, System.Object obj6,
        System.Object obj7, System.Object obj8)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj1);
            func.Push(obj2);
            func.Push(obj3);
            func.Push(obj4);
            func.Push(obj5);
            func.Push(obj6);
            func.Push(obj7);
            func.Push(obj8);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    public void DelegateParam9(System.Object obj1, System.Object obj2, System.Object obj3, System.Object obj4, System.Object obj5, System.Object obj6,
        System.Object obj7, System.Object obj8, System.Object obj9)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj1);
            func.Push(obj2);
            func.Push(obj3);
            func.Push(obj4);
            func.Push(obj5);
            func.Push(obj6);
            func.Push(obj7);
            func.Push(obj8);
            func.Push(obj9);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    public void DelegateParam10(System.Object obj1, System.Object obj2, System.Object obj3, System.Object obj4, System.Object obj5, System.Object obj6,
        System.Object obj7, System.Object obj8, System.Object obj9, System.Object obj10)
    {
        func.BeginPCall();
        try
        {
            func.Push(obj1);
            func.Push(obj2);
            func.Push(obj3);
            func.Push(obj4);
            func.Push(obj5);
            func.Push(obj6);
            func.Push(obj7);
            func.Push(obj8);
            func.Push(obj9);
            func.Push(obj10);
            func.PCall();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        func.EndPCall();
    }

    //-----------------------------------------------------------//

    public static void AddEventHandler(System.Type eventOwner, System.Object eventInstance, string eventName, LuaFunction func)
    {
        var ei = GetEventInfo(eventOwner, eventName);

        Type toolType = typeof(DelegateTool);
        DelegateTool instance = new DelegateTool(func);

        for (int i = 0; i < FunctionCount; ++i)
        {
            try
            {
                string funName = FunctionName + i;
                MethodInfo mi = GetMethodInfo(toolType, funName);
                Delegate del = Delegate.CreateDelegate(ei.EventHandlerType, instance, mi);
                if (del != null)
                {
                    ei.AddEventHandler(eventInstance, del);
                    break;
                }
            }
            catch (Exception)
            {
            }
        }
    }

    public static EventInfo GetEventInfo(System.Type type, string eventName)
    {
        var flags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        EventInfo ei = type.GetEvent(eventName, flags);
        while (ei == null && type.BaseType != null)
        {
            type = type.BaseType;
            ei = type.GetEvent(eventName, flags);
        }

        return ei;
    }

    public static bool IsStaticEvent(System.Type type, string eventName)
    {
        var flags = BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        EventInfo ei = type.GetEvent(eventName, flags);
        while (ei == null && type.BaseType != null)
        {
            type = type.BaseType;
            ei = type.GetEvent(eventName, flags);
        }

        if (ei != null)
        {
            return true;
        }

        return false;
    }

    public static MethodInfo GetMethodInfo(System.Type type, string methodName)
    {
        System.Reflection.BindingFlags bindingAttr = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;

        System.Reflection.MethodInfo methodInfo = type.GetMethod(methodName, bindingAttr);

        while (methodInfo == null && type.BaseType != null)
        {
            type = type.BaseType;
            methodInfo = type.GetMethod(methodName, bindingAttr);
        }

        return methodInfo;
    }
}

public class DynamicImportTool
{
    private class DelegateInfo
    {
        private Type m_Type = null;
        private MemberInfo m_MemberInfo = null;

        DelegateInfo(System.Type type, MemberInfo memberInfo)
        {
            m_Type = type;
            m_MemberInfo = memberInfo;
        }

        //函数
        public static Delegate GetNewFunctionDelegate(Type type, MemberInfo memberInfo)
        {
            DelegateInfo info = new DelegateInfo(type, memberInfo);
            Delegate del = Delegate.CreateDelegate(typeof(LuaCSFunction), info, DelegateInfo.GetMethodInfo(typeof(DelegateInfo), "DispatchFunctionLogic"));
            return del;
        }
     
        //属性-成员变量
        public static Delegate GetNewGetFunctionDelegate(Type type, MemberInfo memberInfo)
        {
            DelegateInfo info = new DelegateInfo(type, memberInfo);
            Delegate del = Delegate.CreateDelegate(typeof(LuaCSFunction), info, DelegateInfo.GetMethodInfo(typeof(DelegateInfo), "DispatchGetFunctionLogic"));
            return del;
        }

        //属性-成员变量
        public static Delegate GetNewSetFunctionDelegate(Type type, MemberInfo memberInfo)
        {
            DelegateInfo info = new DelegateInfo(type, memberInfo);
            Delegate del = Delegate.CreateDelegate(typeof(LuaCSFunction), info, DelegateInfo.GetMethodInfo(typeof(DelegateInfo), "DispatchSetFunctionLogic"));
            return del;
        }

        //构造函数
        public static Delegate GetNewConstructorDelegate(Type type)
        {
            DelegateInfo info = new DelegateInfo(type, null);
            Delegate del = Delegate.CreateDelegate(typeof(LuaCSFunction), info, DelegateInfo.GetMethodInfo(typeof(DelegateInfo), "DispatchConstructorFunctionLogic"));
            return del;
        }

        //事件
        public static Delegate GetNewGetEventDelegate(Type type, MemberInfo memberInfo)
        {
            DelegateInfo info = new DelegateInfo(type, memberInfo);
            Delegate del = Delegate.CreateDelegate(typeof(LuaCSFunction), info, DelegateInfo.GetMethodInfo(typeof(DelegateInfo), "DispatchNewGetEventDelegate"));
            return del;
        }

        //事件
        public static Delegate GetNewSetEventDelegate(Type type, MemberInfo memberInfo)
        {
            DelegateInfo info = new DelegateInfo(type, memberInfo);
            Delegate del = Delegate.CreateDelegate(typeof(LuaCSFunction), info, DelegateInfo.GetMethodInfo(typeof(DelegateInfo), "DispatchNewSetEventDelegate"));
            return del;
        }

        //-----------------------------------------------------------------------------//

        public static MethodInfo GetMethodInfo(System.Type type, string methodName)
        {
            System.Reflection.BindingFlags bindingAttr = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance 
                | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;

            System.Reflection.MethodInfo methodInfo = type.GetMethod(methodName, bindingAttr);

            while (methodInfo == null && type.BaseType != null)
            {
                type = type.BaseType;
                methodInfo = type.GetMethod(methodName, bindingAttr);
            }

            return methodInfo;
        }

        //-----------------------------------------------------------------------------//

        //构造函数
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public int DispatchConstructorFunctionLogic(IntPtr L)
        {
            try
            {
                //string str = string.Format("DynamicImportingTool.DelegateInfo.DispatchConstructorFunctionLogic: type name {0}, function name {1} ", m_Type.Name);
                //UnityEngine.Debug.Log(str);

                int count = LuaDLL.lua_gettop(L);
                List<System.Object> list = new List<object>();
                for (int i = 1; i <= count; ++i)
                {
                    System.Object obj = ToLua.ToVarObject(L, i);
                    list.Add(obj);
                }

                if(list.Count > 0)
                {
                    var obj = m_Type.GetConstructor(Type.EmptyTypes).Invoke(list.ToArray());
                    ToLua.Push(L, obj);
                    return 1;
                }
                else
                {
                    var obj = m_Type.GetConstructor(Type.EmptyTypes).Invoke(null);
                    ToLua.Push(L, obj);
                    return 1;
                }
            }
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }
        }

        //函数
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public int DispatchFunctionLogic(IntPtr L)
        {
            try
            {
                //string str = string.Format("DynamicImportingTool.DelegateInfo.DispatchFunctionLogic: type name {0}, function name {1} ", m_Type.Name, m_MemberInfo.Name);
                //UnityEngine.Debug.LogError(str);

                MethodInfo mi = m_Type.GetMethod(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if(mi != null)
                {
                    //实例
                    int count = LuaDLL.lua_gettop(L);
                    System.Object instance = ToLua.ToVarObject(L, 1);

                    List<System.Object> list = new List<object>();
                    for (int i = 2; i <= count; ++i)
                    {
                        System.Object obj = ToLua.ToVarObject(L, i);
                        list.Add(obj);
                    }

                    System.Object retObj = ReflectionTool.CallInstanceFunction(instance, m_MemberInfo.Name, list.ToArray());
                    if (mi.ReturnType != typeof(void))
                    {
                        ToLua.Push(L, retObj);
                    }

                    return 1;
                }

                mi = m_Type.GetMethod(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (mi != null)
                {
                    //静态
                    int count = LuaDLL.lua_gettop(L);
                    List<System.Object> list = new List<object>();
                    for (int i = 1; i <= count; ++i)
                    {
                        System.Object obj = ToLua.ToVarObject(L, i);
                        list.Add(obj);
                    }

                    System.Object retObj = ReflectionTool.CallStaticFunction(m_Type, m_MemberInfo.Name, list.ToArray());
                    if (mi.ReturnType != typeof(void))
                    {
                        ToLua.Push(L, retObj);
                    }

                    return 1;
                }

                return 1;
            }
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }
        }

        //属性-成员变量
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public int DispatchSetFunctionLogic(IntPtr L)
        {
            try
            {
                //string str = string.Format("DynamicImportingTool.DelegateInfo.DispatchFunctionLogic: type name {0}, function name {1} ", m_Type.Name, m_MemberInfo.Name);
                //UnityEngine.Debug.LogError(str);

                int count = LuaDLL.lua_gettop(L);

                if (m_MemberInfo.MemberType == MemberTypes.Field)
                {
                    FieldInfo fi = m_Type.GetField(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if(fi != null)
                    {
                        System.Object instance = ToLua.ToVarObject(L, 1);
                        System.Object value = ToLua.ToVarObject(L, 2);
                        ReflectionTool.SetInstanceFieldValue(instance, m_MemberInfo.Name, value);
                        return 0;
                    }

                    fi = m_Type.GetField(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fi != null)
                    {
                        System.Object value1 = ToLua.ToVarObject(L, 1);
                        System.Object value2 = ToLua.ToVarObject(L, 2);
                        ReflectionTool.SetStaticFieldValue(m_Type, m_MemberInfo.Name, value2);
                        return 0;
                    }
                }
                else if(m_MemberInfo.MemberType == MemberTypes.Property)
                {
                    PropertyInfo pi = m_Type.GetProperty(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (pi != null)
                    {
                        System.Object instance = ToLua.ToVarObject(L, 1);
                        System.Object value = ToLua.ToVarObject(L, 2);
                        ReflectionTool.SetInstancePropertyValue(instance, m_MemberInfo.Name, value);
                        return 0;
                    }

                    pi = m_Type.GetProperty(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (pi != null)
                    {
                        System.Object value1 = ToLua.ToVarObject(L, 1);
                        System.Object value2 = ToLua.ToVarObject(L, 2);
                        ReflectionTool.SetStaticPropertyValue(m_Type, m_MemberInfo.Name, value2);
                        return 0;
                    }
                }

                return 0;
            }
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }
        }

        //属性-成员变量
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public int DispatchGetFunctionLogic(IntPtr L)
        {
            try
            {
                int count = LuaDLL.lua_gettop(L);

                if (m_MemberInfo.MemberType == MemberTypes.Field)
                {
                    FieldInfo fi = m_Type.GetField(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fi != null)
                    {
                        System.Object instance = ToLua.ToVarObject(L, 1);
                        System.Object value = ReflectionTool.GetInstanceFieldValue(instance, m_MemberInfo.Name);
                        ToLua.Push(L, value);
                        return 1;
                    }

                    fi = m_Type.GetField(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fi != null)
                    {
                        System.Object value = ReflectionTool.GetStaticFieldValue(m_Type, m_MemberInfo.Name);
                        ToLua.Push(L, value);
                        return 1;
                    }
                }
                else if (m_MemberInfo.MemberType == MemberTypes.Property)
                {
                    PropertyInfo pi = m_Type.GetProperty(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (pi != null)
                    {
                        System.Object instance = ToLua.ToVarObject(L, 1);
                        System.Object value = ReflectionTool.GetInstancePropertyValue(instance, m_MemberInfo.Name);
                        ToLua.Push(L, value);
                        return 1;
                    }

                    pi = m_Type.GetProperty(m_MemberInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    if (pi != null)
                    {
                        System.Object value = ReflectionTool.GetStaticPropertyValue(m_Type, m_MemberInfo.Name);
                        ToLua.Push(L, value);
                        return 1;
                    }
                }

                string str = string.Format("DynamicImportingTool.DelegateInfo.DispatchFunctionLogic: type name {0}, function name {1} ", m_Type.Name, m_MemberInfo.Name);
                UnityEngine.Debug.LogError(str);
                return 1;
            }
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }
        }

        //事件
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public int DispatchNewGetEventDelegate(IntPtr L)
        {
            try
            {
                // ToLua.Push(L, new EventObject(m_Type.Name + m_MemberInfo.Name));
                return 1;
            }
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }
        }

        //事件
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public int DispatchNewSetEventDelegate(IntPtr L)
        {
            try
            {
                EventObject arg0 = null;

                int count = LuaDLL.lua_gettop(L);

                if (LuaDLL.lua_isuserdata(L, 2) != 0)
                {
                    arg0 = (EventObject)ToLua.ToObject(L, 2);
                }
                else
                {
                    return LuaDLL.luaL_throw(L, "The event 'DynamicImportTool.DelegateInfo.DispatchNewSetEventDelegate' can only appear on the left hand side of += or -= when used outside of the type 'UnityEngine.Application'");
                }

                System.Object instance = ToLua.ToVarObject(L, 1);
                System.Object value = ToLua.ToVarObject(L, 2);

                EventInfo ei = ReflectionTool.GetInstanceEventInfo(m_Type, m_MemberInfo.Name);
                if (ei == null)
                {
                    instance = null;
                    ei = ReflectionTool.GetStaticEventInfo(m_Type, m_MemberInfo.Name);
                }
                
                if (ei != null)
                {
                    if (arg0.op == EventOp.Add)
                    {
                        // DelegateTool.AddEventHandler(m_Type, instance, m_MemberInfo.Name, arg0.func);
                    }
                    else if(arg0.op == EventOp.Sub)
                    {
                        Delegate ev = LuaMisc.GetEventHandler(instance, m_Type, m_MemberInfo.Name);
                        Delegate[] ds = ev.GetInvocationList();
                        LuaState state = LuaState.Get(L);

                        // for (int i = 0; i < ds.Length; i++)
                        // {
                        //     ev = ds[i];
                        //     LuaDelegate ld = ev.Target as LuaDelegate;

                        //     if (ld != null && ld.func == arg0.func)
                        //     {
                        //         ei.RemoveEventHandler(instance, ev);
                        //         state.DelayDispose(ld.func);
                        //         break;
                        //     }
                        // }

                        // arg0.func.Dispose();
                    }
                    return 0;
                }

                return 0;
            }
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }
        }
    }

    //-----------------------------------------------------------------------------//

    static LuaState m_LuaState = null;

    public static void Init(LuaState luaState)
    {
        m_LuaState = luaState;
    }

    public static bool IsInit()
    {
        return m_LuaState != null;
    }

    //-----------------------------------------------------------------------------//

    public static void Import(string typeName)
    {
        System.Type type = System.Type.GetType(typeName);
        Import(type);
    }

    public static void Import(System.Type type)
    {
        if(string.IsNullOrEmpty(type.Namespace))
        {
            m_LuaState.BeginModule(null);
            Register(type);
            m_LuaState.EndModule();
            return;
        }

        string[] nps = type.Namespace.Split('.');
        if(nps != null && nps.Length > 0)
        {
            m_LuaState.BeginModule(null);

            //导入命名空间
            for (int i = 0; i < nps.Length; ++i)
            {
                string space = nps[i];
                m_LuaState.BeginModule(space);
            }

            Register(type);

            for (int i = nps.Length -1; i >= 0; --i)
            {
                string space = nps[i];
                m_LuaState.EndModule();
            }

            m_LuaState.EndModule();
        }
    }

    private static void Register(System.Type type)
    {
        if(type.IsClass)
        {
            RegisterClass(type);
        }
        else if(type.IsEnum)
        {
            RegisterEnum(type);
        }
    }

    private static void RegisterClass(System.Type type)
    {
        m_LuaState.BeginClass(type, type.BaseType);

        List<MemberInfo> mbs = GetTypeMembers(type);
        if (mbs != null && mbs.Count > 0)
        {
            foreach (MemberInfo mi in mbs)
            {
                switch (mi.MemberType)
                {
                    case MemberTypes.Field:
                        {
                            Delegate delSet = DelegateInfo.GetNewSetFunctionDelegate(type, mi);
                            Delegate delGet = DelegateInfo.GetNewGetFunctionDelegate(type, mi);
                            m_LuaState.RegVar(mi.Name, (LuaCSFunction)delGet, (LuaCSFunction)delSet);
                        }
                        break;
                    case MemberTypes.Property:
                        {
                            Delegate delSet = DelegateInfo.GetNewSetFunctionDelegate(type, mi);
                            Delegate delGet = DelegateInfo.GetNewGetFunctionDelegate(type, mi);
                            m_LuaState.RegVar(mi.Name, (LuaCSFunction)delGet, (LuaCSFunction)delSet);
                        }
                        break;
                    case MemberTypes.Event:
                        {
                            Delegate delSet = DelegateInfo.GetNewSetEventDelegate(type, mi);
                            Delegate delGet = DelegateInfo.GetNewGetEventDelegate(type, mi);
                            m_LuaState.RegVar(mi.Name, (LuaCSFunction)delGet, (LuaCSFunction)delSet);
                        }
                        break;
                    default:
                        Delegate del = DelegateInfo.GetNewFunctionDelegate(type, mi);
                        m_LuaState.RegFunction(mi.Name, (LuaCSFunction)del);
                        break;
                }
            }
        }

        Delegate delNew = DelegateInfo.GetNewConstructorDelegate(type);
        m_LuaState.RegFunction("New", (LuaCSFunction)delNew);
        m_LuaState.RegFunction("__tostring", ToLua.op_ToString);

        m_LuaState.EndClass();
    }

    private static void RegisterEnum(System.Type type)
    {
        m_LuaState.BeginEnum(type);

        List<MemberInfo> mbs = GetTypeMembers(type);
        if (mbs != null && mbs.Count > 0)
        {
            foreach (MemberInfo mi in mbs)
            {
                switch (mi.MemberType)
                {
                    case MemberTypes.Field:
                    case MemberTypes.Property:
                        Delegate delGet = DelegateInfo.GetNewGetFunctionDelegate(type, mi);
                        m_LuaState.RegVar(mi.Name, (LuaCSFunction)delGet, null);
                        break;
                    default:
                        Delegate del = DelegateInfo.GetNewFunctionDelegate(type, mi);
                        m_LuaState.RegFunction(mi.Name, (LuaCSFunction)del);
                        break;
                }
            }
        }

        m_LuaState.EndEnum();
    }

    private static List<MemberInfo> GetTypeMembers(System.Type type)
    {
        Dictionary<string, MemberInfo> dict = new Dictionary<string, MemberInfo>();

        GetTypeMembersImp(type, ref dict);

        while (type.BaseType != null)
        {
            type = type.BaseType;
            GetTypeMembersImp(type, ref dict);
        }

        var list = new List<MemberInfo>(dict.Values);
        return list;
    }

    private static void GetTypeMembersImp(System.Type type, ref Dictionary<string, MemberInfo> dict)
    {
        System.Reflection.BindingFlags bindingAttr = BindingFlags.CreateInstance | BindingFlags.IgnoreCase |
            BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty |
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var members = type.GetMembers(bindingAttr);

        if (members != null && members.Length > 0)
        {
            foreach (MemberInfo mi in members)
            {
                if (dict.ContainsKey(mi.Name))
                {
                    MemberInfo oldMI = dict[mi.Name];
                    if (oldMI.MemberType == MemberTypes.Field && mi.MemberType == MemberTypes.Event)
                    {
                        dict[mi.Name] = mi;
                        continue;
                    }

                    if (oldMI.MemberType == MemberTypes.Event && mi.MemberType == MemberTypes.Field)
                    {
                        continue;
                    }

                    //string str = string.Format("DynamicImportTool.GetTypeMembers:{0}有名字为{1}的重复MemberInfo", type.Name, mi.Name);
                    //Debug.LogError(str);
                    continue;
                }

                dict.Add(mi.Name, mi);
            }
        }
    }

    //-----------------------------------------------------------------------------//

    public static string ToString(System.Object obj)
    {
        return obj.ToString();
    }
}

//暴露给Lua使用
public class LuaDynamicImportTool
{
    public static void Import(string typeName)
    {
        if(DynamicImportTool.IsInit() == false)
        {
            // LuaManager mgr = (LuaManager)LuaManager.Instance;
            // DynamicImportTool.Init(mgr.GetLuaState());
        }

        DynamicImportTool.Import(typeName);
    }
}

public class AssemblyTool
{
    public List<System.Type> GetAllTypes()
    {
        List<Assembly> listAssembly = new List<Assembly>();

        //mscorlib.dll
        listAssembly.Add(Assembly.GetAssembly(typeof(System.Byte)));

        //System.dll
        listAssembly.Add(Assembly.GetAssembly(typeof(System.Net.Sockets.Socket)));

        //System.Core.dll
        listAssembly.Add(Assembly.GetAssembly(typeof(System.Linq.Enumerable)));

        //UnityEngine.dll
        listAssembly.Add(Assembly.GetAssembly(typeof(UnityEngine.Application)));

        //UnityEngine.UI.dll
        listAssembly.Add(Assembly.GetAssembly(typeof(UnityEngine.UI.Text)));
        
        //Plugins------Assembly-CSharp-firstpass.dll
        listAssembly.Add(Assembly.GetAssembly(typeof(DebugLogManager)));

        //Assembly-CSharp.dll
        // listAssembly.Add(Assembly.GetAssembly(typeof(GlobalManager)));

        List<Type> listType = new List<Type>();

        foreach (var item in listAssembly)
        {
            listType.AddRange(item.GetTypes());
        }

        return listType;
    }

    public List<Type> GetDelegateTypes(Type type)
    {
        List<Type> listType = new List<Type>();

        System.Reflection.BindingFlags flag = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | 
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;

        PropertyInfo[] pis = type.GetProperties(flag);
        FieldInfo[] fis = type.GetFields(flag);
        foreach (var item in pis)
        {
            Type delType = item.PropertyType.BaseType;
            if (delType == typeof(System.Delegate) || delType == typeof(System.MulticastDelegate))
            {
                listType.Add(delType);
            }
        }

        return listType;
    }

    public void Generate()
    {
        List<Type> listType = GetAllTypes();
        foreach (var item in listType)
        {
            EventInfo[] eis = item.GetEvents();
            foreach (var ei in eis)
            {
                
            }
        }
    }
}