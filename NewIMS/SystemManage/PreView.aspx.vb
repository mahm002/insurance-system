Imports Microsoft.Reporting.WebForms

Partial Public Class OutPutManagement_Preview
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            rptViewer.ServerReport.ReportServerUrl = New Uri(ConfigurationManager.AppSettings("ReportServerEndPoint"))
            rptViewer.ServerReport.ReportPath = "/IMSReports/CallDetail"
            Parm = Array.CreateInstance(GetType(ReportParameter), 1)
            SetRepPm("CurDate", True, GenArray(Format(Today.Date, "yyyy/MM/dd")), Parm, 0)
            rptViewer.ServerReport.SetParameters(Parm)

        End If
    End Sub

End Class