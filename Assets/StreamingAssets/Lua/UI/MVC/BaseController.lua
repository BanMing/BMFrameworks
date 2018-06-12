BaseController = class("BaseController")

function BaseController:initialize(...)
    self.view = nil
    self.isAreadyLoad = false
end

function BaseController:Open()
    if not self.isAreadyLoad then
        self.view:LoadInstance()
    end
    self.view:Show()
end

function BaseController:Close(...)
end
