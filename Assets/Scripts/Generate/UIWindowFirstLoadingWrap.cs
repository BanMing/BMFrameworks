﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UIWindowFirstLoadingWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UIWindowFirstLoading), typeof(SingletonGameObject<UIWindowFirstLoading>));
		L.RegFunction("Init", Init);
		L.RegFunction("ShowText", ShowText);
		L.RegFunction("SetTargetProgress", SetTargetProgress);
		L.RegFunction("UpdateProgress", UpdateProgress);
		L.RegFunction("Close", Close);
		L.RegFunction("Show", Show);
		L.RegFunction("Hide", Hide);
		L.RegFunction("HideUpdateItems", HideUpdateItems);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegConstant("StartProgressValue", 0.200000002980232);
		L.RegConstant("InitResProgressValue", 0.5);
		L.RegConstant("FinishResProgressValue", 1);
		L.RegConstant("FinishLuaProgressValue", 1);
		L.RegConstant("FullProgressValue", 1);
		L.RegVar("m_TargetProgress", get_m_TargetProgress, set_m_TargetProgress);
		L.RegVar("m_CurProgress", get_m_CurProgress, set_m_CurProgress);
		L.RegVar("ResPath", get_ResPath, set_ResPath);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UIWindowFirstLoading obj = (UIWindowFirstLoading)ToLua.CheckObject<UIWindowFirstLoading>(L, 1);
			obj.Init();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowText(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UIWindowFirstLoading obj = (UIWindowFirstLoading)ToLua.CheckObject<UIWindowFirstLoading>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			obj.ShowText(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTargetProgress(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UIWindowFirstLoading obj = (UIWindowFirstLoading)ToLua.CheckObject<UIWindowFirstLoading>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.SetTargetProgress(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateProgress(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UIWindowFirstLoading obj = (UIWindowFirstLoading)ToLua.CheckObject<UIWindowFirstLoading>(L, 1);
				obj.UpdateProgress();
				return 0;
			}
			else if (count == 2)
			{
				UIWindowFirstLoading obj = (UIWindowFirstLoading)ToLua.CheckObject<UIWindowFirstLoading>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				obj.UpdateProgress(arg0);
				return 0;
			}
			else if (count == 3)
			{
				UIWindowFirstLoading obj = (UIWindowFirstLoading)ToLua.CheckObject<UIWindowFirstLoading>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				string arg1 = ToLua.CheckString(L, 3);
				obj.UpdateProgress(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UIWindowFirstLoading.UpdateProgress");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Close(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UIWindowFirstLoading.Close();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Show(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UIWindowFirstLoading.Show();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Hide(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UIWindowFirstLoading.Hide();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HideUpdateItems(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UIWindowFirstLoading.HideUpdateItems();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_TargetProgress(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIWindowFirstLoading obj = (UIWindowFirstLoading)o;
			float ret = obj.m_TargetProgress;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_TargetProgress on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_m_CurProgress(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIWindowFirstLoading obj = (UIWindowFirstLoading)o;
			float ret = obj.m_CurProgress;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_CurProgress on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ResPath(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, UIWindowFirstLoading.ResPath);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_TargetProgress(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIWindowFirstLoading obj = (UIWindowFirstLoading)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.m_TargetProgress = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_TargetProgress on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_m_CurProgress(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIWindowFirstLoading obj = (UIWindowFirstLoading)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.m_CurProgress = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index m_CurProgress on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ResPath(IntPtr L)
	{
		try
		{
			string arg0 = ToLua.CheckString(L, 2);
			UIWindowFirstLoading.ResPath = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

