Imports System.Web.Script.Serialization

Public Class PaymentHandler : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        context.Response.ContentEncoding = System.Text.Encoding.UTF8

        Try
            Dim data = GetRequestData(context)
            Dim action = GetAction(data)
            Dim result = ProcessAction(action, data, context)

            context.Response.Write(New JavaScriptSerializer().Serialize(result))
        Catch ex As Exception
            HandleError(context, ex)
        End Try
    End Sub

    Private Function GetRequestData(context As HttpContext) As Dictionary(Of String, Object)
        Dim jsonSerializer As New JavaScriptSerializer()
        jsonSerializer.MaxJsonLength = Integer.MaxValue

        Using reader = New System.IO.StreamReader(context.Request.InputStream)
            Dim jsonString = reader.ReadToEnd()
            If String.IsNullOrEmpty(jsonString) Then
                Return New Dictionary(Of String, Object)()
            End If
            Return jsonSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonString)
        End Using
    End Function

    Private Function GetAction(data As Dictionary(Of String, Object)) As String
        Return If(data.ContainsKey("action"), data("action").ToString().ToLower(), "")
    End Function

    Private Function ProcessAction(action As String, data As Dictionary(Of String, Object), context As HttpContext) As Object
        Select Case action
            Case "processmultipayment"
                Return ProcessMultiPayment(data)
            Case Else
                context.Response.StatusCode = 400
                Return New With {.success = False, .message = "إجراء غير معروف"}
        End Select
    End Function

    Private Function ProcessMultiPayment(data As Dictionary(Of String, Object)) As Object
        Try
            If Not data.ContainsKey("data") Then
                Return New With {.success = False, .message = "بيانات الدفع غير موجودة"}
            End If

            Dim receiptData As Dictionary(Of String, Object) = CType(data("data"), Dictionary(Of String, Object))

            ' الحصول على بيانات الاستلام
            Dim customerName As String = If(receiptData.ContainsKey("customerName"), receiptData("customerName").ToString(), "")
            Dim moveDateStr As String = If(receiptData.ContainsKey("moveDate"), receiptData("moveDate").ToString(), DateTime.Now.ToString("yyyy-MM-dd"))
            Dim notes As String = If(receiptData.ContainsKey("notes"), receiptData("notes").ToString(), "/")
            Dim totalDue As Decimal = If(receiptData.ContainsKey("totalDue"), Convert.ToDecimal(receiptData("totalDue")), 0D)
            Dim branchCode As String = If(receiptData.ContainsKey("branchCode"), receiptData("branchCode").ToString(), "001")
            Dim currentUser As String = If(receiptData.ContainsKey("currentUser"), receiptData("currentUser").ToString(), "system")
            Dim policyNumber As String = If(receiptData.ContainsKey("policyNumber"), receiptData("policyNumber").ToString(), "")
            Dim endNo As String = If(receiptData.ContainsKey("endNo"), receiptData("endNo").ToString(), "")
            Dim loadNo As String = If(receiptData.ContainsKey("loadNo"), receiptData("loadNo").ToString(), "")

            Dim moveDate As DateTime
            If Not DateTime.TryParse(moveDateStr, moveDate) Then
                moveDate = DateTime.Now
            End If

            ' معالجة المدفوعات
            Dim payments As New List(Of Dictionary(Of String, Object))()
            If receiptData.ContainsKey("payments") Then
                Dim paymentsArray As ArrayList = CType(receiptData("payments"), ArrayList)
                For Each paymentObj As Object In paymentsArray
                    Dim paymentDict As Dictionary(Of String, Object) = CType(paymentObj, Dictionary(Of String, Object))
                    payments.Add(paymentDict)
                Next
            End If

            ' حساب إجمالي المدفوعات
            Dim totalPaid As Decimal = 0D
            For Each payment In payments
                If payment.ContainsKey("amount") Then
                    totalPaid += Convert.ToDecimal(payment("amount"))
                End If
            Next

            ' التحقق من التطابق بين المبلغ المستحق والمدفوع
            Dim tolerance As Decimal = 0.001D
            If Math.Abs(totalPaid - totalDue) > tolerance Then
                Return New With {
                    .success = False,
                    .message = $"إجمالي المدفوعات ({totalPaid:N3}) لا يساوي المبلغ المستحق ({totalDue:N3})"
                }
            End If

            ' بدء عملية السداد (استدعاء الإجراءات المخزنة)
            Dim receiptNumber As String = GenerateReceiptNumber(branchCode)
            Dim dailyNumber As String = GenerateDailyNumber(branchCode)

            ' تسجيل العملية في السجل
            LogSystemActivity("MultiPayment", $"تم السداد المتعدد برقم {receiptNumber} للمستخدم {currentUser}")

            Return New With {
                .success = True,
                .message = "تمت عملية السداد المتعدد بنجاح",
                .receiptNumber = receiptNumber,
                .dailyNumber = dailyNumber
            }
        Catch ex As Exception
            LogError("ProcessMultiPayment", ex)
            Return New With {
                .success = False,
                .message = "خطأ في معالجة الدفع: " & ex.Message
            }
        End Try
    End Function

    Private Function GenerateReceiptNumber(branchCode As String) As String
        ' محاكاة توليد رقم إيصال - في الإنتاج سيستدعي إجراءا مخزنا
        Dim random As New Random()
        Dim randomSuffix As String = random.Next(10000, 99999).ToString()
        Return $"R{DateTime.Now:yyyyMMdd}-{branchCode}-{randomSuffix}"
    End Function

    Private Function GenerateDailyNumber(branchCode As String) As String
        ' محاكاة توليد رقم يومية - في الإنتاج سيستدعي إجراءا مخزنا
        Dim random As New Random()
        Dim randomSuffix As String = random.Next(100, 999).ToString()
        Return $"D{DateTime.Now:yyMMdd}-{branchCode}-{randomSuffix}"
    End Function

    Private Sub LogSystemActivity(activityType As String, description As String)
        Try
            Dim logPath As String = HttpContext.Current.Server.MapPath("~/Logs/SystemActivity.log")
            Dim logDir As String = System.IO.Path.GetDirectoryName(logPath)

            If Not System.IO.Directory.Exists(logDir) Then
                System.IO.Directory.CreateDirectory(logDir)
            End If

            Using sw As New System.IO.StreamWriter(logPath, True)
                sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {activityType} - {description}")
            End Using
        Catch
            ' تجاهل أخطاء التسجيل
        End Try
    End Sub

    Private Sub HandleError(context As HttpContext, ex As Exception)
        context.Response.StatusCode = 500
        context.Response.Write(New JavaScriptSerializer().Serialize(New With {
            .success = False,
            .message = "حدث خطأ في الخادم: " & ex.Message
        }))
    End Sub

    Private Sub LogError(methodName As String, ex As Exception)
        Try
            Dim logPath As String = HttpContext.Current.Server.MapPath("~/Logs/PaymentHandler.log")
            Dim logDir As String = System.IO.Path.GetDirectoryName(logPath)

            If Not System.IO.Directory.Exists(logDir) Then
                System.IO.Directory.CreateDirectory(logDir)
            End If

            Using sw As New System.IO.StreamWriter(logPath, True)
                sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {methodName}")
                sw.WriteLine($"Message: {ex.Message}")
                sw.WriteLine($"StackTrace: {ex.StackTrace}")
                sw.WriteLine("--------------------------------------------------")
            End Using
        Catch
            ' تجاهل أخطاء التسجيل
        End Try
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class