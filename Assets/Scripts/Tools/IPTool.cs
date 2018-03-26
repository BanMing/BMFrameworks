/******************************************************************
** 文件名:	
** 版  权:  (C)  
** 创建人:  Liange
** 日  期:  2016/09/21
** 描  述: 	

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/

using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.InteropServices;

public class IPTool
{
#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern string getIPv6(string mHost);  
#endif

    public static IPAddress GetIPAddress(string ip)
    {
        try
        {
            IPAddress address = null;

            //解析域名
            IPAddress[] ipAddress = Dns.GetHostAddresses(ip);
            if (ipAddress.Length == 0)
            {
                string str = string.Format("GetIPAddress:Dns.GetHostAddresses Error, URL is {0}", ip);
                UnityEngine.Debug.LogError(str);
            }
            else if (ipAddress.Length == 1)
            {
                address = ipAddress[0];
            }
            else
            {
                foreach (IPAddress add in ipAddress)
                {
                    if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)
                    {
                        if (add.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            address = add;
                            break;
                        }
                    }
                    else
                    {
                        address = add;
                        break;
                    }
                }

                //如果在IOS平台没有取到IP V6的地址，则直接取第一个ip地址
                if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer && address == null)
                {
                    address = ipAddress[0];
                }
            }

            //针对服务器没有IPV6的情况
            if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer && address.AddressFamily != AddressFamily.InterNetworkV6)
            {
#if UNITY_IPHONE
                string newIP = getIPv6(ip);
                address = IPAddress.Parse(newIP);
#endif
            }

            return address;
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogException(ex);
            return null;
        }
    }
}
