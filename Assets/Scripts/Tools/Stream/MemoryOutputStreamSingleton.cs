/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:	
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
public class MemoryOutputStreamSingleton : MemoryOutputStream
{
	private static MemoryOutputStreamSingleton _instance;
	private MemoryOutputStreamSingleton() : base(4096)
	{
	}

	public static MemoryOutputStreamSingleton GetInstance()
	{
		if (MemoryOutputStreamSingleton._instance == null)
		{
			MemoryOutputStreamSingleton._instance = new MemoryOutputStreamSingleton();
		}
		return MemoryOutputStreamSingleton._instance;
	}
}
