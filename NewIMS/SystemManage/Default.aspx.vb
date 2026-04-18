Imports DevExpress.Web

Partial Public Class SysDefault
    Inherits Page

    Private SelPolicyRep As String
    Private DataRet As New DataView
    Private Lo() As String
    Private DailyNo As String

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If myList Is Nothing Then
            FormsAuthentication.RedirectToLoginPage()
            'ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 6)
        End If
        'Lo = Me.Session("LogInfo")
        'If IsNothing(Lo) Then
        '    Me.ClientScript.RegisterStartupScript(Me.GetType, "Msg", "alert('·„ Ì „  ”ÃÌ· «·œŒÊ·');", True)
        '    Response.Write("<script> window.open('../SystemManage/LogOn.aspx?ReturnUrl=" & Me.Page.AppRelativeVirtualPath & "','_self'); </script>")
        '    'Response.Write("<script> window.open('http://" & IHost & DomainName & "/Ins/SystemManag/LogIn.aspx?ReturnUrl=" & Me.Page.AppRelativeVirtualPath & "','_self'); </script>")
        'Else
        '    Dim Tr As TreeView = Nothing
        '    Tr = Session("Tree")
        '    Call SetUserPerm(Tr, Lo, 6)
        '    Dim Lbl1 As Label = Master.Master.FindControl("BannerTitle")
        '    Lbl1.Text = Lo(0) & " at " & Format(Now, "HH:mm:ss") & " ›—⁄ " & GetBranchName(Lo(7))
        '    Dim MenuTable As Table = Master.Master.FindControl("Table1")
        '    MenuTable.BackImageUrl = "~/images/GlossyYellow.png"
        '    For i As Integer = 1 To 7
        '        Dim Hyp As HyperLink = Master.Master.FindControl("HyperLink" & CStr(i) & "")
        '        If Right(Session("Branch"), 2) <> "00" And i <> 1 And i <> 6 And i <> 7 Then
        '            Hyp.Visible = False
        '        Else
        '            Hyp.Visible = True
        '            Hyp.ForeColor = Drawing.Color.Black
        '        End If
        '    Next
        'End If
    End Sub

    Private Sub SetPolTp()
    End Sub

    Private Sub SetSqlSource(S As String)
    End Sub

    Protected Sub Page_Unload(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

End Class