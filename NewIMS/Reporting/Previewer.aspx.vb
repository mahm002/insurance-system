Imports Microsoft.Reporting.WebForms

Public Class Previewer
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub Previewer_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not IsPostBack Then
            ReportViewer.ProcessingMode = ProcessingMode.Remote
            ReportViewer.ServerReport.ReportServerUrl = New Uri(ConfigurationManager.AppSettings("ReportServerEndPoint"))
            ReportViewer.ServerReport.ReportPath = Request("Report")
            If IsNothing(Session("Parms")) Then
                HttpContext.Current.Session.Remove("Parms")
            Else
                ReportViewer.ServerReport.SetParameters(DirectCast(Session("Parms"), List(Of ReportParameter)))
            End If
        End If
        HttpContext.Current.Session.Remove("Parms")
    End Sub

End Class