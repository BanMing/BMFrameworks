local LoginView = class("LoginView", BaseView)

function LoginView:initialize()
    BaseView.initialize(self)
    self.UICom = {
        {Name = "LoginBtn", Type = Button}
    }
end
function LoginView:CreateInit()
    BaseView.CreateInit(self)
    self.LoginBtn.onClick:AddListener(
        function()
            print("@@@@@@@@")
        end
    )
end

function LoginView:ShowInit()
    BaseView.ShowInit(self)
end

function LoginView:Show()
    BaseView.Show(self)
end

function LoginView:CloseInit()
    BaseView.CloseInit(self)
end

function LoginView:Close()
    BaseView.Close(self)
end
loginView = LoginView()
