Imports DevExpress.Web

Partial Public Class MDefault

    Inherits Page
    Public Shared CurUser As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('Policy/PolMain.aspx','_self'); </script>")
        'Response.Write("<script> window.open('http://" & IHost & domainName & ApplicationName & "/policymanagement/Default.aspx" & "','_self'); </script>")
        Dim myList = CType(Session("UserInfo"), List(Of String))
        If myList Is Nothing Then
            FormsAuthentication.RedirectToLoginPage(AppRelativeVirtualPath)
        Else
            Dim stringArray As String() = New String() {myList.Item(1).Trim, myList.Item(2).Trim, myList.Item(3).Trim, myList.Item(4).Trim, myList.Item(5).Trim, myList.Item(6).Trim}
            Dim longest As String = stringArray.OrderByDescending(Function(x) x.Length).FirstOrDefault()

            Dim Hmenue = TryCast(FindControlRecursive(Master, "HeaderMenu"), ASPxMenu)
            Hmenue.Items.IndexOfName(0)

            Select Case longest
                Case = myList.Item(1)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('Policy/','_self'); </script>")
                Case = myList.Item(2)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('#','_self'); </script>")
                Case = myList.Item(3)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('Finance/','_self'); </script>")
                Case = myList.Item(4)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('#','_self'); </script>")
                Case = myList.Item(5)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('#','_self'); </script>")
                Case = myList.Item(6)
                    ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/','_self'); </script>")
            End Select

            'If longest = myList.Item(1) Then Hmenue.SelectedItem.Index = 0 'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('Policy/PolMain.aspx','_self'); </script>") : Exit Sub 'Response.Redirect("~/Policy/PolMain.aspx")
            'If longest = myList.Item(2) Then Hmenue.SelectedItem.Index = 1 'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('#','_self'); </script>")
            'If longest = myList.Item(3) Then Hmenue.SelectedItem.Index = 2 'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('Finance/Coa.aspx','_self'); </script>") : Exit Sub 'Response.Redirect("~/Finance/Coa.aspx")
            'If longest = myList.Item(4) Then Hmenue.SelectedItem.Index = 3 'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('#','_self'); </script>") : Exit Sub
            'If longest = myList.Item(5) Then Hmenue.SelectedItem.Index = 4 'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('#','_self'); </script>") : Exit Sub
            'If longest = myList.Item(6) Then Hmenue.SelectedItem.Index = 5 'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/Default.aspx','_self'); </script>") : Exit Sub 'Response.Redirect("~/SystemManage/Default.aspx")
        End If

    End Sub

End Class