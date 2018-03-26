/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:	2014.2
** 描  述: 	
 
     !!!!!!!!!!!!!!!!!!!!!!!警告!!!!!!!!!!!!!!!!!!!!!!!!
    MemoryInputStream和MemoryOutputStream在序列和反序列化结构体时，
    IOS平台的复合数据类型只能是struct不能是class,且不能使用数组
        

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MemoryInputStream
{
	private byte[] _buffer;
	private int _bufferSize;
	private int _pos;

	public MemoryInputStream(byte[] buffer, int bufferSize, int startPos)
	{
		this._bufferSize = bufferSize;
		this._buffer = buffer;
		this._pos = startPos;
	}

	public MemoryInputStream(byte[] buffer)
	{
		this._buffer = buffer;
		this._bufferSize = buffer.Length;
		this._pos = 0;
	}

	public T Pop<T>() where T : new()
	{
		T result;
		try
		{
			int num = Marshal.SizeOf(typeof(T));
			if (this._pos + num > this._bufferSize)
			{
				Debug.LogError("MemoryInputStream.Pop: Reach Buffer End");
				this._pos = this._bufferSize;
				result = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			}
			else
			{
				IntPtr intPtr = Marshal.AllocHGlobal(num);
				Marshal.Copy(this._buffer, this._pos, intPtr, num);
				T t = (T)((object)Marshal.PtrToStructure(intPtr, typeof(T)));
				Marshal.FreeHGlobal(intPtr);
				this._pos += num;
				result = t;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("MemoryInputStream:Pop" + ex.Message);
			result = default(T);
		}
		return result;
	}

	public byte[] PopByteArray(int size)
	{
		if (size == 0)
		{
			return null;
		}

		if (size < 0)
		{
			Debug.LogError("MemoryInputStream.PopByteArray: size less than 0");
		}

		if (this._pos + size > this._bufferSize)
		{
			Debug.LogError("MemoryInputStream.PopByteArray: Reach Buffer End");
			this._pos = this._bufferSize;
			return null;
		}

		byte[] array = new byte[size];
		Array.Copy(this._buffer, this._pos, array, 0, size);
		this._pos += size;
		return array;
	}

	public bool IsEnd()
	{
		return this._pos >= this._bufferSize;
	}
}
