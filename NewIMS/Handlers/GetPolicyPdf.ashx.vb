
Imports System
Imports System.Web
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Net.Http.Headers
Imports System.IO.Compression

Public Class GetPolicyPdf
    Implements IHttpHandler
    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            ' Get parameters
            Dim cardNumber As String = context.Request.QueryString("card")
            Dim isTest As Boolean = context.Request.QueryString("test") = "1"
            Dim forceDownload As Boolean = context.Request.QueryString("download") = "1"

            If String.IsNullOrEmpty(cardNumber) Then
                cardNumber = "LBY/373825"
                isTest = True
            End If

            ' Get PDF bytes
            Dim pdfBytes As Byte() = Nothing

            If isTest Then
                pdfBytes = CreateTestPDF(cardNumber)
            Else
                pdfBytes = GetPdfFromApiSync(cardNumber)
            End If

            If pdfBytes Is Nothing OrElse pdfBytes.Length = 0 Then
                Throw New Exception("No PDF received from API")
            End If

            ' --- GZIP COMPRESSION ---
            Dim acceptEncoding As String = context.Request.Headers("Accept-Encoding")
            If Not String.IsNullOrEmpty(acceptEncoding) AndAlso acceptEncoding.Contains("gzip") Then
                pdfBytes = CompressBytesGzip(pdfBytes)
                context.Response.AddHeader("Content-Encoding", "gzip")
            End If

            ' Optional: Validate PDF (only before compression, or after decompression)
            ' (Your existing PDF header check can be moved here, before compression)

            ' Set response headers
            context.Response.Clear()
            context.Response.Buffer = True
            context.Response.ContentType = "application/pdf"

            If forceDownload Then
                context.Response.AddHeader("Content-Disposition", $"attachment; filename=""Policy_{CleanFileName(cardNumber)}.pdf""")
            Else
                context.Response.AddHeader("Content-Disposition", "inline")
            End If

            context.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate")
            context.Response.AddHeader("Pragma", "no-cache")
            context.Response.AddHeader("Expires", "0")

            ' Write PDF
            context.Response.BinaryWrite(pdfBytes)
            context.Response.Flush()

        Catch ex As Exception
            context.Response.Clear()
            context.Response.StatusCode = 500
            context.Response.ContentType = "text/plain"

            ' Log the full error (replace with your logging framework)
            System.Diagnostics.EventLog.WriteEntry("Application", $"GetPolicyPdf Error | Card: {context.Request.QueryString("card")} | {ex.Message}{Environment.NewLine}{ex.StackTrace}",
        System.Diagnostics.EventLogEntryType.Error)

            ' Return a safe message to the client
            context.Response.Write($"Error generating PDF for card {context.Request.QueryString("card")}. Please try again or contact System&LIFO support.")
            context.Response.End()
        End Try
    End Sub

    ' Synchronous version to avoid deadlock
    Private Function GetPdfFromApiSync(cardNumber As String) As Byte()
        Try
            Dim ApiUrl As String = ConfigurationManager.AppSettings("ApiBaseUrl")
            Dim User As String = ConfigurationManager.AppSettings("ApiUsername")
            Dim Pass As String = ConfigurationManager.AppSettings("ApiPassword")

            Dim EndPoint As String = "api/insurance/orangecard/printcard"
            Dim Url As String = ApiUrl & EndPoint

            ' Use HttpClient with multipart/form-data (matches what the API expects)
            Using client As New HttpClient()
                'client.Timeout = TimeSpan.FromSeconds(20)

                Dim form As New MultipartFormDataContent()
                form.Add(New StringContent(User), "user_name")
                form.Add(New StringContent(Pass), "pass_word")
                form.Add(New StringContent(cardNumber), "card_number")

                ' Disable SSL validation for testing only — remove in production
                ' (HttpClient version — see note below)

                Dim response As HttpResponseMessage = client.PostAsync(Url, form).GetAwaiter().GetResult()

                If Not response.IsSuccessStatusCode Then
                    Dim errorBody As String = response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
                    Throw New Exception($"API returned {CInt(response.StatusCode)} {response.ReasonPhrase}: {errorBody}")
                End If

                Dim bytes As Byte() = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult()

                If bytes Is Nothing OrElse bytes.Length = 0 Then
                    Throw New Exception("API returned an empty response body")
                End If

                ' Validate it's actually a PDF
                If bytes.Length < 4 OrElse Encoding.ASCII.GetString(bytes, 0, 4) <> "%PDF" Then
                    Dim preview As String = Encoding.UTF8.GetString(bytes, 0, Math.Min(200, bytes.Length))
                    Throw New Exception($"API did not return a valid PDF. Response preview: {preview}")
                End If

                Return bytes
            End Using

        Catch ex As HttpRequestException
            Throw New Exception($"Network/connection error calling print API: {ex.Message}", ex)
        Catch ex As TaskCanceledException
            Throw New Exception("Request to print API timed out after 20 seconds", ex)
        Catch ex As Exception
            Throw New Exception($"Failed to get PDF from API: {ex.Message}", ex)

        End Try
    End Function

    Private Function CreateTestPDF(cardNumber As String) As Byte()
        ' Simple test PDF
        Return Encoding.ASCII.GetBytes("%PDF-1.4
%PDF Test Document
1 0 obj
<< /Type /Catalog /Pages 2 0 R >>
endobj
2 0 obj
<< /Type /Pages /Kids [3 0 R] /Count 1 >>
endobj
3 0 obj
<< /Type /Page /Parent 2 0 R /Resources << /Font << /F1 4 0 R >> >> /MediaBox [0 0 612 792] /Contents 5 0 R >>
endobj
4 0 obj
<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>
endobj
5 0 obj
<< /Length 100 >>
stream
BT /F1 24 Tf 100 700 Td (Test Policy: " & cardNumber & ") Tj ET
endstream
endobj
xref
0 6
0000000000 65535 f
0000000010 00000 n
0000000053 00000 n
0000000106 00000 n
0000000173 00000 n
0000000273 00000 n
trailer
<< /Size 6 /Root 1 0 R >>
startxref
373
%%EOF")
    End Function

    Private Function CleanFileName(fileName As String) As String
        If String.IsNullOrEmpty(fileName) Then Return "file"

        Dim invalidChars As Char() = Path.GetInvalidFileNameChars()
        For Each c As Char In invalidChars
            fileName = fileName.Replace(c, "_"c)
        Next

        Return fileName.Replace("/", "_").Replace("\", "_")
    End Function


    Private Function CompressBytesGzip(data As Byte()) As Byte()
        Using outputStream As New MemoryStream()
            Using gzipStream As New GZipStream(outputStream, CompressionMode.Compress)
                gzipStream.Write(data, 0, data.Length)
            End Using
            Return outputStream.ToArray()
        End Using
    End Function
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class