Imports System.Data.SqlClient
Imports System.Net
Imports System.IO

Public Class ValidateDoc
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim pno As String = Request("Pno")
        If String.IsNullOrEmpty(pno) Then
            Response.StatusCode = 400
            Response.Write("Missing Pno parameter")
            Response.End()
            Return
        End If

        ' التحقق من وجود معامل pdf لعرض الملف مباشرة
        If Request("pdf") = "1" Then
            GeneratePDF(pno)
        Else
            DisplayHTML(pno)
        End If
    End Sub

    ''' <summary>
    ''' عرض صفحة HTML التي تحتوي على بيانات الوثيقة
    ''' </summary>
    Private Sub DisplayHTML(pno As String)
        Dim orderNo As String = ""
        Dim insuredName As String = ""
        Dim policyStatus As String = ""
        Dim expiryDate As String = ""
        Dim SystemName As String = ""

        Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

        Using con As New SqlConnection(connectionString)
            con.Open()
            Dim sql As String = ""
            If pno.Length <= 24 Then
                sql = "SELECT PolNo OrderNo, CustName As InsuredName,[dbo].[SysName](SubIns) As SystemName, iif(CoverTo>=GetDate(),'الوثيقة سارية', 'الوثيقة منتهية') As PolicyStatus, CoverTo ExpiryDate FROM PolicyFile Left Join CustomerFile On PolicyFile.CustNo=CustomerFile.CustNo WHERE OrderNo = @Pno"
            Else
                sql = "SELECT PolNo OrderNo, CustName As InsuredName, [dbo].[SysName](SubIns) As SystemName, iif(CoverTo>=GetDate(),'الوثيقة سارية', 'الوثيقة منتهية') As PolicyStatus, CoverTo ExpiryDate FROM PolicyFile Left Join CustomerFile On PolicyFile.CustNo=CustomerFile.CustNo WHERE UniqueID = @Pno"
            End If

            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@Pno", pno)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        orderNo = reader("OrderNo").ToString()
                        insuredName = reader("InsuredName").ToString()
                        policyStatus = reader("PolicyStatus").ToString().Trim()
                        SystemName = reader("SystemName").ToString().Trim()
                        If reader("ExpiryDate") IsNot DBNull.Value Then
                            expiryDate = CDate(reader("ExpiryDate")).ToString("dd/MM/yyyy")
                        End If
                    Else
                        Response.StatusCode = 404
                        Response.Write("لم يتم العثور على الوثيقة")
                        Response.End()
                        Return
                    End If
                End Using
            End Using
        End Using

        ' إنشاء رابط PDF
        Dim pdfUrl As String = $"{Request.Url.AbsolutePath}?Pno={pno}&pdf=1"

        ' عرض البيانات في قالب HTML
        Response.Clear()
        Response.ContentType = "text/html"
        Response.Write(GenerateHtml(orderNo, insuredName, policyStatus, expiryDate, pdfUrl, SystemName))
        Response.End()
    End Sub

    ''' <summary>
    ''' إنشاء محتوى HTML مع بيانات الوثيقة ورابط PDF
    ''' </summary>
    Private Function GenerateHtml(orderNo As String, insuredName As String, policyStatus As String, expiryDate As String, pdfUrl As String, SysName As String) As String
        Dim html As New StringBuilder()

        html.AppendLine("<!DOCTYPE html>")
        html.AppendLine("<html>")
        html.AppendLine("<head>")
        html.AppendLine("<meta charset='utf-8'>")
        html.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1'>")
        html.AppendLine("<title>معلومات الوثيقة</title>")
        html.AppendLine("<style>")
        html.AppendLine("body { font-family: 'Sakkal Majalla', 'Traditional Arabic', Tahoma, Arial, sans-serif; background-color: #f5f5f5; margin: 0; padding: 20px; direction: rtl; font-size: 1.1rem; }")
        html.AppendLine(".container { max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); overflow: hidden; }")
        html.AppendLine(".header { background-color: #6c757d; color: white; padding: 20px; text-align: center; }")
        html.AppendLine(".header h1 { margin: 0; font-size: 24px; }")
        html.AppendLine(".header img { max-width: 200px; margin-bottom: 10px; filter: drop-shadow(0 0 8px white); }")
        html.AppendLine(".content { padding: 30px; }")
        ' New row styling for three-column layout
        html.AppendLine(".info-row { display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px; border-bottom: 1px solid #eee; padding-bottom: 10px; }")
        html.AppendLine(".label-ar { font-weight: bold; color: #555; text-align: right; width: 30%; }")
        html.AppendLine(".label-en { font-weight: bold; color: #555; text-align: left; width: 30%; }")
        html.AppendLine(".info-value { color: #333; text-align: center; width: 40%; }")  ' adjust as needed
        html.AppendLine(".status-active { color: green; font-weight: bold; }")
        html.AppendLine(".status-expired { color: red; font-weight: bold; }")
        html.AppendLine(".pdf-link { margin-top: 25px; text-align: center; }")
        html.AppendLine(".pdf-link a { display: inline-block; background-color: #3498db; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold; transition: background-color 0.3s; }")
        html.AppendLine(".pdf-link a:hover { background-color: #2980b9; }")
        html.AppendLine(".footer { background-color: #ecf0f1; padding: 15px; text-align: center; font-size: 12px; color: #7f8c8d; }")
        html.AppendLine("</style>")
        html.AppendLine("</head>")
        html.AppendLine("<body>")
        html.AppendLine("<div class='container'>")
        html.AppendLine("<div class='header'>")
        html.AppendLine("<img src='" & ResolveUrl("~/Bootstrap/login/css/login-box-backg.png") & "' alt='Company Logo' />")
        html.AppendLine("<h1>تفاصيل الوثيقة</h1>")
        html.AppendLine("</div>")
        html.AppendLine("<div class='content'>")

        ' Policy Number
        html.AppendLine("<div class='info-row'>")
        html.AppendLine("<span class='label-ar'>رقم الوثيقة:</span>")
        html.AppendLine("<span class='info-value'>" & SysName.TrimEnd & " / " & orderNo & "</span>")
        html.AppendLine("<span class='label-en'>:Policy No.</span>")
        html.AppendLine("</div>")

        ' Insured Name
        html.AppendLine("<div class='info-row'>")
        html.AppendLine("<span class='label-ar'>اسم المؤمن له:</span>")
        html.AppendLine("<span class='info-value'>" & insuredName & "</span>")
        html.AppendLine("<span class='label-en'>:Insured Name</span>")
        html.AppendLine("</div>")

        ' Policy Status (with dynamic coloring)
        Dim statusClass As String = ""
        If policyStatus.Contains("سارية") Then
            statusClass = "status-active"
        ElseIf policyStatus.Contains("منتهية") Then
            statusClass = "status-expired"
        End If

        If IsStoped(orderNo, 0, 0) Then
            policyStatus = "الوثيقة موقوفة"
            statusClass = "status-expired"
        End If

        html.AppendLine("<div class='info-row'>")
        html.AppendLine("<span class='label-ar'>حالة الوثيقة:</span>")
        html.AppendLine("<span class='info-value " & statusClass & "'>" & policyStatus & "</span>")
        html.AppendLine("<span class='label-en'>:Policy Status</span>")
        html.AppendLine("</div>")

        ' Expiry Date
        html.AppendLine("<div class='info-row'>")
        html.AppendLine("<span class='label-ar'>تاريخ الانتهاء:</span>")
        html.AppendLine("<span class='info-value'>" & expiryDate & "</span>")
        html.AppendLine("<span class='label-en'>:Expiry Date</span>")
        html.AppendLine("</div>")

        ' PDF Link (unchanged)
        html.AppendLine("<div class='pdf-link'>")
        html.AppendLine("<a href='" & pdfUrl & "' target='_blank'>📄 عرض الوثيقة (PDF)</a>")
        html.AppendLine("</div>")

        html.AppendLine("</div>") ' content
        html.AppendLine("<div class='footer'>")
        html.AppendLine("تم إنشاء هذا المستند إلكترونياً ويُعتبر صحيحاً دون توقيع")
        html.AppendLine("</div>")
        html.AppendLine("</div>") ' container
        html.AppendLine("</body>")
        html.AppendLine("</html>")

        Return html.ToString()
    End Function



    ''' <summary>
    ''' جلب وعرض ملف PDF من خادم التقارير
    ''' </summary>
    Private Sub GeneratePDF(pno As String)
        ' نحتاج إلى معرفة SubIns لتحديد مسار التقرير
        Dim orderNo As String = ""
        Dim subIns As String = ""
        Dim endNo As String = ""

        Me.Page.Title = "📄 عرض الوثيقة (PDF)"

        Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

        Using con As New SqlConnection(connectionString)
            con.Open()
            Dim sql As String = ""
            If pno.Length <= 24 Then
                sql = "SELECT OrderNo, SubIns, EndNo, LoadNo FROM PolicyFile WHERE OrderNo = @Pno"
            Else
                sql = "SELECT OrderNo, SubIns, EndNo, LoadNo FROM PolicyFile WHERE UniqueID = @Pno"
            End If

            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@Pno", pno)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        orderNo = reader("OrderNo").ToString()
                        subIns = reader("SubIns").ToString()
                        endNo = reader("EndNo").ToString()
                        Dim loadNo As String = reader("LoadNo").ToString()
                    Else
                        Response.StatusCode = 404
                        Response.Write("لم يتم العثور على الوثيقة")
                        Response.End()
                        Return
                    End If
                End Using
            End Using
        End Using

        ' تحديد مسار التقرير (استخدم دالة PolRep كما في السابق)
        Dim reportPath As String = "/IMSReports/" & PolRep(subIns)

        ' بناء رابط SSRS
        Dim reportServerUrl As String = ConfigurationManager.AppSettings("ReportServerEndPoint").TrimEnd("/"c)
        Dim url As String = $"{reportServerUrl}?{reportPath}&rs:Format=PDF&PolicyNo={orderNo}&EndNo={endNo}&Sys={subIns}&rs:Command=Render"
        url = url.Replace(" ", "%20")

        ' جلب PDF باستخدام هوية التطبيق
        Dim pdfBytes() As Byte = Nothing
        Try
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim request As HttpWebRequest = WebRequest.Create(url)
            request.UseDefaultCredentials = True
            request.PreAuthenticate = True
            request.Method = "GET"

            Using response As HttpWebResponse = request.GetResponse()
                Using memoryStream As New MemoryStream()
                    response.GetResponseStream().CopyTo(memoryStream)
                    pdfBytes = memoryStream.ToArray()
                End Using
            End Using
        Catch ex As WebException
            ' تسجيل الخطأ
            Dim logPath As String = Server.MapPath("~/App_Data/ErrorLog.txt")
            Dim logDir As String = Path.GetDirectoryName(logPath)
            If Not Directory.Exists(logDir) Then
                Directory.CreateDirectory(logDir)
            End If
            File.AppendAllText(logPath, DateTime.Now.ToString() & ": PDF Error - " & ex.ToString() & vbCrLf)

            Response.StatusCode = 500
            Response.Write("حدث خطأ في تحميل ملف PDF. يرجى المحاولة لاحقاً.")
            Response.End()
            Return
        End Try

        ' إرسال PDF إلى المتصفح
        Response.Buffer = True
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "inline; filename=" & Guid.NewGuid().ToString() & ".pdf")
        Response.BinaryWrite(pdfBytes)
        Response.End()
    End Sub


End Class