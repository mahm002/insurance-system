Imports System.Data.SqlClient
Imports Newtonsoft.Json

Public Class MultiPaymentReceipt
    Inherits Page

    Private ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

    ' Add this property to store PolicyData in ViewState
    Private Property PolicyDataStore As Dictionary(Of String, Object)
        Get
            Return If(ViewState("PolicyDataStore"), New Dictionary(Of String, Object))
        End Get
        Set(value As Dictionary(Of String, Object))
            ViewState("PolicyDataStore") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            ' التحقق من الصلاحيات أولاً
            If Session("User") Is Nothing Then
                FormsAuthentication.RedirectToLoginPage()
                Return
            End If

            ' Only run initialization logic on first page load
            If Not IsPostBack Then
                ' تعيين القيم الأساسية
                hdnCurrentDate.Value = DateTime.Now.ToString("yyyy/MM/dd")
                hdnCurrentUser.Value = If(Session("User"), "").ToString()
                lblBranch.Text = If(Session("Branch"), "001").ToString()
                hdnBranchCode.Value = lblBranch.Text

                ' Load document data on first load only
                LoadDocumentData()
            End If

            ' معالجة حالة الإرسال النهائي
            ' Check ViewState to prevent double execution
            Dim alreadyProcessed As Boolean = If(ViewState("ProcessMultiPaymentExecuted"), False)

            If Not alreadyProcessed AndAlso hdnIsFinalSubmit.Value = "1" Then
                ' Mark as executed
                ViewState("ProcessMultiPaymentExecuted") = True
                ProcessMultiPayment()
                hdnIsFinalSubmit.Value = "0"
            End If
        Catch ex As Exception
            LogError(ex)
            ShowErrorMessage("حدث خطأ أثناء تحميل الصفحة: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadDocumentData()
        Try
            Dim polNo As String = Request("PolNo")
            Dim endNo As String = Request("EndNo")
            Dim loadNo As String = Request("LoadNo")

            ' ===== DEBUG: Log what we're loading =====
            Debug.WriteLine($"LoadDocumentData - PolNo: {polNo}, EndNo: {endNo}, LoadNo: {loadNo}")

            ' ===== ALWAYS CLEAR EVERYTHING FIRST =====
            ClearAllPolicyData()

            If String.IsNullOrEmpty(polNo) OrElse String.IsNullOrEmpty(endNo) OrElse String.IsNullOrEmpty(loadNo) Then
                ' Check session as alternative
                If Session("CurrentPolNo") IsNot Nothing Then
                    polNo = Session("CurrentPolNo").ToString()
                    endNo = Session("CurrentEndNo").ToString()
                    loadNo = Session("CurrentLoadNo").ToString()
                Else
                    ShowErrorMessage("لم يتم تحديد وثيقة للسداد. الرجاء اختيار وثيقة من القائمة.")
                    Return
                End If
            End If

            ' ===== CRITICAL: Save in session AFTER clearing =====
            Session("CurrentPolNo") = polNo
            Session("CurrentEndNo") = endNo
            Session("CurrentLoadNo") = loadNo

            ' Determine receipt type
            If polNo = "أخـرى" Then
                Debug.WriteLine("Loading Other Receipts")
                HandleOtherReceipts()
            Else
                Debug.WriteLine($"Loading Policy: {polNo}/{endNo}/{loadNo}")
                ' Ensure flag is set to 0
                hdnIsOtherReceipt.Value = "0"
                LoadPolicyData(polNo, endNo, loadNo)
            End If
        Catch ex As Exception
            LogError(ex)
            ShowErrorMessage("حدث خطأ أثناء تحميل بيانات الوثيقة: " & ex.Message)
        End Try
    End Sub

    Private Sub ClearAllPolicyData()
        Try
            ' ===== CLEAR VIEWSTATE =====
            Me.PolicyDataStore = New Dictionary(Of String, Object)
            ViewState.Remove("PolicyDataStore")
            ViewState.Remove("ProcessMultiPaymentExecuted")
            ViewState.Remove("OtherReceipt_Customer")

            ' ===== CLEAR SESSION - EVERYTHING RELATED =====
            Session.Remove("PolicyData")
            Session.Remove("CurrentPolNo")
            Session.Remove("CurrentEndNo")
            Session.Remove("CurrentLoadNo")
            Session.Remove("PolicyData_" & Session.SessionID)
            Session.Remove("LastCustomer")

            ' ===== CLEAR APPLICATION STATE =====
            Dim appKey As String = $"PolicyData_{Session.SessionID}"
            Application.Lock()
            Application.Remove(appKey)
            Application.UnLock()

            ' ===== CLEAR ALL HIDDEN FIELDS =====
            hdnPolicyData.Value = ""
            hdnGridData.Value = ""
            hdnTotalDue.Value = "0"
            hdnPayments.Value = ""
            hdnPaymentEntries.Value = "[]"
            hdnIsOtherReceipt.Value = "0" ' ALWAYS reset to 0
            hdnIsFinalSubmit.Value = "0"

            ' ===== CLEAR UI CONTROLS =====
            Customer.Text = ""
            Note.Text = "/"
            TOTPRM.Text = "0.000"

            ' Reset validation
            Customer.ValidationSettings.RequiredField.IsRequired = False
            Note.ValidationSettings.RequiredField.IsRequired = False
            MoveDate.ValidationSettings.RequiredField.IsRequired = False

            ' Clear grid
            GridData.DataSource = Nothing
            GridData.DataBind()

            ' Reset MoveDate to default (today)
            MoveDate.ClientEnabled = False
            MoveDate.Value = DateTime.Now.Date

            ' Show/hide appropriate controls
            TOTPRM.Visible = True
            GridData.Visible = True
            'CheckCust.Visible = True

            Debug.WriteLine("=== ALL POLICY DATA CLEARED ===")
        Catch ex As Exception
            Debug.WriteLine($"Error in ClearAllPolicyData: {ex.Message}")
        End Try
    End Sub

    Private Sub HandleOtherReceipts()
        Try
            ' ===== CLEAR ALL PREVIOUS POLICY DATA FIRST =====
            ClearAllPolicyData()

            ' Set Other Receipt flag
            hdnIsOtherReceipt.Value = "1"
            hdnReceiptType.Value = "مقبوضات أخرى"
            lblTitle.Text = "مقبوضات أخرى متعددة"

            ' Clear and make Customer REQUIRED
            Customer.Text = ""
            Customer.ValidationSettings.RequiredField.IsRequired = True
            Customer.ValidationSettings.RequiredField.ErrorText = "اسم العميل مطلوب"
            Customer.ClientEnabled = True

            ' Set MoveDate to TODAY and make it REQUIRED
            MoveDate.ClientEnabled = True
            MoveDate.Value = DateTime.Now.Date
            MoveDate.ValidationSettings.RequiredField.IsRequired = True
            MoveDate.ValidationSettings.RequiredField.ErrorText = "تاريخ الحركة مطلوب"

            ' Clear Note and make it REQUIRED
            Note.Text = ""
            Note.ValidationSettings.RequiredField.IsRequired = True
            Note.ValidationSettings.RequiredField.ErrorText = "الملاحظات مطلوبة"

            ' Hide unnecessary fields for Other receipts
            TOTPRM.Text = "0.000"
            TOTPRM.Visible = False
            GridData.Visible = False
            ' CheckCust.Visible = False
            hdnTotalDue.Value = "0"

            ' تعيين اسم العميل إذا كان متوفرًا في Request
            If Not String.IsNullOrEmpty(Request("CustomerName")) Then
                Customer.Text = Request("CustomerName")
            ElseIf Session("LastCustomer") IsNot Nothing Then
                Customer.Text = Session("LastCustomer").ToString()
            End If

            ' Clear any existing grid data source
            GridData.DataSource = Nothing
            GridData.DataBind()

            ' Show message to user
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OtherReceiptInfo",
            "showMessage('يرجى ملء بيانات المقبوضات الأخرى: اسم العميل، تاريخ الحركة، والملاحظات', 'info');", True)

            Debug.WriteLine("HandleOtherReceipts completed - All data cleared")
        Catch ex As Exception
            LogError(ex)
            ShowErrorMessage("حدث خطأ في تهيئة صفحة المقبوضات الأخرى: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadPolicyData(polNo As String, endNo As String, loadNo As String)
        Try
            ' ===== ENSURE WE'RE NOT IN OTHER RECEIPT MODE =====
            hdnIsOtherReceipt.Value = "0"

            Using conn As New SqlConnection(connectionString)
                Dim query As String = " SELECT PolNo,OrderNo, PolicyFile.CustNo, Currency, SubIns, EndNo, LoadNo, TOTPRM-InBox As TOTPRM,
            IssuDate, CustName, ExtraInfo.TpName As TpName,
            CASE WHEN Commision <> 0 AND Broker <> 0
            THEN CAST(Commision AS NVARCHAR(100)) +
            CASE WHEN CommisionType = 1 THEN ' ' + EXTRAINFO.TPName ELSE ' %' END + '-' + BrokersInfo.TPName
            ELSE '0' END As Commissioned,
            PolicyFile.Branch
            FROM PolicyFile
            INNER JOIN ExtraInfo ON TP='Cur' AND TpNo=Currency
            INNER JOIN CustomerFile ON CustomerFile.CustNo=PolicyFile.CustNo
            LEFT OUTER JOIN EXTRAINFO As BrokersInfo ON BrokersInfo.TP='Broker' AND PolicyFile.Broker=BrokersInfo.TPNo
            WHERE PolNo=@PolNo AND EndNo=@EndNo AND LoadNo=@LoadNo AND PolicyFile.Branch=@Branch"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@PolNo", polNo)
                    cmd.Parameters.AddWithValue("@EndNo", endNo)
                    cmd.Parameters.AddWithValue("@LoadNo", loadNo)
                    cmd.Parameters.AddWithValue("@Branch", hdnBranchCode.Value)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            ' تعيين بيانات الوثيقة
                            Dim totalAmount As Decimal = Convert.ToDecimal(reader("TOTPRM"))
                            TOTPRM.Text = Format(totalAmount, "###,#0.000")
                            Customer.Text = reader("CustName").ToString()
                            hdnTotalDue.Value = totalAmount.ToString()

                            ' تعيين رمز الفرع إذا لم يكن موجودًا
                            If Session("Branch") Is Nothing Then
                                Session("Branch") = reader("Branch").ToString()
                                lblBranch.Text = Session("Branch").ToString()
                                hdnBranchCode.Value = Session("Branch").ToString()
                            End If

                            ' تحديد تاريخ الحركة
                            Dim issueDate As Date = Convert.ToDateTime(reader("IssuDate"))
                            SetMoveDate(issueDate)

                            ' تخزين بيانات الوثيقة في Dictionary
                            Dim policyData As New Dictionary(Of String, Object) From {
                        {"PolNo", polNo},
                        {"EndNo", endNo},
                        {"LoadNo", loadNo},
                        {"CustNo", reader("CustNo").ToString()},
                        {"SubIns", reader("SubIns").ToString()},
                        {"tpName", reader("TpName").ToString()},
                        {"CustomerName", reader("CustName").ToString()},
                        {"OrderNo", reader("OrderNo").ToString()},
                        {"Note", "/"}
                    }

                            ' تخزين في متعددة الأماكن
                            Session("PolicyData") = policyData
                            Me.PolicyDataStore = policyData
                            Dim jsonData As String = JsonConvert.SerializeObject(policyData)
                            hdnPolicyData.Value = jsonData

                            ' IMPORTANT: Remove any old Application data first
                            Dim appKey As String = $"PolicyData_{Session.SessionID}"
                            Application.Lock()
                            If Application(appKey) IsNot Nothing Then
                                Application.Remove(appKey)
                            End If
                            Application(appKey) = policyData
                            Application.UnLock()

                            ' Clear any Other receipt data
                            Session.Remove("LastCustomer")

                            ' عرض بيانات الوثيقة في Grid
                            BindPolicyDataToGrid(polNo, endNo, loadNo)

                            ' ===== CRITICAL: Set flag for policy payments =====
                            hdnIsOtherReceipt.Value = "0"
                            Debug.WriteLine($"LoadPolicyData: Setting hdnIsOtherReceipt to 0")
                        Else
                            ShowErrorMessage("لم يتم العثور على الوثيقة المطلوبة")
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LogError(ex)
            ShowErrorMessage("حدث خطأ أثناء تحميل بيانات الوثيقة: " & ex.Message)
        End Try
    End Sub

    Private Sub SetMoveDate(issueDate As Date)
        If issueDate.Month = Today.Month Then
            MoveDate.ClientEnabled = False
            MoveDate.Value = Today.Date
        Else
            MoveDate.ClientEnabled = False
            MoveDate.Value = LastDayOfMonth(issueDate)
        End If
    End Sub

    Private Function LastDayOfMonth(d As DateTime) As DateTime
        Return New DateTime(d.Year, d.Month, Date.DaysInMonth(d.Year, d.Month))
    End Function

    Private Sub BindPolicyDataToGrid(polNo As String, endNo As String, loadNo As String)
        Try
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "
                    SELECT PolNo, OrderNo, EndNo, LoadNo, TOTPRM-InBox As TOTPRM,
                    (SELECT TpName FROM EXTRAINFO WHERE TP='Cur' AND TpNo=PolicyFile.Currency) AS TpName,
                    CASE WHEN Commision <> 0 AND Broker <> 0
                    THEN CAST(Commision AS NVARCHAR(100)) +
                    CASE WHEN CommisionType = 1 THEN ' ' + (SELECT TPName FROM EXTRAINFO WHERE TP='Cur' AND TpNo=PolicyFile.Currency) ELSE ' %' END + '-' +
                    (SELECT TPName FROM EXTRAINFO WHERE TP='Broker' AND TPNo=PolicyFile.Broker)
                    ELSE '0' END As Commissioned
                    FROM PolicyFile
                    WHERE PolNo=@PolNo AND EndNo=@EndNo AND LoadNo=@LoadNo"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@PolNo", polNo)
                    cmd.Parameters.AddWithValue("@EndNo", Integer.Parse(endNo))
                    cmd.Parameters.AddWithValue("@LoadNo", Integer.Parse(loadNo))
                    conn.Open()

                    Dim dt As New DataTable()
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using

                    If dt.Rows.Count > 0 Then
                        GridData.DataSource = dt
                        GridData.DataBind()
                        hdnGridData.Value = JsonConvert.SerializeObject(dt)
                    Else
                        ShowErrorMessage("لم يتم العثور على بيانات للعرض في الجدول")
                    End If
                End Using
            End Using
        Catch ex As Exception
            LogError(ex)
            ShowErrorMessage("حدث خطأ أثناء ربط البيانات بالجدول: " & ex.Message)
        End Try
    End Sub

    'Protected Sub CheckCust_CheckedChanged(sender As Object, e As EventArgs) Handles CheckCust.CheckedChanged
    '    'No logic needed here
    'End Sub

    'Private Sub LoadAllCustomerDocuments()
    '    'Original code for loading all customer documents
    'End Sub

    Protected Sub Sdad_Click(sender As Object, e As EventArgs) Handles sdad.Click
        'No logic needed here
    End Sub

    Public Function IsOtherReceipt1() As Boolean
        ' Check if this is Other receipt by multiple methods
        Try
            ' Method 1: Direct check of hidden field
            If hdnIsOtherReceipt.Value = "1" Then
                Return True
            End If

            ' Method 2: Check receipt type
            If hdnReceiptType.Value = "مقبوضات أخرى" Then
                Return True
            End If

            ' Method 3: Check if PolNo is "أخرى"
            Dim polNo As String = Request("PolNo")
            If Not String.IsNullOrEmpty(polNo) AndAlso polNo = "أخـرى" Then
                Return True
            End If

            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub ProcessMultiPayment()
        Try
            ' ===== USE THE FUNCTION TO DETERMINE RECEIPT TYPE =====
            Dim isOtherReceipt As Boolean = IsOtherReceipt1()

            Debug.WriteLine($"ProcessMultiPayment - IsOtherReceipt: {isOtherReceipt}")
            Debug.WriteLine($"hdnIsOtherReceipt.Value: {hdnIsOtherReceipt.Value}")
            Debug.WriteLine($"hdnReceiptType.Value: {hdnReceiptType.Value}")

            Dim totalDue As Decimal = 0

            ' ===== VALIDATION FOR OTHER RECEIPTS =====
            If isOtherReceipt Then
                ' Validate required fields
                Dim validationErrors As New List(Of String)

                If String.IsNullOrEmpty(Customer.Text.Trim()) Then
                    validationErrors.Add("اسم العميل مطلوب للمقبوضات الأخرى")
                End If

                If MoveDate.Value Is Nothing OrElse MoveDate.Value = Date.MinValue Then
                    validationErrors.Add("تاريخ الحركة مطلوب للمقبوضات الأخرى")
                End If

                If String.IsNullOrEmpty(Note.Text.Trim()) OrElse Note.Text.Trim() = "/" Then
                    validationErrors.Add("الملاحظات مطلوبة للمقبوضات الأخرى")
                End If

                ' ===== FIXED: Correctly check totalDue =====
                If Not Decimal.TryParse(hdnTotalDue.Value, totalDue) OrElse totalDue <= 0 Then
                    validationErrors.Add("المبلغ الإجمالي غير محدد")
                End If

                If validationErrors.Count > 0 Then
                    ShowErrorMessage("<strong>أخطاء في البيانات:</strong><br>" & String.Join("<br>", validationErrors))
                    ViewState("ProcessMultiPaymentExecuted") = False
                    hdnIsFinalSubmit.Value = "0"
                    Return
                End If
            End If
            ' ===== END VALIDATION =====

            ' Continue with existing logic...
            Dim paymentsJson As String = hdnPayments.Value
            If String.IsNullOrEmpty(paymentsJson) Then
                Throw New Exception("لم يتم تحديد أي مدفوعات")
            End If

            Dim payments As List(Of Dictionary(Of String, Object)) = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(paymentsJson)
            Dim totalPaid As Decimal = payments.Sum(Function(p) Convert.ToDecimal(p("Amount")))

            ' ===== CRITICAL: Get totalDue correctly =====
            If Not isOtherReceipt Then
                ' For policy payments, parse from hdnTotalDue
                totalDue = Convert.ToDecimal(hdnTotalDue.Value)
            End If

            ' ===== ADD VALIDATION FOR PAYMENTS COUNT =====
            If payments.Count = 0 Then
                ShowErrorMessage("لم يتم إضافة أي مدفوعات")
                ViewState("ProcessMultiPaymentExecuted") = False
                hdnIsFinalSubmit.Value = "0"
                Return
            End If
            ' ===== END PAYMENTS VALIDATION =====

            ' === استرجاع بيانات الوثيقة ===
            Dim policyData As Dictionary(Of String, Object) = Nothing

            ' For "أخرى" payments, create minimal policy data
            If isOtherReceipt Then
                policyData = New Dictionary(Of String, Object) From {
                {"PolNo", "أخـرى"},
                {"EndNo", "0"},
                {"LoadNo", "0"},
                {"CustNo", "0"},
                {"SubIns", ""},
                {"tpName", "دينار ليبي"},
                {"CustomerName", Customer.Text.Trim()},
                {"OrderNo", Date.Now.ToString("yyyyMMddHHmmss")},
                {"Note", Note.Text}
            }

                Debug.WriteLine("Created Other Receipt Policy Data")
            Else
                ' Original logic for policy payments...
                policyData = Me.PolicyDataStore

                If policyData Is Nothing OrElse policyData.Count = 0 Then
                    policyData = TryCast(Session("PolicyData"), Dictionary(Of String, Object))
                End If

                If (policyData Is Nothing OrElse policyData.Count = 0) AndAlso Not String.IsNullOrEmpty(hdnPolicyData.Value) Then
                    Try
                        policyData = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(hdnPolicyData.Value)
                    Catch ex As Exception
                        ' تجاهل الخطأ
                    End Try
                End If

                If policyData Is Nothing OrElse policyData.Count = 0 Then
                    policyData = TryCast(Application($"PolicyData_{Session.SessionID}"), Dictionary(Of String, Object))
                End If

                If policyData Is Nothing OrElse policyData.Count = 0 Then
                    policyData = New Dictionary(Of String, Object) From {
                    {"PolNo", Request("PolNo")},
                    {"EndNo", Request("EndNo")},
                    {"LoadNo", Request("LoadNo")},
                    {"CustNo", ""},
                    {"SubIns", ""},
                    {"tpName", ""},
                    {"CustomerName", Customer.Text},
                    {"OrderNo", ""},
                    {"Note", Note.Text}
                }
                End If

                Debug.WriteLine("Created Policy Payment Data")
            End If

            ' Debug: Log what we found
            Debug.WriteLine($"PolicyData retrieved - Count: {policyData.Count}")
            For Each kvp As KeyValuePair(Of String, Object) In policyData
                Debug.WriteLine($"{kvp.Key}: {kvp.Value}")
            Next

            ' === استدعاء الطبقة المنطقية ===

            If Request("PolNo") = "أخـرى" Then
                hdnReceiptType.Value = "مقبوضات أخرى"
            End If
            Dim receiptNo As String = PaymentProcessor.ProcessMultiplePayments(
                payments,
                totalPaid,
                totalDue,
                hdnReceiptType.Value,
                hdnBranchCode.Value,
                hdnCurrentUser.Value,
                MoveDate.Value,
                Note.Value,
                Customer.Value,
                policyData,
                connectionString
            )

            ShowSuccessMessage("تمت عملية السداد المتعدد بنجاح")

            ' Clear data before redirecting
            ClearAfterSuccessfulPayment()

            ' Fix: Use Response.Redirect with endResponse=False to avoid ThreadAbortException
            Response.Redirect($"~/Finance/DailySarf.aspx?daily={receiptNo}&Sys=1", False)
            Context.ApplicationInstance.CompleteRequest()
            Return

        Catch ex As Exception
            ' Check if it's ThreadAbortException (shouldn't happen now, but just in case)
            If TypeOf ex Is Threading.ThreadAbortException Then
                Return
            End If

            LogError(ex)
            ShowErrorMessage("حدث خطأ أثناء معالجة الدفع: " & ex.Message)

            ' Reset the flag to allow retry
            ViewState("ProcessMultiPaymentExecuted") = False
            hdnIsFinalSubmit.Value = "0"
        End Try
    End Sub

    Private Sub ClearAfterSuccessfulPayment()
        Try
            ' Clear all data after successful payment
            ClearAllPolicyData()

            ' Clear session data
            Session.Remove("CurrentPolNo")
            Session.Remove("CurrentEndNo")
            Session.Remove("CurrentLoadNo")
            Session.Remove("PolicyData")

            ' Reset flags
            hdnIsFinalSubmit.Value = "0"
            ViewState("ProcessMultiPaymentExecuted") = False

            ' Clear UI
            Customer.Text = ""
            Note.Text = "/"
            MoveDate.Value = DateTime.Now.Date

            ' Clear grid
            GridData.DataSource = Nothing
            GridData.DataBind()

            Debug.WriteLine("Data cleared after successful payment")
        Catch ex As Exception
            Debug.WriteLine($"Error clearing data after payment: {ex.Message}")
        End Try
    End Sub

    ' --- الدوال المساعدة للواجهة فقط ---
    Private Sub ShowErrorMessage(message As String)
        Dim script As String = $"<script>alert('{message.Replace("'", "\'")}');</script>"
        ClientScript.RegisterStartupScript([GetType](), "ErrorMessage", script, True)
    End Sub

    Private Sub ShowSuccessMessage(message As String)
        Dim script As String = $"<script>alert('{message.Replace("'", "\'")}');</script>"
        ClientScript.RegisterStartupScript([GetType](), "SuccessMessage", script, True)
    End Sub

    Private Sub LogError(ex As Exception)
        Try
            Dim logPath As String = Server.MapPath("~/Logs/Errors.log")
            Dim logDir As String = IO.Path.GetDirectoryName(logPath)

            If Not IO.Directory.Exists(logDir) Then
                IO.Directory.CreateDirectory(logDir)
            End If

            Using sw As New IO.StreamWriter(logPath, True)
                sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {ex.Message}")
                sw.WriteLine($"StackTrace: {ex.StackTrace}")
                sw.WriteLine($"Page: {Request.Url.AbsolutePath}")
                sw.WriteLine($"User: {Session("User")}")
                sw.WriteLine($"Branch: {Session("Branch")}")
                sw.WriteLine("----------------------------------------")
            End Using
        Catch
            ' تجاهل أي أخطاء في تسجيل السجل
        End Try
    End Sub

End Class