Imports System.Data.SqlClient
Imports System.Configuration
Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports System

' ✅ ضع اسم المشروع كـ Namespace
'Namespace BusinessLogic
Public Class PaymentProcessor

        Public Shared Function ProcessMultiplePayments(
        payments As List(Of Dictionary(Of String, Object)),
        totalPaid As Decimal,
        totalDue As Decimal,
        receiptType As String,
        branchCode As String,
        currentUser As String,
        moveDate As DateTime,
        note As String,
        customerName As String,
        policyData As Dictionary(Of String, String),
        connectionString As String
    ) As String

            Using conn As New SqlConnection(connectionString)
                conn.Open()
                Using transaction As SqlTransaction = conn.BeginTransaction()
                    Try
                        ' 1. التحقق من التوازن
                        Dim tolerance As Decimal = 0.001D
                        If Math.Abs(totalPaid - totalDue) > tolerance Then
                            Throw New Exception($"إجمالي المدفوعات ({totalPaid:N3}) لا يساوي المبلغ المستحق ({totalDue:N3})")
                        End If

                        ' 2. الحصول على رقم الإيصال
                        Dim receiptNumber As String = GetReceiptNumber(conn, transaction, branchCode)

                        ' 3. حفظ تفاصيل الدفع
                        SavePaymentDetails(conn, transaction, receiptNumber, payments, receiptType, branchCode,
                                      currentUser, moveDate, note, customerName, policyData)

                        ' 4. تحديث حالة الوثائق (إذا كانت وثيقة تأمين)
                        If receiptType <> "مقبوضات أخرى" AndAlso policyData IsNot Nothing Then
                            UpdateDocumentStatus(conn, transaction, policyData, totalPaid, branchCode)
                        End If

                        ' 5. إنشاء إدخالات اليومية
                        CreateJournalEntries(conn, transaction, receiptNumber, payments, totalPaid, receiptType,
                                        branchCode, currentUser, note, policyData)

                        ' 6. تسجيل النشاط
                        LogActivity(conn, transaction, receiptNumber, payments.Count, branchCode, currentUser)

                        ' 7. تأكيد
                        transaction.Commit()

                        Return receiptNumber

                    Catch ex As Exception
                        Try
                            transaction.Rollback()
                        Catch
                        End Try
                        Throw
                    End Try
                End Using
            End Using
        End Function

        Private Shared Function GetReceiptNumber(conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            Using cmd As New SqlCommand("RecetNo", conn, trans)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@BR", branch)
                Dim result As Object = cmd.ExecuteScalar()
                Return If(result IsNot Nothing, result.ToString(), "")
            End Using
        End Function

        Private Shared Sub SavePaymentDetails(
        conn As SqlConnection,
        trans As SqlTransaction,
        receiptNumber As String,
        payments As List(Of Dictionary(Of String, Object)),
        receiptType As String,
        branchCode As String,
        currentUser As String,
        moveDate As DateTime,
        note As String,
        customerName As String,
        policyData As Dictionary(Of String, String)
    )
            For i As Integer = 0 To payments.Count - 1
                Dim payment As Dictionary(Of String, Object) = payments(i)
                Dim subReceiptNo As String = $"{receiptNumber}-{(i + 1).ToString("D2")}"
                Dim accountInfo As String() = GetAccountInfoForPayment(payment, conn, trans, branchCode)

                Using cmd As New SqlCommand("
                    INSERT INTO ACCMOVE(DocNo, SubDocNo, DocDat, CustName, PayMent, Amount, ForW, 
                    EndNo, LoadNo, Tp, Branch, AccNo, Bank, Cur, Node, PayTp, UserName, Note, PaymentDetail, AccountUsed) 
                    VALUES(@DocNo, @SubDocNo, @DocDat, @CustName, @PayMent, @Amount, @ForW, 
                    @EndNo, @LoadNo, @Tp, @Branch, @AccNo, @Bank, @Cur, @Node, @PayTp, @UserName, @Note, @PaymentDetail, @AccountUsed)
                ", conn, trans)
                    cmd.Parameters.AddWithValue("@DocNo", receiptNumber)
                    cmd.Parameters.AddWithValue("@SubDocNo", subReceiptNo)
                    cmd.Parameters.AddWithValue("@DocDat", moveDate)
                    cmd.Parameters.AddWithValue("@CustName", customerName)
                    cmd.Parameters.AddWithValue("@PayMent", Convert.ToDecimal(payment("Amount")))
                    cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(payment("Amount")))
                    cmd.Parameters.AddWithValue("@ForW", If(receiptType = "مقبوضات أخرى", "أخـرى", ""))
                    cmd.Parameters.AddWithValue("@EndNo", If(receiptType = "مقبوضات أخرى", 0, If(policyData IsNot Nothing, CInt(policyData("EndNo")), 1)))
                    cmd.Parameters.AddWithValue("@LoadNo", If(receiptType = "مقبوضات أخرى", 0, If(policyData IsNot Nothing, CInt(policyData("LoadNo")), 1)))
                    cmd.Parameters.AddWithValue("@Tp", If(receiptType = "مقبوضات أخرى", "مقبوضات أخرى", "وثيقة تأمين"))
                    cmd.Parameters.AddWithValue("@Branch", branchCode)
                    cmd.Parameters.AddWithValue("@AccNo", accountInfo(0))
                    cmd.Parameters.AddWithValue("@Bank", accountInfo(1))
                    cmd.Parameters.AddWithValue("@Cur", "دينار ليبي")
                    cmd.Parameters.AddWithValue("@Node", "/")
                    cmd.Parameters.AddWithValue("@PayTp", payment("Type").ToString())
                    cmd.Parameters.AddWithValue("@UserName", currentUser)
                    cmd.Parameters.AddWithValue("@Note", note)
                    cmd.Parameters.AddWithValue("@PaymentDetail", $"{payment("TypeName")} - {payment("Details")}")
                    cmd.Parameters.AddWithValue("@AccountUsed", payment("Account").ToString())
                    cmd.ExecuteNonQuery()
                End Using
            Next
        End Sub

        Private Shared Function GetAccountInfoForPayment(
        payment As Dictionary(Of String, Object),
        conn As SqlConnection,
        trans As SqlTransaction,
        branchCode As String
    ) As String()
            Dim accountNo As String
            Dim bankInfo As String

            Try
                Select Case payment("Type").ToString()
                    Case "1" ' نقداً
                        accountNo = GetCashAccount(conn, trans, branchCode)
                        bankInfo = "/"
                    Case "2" ' بصك
                        accountNo = GetCheckAccount(conn, trans, branchCode)
                        bankInfo = If(String.IsNullOrEmpty(payment("Details").ToString()), "/", payment("Details").ToString())
                    Case "3", "5" ' بإشعار, بطاقة مصرفية
                        If String.IsNullOrEmpty(payment("Account").ToString()) Then
                            accountNo = GetCashAccount(conn, trans, branchCode)
                        Else
                            accountNo = payment("Account").ToString()
                        End If
                        bankInfo = If(String.IsNullOrEmpty(payment("Details").ToString()), "/", payment("Details").ToString())
                    Case "4", "6" ' على الحساب, تحت التحصيل
                        If String.IsNullOrEmpty(payment("Account").ToString()) Then
                            accountNo = GetDefaultAccountForType(payment("Type").ToString(), conn, trans, branchCode)
                        Else
                            accountNo = payment("Account").ToString()
                        End If
                        bankInfo = "/"
                    Case Else
                        accountNo = GetCashAccount(conn, trans, branchCode)
                        bankInfo = "/"
                End Select

            Catch ex As Exception
                ' في حال الخطأ، استخدم الحساب النقدي كاحتياطي
                accountNo = GetCashAccount(conn, trans, branchCode)
                bankInfo = "/"
            End Try

            Return {accountNo, bankInfo}
        End Function

        Private Shared Function GetCashAccount(conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            Using cmd As New SqlCommand("
                SELECT TOP 1 AccountNo 
                FROM Accounts 
                WHERE Branch = @Branch 
                AND AccountName LIKE '%صندوق%' 
                AND Level >= 4
                ORDER BY AccountNo
            ", conn, trans)
                cmd.Parameters.AddWithValue("@Branch", branch)
                Dim result As Object = cmd.ExecuteScalar()
                Return If(result IsNot Nothing, result.ToString(), "1.1.1.1.1")
            End Using
        End Function

        Private Shared Function GetCheckAccount(conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            Using cmd As New SqlCommand("
                SELECT TOP 1 AccountNo 
                FROM Accounts 
                WHERE Branch = @Branch 
                AND AccountName LIKE '%صك%' 
                AND Level >= 4
                ORDER BY AccountNo
            ", conn, trans)
                cmd.Parameters.AddWithValue("@Branch", branch)
                Dim result As Object = cmd.ExecuteScalar()
                Return If(result IsNot Nothing, result.ToString(), "1.1.1.2.1")
            End Using
        End Function

        Private Shared Function GetDefaultAccountForType(paymentType As String, conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            Select Case paymentType
                Case "4" ' على الحساب
                    Return "1.1.3.1.1"
                Case "6" ' تحت التحصيل
                    Return "1.1.10.1.1"
                Case Else
                    Return GetCashAccount(conn, trans, branch)
            End Select
        End Function

        Private Shared Sub UpdateDocumentStatus(
        conn As SqlConnection,
        trans As SqlTransaction,
        policyData As Dictionary(Of String, String),
        totalPaid As Decimal,
        branchCode As String
    )
            Using cmd As New SqlCommand("
                UPDATE PolicyFile 
                SET Inbox = Inbox + @PaidAmount, Financed = 1 
                WHERE PolNo = @PolNo AND EndNo = @EndNo AND LoadNo = @LoadNo
                AND Branch = @Branch
            ", conn, trans)
                cmd.Parameters.AddWithValue("@PaidAmount", totalPaid)
                cmd.Parameters.AddWithValue("@PolNo", policyData("PolNo"))
                cmd.Parameters.AddWithValue("@EndNo", policyData("EndNo"))
                cmd.Parameters.AddWithValue("@LoadNo", policyData("LoadNo"))
                cmd.Parameters.AddWithValue("@Branch", branchCode)
                cmd.ExecuteNonQuery()
            End Using
        End Sub

        Private Shared Sub CreateJournalEntries(
        conn As SqlConnection,
        trans As SqlTransaction,
        receiptNumber As String,
        payments As List(Of Dictionary(Of String, Object)),
        totalAmount As Decimal,
        receiptType As String,
        branchCode As String,
        currentUser As String,
        note As String,
        policyData As Dictionary(Of String, String)
    )
            ' 1. توليد رقم اليومية
            Dim dailyNumber As String = GetDailyNumber(conn, trans, branchCode)

            ' 2. إدخال رئيسي في اليومية
            Using cmd As New SqlCommand("
                INSERT INTO MainJournal ([DAILYNUM], [DAILYDTE], [DailyTyp], [ANALSNUM], [Comment], 
                [Currency], [Exchange], [CurUser], [RecNo], [DailyChk], [Branch])
                VALUES (@DAILYNUM, @DAILYDTE, @DailyTyp, @ANALSNUM, @Comment, 
                @Currency, @Exchange, @CurUser, @RecNo, @DailyChk, @Branch)
            ", conn, trans)
                cmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                cmd.Parameters.AddWithValue("@DAILYDTE", DateTime.Now)
                cmd.Parameters.AddWithValue("@DailyTyp", 1)
                cmd.Parameters.AddWithValue("@ANALSNUM", If(receiptType = "مقبوضات أخرى", "P", "A"))
                cmd.Parameters.AddWithValue("@Comment", $"سداد متعدد {If(receiptType = "مقبوضات أخرى", "مقبوضات أخرى", $"لل(policy: {policyData?.Item("PolNo")})")} - {note}")
                cmd.Parameters.AddWithValue("@Currency", 1)
                cmd.Parameters.AddWithValue("@Exchange", 1)
                cmd.Parameters.AddWithValue("@CurUser", currentUser)
                cmd.Parameters.AddWithValue("@RecNo", receiptNumber)
                cmd.Parameters.AddWithValue("@DailyChk", 1)
                cmd.Parameters.AddWithValue("@Branch", branchCode)
                cmd.ExecuteNonQuery()
            End Using

            ' 3. إدخالات المدين والدائن
            If receiptType = "مقبوضات أخرى" Then
                CreateOtherReceiptsJournalEntries(conn, trans, dailyNumber, payments, branchCode, currentUser)
            Else
                CreateInsuranceJournalEntries(conn, trans, dailyNumber, payments, totalAmount, branchCode, currentUser, policyData)
            End If
        End Sub

        Private Shared Function GetDailyNumber(conn As SqlConnection, trans As SqlTransaction, branch As String) As String
            Using cmd As New SqlCommand("LastDailyNo", conn, trans)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@TP", "1")
                cmd.Parameters.AddWithValue("@Year", DateTime.Now.Year.ToString().Substring(2))
                cmd.Parameters.AddWithValue("@Br", branch)
                Dim result As Object = cmd.ExecuteScalar()
                Return If(result IsNot Nothing, result.ToString(), "")
            End Using
        End Function

        Private Shared Sub CreateInsuranceJournalEntries(
        conn As SqlConnection,
        trans As SqlTransaction,
        dailyNumber As String,
        payments As List(Of Dictionary(Of String, Object)),
        totalAmount As Decimal,
        branchCode As String,
        currentUser As String,
        policyData As Dictionary(Of String, String)
    )
            Dim receivableAccount As String = GetReceivableAccount(conn, trans, policyData, branchCode)

            ' إدخال مدين (المستحقات)
            Using cmd As New SqlCommand("
                INSERT INTO Journal([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch], [Comment])
                VALUES (@DAILYNUM, @TP, @AccountNo, @Dr, @Cr, @CurUser, @Branch, @Comment)
            ", conn, trans)
                cmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                cmd.Parameters.AddWithValue("@TP", 1)
                cmd.Parameters.AddWithValue("@AccountNo", receivableAccount)
                cmd.Parameters.AddWithValue("@Dr", totalAmount)
                cmd.Parameters.AddWithValue("@Cr", 0)
                cmd.Parameters.AddWithValue("@CurUser", currentUser)
                cmd.Parameters.AddWithValue("@Branch", branchCode)
                cmd.Parameters.AddWithValue("@Comment", "إجمالي السداد المتعدد")
                cmd.ExecuteNonQuery()
            End Using

            ' إدخالات دائن حسب نوع الدفع
            For Each payment In payments
                Dim accountInfo As String() = GetAccountInfoForPayment(payment, conn, trans, branchCode)
                Dim accountNo As String = accountInfo(0)

                Using cmd As New SqlCommand("
                    INSERT INTO Journal([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch], [Comment])
                    VALUES (@DAILYNUM, @TP, @AccountNo, @Dr, @Cr, @CurUser, @Branch, @Comment)
                ", conn, trans)
                    cmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                    cmd.Parameters.AddWithValue("@TP", 1)
                    cmd.Parameters.AddWithValue("@AccountNo", accountNo)
                    cmd.Parameters.AddWithValue("@Dr", 0)
                    cmd.Parameters.AddWithValue("@Cr", Convert.ToDecimal(payment("Amount")))
                    cmd.Parameters.AddWithValue("@CurUser", currentUser)
                    cmd.Parameters.AddWithValue("@Branch", branchCode)
                    cmd.Parameters.AddWithValue("@Comment", payment("TypeName").ToString())
                    cmd.ExecuteNonQuery()
                End Using
            Next
        End Sub

        Private Shared Sub CreateOtherReceiptsJournalEntries(
        conn As SqlConnection,
        trans As SqlTransaction,
        dailyNumber As String,
        payments As List(Of Dictionary(Of String, Object)),
        branchCode As String,
        currentUser As String
    )
            For Each payment In payments
                Dim accountInfo As String() = GetAccountInfoForPayment(payment, conn, trans, branchCode)
                Dim accountNo As String = accountInfo(0)

                Using cmd As New SqlCommand("
                    INSERT INTO Journal([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch], [Comment])
                    VALUES (@DAILYNUM, @TP, @AccountNo, @Dr, @Cr, @CurUser, @Branch, @Comment)
                ", conn, trans)
                    cmd.Parameters.AddWithValue("@DAILYNUM", dailyNumber)
                    cmd.Parameters.AddWithValue("@TP", 1)
                    cmd.Parameters.AddWithValue("@AccountNo", accountNo)
                    cmd.Parameters.AddWithValue("@Dr", Convert.ToDecimal(payment("Amount")))
                    cmd.Parameters.AddWithValue("@Cr", 0)
                    cmd.Parameters.AddWithValue("@CurUser", currentUser)
                    cmd.Parameters.AddWithValue("@Branch", branchCode)
                    cmd.Parameters.AddWithValue("@Comment", payment("TypeName").ToString())
                    cmd.ExecuteNonQuery()
                End Using
            Next
        End Sub

        Private Shared Function GetReceivableAccount(
        conn As SqlConnection,
        trans As SqlTransaction,
        policyData As Dictionary(Of String, String),
        branchCode As String
    ) As String
            ' محاولة جلب الحساب من ملف العميل
            Using cmd As New SqlCommand("
                SELECT AccountNo 
                FROM CustomerFile 
                WHERE CustNo = @CustNo AND Branch = @Branch
            ", conn, trans)
                cmd.Parameters.AddWithValue("@CustNo", policyData("CustNo"))
                cmd.Parameters.AddWithValue("@Branch", branchCode)
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing Then Return result.ToString()
            End Using

            ' إذا فشل، استخدام الحساب الافتراضي
            Using cmd As New SqlCommand("
                SELECT TOP 1 AccountNo 
                FROM Accounts 
                WHERE AccountName LIKE '%مستحقات%' AND Branch = @Branch 
                ORDER BY AccountNo
            ", conn, trans)
                cmd.Parameters.AddWithValue("@Branch", branchCode)
                Dim result As Object = cmd.ExecuteScalar()
                Return If(result IsNot Nothing, result.ToString(), "1.1.3.1.1")
            End Using
        End Function

        Private Shared Sub LogActivity(
        conn As SqlConnection,
        trans As SqlTransaction,
        receiptNumber As String,
        paymentCount As Integer,
        branchCode As String,
        currentUser As String
    )
            Using cmd As New SqlCommand("
                INSERT INTO SystemLog ([ActivityDate], [UserName], [ActivityType], 
                [Description], [BranchCode], [ReferenceNo])
                VALUES (@ActivityDate, @UserName, @ActivityType, 
                @Description, @BranchCode, @ReferenceNo)
            ", conn, trans)
                cmd.Parameters.AddWithValue("@ActivityDate", DateTime.Now)
                cmd.Parameters.AddWithValue("@UserName", currentUser)
                cmd.Parameters.AddWithValue("@ActivityType", "MULTI_PAYMENT")
                cmd.Parameters.AddWithValue("@Description", $"سداد متعدد - عدد الطرق: {paymentCount}")
                cmd.Parameters.AddWithValue("@BranchCode", branchCode)
                cmd.Parameters.AddWithValue("@ReferenceNo", receiptNumber)
                cmd.ExecuteNonQuery()
            End Using
        End Sub

    End Class

'End Namespace