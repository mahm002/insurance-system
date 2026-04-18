Imports System.Drawing.Printing
Imports Microsoft.Reporting.WebForms

Public Class PreviewPDF
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'ReportViewer.ProcessingMode = ProcessingMode.Remote
        'ReportViewer.ServerReport.ReportServerUrl = New Uri(ConfigurationManager.AppSettings("ReportServerEndPoint"))
        'ReportViewer.ServerReport.ReportPath = Request("Report")
        '' ReportViewer.ServerReport.GetDefaultPageSettings.PaperSize() = Drawing.Printing.PaperKind.A5
        'Dim PS As PageSettings = New PageSettings()
        ''PS.PaperSize = New Drawing.Printing.PaperSize("custom", 827, 1169)
        'PS.Margins = New Margins(0, 0, 0, 0)
        'PS.Landscape = False

        'Dim PSA5 As PageSettings = New PageSettings()
        ''PSA5.PaperSize = New Drawing.Printing.PaperSize("custom", 584, 827)
        'PSA5.Margins = New Margins(0, 0, 0, 0)
        'PSA5.Landscape = False

        'ReportViewer.ServerReport.SetParameters(Session("Parms"))

        'Dim warnings As Warning() = Nothing
        'Dim streamids As String() = Nothing
        'Dim mimeType As String = Nothing
        'Dim encoding As String = Nothing
        'Dim extension As String = Nothing
        'Dim deviceInfoA5 As String = String.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth></DeviceInfo>", "8.3in", "5.8in")

        ''For i = 0 To pp.PrinterSettings.PaperSizes.Count - 1
        ''    If pp.PrinterSettings.PaperSizes.Item(i).Kind = System.Drawing.Printing.PaperKind.A5 Then
        ''        pp.PaperSize = pp.PrinterSettings.PaperSizes.Item(i)
        ''        pp.PrinterSettings.Copies = 3

        ''        Dim PaperSize As PaperSize = New PaperSize()
        ''        'ToDo: update with the PaperKind
        ''        'that your printer uses
        ''        pp.PaperSize.RawKind = 11 ' PaperKind.A5
        ''        pp.PrinterSettings.DefaultPageSettings.PaperSize = PaperSize
        ''        Exit For
        ''    End If
        ''Next

        'Dim deviceInfoA4 As String = String.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth></DeviceInfo>", "11.7in", "8.3in")
        ''Dim deviceInfoA4 As String = "<DeviceInfo>" &
        ''    "<OutputFormat>EMF</OutputFormat>" &
        ''    "<PageWidth>8.27in</PageWidth>" &
        ''    "<PageHeight>11.69in</PageHeight>" &
        ''    "<MarginTop>0.0in</MarginTop>" &
        ''    "<MarginLeft>0in</MarginLeft>" &
        ''    "<MarginRight>0in</MarginRight>" &
        ''    "<MarginBottom>0in</MarginBottom>" &
        ''    "</DeviceInfo>"
        ''If Request("Report") = "/IMSReports/McMotorPolicy" Or Request("Report") = "/IMSReports/McMotorPolicyM" Then

        ''    ReportViewer.ResetPageSettings()
        ''    ReportViewer.SetPageSettings(PSA5)
        ''    Dim pd As PrintDocument = New PrintDocument()
        ''    pd.DefaultPageSettings.PaperSize = New PaperSize("A5", 584, 827)

        ''    Dim bytes() As Byte = ReportViewer.ServerReport.Render("PDF", deviceInfoA5, mimeType, encoding, extension, streamids, warnings)
        ''    Response.Buffer = True
        ''    Response.Clear()
        ''    Response.ContentType = mimeType
        ''    '
        ''    '     This header is for saving it as an Attachment and popup window should display to to offer save as or open a PDF file
        ''    '     Response.AddHeader("Content-Disposition", "attachment; filename=" + extension)
        ''    '*/

        ''    'This Header Is use for open it in browser.
        ''    Response.AddHeader("content-disposition", "inline; filename=PolNo" + Now() + "." + extension)
        ''    Response.ContentType = "application/pdf"
        ''    Response.BinaryWrite(bytes)
        ''    Response.Flush()
        ''    Response.Close()

        ''Else
        'ReportViewer.ResetPageSettings()
        '    ReportViewer.SetPageSettings(PS)
        '    Dim pd As PrintDocument = New PrintDocument()
        '    pd.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
        '    Dim bytes() As Byte = ReportViewer.ServerReport.Render("PDF", deviceInfoA4, mimeType, encoding, extension, streamids, warnings)
        '    Response.Buffer = True
        '    Response.Clear()
        '    Response.ContentType = mimeType
        '    '
        '    '     This header is for saving it as an Attachment and popup window should display to to offer save as or open a PDF file
        '    '     Response.AddHeader("Content-Disposition", "attachment; filename=" + extension)
        '    '*/

        '    'This Header Is use for open it in browser.
        '    Response.AddHeader("content-disposition", "inline; filename=PolNo" + Now() + "." + extension)
        '    Response.ContentType = "application/pdf"
        '    Response.BinaryWrite(bytes)
        '    Response.Flush()
        '    Response.Close()
        'End If

    End Sub

    Private Sub PreviewPDF_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        ReportViewer.ProcessingMode = ProcessingMode.Remote
        ReportViewer.ServerReport.ReportServerUrl = New Uri(ConfigurationManager.AppSettings("ReportServerEndPoint"))
        ReportViewer.ServerReport.ReportPath = Request("Report")

        ' ReportViewer.ServerReport.GetDefaultPageSettings.PaperSize() = Drawing.Printing.PaperKind.A5
        'PS.PaperSize = New Drawing.Printing.PaperSize("custom", 827, 1169)
        Dim PS As New PageSettings With {
            .Margins = New Margins(0, 0, 0, 0),
            .Landscape = False
        }

        'PSA5.PaperSize = New Drawing.Printing.PaperSize("custom", 584, 827)
        Dim PSA5 As New PageSettings With {
            .Margins = New Margins(0, 0, 0, 0),
            .Landscape = False
        }

        ReportViewer.ServerReport.SetParameters(TryCast(Session("Parms"), IEnumerable(Of ReportParameter)))

        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim deviceInfoA5 As String = String.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth></DeviceInfo>", "8.3in", "5.8in")

        'For i = 0 To pp.PrinterSettings.PaperSizes.Count - 1
        '    If pp.PrinterSettings.PaperSizes.Item(i).Kind = System.Drawing.Printing.PaperKind.A5 Then
        '        pp.PaperSize = pp.PrinterSettings.PaperSizes.Item(i)
        '        pp.PrinterSettings.Copies = 3

        '        Dim PaperSize As PaperSize = New PaperSize()
        '        'ToDo: update with the PaperKind
        '        'that your printer uses
        '        pp.PaperSize.RawKind = 11 ' PaperKind.A5
        '        pp.PrinterSettings.DefaultPageSettings.PaperSize = PaperSize
        '        Exit For
        '    End If
        'Next

        Dim deviceInfoA4 As String = String.Format("<DeviceInfo><PageHeight>{0}</PageHeight><PageWidth>{1}</PageWidth></DeviceInfo>", "11.7in", "8.3in")
        'Dim deviceInfoA4 As String = "<DeviceInfo>" &
        '    "<OutputFormat>EMF</OutputFormat>" &
        '    "<PageWidth>8.27in</PageWidth>" &
        '    "<PageHeight>11.69in</PageHeight>" &
        '    "<MarginTop>0.0in</MarginTop>" &
        '    "<MarginLeft>0in</MarginLeft>" &
        '    "<MarginRight>0in</MarginRight>" &
        '    "<MarginBottom>0in</MarginBottom>" &
        '    "</DeviceInfo>"
        'If Request("Report") = "/IMSReports/McMotorPolicy" Or Request("Report") = "/IMSReports/McMotorPolicyM" Then

        '    ReportViewer.ResetPageSettings()
        '    ReportViewer.SetPageSettings(PSA5)
        '    Dim pd As PrintDocument = New PrintDocument()
        '    pd.DefaultPageSettings.PaperSize = New PaperSize("A5", 584, 827)

        '    Dim bytes() As Byte = ReportViewer.ServerReport.Render("PDF", deviceInfoA5, mimeType, encoding, extension, streamids, warnings)
        '    Response.Buffer = True
        '    Response.Clear()
        '    Response.ContentType = mimeType
        '    '
        '    '     This header is for saving it as an Attachment and popup window should display to to offer save as or open a PDF file
        '    '     Response.AddHeader("Content-Disposition", "attachment; filename=" + extension)
        '    '*/

        '    'This Header Is use for open it in browser.
        '    Response.AddHeader("content-disposition", "inline; filename=PolNo" + Now() + "." + extension)
        '    Response.ContentType = "application/pdf"
        '    Response.BinaryWrite(bytes)
        '    Response.Flush()
        '    Response.Close()

        'Else
        ReportViewer.ResetPageSettings()
        ReportViewer.SetPageSettings(PS)
        Dim pd As New PrintDocument()
        pd.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
        Dim bytes() As Byte = ReportViewer.ServerReport.Render("PDF", deviceInfoA4, mimeType, encoding, extension, streamids, warnings)
        Response.Buffer = True
        Response.Clear()
        Response.ContentType = mimeType
        '
        '     This header is for saving it as an Attachment and popup window should display to to offer save as or open a PDF file
        '     Response.AddHeader("Content-Disposition", "attachment; filename=" + extension)
        '*/

        'This Header Is use for open it in browser.
        Response.AddHeader("content-disposition", "inline; filename=PolNo" + Now() + "." + extension)
        Response.ContentType = "application/pdf"
        Response.BinaryWrite(bytes)
        Response.Flush()
        Response.Close()
        HttpContext.Current.Session.Remove("Parms")
        'Session.Add("Parms", Nothing)
    End Sub

End Class