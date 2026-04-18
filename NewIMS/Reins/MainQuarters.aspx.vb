Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Partial Public Class Reins_MainQuarters
    Inherits Page

    Private ReadOnly DataRet As New DataView

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If myList Is Nothing Then
            FormsAuthentication.SignOut()
            'FormsAuthentication.RedirectToLoginPage()
            'ASPxWebControl.RedirectOnCallback(TARGET_URL)
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 4)

            TryCast(FindControlRecursive(Form, "RightPane"), ASPxPanel).FixedPosition = PanelFixedPosition.WindowLeft

            TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar).RightToLeft = DevExpress.Utils.DefaultBoolean.False

        End If

        If Not IsPostBack Then
            If DatePart("q", Today.Date) = 1 Then
                Quarters.Text = "Q4"
                Years.Text = Year(Today.Date) - 1
            Else
                Quarters.Text = "Q" + CStr(DatePart("q", Today.Date) - 1)
                Years.Value = Year(Today.Date)
            End If
        End If
        If IsCallback Then
            Session("Year") = Years.Text()
            Session("Q") = Quarters.Value()
        Else
            Session("Year") = Years.Text
            Session("Q") = Quarters.Value()
        End If
        MainGrid.DataBind()
    End Sub

    Protected Sub DetailGrid_DataSelect(sender As Object, e As EventArgs)
        Session("Id") = TryCast(sender, ASPxGridView).GetMasterRowKeyValue()
    End Sub

    Protected Sub ChkSingleExpanded_CheckedChanged(sender As Object, e As EventArgs)
        MainGrid.SettingsDetail.AllowOnlyOneMasterRowExpanded = chkSingleExpanded.Checked
        If MainGrid.SettingsDetail.AllowOnlyOneMasterRowExpanded Then
            MainGrid.DetailRows.CollapseAllRows()
        End If
        MainGrid.DataBind()
    End Sub

    Protected Sub Years_SelectedIndexChanged(sender As Object, e As EventArgs)
        Session("Year") = Years.Text()
        MainGrid.DataBind()
    End Sub

    Protected Sub Quarters_SelectedIndexChanged(sender As Object, e As EventArgs)
        Session("Q") = Quarters.Value()
    End Sub

    Protected Sub KeyFieldLink_Init(sender As Object, e As EventArgs)
        Dim link As ASPxHyperLink = TryCast(sender, ASPxHyperLink)
        Dim container As GridViewDataItemTemplateContainer = TryCast(link.NamingContainer, GridViewDataItemTemplateContainer)
        link.Text = "Print Slip Of " & container.KeyValue
        link.Target = "_blank"
        'Dim p As ReportParameter() = New ReportParameter(1) {}

        'p.SetValue(New ReportParameter("year", Session("Year").ToString, True), 0)
        'p.SetValue(New ReportParameter("InsType", container.KeyValue.ToString, True), 1)

        Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("year", Session("Year").ToString, True),
                    New ReportParameter("InsType", container.KeyValue.ToString, True)
                     }
        Session.Add("Parms", P)

        Select Case container.KeyValue
            Case "Fire", "General Accident"
                link.NavigateUrl = "~/Reporting/Previewer.aspx?Report=" & ReportsPath & "SoaCC"
            Case Else
                link.NavigateUrl = "~/Reporting/Previewer.aspx?Report=" & ReportsPath & "SoaUW"
        End Select
        'link.NavigateUrl = "Default2.aspx?id=" & container.KeyValue
    End Sub

End Class