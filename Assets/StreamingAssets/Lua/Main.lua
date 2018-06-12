--主入口函数。从这里开始lua逻辑
function Main()
    math.randomseed(os.time())
    print("logic start")
    print("logic start11")
    print("logic start8888888888888888888")
    coroutine.start(ImprotFiles)
end

local function InitMain()
    UIWindowFirstLoading.Close()
    loginController:Open()
end

--场景切换通知
function OnLevelWasLoaded(level)
    collectgarbage("collect")
    Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end

function ImprotFiles()
    print("ImprotFiles start")
    coroutine.wait(0.01)
    require("Libs/LibsHead")
    coroutine.wait(0.01)
    require("UI/UIHead")
    coroutine.wait(0.01)
    InitMain()
end
