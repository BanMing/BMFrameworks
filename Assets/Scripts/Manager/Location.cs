using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Location : MonoBehaviour {
	// Use this for initialization
	private bool isGPS = false;									//是否正在定位
	private bool hasGotLoc = false;								//是否已获取到位置
	//private bool isReachable = true;							//定位功能是否可用
	private float latitude = -1.0f;								//纬度
	private float longitude = -1.0f;							//经度
	private float gpsAccuracy = 300;							//精度
	private float gpsUpdateDistance = 100;						//刷新距离
	private float gpsTimeStamp = -1;							//时间戳
	private System.Action<Vector3> actionUpdate;				//UpdateLoc方法回调
	private System.Action<Vector3> actionGet;					//GetLoc方法回调
	IEnumerator Gps()//GPS定位
	{
		isGPS = true;
		if (!Input.location.isEnabledByUser) {//GPS不可用
			print ("GPS not reachable");
			//isReachable = false;
			isGPS = false;
			if (actionUpdate != null) {	
				actionUpdate (new Vector3 (longitude, latitude, gpsTimeStamp));
				actionUpdate = null;
			}
			if (actionGet != null) {
				actionGet (new Vector3 (longitude, latitude, gpsTimeStamp));
				actionGet = null;
			}
			yield break;
		}
		//isReachable = true;
		Input.location.Start(gpsAccuracy,gpsUpdateDistance);//启动GPS
		int maxWait = 20;//最大等待时间
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {//等待GPS启动成功
			yield return new WaitForSeconds (1);
			print (maxWait);
			maxWait--;
		}
		if (maxWait < 1) {//启动GPS超时
			print ("GPS Time Out");
			isGPS = false;
			actionUpdate = null;
			actionGet = null;
			yield break;
		}

		if (Input.location.status == LocationServiceStatus.Failed) {//GPS启动失败
			print ("GPS Unable to determine device location");
		} else {	//获取到位置信息
			print ("GPS Location: " + Input.location.lastData.latitude
				+ " " + Input.location.lastData.longitude 
				+ " in " + Input.location.lastData.timestamp);
			latitude = Input.location.lastData.latitude;
			longitude = Input.location.lastData.longitude;
			gpsTimeStamp = (float)Input.location.lastData.timestamp;
			hasGotLoc = true;
			//执行回调
			if (actionUpdate != null) {	
				actionUpdate (new Vector3 (longitude, latitude, gpsTimeStamp));
				actionUpdate = null;
			}
			if (actionGet != null) {
				actionGet (new Vector3 (longitude, latitude, gpsTimeStamp));
				actionGet = null;
			}
		}
		Input.location.Stop ();
		isGPS = false;
	}
	public void GetLoc(System.Action<Vector3> callback){//开始获取位置
		if (!isGPS && !hasGotLoc) {
			print ("Start GPS..");
			actionGet = callback;
			StartCoroutine (Gps ());
		} else if(isGPS){
			print("GPS is running");
			actionGet = callback;
		}
	}
	public void UpdateLoc(System.Action<Vector3> callback)//开始更新位置
	{
		if (!isGPS) {
			print ("Update Location..");
			actionUpdate = callback;
			StartCoroutine (Gps ());
		} else {
			print ("GPS is running");
		}
	}
	public float GetLatitude(){//返回获取到的纬度
		return hasGotLoc ? latitude : -1.0f;
	}
	public float GetLongitude(){//返回获取到的经度
		return hasGotLoc ? longitude : -1.0f;
	}
	public void SetGpsAccuracy(float accuracy)//设置精度
	{
		gpsAccuracy = accuracy;
	}
	public void SetGpsUpdateDistance(float updateDistance)//设置更新距离
	{
		gpsUpdateDistance = updateDistance;
	}
}
