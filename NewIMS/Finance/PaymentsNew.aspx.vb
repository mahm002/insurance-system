Imports System.Data.SqlClient
Imports System.Web.UI
Imports DevExpress.Web

Partial Class PaymentsNew
    Inherits Page
    Private RNo As String = ""
    Private Const SessionInvoicesData As String = "InvoicesData"
    Private Const SessionPaymentAmount As String = "PaymentAmount"

    Private ReadOnly Property BranchNo() As String
        Get
            Return If(Session("Branch"), "01000")
        End Get
    End Property

    'Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
    '    If Not IsPostBack Then
    '        InitializePage()
    '        BindAllData()
    '    End If

    '    ' Update balance if customer is selected
    '    If cmbCustomer.Value IsNot Nothing Then
    '        UpdateAccountBalance()
    '    End If
    'End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        System.Diagnostics.Debug.WriteLine("=== PAGE LOAD START ===")

        If Not IsPostBack Then
            System.Diagnostics.Debug.WriteLine("First page load")
            InitializePage()
            BindAllData()

            ' Debug: Check if comboboxes have data
            System.Diagnostics.Debug.WriteLine($"cmbCustomer items: {cmbCustomer.Items.Count}")
            System.Diagnostics.Debug.WriteLine($"cmbProductType items: {cmbProductType.Items.Count}")
            System.Diagnostics.Debug.WriteLine($"PayTyp items: {PayTyp.Items.Count}")
        Else
            System.Diagnostics.Debug.WriteLine("Postback")
        End If

        ' Update balance if customer is selected
        If cmbCustomer.Value IsNot Nothing Then
            UpdateAccountBalance()
        End If

        System.Diagnostics.Debug.WriteLine("=== PAGE LOAD END ===")
    End Sub

    Private Sub InitializePage()
        ' Set branch label
        lblBranch.Text = $"الفرع: {BranchNo}"

        ' Set move date
        MoveDate.Date = DateTime.Today

        ' Initialize JavaScript properties
        Callback.JSProperties("cpMessage") = "تم تهيئة النظام"

        ' Show all controls
        cmbCustomer.ClientVisible = True
        cmbProductType.ClientVisible = True
        PayTyp.ClientVisible = True
        AccName.ClientVisible = True

        ' Set default text for payment amount
        txtPaymentAmount.Text = "0"
    End Sub

    Private Sub BindAllData()
        Try
            ' Bind all data sources
            Systems.DataBind()
            Accounts.DataBind()
            Pay.DataBind()
            BankAccounts.DataBind()
            AccountPayable.DataBind()

            ' Bind comboboxes
            cmbProductType.DataBind()
            cmbCustomer.DataBind()
            PayTyp.DataBind()
            AccName.DataBind()

            ' Set default selections
            If cmbProductType.Items.Count > 0 Then
                cmbProductType.SelectedIndex = -1
            End If

            If cmbCustomer.Items.Count > 0 Then
                cmbCustomer.SelectedIndex = -1
                UpdateAccountBalance()
            End If

            If PayTyp.Items.Count > 0 Then
                PayTyp.SelectedIndex = -1
            End If

            ' Handle initial payment type
            HandlePaymentTypeChanges()

        Catch ex As Exception
            Callback.JSProperties("cpErrorMessage") = $"خطأ في تحميل البيانات: {ex.Message}"
        End Try
    End Sub

    Private Sub UpdateAccountBalance()
        Try
            If cmbCustomer.Value Is Nothing OrElse String.IsNullOrEmpty(cmbCustomer.Value.ToString()) Then
                Balance.Value = 0
                Return
            End If

            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString)
                Dim query As String = "SELECT ISNULL(SUM(Cr + Dr), 0) As Balance FROM Journal WHERE AccountNo = @AccountNo"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@AccountNo", cmbCustomer.Value.ToString())
                    conn.Open()
                    Dim result = cmd.ExecuteScalar()
                    Balance.Value = If(result Is DBNull.Value OrElse result Is Nothing, 0, Convert.ToDecimal(result))
                End Using
            End Using

            ' Update JavaScript display
            Callback.JSProperties("cpUpdateBalance") = True

        Catch ex As Exception
            Balance.Value = 0
            Callback.JSProperties("cpErrorMessage") = $"خطأ في حساب الرصيد: {ex.Message}"
        End Try
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase)
        Dim callbackPanel As ASPxCallbackPanel = CType(sender, ASPxCallbackPanel)

        ' Debug: Log what action is being called
        System.Diagnostics.Debug.WriteLine($"Callback received: {e.Parameter}")

        Try
            Dim parameters() As String = e.Parameter.Split(":"c)
            Dim action As String = parameters(0)

            System.Diagnostics.Debug.WriteLine($"Action: {action}")

            Select Case action
                Case "UPDATE_BALANCE"
                    System.Diagnostics.Debug.WriteLine("Updating balance...")
                    UpdateAccountBalance()
                    callbackPanel.JSProperties("cpMessage") = "تم تحديث الرصيد"

                Case "LOAD_INVOICES"
                    System.Diagnostics.Debug.WriteLine("Loading invoices...")

                    ' Debug customer and product type values
                    System.Diagnostics.Debug.WriteLine($"Customer value: {cmbCustomer.Value}")
                    System.Diagnostics.Debug.WriteLine($"ProductType value: {cmbProductType.Value}")

                    If cmbCustomer.Value IsNot Nothing AndAlso cmbProductType.Value IsNot Nothing Then
                        LoadInvoicesFromDatabase()
                        System.Diagnostics.Debug.WriteLine($"Loaded {InvoicesData.Rows.Count} invoices")
                        callbackPanel.JSProperties("cpMessage") = $"تم تحميل {InvoicesData.Rows.Count} وثيقة"
                        callbackPanel.JSProperties("cpRefreshGrid") = True
                    Else
                        System.Diagnostics.Debug.WriteLine("Validation failed - missing selections")
                        callbackPanel.JSProperties("cpErrorMessage") = "يرجى اختيار الحساب ونوع التأمين أولاً"
                    End If

                Case "CALCULATE"
                    System.Diagnostics.Debug.WriteLine("Calculating payments...")
                    If parameters.Length >= 2 AndAlso Decimal.TryParse(parameters(1), PaymentAmount) Then
                        CalculatePayments()
                        callbackPanel.JSProperties("cpMessage") = $"تم توزيع {PaymentAmount:N3} على الوثائق المختارة"
                        callbackPanel.JSProperties("cpRefreshGrid") = True
                    End If

                Case "SAVE_PAYMENTS"
                    System.Diagnostics.Debug.WriteLine("Saving payments...")
                    If ValidatePaymentAmount() Then
                        SavePaymentsToDatabase()
                        ClearForm()
                        callbackPanel.JSProperties("cpSuccessMessage") = "تم حفظ التوزيع بنجاح!"
                        callbackPanel.JSProperties("cpRefreshGrid") = True
                    Else
                        callbackPanel.JSProperties("cpErrorMessage") = "يرجى التحقق من البيانات قبل الحفظ"
                    End If

                Case "PaymentChanged"
                    System.Diagnostics.Debug.WriteLine("Payment type changed")
                    HandlePaymentTypeChanges()
                    callbackPanel.JSProperties("cpMessage") = "تم تحديث طريقة الدفع"
                Case "TEST_ACTION"
                    callbackPanel.JSProperties("cpMessage") = "Callback is working! Server received the test action."
                Case Else
                    System.Diagnostics.Debug.WriteLine($"Unknown action: {action}")
                    UpdateAccountBalance()
            End Select

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Callback error: {ex.Message}")
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}")
            callbackPanel.JSProperties("cpErrorMessage") = $"حدث خطأ: {ex.Message}"
        End Try
    End Sub

    Private Sub HandlePaymentTypeChanges()
        Try
            Dim paymentType As Integer = If(PayTyp.Value Is Nothing, 1, Convert.ToInt32(PayTyp.Value))

            Select Case paymentType
                Case 1 ' نقداً
                    AccNo.ClientVisible = False
                    Bank.ClientVisible = False
                    AccName.ClientEnabled = False
                    txtPaymentAmount.ClientEnabled = True
                    Customer.ClientEnabled = True
                    AccName.Caption = "الحساب المصرفي"

                Case 2 ' شيك
                    AccNo.ClientVisible = True
                    Bank.ClientVisible = True
                    AccName.ClientEnabled = False
                    txtPaymentAmount.ClientEnabled = True
                    Customer.ClientEnabled = True
                    AccName.Caption = "الحساب المصرفي"

                Case 3 ' حوالة
                    AccNo.ClientVisible = True
                    Bank.ClientVisible = True
                    AccName.ClientEnabled = True
                    txtPaymentAmount.ClientEnabled = True
                    Customer.ClientEnabled = True
                    AccName.DataSourceID = "BankAccounts"
                    AccName.Caption = "الحساب المصرفي رقم"
                    AccName.DataBind()

                Case 4 ' تحويل مديونية
                    AccNo.ClientVisible = False
                    Bank.ClientVisible = False
                    AccName.ClientEnabled = True
                    txtPaymentAmount.ClientEnabled = False
                    Customer.ClientEnabled = False
                    AccName.DataSourceID = "AccountPayable"
                    AccName.Caption = "حساب المدينون رقم"
                    AccName.DataBind()

                    ' Calculate selected invoices total
                    CalculateSelectedInvoicesTotal()

                Case 5 ' بطاقة ائتمان
                    AccNo.ClientVisible = False
                    Bank.ClientVisible = False
                    AccName.ClientEnabled = True
                    txtPaymentAmount.ClientEnabled = True
                    Customer.ClientEnabled = True
                    AccName.DataSourceID = "BankAccounts"
                    AccName.Caption = "الحساب المصرفي للبطاقة"
                    AccName.DataBind()
            End Select

        Catch ex As Exception
            Callback.JSProperties("cpErrorMessage") = $"خطأ في تغيير طريقة الدفع: {ex.Message}"
        End Try
    End Sub

    Private Sub CalculateSelectedInvoicesTotal()
        Try
            Dim selectedItems = ASPxGridView1.GetSelectedFieldValues("TOTPRM")
            If selectedItems.Count = 0 Then
                txtPaymentAmount.Value = 0
                Return
            End If

            Dim total As Decimal = 0
            For Each item As Object In selectedItems
                total += Convert.ToDecimal(item)
            Next

            txtPaymentAmount.Value = total

        Catch ex As Exception
            txtPaymentAmount.Value = 0
        End Try
    End Sub

    Private Sub LoadInvoicesFromDatabase()
        Try
            Dim dt As New DataTable()

            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString)
                Dim query As String = "SELECT PolicyFile.PolNo, PolicyFile.OrderNo, PolicyFile.TOTPRM, IssuDate, " &
                                     "PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain, PolicyFile.EndNo, PolicyFile.LoadNo, " &
                                     "ISNULL(PolicyFile.Inbox, 0) AS InBox, DBO.GetExtraCatName('Cur', PolicyFile.Currency) As Curr, " &
                                     "CustName FROM PolicyFile LEFT OUTER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo " &
                                     "LEFT OUTER JOIN BranchInfo ON PolicyFile.Branch = BranchInfo.BranchNo " &
                                     "LEFT OUTER JOIN SUBSYSTEMS ON PolicyFile.SubIns = SUBSYSTEMS.SUBSYSNO AND SUBSYSTEMS.Branch = PolicyFile.Branch " &
                                     "WHERE Inbox <> TOTPRM and AccountNo = @AccntNo AND PolicyFile.Branch = @Br AND PayType = 2 " &
                                     "AND PolicyFile.SubIns = @Sys and PolicyFile.Stop = 0 ORDER BY IssuTime"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@AccntNo", cmbCustomer.Value)
                    cmd.Parameters.AddWithValue("@Br", BranchNo)
                    cmd.Parameters.AddWithValue("@Sys", cmbProductType.Value)

                    conn.Open()
                    dt.Load(cmd.ExecuteReader())
                End Using
            End Using

            ' Add calculated columns
            dt.Columns.Add("Payment", GetType(Decimal))
            dt.Columns.Add("NewRemaining", GetType(Decimal))
            dt.Columns.Add("Status", GetType(String))

            ' Initialize values
            For Each row As DataRow In dt.Rows
                row("Payment") = 0
                row("NewRemaining") = row("Remain")
                row("Status") = If(CDec(row("InBox")) > 0, "سداد_جزئي", "غير_مسدد")
            Next

            InvoicesData = dt

        Catch ex As Exception
            Callback.JSProperties("cpErrorMessage") = $"خطأ في تحميل الوثائق: {ex.Message}"
            InvoicesData = New DataTable()
        End Try
    End Sub

    Private Sub CalculatePayments()
        Try
            Dim dt As DataTable = InvoicesData
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Callback.JSProperties("cpErrorMessage") = "لا توجد وثائق محملة"
                Return
            End If

            Dim remainingPayment As Decimal = PaymentAmount
            Dim selectedOrders = ASPxGridView1.GetSelectedFieldValues("OrderNo").Cast(Of String)().ToList()

            If selectedOrders.Count = 0 Then
                Callback.JSProperties("cpErrorMessage") = "لم يتم اختيار أي وثائق"
                Return
            End If

            For Each row As DataRow In dt.Rows
                Dim orderNo = row("OrderNo").ToString()

                If Not selectedOrders.Contains(orderNo) Then
                    row("Payment") = 0
                    row("NewRemaining") = row("Remain")
                    Continue For
                End If

                Dim currentRemaining = CDec(row("Remain"))
                Dim payment As Decimal = 0

                If remainingPayment > 0 AndAlso currentRemaining > 0 Then
                    If remainingPayment >= currentRemaining Then
                        payment = currentRemaining
                        remainingPayment -= payment
                        row("Status") = "مسدد"
                    Else
                        payment = remainingPayment
                        remainingPayment = 0
                        row("Status") = "سداد_جزئي"
                    End If
                Else
                    row("Status") = If(currentRemaining > 0, "معلق", "غير_مسدد")
                End If

                row("Payment") = payment
                row("NewRemaining") = currentRemaining - payment
            Next

            InvoicesData = dt

        Catch ex As Exception
            Callback.JSProperties("cpErrorMessage") = $"خطأ في حساب التوزيع: {ex.Message}"
        End Try
    End Sub

    Private Sub SavePaymentsToDatabase()
        Try
            Dim dt As DataTable = InvoicesData
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Callback.JSProperties("cpErrorMessage") = "لا توجد بيانات لحفظها"
                Return
            End If

            Dim paymentType As Integer = If(PayTyp.Value Is Nothing, 1, Convert.ToInt32(PayTyp.Value))

            ' Start transaction
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString)
                conn.Open()
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        Dim totalPayment As Decimal = 0
                        Dim pols As String = ""
                        Dim refs As String = ""

                        ' Process each row
                        For Each row As DataRow In dt.Rows
                            Dim payment As Decimal = CDec(row("Payment"))

                            If payment > 0 Then
                                If paymentType = 4 Then
                                    ' Debt transfer - update account number
                                    Dim updateQuery As String = "UPDATE PolicyFile SET AccountNo = @AccountNo " &
                                                                "WHERE OrderNo = @OrderNo AND Branch = @Branch"
                                    Using cmd As New SqlCommand(updateQuery, conn, transaction)
                                        cmd.Parameters.AddWithValue("@AccountNo", AccName.Value)
                                        cmd.Parameters.AddWithValue("@OrderNo", row("OrderNo"))
                                        cmd.Parameters.AddWithValue("@Branch", BranchNo)
                                        cmd.ExecuteNonQuery()
                                    End Using
                                Else
                                    ' Regular payment - update inbox
                                    Dim updateQuery As String = "UPDATE PolicyFile SET Inbox = ISNULL(Inbox, 0) + @Payment " &
                                                                "WHERE OrderNo = @OrderNo AND Branch = @Branch"
                                    Using cmd As New SqlCommand(updateQuery, conn, transaction)
                                        cmd.Parameters.AddWithValue("@Payment", payment)
                                        cmd.Parameters.AddWithValue("@OrderNo", row("OrderNo"))
                                        cmd.Parameters.AddWithValue("@Branch", BranchNo)
                                        cmd.ExecuteNonQuery()
                                    End Using
                                End If

                                ' Accumulate totals
                                totalPayment += payment
                                pols += "/" + Right(row("PolNo").ToString(), 5)
                                refs += row("OrderNo").ToString() + ","
                            End If
                        Next

                        ' Generate receipt number
                        Dim parm = Array.CreateInstance(GetType(SqlParameter), 1)
                        SetPm("@BR", DbType.String, BranchNo, parm, 0)
                        RNo = CallSP("RecetNo", conn, parm)

                        ' Generate daily number
                        parm = Array.CreateInstance(GetType(SqlParameter), 3)
                        SetPm("@TP", DbType.String, "1", parm, 0)
                        SetPm("@Year", DbType.String, Right(Year(MoveDate.Value).ToString(), 2), parm, 1)
                        SetPm("@Br", DbType.String, BranchNo, parm, 2)
                        Dim dly As String = CallSP("LastDailyNo", conn, parm)

                        ' Determine account based on payment type
                        Dim accountNo As String = ""
                        Select Case paymentType
                            Case 1 ' Cash
                                accountNo = GetBoxAccount(BranchNo)
                            Case 2 ' Check
                                accountNo = GetcheqAccount(BranchNo)
                            Case 3, 5 ' Transfer, Credit Card
                                accountNo = AccName.Value.ToString()
                            Case 4 ' Debt Transfer
                                accountNo = AccName.Value.ToString()
                            Case Else
                                accountNo = GetBoxAccount(BranchNo)
                        End Select

                        ' Create description
                        Dim description As String = ""
                        If paymentType = 4 Then
                            description = "وذلك إثبات مديونية الوثائق" + pols + "/" + PayTyp.Text
                        Else
                            description = "وذلك مقابل سداد الوثائق" + pols + "/" + PayTyp.Text
                        End If

                        ' Insert into ACCMOVE
                        Dim accMoveQuery As String = "INSERT INTO ACCMOVE(DocNo, DocDat, CustName, PayMent, Amount, ForW, " &
                                                    "EndNo, LoadNo, Tp, Branch, AccNo, Bank, Cur, Node, PayTp, UserName, Note) " &
                                                    "VALUES(@DocNo, @DocDat, @CustName, @PayMent, @Amount, @ForW, " &
                                                    "@EndNo, @LoadNo, @Tp, @Branch, @AccNo, @Bank, @Cur, @Node, @PayTp, @UserName, @Note)"

                        Using cmd As New SqlCommand(accMoveQuery, conn, transaction)
                            cmd.Parameters.AddWithValue("@DocNo", RNo)
                            cmd.Parameters.AddWithValue("@DocDat", MoveDate.Value)
                            cmd.Parameters.AddWithValue("@CustName", Customer.Text)
                            cmd.Parameters.AddWithValue("@PayMent", totalPayment)
                            cmd.Parameters.AddWithValue("@Amount", totalPayment)
                            cmd.Parameters.AddWithValue("@ForW", description)
                            cmd.Parameters.AddWithValue("@EndNo", 0)
                            cmd.Parameters.AddWithValue("@LoadNo", 0)
                            cmd.Parameters.AddWithValue("@Tp", cmbProductType.Text)
                            cmd.Parameters.AddWithValue("@Branch", BranchNo)
                            cmd.Parameters.AddWithValue("@AccNo", If(String.IsNullOrEmpty(AccNo.Text), "", AccNo.Text))
                            cmd.Parameters.AddWithValue("@Bank", If(String.IsNullOrEmpty(Bank.Text), "", Bank.Text))
                            cmd.Parameters.AddWithValue("@Cur", PayTyp.Value)
                            cmd.Parameters.AddWithValue("@Node", "/")
                            cmd.Parameters.AddWithValue("@PayTp", PayTyp.Value)
                            cmd.Parameters.AddWithValue("@UserName", Session("User"))
                            cmd.Parameters.AddWithValue("@Note", "/")
                            cmd.ExecuteNonQuery()
                        End Using

                        ' Insert into MainJournal
                        Dim mainJournalQuery As String = "INSERT INTO MainJournal (DAILYNUM, DAILYDTE, DailyTyp, ANALSNUM, MoveRef, Comment, " &
                                                        "Currency, Exchange, CurUser, RecNo, DailyChk, Branch) " &
                                                        "VALUES (@DAILYNUM, @DAILYDTE, @DailyTyp, @ANALSNUM, @MoveRef, @Comment, " &
                                                        "@Currency, @Exchange, @CurUser, @RecNo, @DailyChk, @Branch)"

                        Using cmd As New SqlCommand(mainJournalQuery, conn, transaction)
                            cmd.Parameters.AddWithValue("@DAILYNUM", dly)
                            cmd.Parameters.AddWithValue("@DAILYDTE", MoveDate.Value)
                            cmd.Parameters.AddWithValue("@DailyTyp", 1)
                            cmd.Parameters.AddWithValue("@ANALSNUM", "P")
                            cmd.Parameters.AddWithValue("@MoveRef", refs)
                            cmd.Parameters.AddWithValue("@Comment", description + " / RecNo.# " + RNo)
                            cmd.Parameters.AddWithValue("@Currency", 1)
                            cmd.Parameters.AddWithValue("@Exchange", 1)
                            cmd.Parameters.AddWithValue("@CurUser", Session("User"))
                            cmd.Parameters.AddWithValue("@RecNo", RNo)
                            cmd.Parameters.AddWithValue("@DailyChk", 1)
                            cmd.Parameters.AddWithValue("@Branch", BranchNo)
                            cmd.ExecuteNonQuery()
                        End Using

                        ' Insert into Journal (Credit side)
                        Dim journalCreditQuery As String = "INSERT INTO Journal (DAILYNUM, TP, AccountNo, Dr, Cr, CurUser, Branch) " &
                                                          "VALUES (@DAILYNUM, @TP, @AccountNo, @Dr, @Cr, @CurUser, @Branch)"

                        Using cmd As New SqlCommand(journalCreditQuery, conn, transaction)
                            cmd.Parameters.AddWithValue("@DAILYNUM", dly)
                            cmd.Parameters.AddWithValue("@TP", 1)
                            cmd.Parameters.AddWithValue("@AccountNo", accountNo)
                            cmd.Parameters.AddWithValue("@Dr", totalPayment)
                            cmd.Parameters.AddWithValue("@Cr", 0)
                            cmd.Parameters.AddWithValue("@CurUser", Session("User"))
                            cmd.Parameters.AddWithValue("@Branch", BranchNo)
                            cmd.ExecuteNonQuery()
                        End Using

                        ' Insert into Journal (Debit side)
                        Dim journalDebitQuery As String = "INSERT INTO Journal (DAILYNUM, TP, AccountNo, Dr, Cr, CurUser, Branch) " &
                                                          "VALUES (@DAILYNUM, @TP, @AccountNo, @Dr, @Cr, @CurUser, @Branch)"

                        Using cmd As New SqlCommand(journalDebitQuery, conn, transaction)
                            cmd.Parameters.AddWithValue("@DAILYNUM", dly)
                            cmd.Parameters.AddWithValue("@TP", 1)
                            cmd.Parameters.AddWithValue("@AccountNo", cmbCustomer.Value.ToString())
                            cmd.Parameters.AddWithValue("@Dr", 0)
                            cmd.Parameters.AddWithValue("@Cr", totalPayment * -1)
                            cmd.Parameters.AddWithValue("@CurUser", Session("User"))
                            cmd.Parameters.AddWithValue("@Branch", BranchNo)
                            cmd.ExecuteNonQuery()
                        End Using

                        ' Commit transaction
                        transaction.Commit()

                        ' Redirect to daily report
                        ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" + dly + "&Sys=1")

                    Catch ex As Exception
                        ' Rollback on error
                        transaction.Rollback()
                        Throw
                    End Try
                End Using
            End Using

        Catch ex As Exception
            Callback.JSProperties("cpErrorMessage") = $"خطأ في حفظ البيانات: {ex.Message}"
        End Try
    End Sub

    Private Function ValidatePaymentAmount() As Boolean
        Try
            ' Validate payment amount
            Dim amount As Decimal
            If Not Decimal.TryParse(txtPaymentAmount.Text, amount) Then Return False
            If amount <= 0 Then Return False

            ' Validate customer
            If cmbCustomer.Value Is Nothing Then Return False

            ' Validate product type
            If cmbProductType.Value Is Nothing Then Return False

            ' Validate selected invoices
            Dim selectedCount As Integer = ASPxGridView1.GetSelectedFieldValues("OrderNo").Count
            If selectedCount = 0 Then Return False

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub ClearForm()
        ' Clear session data
        Session.Remove(SessionInvoicesData)
        Session.Remove(SessionPaymentAmount)

        ' Clear grid
        ASPxGridView1.DataSource = Nothing
        ASPxGridView1.DataBind()

        ' Clear form fields
        txtPaymentAmount.Text = "0"
        Customer.Text = ""
        AccNo.Text = ""
        Bank.Text = ""
        AccName.Value = Nothing

        ' Reset comboboxes
        If cmbCustomer.Items.Count > 0 Then
            cmbCustomer.SelectedIndex = 0
        End If

        If cmbProductType.Items.Count > 0 Then
            cmbProductType.SelectedIndex = 0
        End If

        If PayTyp.Items.Count > 0 Then
            PayTyp.SelectedIndex = 0
        End If

        ' Update balance
        UpdateAccountBalance()

        Callback.JSProperties("cpMessage") = "تم إعادة تعيين النموذج"
    End Sub

    Protected Sub ASPxGridView1_DataBinding(sender As Object, e As EventArgs)
        ASPxGridView1.DataSource = InvoicesData
    End Sub

    Protected Sub ASPxGridView1_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        ASPxGridView1.DataSource = InvoicesData
        ASPxGridView1.DataBind()
    End Sub

    Protected Sub MoveDate_Init(sender As Object, e As EventArgs)
        MoveDate.Date = DateTime.Today
    End Sub

    Public Function GetStatusBadge(status As String) As String
        Dim badgeClass = status.ToLower().Replace("_", "-")
        Return $"<span class='status-badge status-{badgeClass}'>{status.Replace("_", " ")}</span>"
    End Function

    Private Function CallSP(spName As String, conn As SqlConnection, params As Array) As String
        Using cmd As New SqlCommand(spName, conn)
            cmd.CommandType = CommandType.StoredProcedure
            For Each param As SqlParameter In params
                cmd.Parameters.Add(param)
            Next
            Return cmd.ExecuteScalar().ToString()
        End Using
    End Function

    ' Properties
    Private Property InvoicesData() As DataTable
        Get
            If Session(SessionInvoicesData) Is Nothing Then
                Return New DataTable()
            End If
            Return CType(Session(SessionInvoicesData), DataTable)
        End Get
        Set(value As DataTable)
            Session(SessionInvoicesData) = value
        End Set
    End Property

    Private Property PaymentAmount() As Decimal
        Get
            If Session(SessionPaymentAmount) Is Nothing Then
                Return 0D
            End If
            Return CDec(Session(SessionPaymentAmount))
        End Get
        Set(value As Decimal)
            Session(SessionPaymentAmount) = value
        End Set
    End Property
End Class