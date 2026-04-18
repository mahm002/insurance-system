Imports DevExpress.Web
Imports System.Data.SqlClient
Imports System.Collections.Generic
Public Class PaymentDetail
    Public Property TempID As Integer
    Public Property PaymentType As String
    Public Property Amount As Decimal
    Public Property ReferenceNo As String
    Public Property BankName As String
    Public Property ChequeNo As String
    Public Property CardType As String
    Public Property Note As String
End Class
Partial Class MPaymentEntry
    Inherits Page

    Private ReadOnly Property PaymentDetails As List(Of PaymentDetail)
        Get
            If Session("PaymentDetails") Is Nothing Then
                Session("PaymentDetails") = New List(Of PaymentDetail)()
            End If
            Return CType(Session("PaymentDetails"), List(Of PaymentDetail))
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session.Remove("PaymentDetails")
            dtDocDate.Date = DateTime.Now
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        gridPaymentDetails.DataSource = PaymentDetails
        gridPaymentDetails.DataBind()
        CalculateTotal()
    End Sub

    Protected Sub gridPaymentDetails_RowInserting(sender As Object, e As DevExpress.Web.Data.ASPxDataInsertingEventArgs)
        Dim newPayment As New PaymentDetail() With {
            .TempID = If(PaymentDetails.Count > 0, PaymentDetails.Max(Function(p) p.TempID) + 1, 1),
            .PaymentType = e.NewValues("PaymentType").ToString(),
            .Amount = Convert.ToDecimal(e.NewValues("Amount")),
            .ReferenceNo = If(e.NewValues("ReferenceNo"), ""),
            .BankName = If(e.NewValues("BankName"), ""),
            .ChequeNo = If(e.NewValues("ChequeNo"), ""),
            .CardType = If(e.NewValues("CardType"), ""),
            .Note = If(e.NewValues("Note"), "")
        }

        PaymentDetails.Add(newPayment)
        e.Cancel = True
        gridPaymentDetails.CancelEdit()
        BindGrid()
    End Sub

    Protected Sub gridPaymentDetails_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs)
        Dim tempID As Integer = Convert.ToInt32(e.Keys("TempID"))
        Dim payment As PaymentDetail = PaymentDetails.FirstOrDefault(Function(p) p.TempID = tempID)

        If payment IsNot Nothing Then
            payment.PaymentType = e.NewValues("PaymentType").ToString()
            payment.Amount = Convert.ToDecimal(e.NewValues("Amount"))
            payment.ReferenceNo = If(e.NewValues("ReferenceNo"), "")
            payment.BankName = If(e.NewValues("BankName"), "")
            payment.ChequeNo = If(e.NewValues("ChequeNo"), "")
            payment.CardType = If(e.NewValues("CardType"), "")
            payment.Note = If(e.NewValues("Note"), "")
        End If

        e.Cancel = True
        gridPaymentDetails.CancelEdit()
        BindGrid()
    End Sub

    Protected Sub gridPaymentDetails_RowDeleting(sender As Object, e As DevExpress.Web.Data.ASPxDataDeletingEventArgs)
        Dim tempID As Integer = Convert.ToInt32(e.Keys("TempID"))
        Dim payment As PaymentDetail = PaymentDetails.FirstOrDefault(Function(p) p.TempID = tempID)

        If payment IsNot Nothing Then
            PaymentDetails.Remove(payment)
        End If

        e.Cancel = True
        BindGrid()
    End Sub

    Protected Sub gridPaymentDetails_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        BindGrid()
    End Sub

    Private Sub CalculateTotal()
        Dim total As Decimal = PaymentDetails.Sum(Function(p) p.Amount)
        txtTotalAmount.Text = total.ToString("C2")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs)
        If ValidateData() Then
            SavePayment()
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
        ClearForm()
    End Sub

    Private Function ValidateData() As Boolean
        If String.IsNullOrEmpty(txtDocNo.Text) Then
            ShowErrorMessage("Please enter document number.")
            Return False
        End If

        If String.IsNullOrEmpty(txtCustomerName.Text) Then
            ShowErrorMessage("Please enter customer name.")
            Return False
        End If

        If PaymentDetails.Count = 0 Then
            ShowErrorMessage("Please add at least one payment method.")
            Return False
        End If

        For Each payment In PaymentDetails
            If String.IsNullOrEmpty(payment.PaymentType) Then
                ShowErrorMessage("Please select payment type for all payments.")
                Return False
            End If

            If payment.Amount <= 0 Then
                ShowErrorMessage("Please enter valid amount for all payments.")
                Return False
            End If
        Next

        Return True
    End Function

    Private Sub SavePayment()
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ConnectionString

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using transaction As SqlTransaction = connection.BeginTransaction()
                Try
                    ' Insert main ACCMOVE record
                    Dim accMoveSql As String = "INSERT INTO [ACCMOVE]  (DocNo, DocDat, CustName, PayMent, Amount, ForW, AccNo, Bank, Note, EndNo, LoadNo, Tp, Node, Cur, PayTp, UserName, Branch, Sysdate)
                                                VALUES 
                                                (@DocNo, @DocDat, @CustName, @PayMent, @Amount, @ForW, @AccNo, @Bank, @Note, @EndNo, @LoadNo, @Tp, @Node, @Cur, @PayTp, @UserName, @Branch, @Sysdate);
                                               SELECT SCOPE_IDENTITY();"

                    Dim accMoveSerNo As Long
                    Using cmd As New SqlCommand(accMoveSql, connection, transaction)
                        cmd.Parameters.AddWithValue("@DocNo", txtDocNo.Text)
                        cmd.Parameters.AddWithValue("@DocDat", dtDocDate.Date)
                        cmd.Parameters.AddWithValue("@CustName", txtCustomerName.Text)
                        cmd.Parameters.AddWithValue("@PayMent", PaymentDetails.Sum(Function(p) p.Amount))
                        cmd.Parameters.AddWithValue("@Amount", PaymentDetails.Sum(Function(p) p.Amount))
                        cmd.Parameters.AddWithValue("@ForW", DBNull.Value)
                        cmd.Parameters.AddWithValue("@AccNo", DBNull.Value)
                        cmd.Parameters.AddWithValue("@Bank", DBNull.Value)
                        cmd.Parameters.AddWithValue("@Note", If(String.IsNullOrEmpty(txtNote.Text), DBNull.Value, txtNote.Text))
                        cmd.Parameters.AddWithValue("@EndNo", DBNull.Value)
                        cmd.Parameters.AddWithValue("@LoadNo", DBNull.Value)
                        cmd.Parameters.AddWithValue("@Tp", DBNull.Value)
                        cmd.Parameters.AddWithValue("@Node", "N")
                        cmd.Parameters.AddWithValue("@Cur", "USD")
                        cmd.Parameters.AddWithValue("@PayTp", 1)
                        cmd.Parameters.AddWithValue("@UserName", Context.User.Identity.Name)
                        cmd.Parameters.AddWithValue("@Branch", DBNull.Value)
                        cmd.Parameters.AddWithValue("@Sysdate", DateTime.Now)

                        accMoveSerNo = Convert.ToInt64(cmd.ExecuteScalar())
                    End Using

                    ' Insert payment details
                    For Each payment In PaymentDetails
                        Dim paymentSql As String = "INSERT INTO [PaymentDetails] 
(AccMoveSerNo, PaymentType, PaymentAmount, PaymentDate, ReferenceNo, BankName, ChequeNo, CardType, Note)
VALUES 
(@AccMoveSerNo, @PaymentType, @PaymentAmount, @PaymentDate, @ReferenceNo, @BankName, @ChequeNo, @CardType, @Note)"

                        Using paymentCmd As New SqlCommand(paymentSql, connection, transaction)
                            paymentCmd.Parameters.AddWithValue("@AccMoveSerNo", accMoveSerNo)
                            paymentCmd.Parameters.AddWithValue("@PaymentType", payment.PaymentType)
                            paymentCmd.Parameters.AddWithValue("@PaymentAmount", payment.Amount)
                            paymentCmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now)
                            paymentCmd.Parameters.AddWithValue("@ReferenceNo", If(String.IsNullOrEmpty(payment.ReferenceNo), DBNull.Value, payment.ReferenceNo))
                            paymentCmd.Parameters.AddWithValue("@BankName", If(String.IsNullOrEmpty(payment.BankName), DBNull.Value, payment.BankName))
                            paymentCmd.Parameters.AddWithValue("@ChequeNo", If(String.IsNullOrEmpty(payment.ChequeNo), DBNull.Value, payment.ChequeNo))
                            paymentCmd.Parameters.AddWithValue("@CardType", If(String.IsNullOrEmpty(payment.CardType), DBNull.Value, payment.CardType))
                            paymentCmd.Parameters.AddWithValue("@Note", If(String.IsNullOrEmpty(payment.Note), DBNull.Value, payment.Note))

                            paymentCmd.ExecuteNonQuery()
                        End Using
                    Next

                    transaction.Commit()
                    ShowSuccessMessage("Payment saved successfully!")
                    ClearForm()

                Catch ex As Exception
                    transaction.Rollback()
                    ShowErrorMessage($"Error saving payment: {ex.Message}")
                End Try
            End Using
        End Using
    End Sub

    Private Sub ClearForm()
        Session.Remove("PaymentDetails")
        txtDocNo.Text = ""
        txtCustomerName.Text = ""
        txtTotalAmount.Text = "0.00"
        txtNote.Text = ""
        dtDocDate.Date = DateTime.Now
        BindGrid()
    End Sub

    Private Sub ShowErrorMessage(message As String)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "errorAlert", $"alert('{message}');", True)
    End Sub

    Private Sub ShowSuccessMessage(message As String)
        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "successAlert", $"alert('{message}');", True)
    End Sub
End Class