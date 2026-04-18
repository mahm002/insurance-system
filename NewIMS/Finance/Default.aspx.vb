Imports DevExpress.Web

Public Class _Default2
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = CType(Session("UserInfo"), List(Of String))
        'Call SetUserPermNAV(FindControlRecursive(Form, "SideBar"), myList.ToArray, 3)
        If myList Is Nothing Then
            FormsAuthentication.SignOut()
            'FormsAuthentication.RedirectToLoginPage()
            'ASPxWebControl.RedirectOnCallback(TARGET_URL)
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 3)
        End If
    End Sub

End Class