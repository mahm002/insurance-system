' 1. أولاً، نقوم بتعريف فئات البيانات المطلوبة
Imports System
Imports System.Collections.Generic

Public Class PolicyReceipt
    Public Property ReceiptNumber As String
    Public Property PolicyNumber As String
    Public Property CustomerName As String
    Public Property CustomerNo As String
    Public Property TotalDue As Decimal
    Public Property Payments As List(Of PaymentDetail)
    Public Property MoveDate As DateTime
    Public Property Notes As String
    Public Property CreatedBy As String
    Public Property BranchCode As String
    ' إضافة خاصية جديدة للمستندات
    Public Property Documents As List(Of DocumentDetail)
End Class

Public Class PaymentDetail
    Public Property PaymentType As String
    Public Property Amount As Decimal
    Public Property Details As String
    Public Property AccountNo As String
End Class

Public Class DocumentDetail
    Public Property PolNo As String
    Public Property EndNo As Integer
    Public Property LoadNo As Integer
    Public Property TOTPRM As Decimal
    Public Property PaidAmount As Decimal
    Public Property TpName As String
End Class