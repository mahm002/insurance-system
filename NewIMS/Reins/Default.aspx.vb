Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class Default1
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If myList Is Nothing Then
            'ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
            FormsAuthentication.SignOut()
            'FormsAuthentication.RedirectToLoginPage()
            'ASPxWebControl.RedirectOnCallback(TARGET_URL)
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 4)

            TryCast(FindControlRecursive(Form, "RightPane"), ASPxPanel).FixedPosition = PanelFixedPosition.WindowLeft

            TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar).RightToLeft = DevExpress.Utils.DefaultBoolean.False
            cmbReports.DataBind()
        End If
    End Sub

    Private Sub CmbReports_Callback(sender As Object, e As CallbackEventArgsBase) Handles cmbReports.Callback
        Dim cmbsplited = e.Parameter.Split("|")
        Select Case cmbsplited(0)

            Case Else
                If IsBranch(Session("Branch")) Then
                    'Dim br As Boolean = IIf(Not IsHeadQuarter(Session("Branch")), False, True)

                    'Dim p As ReportParameter() = New ReportParameter(1) {}

                    'p.SetValue(New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True), 0)
                    'p.SetValue(New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True), 1)

                    Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                    New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True)
                }
                    Session.Add("Parms", P)
                Else
                    'Dim p As ReportParameter() = New ReportParameter(1) {}

                    'p.SetValue(New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True), 0)
                    'p.SetValue(New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True), 1)

                    Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                    New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True)
                }
                    'p.SetValue(New ReportParameter("BranchNo", Branch.Text, False), 2)
                    'p.SetValue(New ReportParameter("Sys", Sys.Text, False), 3)
                    'p.SetValue(New ReportParameter("Agents", Branch.Text, True), 4)
                    'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                    Session.Add("Parms", P)

                End If
                'And (IsBranch(Session("Branch")) Or IsBranchOffice(Session("Branch")))
                If e.Parameter <> "" Then
                    cmbReports.JSProperties("cpResult") = cmbsplited(1)
                    cmbReports.JSProperties("cpMyAttribute") = ""
                    cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                Else

                End If
        End Select
    End Sub

End Class