Imports DevExpress.Web

Public Class Months
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = CType(Session("UserInfo"), List(Of String))
        If myList Is Nothing Then
            'ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
            FormsAuthentication.RedirectToLoginPage()
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 3)
        End If

    End Sub

    Protected Sub DelRow(sender As Object, e As Data.ASPxDataDeletingEventArgs)
        Session("Month") = e.Values("Month")

        grid.DataBind()
    End Sub

    Protected Sub InsRow(sender As Object, e As Data.ASPxDataInsertingEventArgs)
        e.NewValues("Month") = Format(CDate(e.NewValues("Month")), "yyyy/MM")

        grid.DataBind()
    End Sub

End Class