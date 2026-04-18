Imports System
Imports System.Web
Imports System.Web.UI

Partial Class PrintPolicy
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim policyNumber As String = Request.QueryString("policy")
            Dim testMode As Boolean = Request.QueryString("test") = "1"

            If String.IsNullOrEmpty(policyNumber) Then
                Response.Redirect("CreateORPolicy.aspx?error=nopolicy")
                Return
            End If

            ' Build the direct PDF URL
            Dim pdfUrl As String = BuildPdfUrl(policyNumber, testMode)

            ' Set the PDF URL in a hidden field for JavaScript to use
            pdfUrlHidden.Value = pdfUrl
            policyNumberHidden.Value = policyNumber
        End If
    End Sub

    Private Function BuildPdfUrl(policyNumber As String, testMode As Boolean) As String
        Dim baseUrl As String = Request.Url.GetLeftPart(UriPartial.Authority)
        Dim appPath As String = Request.ApplicationPath
        If Not appPath.EndsWith("/") Then appPath &= "/"

        ' Try to find the handler - adjust path as needed
        Dim handlerPath As String = "Handlers/GetPolicyPdf.ashx"

        Dim pdfUrl As String = baseUrl & appPath & handlerPath & "?card=" & Server.UrlEncode(policyNumber) & "&download=0"

        If testMode Then
            pdfUrl &= "&test=1"
        End If

        ' Add timestamp to prevent caching
        pdfUrl &= "&_=" & DateTime.Now.Ticks.ToString()

        Return pdfUrl.Replace("//", "/").Replace("http:/", "http://").Replace("https:/", "https://")
    End Function
End Class