Imports System.Data.SqlClient
Imports DevExpress.Web

Partial Class Payments
    Inherits Page
    Private RNo As String = ""
    Dim temppay As String = ""
    Dim temp1 As String = ""
    Dim temp3 As String = ""
    Private Property InvoicesData() As DataTable
        Get
            If ViewState("InvoicesData") Is Nothing Then
                Return New DataTable()
            End If
            Return CType(ViewState("InvoicesData"), DataTable)
        End Get
        Set(value As DataTable)
            ViewState("InvoicesData") = value
        End Set
    End Property

    Private Property PaymentAmount() As Decimal
        Get
            If ViewState("PaymentAmount") Is Nothing Then
                Return 0
            End If
            Return CDec(ViewState("PaymentAmount"))
        End Get
        Set(value As Decimal)
            ViewState("PaymentAmount") = value
        End Set
    End Property

    Private ReadOnly Property BranchNo() As String
        Get
            If Session("Branch") IsNot Nothing Then
                Return CStr(Session("Branch"))
            Else
                Return "01000"
            End If
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblBranch.Text = $"Current Branch: {BranchNo}"
        End If
        Select Case PayTyp.Value
            Case 1
                AccNo.ClientVisible = False
                Bank.ClientVisible = False
                AccName.ClientEnabled = False
                txtPaymentAmount.ClientEnabled = True
                Customer.ClientEnabled = True
            Case 2
                AccNo.ClientVisible = True
                Bank.ClientVisible = True
                AccName.ClientEnabled = False
                txtPaymentAmount.ClientEnabled = True
                Customer.ClientEnabled = True
            Case 3
                AccNo.ClientVisible = True
                Bank.ClientVisible = True
                AccName.ClientEnabled = True
                txtPaymentAmount.ClientEnabled = True
                Customer.ClientEnabled = True

                AccName.DataSourceID = "BankAccounts"
                AccName.Caption = "الحساب المصرفي رقم"

            Case 4
                AccNo.ClientVisible = False
                Bank.ClientVisible = False
                AccName.ClientEnabled = True
                txtPaymentAmount.ClientEnabled = False
                Customer.ClientEnabled = False
                txtPaymentAmount.Value = 0
                Customer.Value = "/"

                AccName.DataSourceID = "AccountPayable"
                AccName.Caption = "حساب المدينون رقم"

                temppay = " ومديونية "

                Dim temp2 As New List(Of String)

                Dim tempTot As Double = 0

                Dim selectItems As List(Of Object) = ASPxGridView1.GetSelectedFieldValues("PolNo", "OrderNo", "TOTPRM")
                If selectItems.Count = 0 Then
                    txtPaymentAmount.Value = 0
                Else
                    For Each selectedItem As Object In selectItems
                        temp1 += "/" & Right(selectedItem(0).ToString.TrimEnd, 5) & ""
                        temp3 += "" & selectedItem(1).ToString.TrimEnd & ","
                        txtPaymentAmount.Value += selectedItem(2)
                    Next
                End If
            Case 5
                AccNo.ClientVisible = False
                Bank.ClientVisible = False
                AccName.ClientEnabled = True
                txtPaymentAmount.ClientEnabled = True
                Customer.ClientEnabled = True

                AccName.DataSourceID = "BankAccounts"
                AccName.Caption = "الحساب المصرفي للبطاقة"
                'BankAccounts.DataBind()
            Case Else
                Exit Select
        End Select
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase)
        Dim parameters() As String = e.Parameter.Split(":"c)
        Dim action As String = parameters(0)
        Dim callbackPanel As ASPxCallbackPanel = CType(sender, ASPxCallbackPanel)
        If cmbCustomer.Value = "/" Then
            Exit Sub
        Else
            Dim AccBlnc As New DataSet
            Dim dbadapterBlnc = New SqlDataAdapter(String.Format("select Sum(Cr)+Sum(Dr) As Balance from Journal where AccountNo='{0}'", cmbCustomer.Value), Conn)
            dbadapterBlnc.Fill(AccBlnc)
            If AccBlnc.Tables(0).Rows.Count = 0 Then
                Balance.Value = 0
            Else
                Balance.Value = AccBlnc.Tables(0).Rows(0)("Balance")
            End If
        End If
        Try
            Select Case action
                Case "LOAD_INVOICES"
                    If cmbCustomer.SelectedIndex >= 0 AndAlso cmbProductType.SelectedIndex >= 0 Then
                        LoadInvoicesFromDatabase()
                        ASPxGridView1.DataSource = InvoicesData
                        ASPxGridView1.DataBind() ' <-- This was missing!
                        callbackPanel.JSProperties("cpMessage") = $"تم تحميل {InvoicesData.Rows.Count} وثيقة لتوزيع الميلغ."
                        'callbackPanel.JSProperties("cpRefreshGrid") = True
                    Else
                        callbackPanel.JSProperties("cpErrorMessage") = "يرجى اختيار الحساب ونوع التأمين"
                    End If

                Case "CALCULATE"
                    If parameters.Length >= 2 AndAlso Decimal.TryParse(parameters(1), PaymentAmount) AndAlso PaymentAmount > 0 Then
                        'AndAlso PaymentAmount <= Balance.Value
                        LoadInvoicesFromDatabase()
                        CalculatePayments()
                        ASPxGridView1.DataSource = InvoicesData
                        ASPxGridView1.DataBind() ' <-- Ensure binding happens
                        callbackPanel.JSProperties("cpMessage") = $"تم توزيع {PaymentAmount:n3} بنجاح."
                        'callbackPanel.JSProperties("cpRefreshGrid") = True
                    Else
                        callbackPanel.JSProperties("cpErrorMessage") = "يرجى إدخال قيمة السداد بحيث يكون اقل من او يساوي الرصيد للحساب ولا يساوي 0."
                    End If

                Case "SAVE_PAYMENTS"
                    'If PayTyp.Value = 4 Then
                    '    LoadInvoicesFromDatabase()
                    '    CalculatePayments()
                    'Else
                    If parameters.Length >= 2 AndAlso Decimal.TryParse(parameters(1), PaymentAmount) AndAlso PaymentAmount > 0 AndAlso PaymentAmount <= Balance.Value Then
                            LoadInvoicesFromDatabase()
                            CalculatePayments()
                            ASPxGridView1.DataSource = InvoicesData
                            ASPxGridView1.DataBind() ' <-- Ensure binding happens
                            'callbackPanel.JSProperties("cpMessage") = $"تم توزيع {PaymentAmount:C} بنجاح."
                            'callbackPanel.JSProperties("cpRefreshGrid") = True
                        Else
                            callbackPanel.JSProperties("cpErrorMessage") = "يرجى إدخال قيمة السداد بحيث يكون اقل من او يساوي الرصيد للحساب ولا يساوي 0."
                        End If
                    'End If

                    If PaymentAmount > 0 AndAlso InvoicesData.Rows.Count > 0 Then
                        SavePaymentsToDatabase()
                        ' Clear data after successful save
                        InvoicesData = New DataTable()
                        PaymentAmount = 0
                        ' Clear grid
                        ASPxGridView1.DataSource = Nothing
                        ASPxGridView1.DataBind()

                        callbackPanel.JSProperties("cpSuccessMessage") = "تم توزيع المبلغ بنجاح!"
                        callbackPanel.JSProperties("cpRefreshGrid") = True
                    Else
                        If PayTyp.Value = 4 AndAlso txtPaymentAmount.Value > 0 And AccName.IsValid Then
                            SavePaymentsToDatabase()
                            ' Clear data after successful save
                            InvoicesData = New DataTable()
                            PaymentAmount = 0
                            ' Clear grid
                            ASPxGridView1.DataSource = Nothing
                            ASPxGridView1.DataBind()

                            callbackPanel.JSProperties("cpSuccessMessage") = "تم توزيع المبلغ بنجاح!"
                            callbackPanel.JSProperties("cpRefreshGrid") = True
                        Else
                            callbackPanel.JSProperties("cpErrorMessage") = "يرجى توزيع المبلغ قبل الحفظ ."
                        End If
                    End If
                Case "PaymentChanged"
                    Select Case PayTyp.Value
                        Case 1
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientEnabled = False
                            txtPaymentAmount.ClientEnabled = True
                            Customer.ClientEnabled = True
                        Case 2
                            AccNo.ClientVisible = True
                            Bank.ClientVisible = True
                            AccName.ClientEnabled = False
                            txtPaymentAmount.ClientEnabled = True
                            Customer.ClientEnabled = True
                        Case 3
                            AccNo.ClientVisible = True
                            Bank.ClientVisible = True
                            AccName.ClientEnabled = True
                            txtPaymentAmount.ClientEnabled = True
                            Customer.ClientEnabled = True

                            AccName.DataSourceID = "BankAccounts"
                            AccName.Caption = "الحساب المصرفي رقم"

                        Case 4
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientEnabled = True
                            txtPaymentAmount.ClientEnabled = False
                            Customer.ClientEnabled = False
                            txtPaymentAmount.Value = 0
                            Customer.Value = "/"

                            AccName.DataSourceID = "AccountPayable"
                            AccName.Caption = "حساب المدينون رقم"

                            temppay = " ومديونية "

                            Dim temp2 As New List(Of String)

                            Dim tempTot As Double = 0

                            Dim selectItems As List(Of Object) = ASPxGridView1.GetSelectedFieldValues("PolNo", "OrderNo", "TOTPRM")
                            If selectItems.Count = 0 Then
                                txtPaymentAmount.Value = 0
                            Else
                                For Each selectedItem As Object In selectItems
                                    temp1 += "/" & Right(selectedItem(0).ToString.TrimEnd, 5) & ""
                                    temp3 += "" & selectedItem(1).ToString.TrimEnd & ","
                                    txtPaymentAmount.Value += selectedItem(2)
                                Next
                            End If
                        Case 5
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientEnabled = True
                            txtPaymentAmount.ClientEnabled = True
                            Customer.ClientEnabled = True

                            AccName.DataSourceID = "BankAccounts"
                            AccName.Caption = "الحساب المصرفي للبطاقة"
                            'BankAccounts.DataBind()
                        Case Else
                            Exit Select
                    End Select
                Case Else
                    Exit Select
            End Select
        Catch ex As Exception
            callbackPanel.JSProperties("cpErrorMessage") = "خطأ: " & ex.Message
            Debug.WriteLine("Callback error: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadInvoicesFromDatabase()
        Try
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

            Dim query As String = "SELECT PolicyFile.PolNo, PolicyFile.OrderNo, PolicyFile.TOTPRM, IssuDate, " &
                                 "PolicyFile.TOTPRM - ISNULL(PolicyFile.Inbox, 0) As Remain, PolicyFile.EndNo, PolicyFile.LoadNo, " &
                                 "ISNULL(PolicyFile.Inbox, 0) AS InBox, DBO.GetExtraCatName('Cur', PolicyFile.Currency) As Curr, " &
                                 "CustName FROM PolicyFile LEFT OUTER JOIN CustomerFile ON PolicyFile.CustNo = CustomerFile.CustNo " &
                                 "LEFT OUTER JOIN BranchInfo ON PolicyFile.Branch = BranchInfo.BranchNo " &
                                 "LEFT OUTER JOIN SUBSYSTEMS ON PolicyFile.SubIns = SUBSYSTEMS.SUBSYSNO AND SUBSYSTEMS.Branch = PolicyFile.Branch " &
                                 "WHERE Inbox <> TOTPRM and AccountNo = @AccntNo AND PolicyFile.Branch = @Br AND PayType = 2 " &
                                 "AND PolicyFile.SubIns = @Sys and PolicyFile.Stop = 0 ORDER BY IssuTime"

            Using conn3 As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(query, conn3)
                    cmd.Parameters.AddWithValue("@AccntNo", cmbCustomer.Value)
                    cmd.Parameters.AddWithValue("@Br", BranchNo)
                    cmd.Parameters.AddWithValue("@Sys", cmbProductType.Value)

                    conn3.Open()
                    Dim dt As New DataTable()
                    dt.Load(cmd.ExecuteReader())

                    Debug.WriteLine($"Loaded {dt.Rows.Count} invoices from database")

                    ' Add calculated columns for payment distribution
                    dt.Columns.Add("Payment", GetType(Decimal))
                    dt.Columns.Add("NewRemaining", GetType(Decimal))
                    dt.Columns.Add("Status", GetType(String))

                    ' Initialize calculated columns
                    For Each row As DataRow In dt.Rows
                        row("Payment") = 0
                        row("NewRemaining") = row("Remain")
                        row("Status") = If(CDec(row("InBox")) > 0, "سداد_جزئي", "غير_مسدد")
                    Next

                    InvoicesData = dt
                End Using
            End Using
        Catch ex As Exception
            Debug.WriteLine("Error loading invoices: " & ex.Message)
            Throw
        End Try
    End Sub

    Private Sub CalculatePayments()
        Dim remainingPayment As Decimal = PaymentAmount
        ' Work with the actual DataTable, not a copy
        Dim dt As DataTable = InvoicesData
        Dim selectedKeys As String() = ASPxGridView1.GetSelectedFieldValues("OrderNo").Cast(Of Object)().Select(Function(o) o.ToString()).ToArray()
        For Each row As DataRow In dt.Rows
            Dim Order As String = row("OrderNo").ToString()

            ' Only process if this row is selected
            If Not selectedKeys.Contains(Order) Then
                ' Optionally preserve existing values or skip
                ' You might want to reset non-selected rows or leave them unchanged
                Continue For
            End If

            Dim currentRemaining As Decimal = CDec(row("Remain"))
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
            ElseIf currentRemaining > 0 Then
                row("Status") = "معلق"
            Else
                row("Status") = "غير_مسدد"
            End If

            row("Payment") = payment
            row("NewRemaining") = currentRemaining - payment
        Next

        InvoicesData = dt
        Debug.WriteLine($"Payment calculation completed. Remaining payment: {remainingPayment}")
    End Sub
    Protected Sub MoveDate_Init(sender As Object, e As EventArgs)
        MoveDate.Date = New DateTime(Today.Date().Year, Today.Date().Month, Today.Date().Day)
    End Sub
    Private Sub SavePaymentsToDatabase()
        If PayTyp.Value = 4 And AccName.IsValid Then
            If Len(AccName.Value.ToString.TrimEnd) = 1 Then Exit Sub
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString
            Dim TotPayed As Decimal = 0
            Dim Pols As String = ""
            Dim Refs As String = ""
            Using con As New SqlConnection(connectionString)
                con.Open()
                Using transaction As SqlTransaction = con.BeginTransaction()
                    Try
                        For Each row As DataRow In InvoicesData.Rows
                            Dim payment As Decimal = CDec(row("Payment"))

                            If payment > 0 Then
                                ' Update the PolicyFile with the payment
                                Dim UpdateQuery As String = "UPDATE PolicyFile SET AccountNo = @AccountN " _
                                                          & "WHERE OrderNo = @OrderNo AND Branch = @Branch"

                                Using cmd As New SqlCommand(UpdateQuery, con, transaction)
                                    cmd.Parameters.AddWithValue("@AccountN", AccName.Value)
                                    cmd.Parameters.AddWithValue("@OrderNo", row("OrderNo"))
                                    cmd.Parameters.AddWithValue("@Branch", BranchNo)
                                    cmd.ExecuteNonQuery()
                                End Using
                                Refs += row("OrderNo") + ","
                                Pols += "/" + Right(row("PolNo"), 5)
                                TotPayed += payment
                            End If
                        Next
                        'UPDATE OrderNo's that registered in MainJournal
                        For Each row As DataRow In InvoicesData.Rows
                            Dim payment As Decimal = CDec(row("Payment"))
                            If payment > 0 Then
                                If row("Status") = "مسدد" Then
                                    ExecConn("Update MainJournal Set MoveRef=REPLACE(MoveRef, '" & (row("OrderNo")) & "','') where MoveRef like '%" & (row("OrderNo") + ",") & "%' OR MoveRef like '%" & row("OrderNo") & "%'", Conn)
                                End If
                            End If
                        Next
                        ExecConn("Update MainJournal Set MoveRef=REPLACE(MoveRef, ',,',',') ", Conn)

                        Pols = "وذلك إثبات مديونية الوثائق" + Pols + "/" + PayTyp.Text

                        Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                        SetPm("@TP", DbType.String, "1", Parm, 0)
                        SetPm("@Year", DbType.String, Right(Year(MoveDate.Value).ToString, 2), Parm, 1)
                        SetPm("@Br", DbType.String, Session("Branch"), Parm, 2)

                        Dim Dly As String = CallSP("LastDailyNo", Conn, Parm)
                        Dim accnt As String = ""
                        Dim Str As String = Pols
                        Select Case PayTyp.Value
                            Case 1
                                accnt = GetBoxAccount(Session("Branch"))
                            Case 2
                                accnt = GetcheqAccount(Session("Branch"))
                            Case 3, 5
                                accnt = AccName.Value
                            Case Else
                                accnt = AccName.Value
                        End Select

                        'Dim Refsplited = Refs.Split(",")
                        ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] , MoveRef ,[Comment] " _
                                                & ",[Currency] ,[Exchange] ,[CurUser] ,[RecNo],[DailyChk],[Branch])  " _
                                                & " VALUES ('" & Dly & "','" & Format(MoveDate.Value, "yyyy/MM/dd") & "', " _
                                                & 1 & ",'1','" & Refs & "','" & Str & "', " & 1 & "," & 1 & "," _
                                                & "'" & Session("User") & "','" & RNo & "'," & 1 & ",'" & Session("Branch") & "')", Conn)

                        ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch]) VALUES ('" & Dly.ToString & "',1, '" & accnt & "'," & TotPayed & ",0,'" & Session("User") & "','" & Session("Branch") & "')", Conn)
                        ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch]) VALUES ('" & Dly.ToString & "',1, '" & cmbCustomer.Value & "',0," & TotPayed * -1 & ",'" & Session("User") & "','" & Session("Branch") & "')", Conn)

                        ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" + Dly.ToString + "&Sys=1")
                        transaction.Commit()
                        'Debug.WriteLine("Payments saved successfully to database")
                    Catch ex As Exception
                        transaction.Rollback()
                        'Debug.WriteLine("Error saving payments: " & ex.Message)
                        Throw
                    End Try
                End Using
            End Using
        Else
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString
            Dim TotPayed As Decimal = 0
            Dim Pols As String = ""
            Dim Refs As String = ""
            Using con As New SqlConnection(connectionString)
                con.Open()
                Using transaction As SqlTransaction = con.BeginTransaction()
                    Try
                        For Each row As DataRow In InvoicesData.Rows
                            Dim payment As Decimal = CDec(row("Payment"))

                            If payment > 0 Then
                                ' Update the PolicyFile with the payment
                                Dim UpdateQuery As String = "UPDATE PolicyFile SET Inbox = ISNULL(Inbox, 0) + @Payment " _
                                                          & "WHERE OrderNo = @OrderNo AND Branch = @Branch"

                                Using cmd As New SqlCommand(UpdateQuery, con, transaction)
                                    cmd.Parameters.AddWithValue("@Payment", payment)
                                    cmd.Parameters.AddWithValue("@OrderNo", row("OrderNo"))
                                    cmd.Parameters.AddWithValue("@Branch", BranchNo)
                                    cmd.ExecuteNonQuery()
                                End Using
                                Refs += row("OrderNo") + ","
                                Pols += "/" + Right(row("PolNo"), 5)
                                TotPayed += payment
                            End If
                        Next
                        'UPDATE OrderNo's that registered in MainJournal
                        For Each row As DataRow In InvoicesData.Rows
                            Dim payment As Decimal = CDec(row("Payment"))
                            If payment > 0 Then
                                If row("Status") = "مسدد" Then
                                    ExecConn("Update MainJournal Set MoveRef=REPLACE(MoveRef, '" & (row("OrderNo")) & "','') where MoveRef like '%" & (row("OrderNo") + ",") & "%' OR MoveRef like '%" & row("OrderNo") & "%'", Conn)
                                End If
                            End If
                        Next
                        ExecConn("Update MainJournal Set MoveRef=REPLACE(MoveRef, ',,',',') ", Conn)

                        Pols = "وذلك مقابل سداد الوثائق" + Pols + "/" + PayTyp.Text
                        Parm = Array.CreateInstance(GetType(SqlParameter), 1)
                        SetPm("@BR", DbType.String, BranchNo, Parm, 0)
                        RNo = CallSP("RecetNo", Conn, Parm)

                        ExecConn("Insert into ACCMOVE(DocNo,DocDat,CustName,PayMent,Amount,ForW, " _
                                    & "EndNo,LoadNo,Tp,Branch,AccNo,Bank,Cur,Node,PayTp,UserName,Note) Values('" _
                                    & RNo & "'," _
                                    & "CONVERT(DATETIME, '" & Format(MoveDate.Value, "yyyy-MM-dd") & " 00:00:00', 102),'" _
                                    & Customer.Value & "'," _
                                    & TotPayed & "," _
                                    & TotPayed & ",'" _
                                    & Pols & "'," _
                                    & 0 & "," _
                                    & 0 & ",'" _
                                    & cmbProductType.Text & "','" _
                                    & BranchNo & "','" _
                                    & AccNo.Text & "','" _
                                    & Bank.Text & "','دينار ليبي','" _
                                    & "/" & "'," _
                                    & PayTyp.Value & ",'" _
                                    & Session("User") & "','/')", Conn)

                        Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                        SetPm("@TP", DbType.String, "1", Parm, 0)
                        SetPm("@Year", DbType.String, Right(Year(MoveDate.Value).ToString, 2), Parm, 1)
                        SetPm("@Br", DbType.String, Session("Branch"), Parm, 2)

                        Dim Dly As String = CallSP("LastDailyNo", Conn, Parm)
                        Dim accnt As String = ""
                        Dim Str As String = Pols
                        Select Case PayTyp.Value
                            Case 1
                                accnt = GetBoxAccount(Session("Branch"))
                            Case 2
                                accnt = GetcheqAccount(Session("Branch"))
                            Case 3, 5
                                accnt = AccName.Value
                            Case Else
                                Exit Select
                        End Select

                        'Dim Refsplited = Refs.Split(",")
                        ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] , MoveRef ,[Comment] " _
                                                & ",[Currency] ,[Exchange] ,[CurUser] ,[RecNo],[DailyChk],[Branch])  " _
                                                & " VALUES ('" & Dly & "','" & Format(MoveDate.Value, "yyyy/MM/dd") & "', " _
                                                & 1 & ",'P','" & Refs & "','" & Str & " / RecNo.# " & RNo & "', " & 1 & "," & 1 & "," _
                                                & "'" & Session("User") & "','" & RNo & "'," & 1 & ",'" & Session("Branch") & "')", Conn)

                        ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch]) VALUES ('" & Dly.ToString & "',1, '" & accnt & "'," & TotPayed & ",0,'" & Session("User") & "','" & Session("Branch") & "')", Conn)
                        ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch]) VALUES ('" & Dly.ToString & "',1, '" & cmbCustomer.Value & "',0," & TotPayed * -1 & ",'" & Session("User") & "','" & Session("Branch") & "')", Conn)

                        ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" + Dly.ToString + "&Sys=1")
                        transaction.Commit()
                        'Debug.WriteLine("Payments saved successfully to database")
                    Catch ex As Exception
                        transaction.Rollback()
                        'Debug.WriteLine("Error saving payments: " & ex.Message)
                        Throw
                    End Try
                End Using
            End Using
        End If

    End Sub

    Protected Sub ASPxGridView1_DataBinding(sender As Object, e As EventArgs)
        'This is where the grid gets its data
        'Debug.WriteLine($"Grid binding with {InvoicesData.Rows.Count} rows")
        ASPxGridView1.DataSource = InvoicesData
    End Sub

    Protected Sub ASPxGridView1_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        ' Simply rebind the grid to the current data
        ASPxGridView1.DataSource = InvoicesData
        'ASPxGridView1.DataBind()
        'Debug.WriteLine("Grid custom callback executed")
    End Sub

End Class