--主入口函数。从这里开始lua逻辑
function Main()					
	print("logic start")
	-- local textTrans= ResourcesManager.GetInstanceGameOject("Perfabs/Text").transform	 
	-- textTrans:SetParent(UnityEngine.GameObject.Find("Canvas").transform)
	-- textTrans.localPosition=Vector3.zero		
	UnityEngine.GameObject.Find("Text(Clone)"):GetComponent("Text").text="sssssssssssssssss"
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end