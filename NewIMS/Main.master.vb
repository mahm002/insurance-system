Imports DevExpress.Web

Public Class MainMaster
    Inherits MasterPage

    Private Sub MainMaster_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If IsNothing(Session("LogInfo")) Then
            FormsAuthentication.SignOut()
            'FormsAuthentication.RedirectToLoginPage()
            'ASPxWebControl.RedirectOnCallback(TARGET_URL)
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", Page.AppRelativeVirtualPath))
        Else
            'Call SetUserPermNAV(FindControlRecursive(Me, "SideBar"), Session("LogInfo"), 1)
            'Call SetUserPermNAV(Master.FindControl("SideBar"), Session("LogInfo"), 1)
        End If
    End Sub

End Class