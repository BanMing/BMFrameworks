local LoginController = class("LoginController", BaseController)

function LoginController:Opne()
    BaseController.Open(self)
end

function LoginController:Close()
    BaseController.Close(self)
end

loginController = LoginController()
