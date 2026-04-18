Imports DevExpress.Web

Public Class Treaties
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If myList Is Nothing Then
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 4)

            TryCast(FindControlRecursive(Form, "RightPane"), ASPxPanel).FixedPosition = PanelFixedPosition.WindowLeft

            TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar).RightToLeft = DevExpress.Utils.DefaultBoolean.False

        End If
        'Session("Year") = Year(Today.Date())
        If Not IsPostBack Then
            'If DatePart("q", Today.Date) = 1 Then
            'Quarters.Text = "Q4"
            Years.Text = Year(Today.Date)
            'Else
            'Quarters.Text = "Q" + CStr(DatePart("q", Today.Date) - 1)
            'Years.Value = Year(Today.Date)
            'End If
        End If
        If IsCallback Then
            Session("Year") = Years.Value
            'Session("Q") = Quarters.Value()
        Else
            Session("Year") = Years.Value
            'Session("Q") = Quarters.Value()
        End If
        MainGrid.DataBind()
    End Sub

    Protected Sub DetailGrid_DataSelect(sender As Object, e As EventArgs)
        Session("Id") = TryCast(sender, ASPxGridView).GetMasterRowKeyValue()
    End Sub

    Protected Sub Years_SelectedIndexChanged(sender As Object, e As EventArgs)
        Session("Year") = Years.Text()
        MainGrid.DataBind()
    End Sub

    Protected Sub KeyFieldLink_Init(sender As Object, e As EventArgs)
        Dim link As ASPxHyperLink = TryCast(sender, ASPxHyperLink)
        Dim container As GridViewDataItemTemplateContainer = TryCast(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.Text = "Print Slip Of " & container.KeyValue
        link.Target = "_blank"
        Select Case container.KeyValue
            Case "Fire", "General Accident"
                link.NavigateUrl = "~/Reporting/Previewer.aspx?Report=/IMSReports/SoaCC"
            Case Else
                link.NavigateUrl = "~/Reporting/Previewer.aspx?Report=/IMSReports/SoaUW"
        End Select
        'link.NavigateUrl = "Default2.aspx?id=" & container.KeyValue
    End Sub

End Class