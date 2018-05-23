--主入口函数。从这里开始lua逻辑
function Main()					
	print("logic start")	 
	coroutine.start(ImprotFiles)
end

function InitMain()	
	UIWindowFirstLoading.Close()
	MyUnityTool.Find("UIRoot/TopRoot/CommonViews"):SetActive(true)
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
	
end

function ImprotFiles()
	coroutine.wait(1)
	InitMain()
end