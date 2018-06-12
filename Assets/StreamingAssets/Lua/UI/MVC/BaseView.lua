BaseView = class("BaseView")

function BaseView:initialize(...)
    self.transform = nil
    self.viewName = nil
    self.uiLayer = nil
    --UICom 格式{Name="CloseBtn",Type=Button}
    self.UICom = {}
end

function BaseView:LoadInstance()
    local prefabName=self.viewName.."View.prefab"
    local obj=ResourcesManager.Instance:GetUIInstanceSync(prefabName,"")
    -- print(obj.transform.name)
    self.transform=obj.transform
    self.transform.name=self.viewName
    local rectTransform=self.transform:GetComponent("RectTransform")
    MyUnityTool.SetUIParentWithLocalInfo(rectTransform,self.uiLayer)
    MyUnityTool.SetActive(self.transform,false)
    self:AutoGetUICom()
    self:CreateInit()
end

function BaseView:CreateInit()
     
end

function BaseView:ShowInit()
     
end

function BaseView:Show()
    print(self.viewName," open")
    self:ShowInit()
    MyUnityTool.SetActive(self.transform,true)
end

function BaseView:CloseInit()
     
end

function BaseView:Close()
    self:CloseInit()
    MyUnityTool.SetActive(self.transform,false)
end

--[[
    @desc: 找到ui组件
    author:{banming}
    time:2018-06-11 16:02:14
    return
]]
function BaseView:AutoGetUICom()
    for k, v in pairs(self.UICom) do
        -- print("...v.Name::",v.Name,"v.Type::",v.Type)
        self[v.Name] = MyUnityTool.FindScriptInChild(self.transform.gameObject, typeof(v.Type), v.Name)
    end
end
