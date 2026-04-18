Imports DevExpress.Web

Public Class RootMaster
    Inherits MasterPage

    Private Sub RootMaster_Init(sender As Object, e As EventArgs) Handles Me.Init
        If HttpContext.Current.User.Identity.IsAuthenticated AndAlso HttpContext.Current.User IsNot Nothing Then
            Dim myList = CType(Session("UserInfo"), List(Of String))
            If myList IsNot Nothing Then
                UpdateUserMenuItemsVisible()
                UpdateUserInfo()
            Else
                FormsAuthentication.SignOut()
                RedirectToLoginPage()
            End If
        Else
            RedirectToLoginPage()
        End If
    End Sub

    Private Sub RedirectToLoginPage()
        If Page.IsCallback Then
            Dim loginUrl As String = FormsAuthentication.LoginUrl
            loginUrl = VirtualPathUtility.ToAbsolute(loginUrl)
            ASPxWebControl.RedirectOnCallback(loginUrl)
        Else
            FormsAuthentication.RedirectToLoginPage()
        End If
    End Sub

    Protected Sub UpdateUserMenuItemsVisible()
        Dim isAuthenticated As Boolean = HttpContext.Current.User.Identity.IsAuthenticated
        RightAreaMenu.Items.FindByName("SignInItem").Visible = Not isAuthenticated
        RightAreaMenu.Items.FindByName("RegisterItem").Visible = Not isAuthenticated
        RightAreaMenu.Items.FindByName("MyAccountItem").Visible = isAuthenticated
        RightAreaMenu.Items.FindByName("SignOutItem").Visible = isAuthenticated
    End Sub

    Protected Sub UpdateUserInfo()
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim myList = CType(Session("UserInfo"), List(Of String))
            If myList IsNot Nothing Then
                Dim myAccountItem = RightAreaMenu.Items.FindByName("MyAccountItem")
                Dim userName = CType(myAccountItem.FindControl("UserNameLabel"), ASPxLabel)
                Dim email = CType(myAccountItem.FindControl("EmailLabel"), ASPxLabel)
                userName.Text = myList.Item(8)
                email.Text = GetBranchName(Session("Branch"))
                Session("userId") = myList.Item(9)
            End If
        End If
    End Sub

    Private Sub RootMaster_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserInfo") IsNot Nothing Then
            Dim myList = CType(Session("UserInfo"), List(Of String))
            Session("userId") = myList.Item(9)
        End If
        ' No need for UidHFV anymore
    End Sub

    'Private Function GetBranchName(branchCode As Object) As String
    '    ' Implement your branch name logic here
    '    Return If(branchCode?.ToString(), "Unknown")
    'End Function

    Private Sub HeaderMenu_DataBound(sender As Object, e As EventArgs) Handles HeaderMenu.DataBound
        Dim myList = TryCast(Session("UserInfo"), List(Of String))
        If myList Is Nothing Then
            FormsAuthentication.SignOut()
            FormsAuthentication.RedirectToLoginPage()
        Else
            Dim control As ASPxMenu = CType(sender, ASPxMenu)
            For i As Integer = 1 To Math.Min(6, control.Items.Count)
                Dim itemToHide = control.Items(i - 1)
                itemToHide.Visible = Not String.IsNullOrEmpty(Trim(myList.Item(i)))
            Next
        End If
    End Sub

    Protected Sub RightAreaMenu_ItemClick(source As Object, e As MenuItemEventArgs)
        If e.Item.Name = "SignOutItem" Then
            FormsAuthentication.SignOut()
            'FormsAuthentication.RedirectToLoginPage("~/SystemManage/LogIn.aspx")
            FormsAuthentication.RedirectToLoginPage(AppRelativeVirtualPath)
        End If
    End Sub

    Protected Sub ASPxCallback1_Callback(source As Object, e As CallbackEventArgs)
        Dim reqsplt = e.Parameter.Split("|"c)
        Select Case reqsplt(1)
            Case "1"
                '1 ÿ·»«  «·≈·€«¡
                ASPxCallback1.JSProperties("cpMyAttribute") = "Display"
                ASPxCallback1.JSProperties("cpResult") = "ÿ·»«  «·≈·€«¡"
                ASPxCallback1.JSProperties("cpNewWindowUrl") = $"../Policy/ListOfAllRequests.aspx?UserId={Session("userId")}&Type={reqsplt(1)}"
            Case "2"
                '2 ÿ·»«   √„Ì‰
                ASPxCallback1.JSProperties("cpMyAttribute") = "REQUESTS"
                ASPxCallback1.JSProperties("cpResult") = "ÿ·»«  «· √„Ì‰"
                ASPxCallback1.JSProperties("cpNewWindowUrl") = "../Policy/InsRequests.aspx"
            Case "3"
                '3 System ≈‘⁄«—« 
                ASPxCallback1.JSProperties("cpMyAttribute") = "System Notifications"
                ASPxCallback1.JSProperties("cpResult") = "≈‘⁄«—«  «·‰Ÿ«„"
                ASPxCallback1.JSProperties("cpNewWindowUrl") = $"../Policy/UserPaymentsFollowUp.aspx?UserId={Session("userId")}"
            Case "ResetPass"
                ASPxCallback1.JSProperties("cpMyAttribute") = "ResetPass"
                ASPxCallback1.JSProperties("cpResult") = "≈⁄«œ…  ⁄ÌÌ‰ ﬂ·„… «·„—Ê—"
                ASPxCallback1.JSProperties("cpNewWindowUrl") = $"../SystemManage/ResetPassword.aspx?log={Session("userId")}&Br={Session("Branch")}"
        End Select
    End Sub

End Class