Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class CreditPayments
    Inherits Page
    Private TotalRemain As Decimal = 0
    Private TotalTOTPRM As Decimal = 0

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        'Dim callbackPanel As ASPxCallbackPanel = TryCast(sender, ASPxCallbackPanel)
        ' Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

        GridData.DataSourceID = "MoveDS"
        GridData.DataBind()
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase)
        If AccName.Value = "/" Then
            Exit Sub
        Else
            Dim AccBlnc As New DataSet
            Dim dbadapterBlnc = New SqlDataAdapter(String.Format("select Sum(Cr)+Sum(Dr) As Balance from Journal where AccountNo='{0}'", AccName.Value), Conn)
            dbadapterBlnc.Fill(AccBlnc)
            If AccBlnc.Tables(0).Rows.Count = 0 Then
                Balance.Value = 0
            Else
                Balance.Value = AccBlnc.Tables(0).Rows(0)("Balance")
            End If
        End If

        Select Case e.Parameter

            Case Else
                Exit Select
        End Select
    End Sub

    Protected Sub Sys_ValueChanged(sender As Object, e As EventArgs)

        GridData.DataBind()
    End Sub

    Protected Sub PayTyp_ValueChanged(sender As Object, e As EventArgs)

        GridData.DataBind()
    End Sub

    Protected Sub AccName_ValueChanged(sender As Object, e As EventArgs)

        GridData.DataBind()
    End Sub

    Protected Sub Method_ValueChanged(sender As Object, e As EventArgs)

        GridData.DataBind()
    End Sub

    'Protected Sub Calculated_DataBound(sender As Object, e As EventArgs)
    '    Dim label As ASPxLabel = DirectCast(sender, ASPxLabel)
    '    Dim container As GridViewDataItemTemplateContainer = DirectCast(label.NamingContainer, GridViewDataItemTemplateContainer)
    '    Dim dataItem As Object = container.Grid.GetRow(container.VisibleIndex)

    '    ' Get column values
    '    Dim remain As Decimal = GetDecimalValue(dataItem, "Remain")
    '    Dim totPrm As Decimal = GetDecimalValue(dataItem, "TOTPRM")
    '    Dim inBox As Decimal = GetDecimalValue(dataItem, "InBox")

    '    ' Check if we're in payment distribution mode
    '    If Not IsNothing(Session("PaymentAmount")) AndAlso Convert.ToDecimal(Session("PaymentAmount")) > 0 Then
    '        Dim paymentAmount As Decimal = Convert.ToDecimal(Session("PaymentAmount"))
    '        Dim paymentMethod As Integer = Convert.ToInt32(Session("PaymentMethod"))

    '        Select Case Method.Value
    '            Case 1 ' Priority payment (oldest invoices first)
    '                If remain > 0 Then
    '                    If paymentAmount >= remain Then
    '                        ' Full payment for this invoice
    '                        label.Text = remain.ToString("n3")
    '                        Session("PaymentAmount") = paymentAmount - remain
    '                    Else
    '                        ' Partial payment for this invoice
    '                        label.Text = paymentAmount.ToString("n3")
    '                        Session("PaymentAmount") = 0
    '                    End If
    '                Else
    '                    label.Text = "0"
    '                End If

    '            Case 2 ' Proportional distribution
    '                If totPrm > 0 AndAlso remain > 0 Then
    '                    Dim totalRemaining As Decimal = GetTotalRemaining()
    '                    If totalRemaining > 0 Then
    '                        Dim proportionalAmount As Decimal = (remain / totalRemaining) * paymentAmount
    '                        If proportionalAmount > remain Then
    '                            proportionalAmount = remain
    '                        End If
    '                        label.Text = proportionalAmount.ToString("n3")
    '                    Else
    '                        label.Text = "0"
    '                    End If
    '                Else
    '                    label.Text = "0"
    '                End If

    '            Case Else
    '                label.Text = "0"
    '        End Select

    '    Else
    '        ' Normal percentage calculation
    '        Dim result As Decimal = 0
    '        If totPrm > 0 Then
    '            result = (Math.Abs(remain) / totPrm) * 100
    '        End If

    '        If inBox = 0 Then
    '            result = 100
    '        ElseIf inBox >= totPrm Then
    '            result = 0
    '        Else
    '            result = ((totPrm - inBox) / totPrm) * 100
    '        End If

    '        label.Text = String.Format("{0:n2}%", result)
    '    End If
    'End Sub

    ' Helper method to get total remaining amount across all rows

    ' Helper method to safely get decimal values

    'Protected Sub Calculated_DataBound(sender As Object, e As EventArgs)
    '    Dim label As ASPxLabel = DirectCast(sender, ASPxLabel)
    '    Dim container As GridViewDataItemTemplateContainer = DirectCast(label.NamingContainer, GridViewDataItemTemplateContainer)
    '    Dim dataItem As Object = container.Grid.GetRow(container.VisibleIndex)

    '    ' Get multiple column values
    '    Dim remain As Decimal = GetDecimalValue(dataItem, "Remain")
    '    Dim totPrm As Decimal = GetDecimalValue(dataItem, "TOTPRM")
    '    Dim inBox As Decimal = GetDecimalValue(dataItem, "InBox")
    '    ' Your custom calculation logic
    '    Dim result As Decimal
    '    If Not IsNothing(Method.Value) And Payed.Value <> 0 And Payed.Value > 0 Then
    '        Select Case Method.Value
    '            Case 1 ' Priority
    '                'And Session("PaymentAmount") <> 1
    '                If HiddenPayments.Value = Payed.Value Then
    '                    label.Text = 0
    '                Else
    '                    If (Payed.Value - HiddenPayments.Value) < (totPrm - inBox) Then
    '                        label.Text = HiddenPayments.Value
    '                        HiddenPayments.Value = Payed.Value ' IIf(HiddenPayments.Value = Payed.Value, Payed.Value, HiddenPayments.Value) - (totPrm - inBox)
    '                        'Session("PaymentAmount") = 1
    '                    Else
    '                        label.Text = (totPrm - inBox)
    '                        HiddenPayments.Value += (totPrm - inBox)
    '                    End If
    '                End If

    '            Case 2  ' deviding value

    '                '                For Each selectedItem As Object In selectItems
    '                '                    Dim TlabelV As ASPxLabel = CType(FindControlRecursive(sender, "Calculated"), ASPxLabel)
    '                '                    TlabelV.Value = selectItems(0)(5)
    '                '                Next
    '                '            Case Else
    '                '                Dim TlabelV As ASPxLabel = CType(FindControlRecursive(sender, "Calculated"), ASPxLabel)
    '                '                TlabelV.Value = 0
    '                'label.Text = 0
    '        End Select
    '    Else
    '        'If totPrm > 0 Then
    '        '    result = (Math.Abs(remain) / totPrm) * 100
    '        'End If

    '        '' Example: Different calculation based on conditions
    '        'If inBox = 0 Then
    '        '    result = 100 ' 100% remaining if nothing paid
    '        'ElseIf inBox >= totPrm Then
    '        '    result = 0 ' 0% remaining if fully paid
    '        'Else
    '        '    result = ((totPrm - inBox) / totPrm) * 100
    '        'End If

    '        'label.Text = String.Format("{0:n2}%", result)
    '    End If
    '    ' Example: Calculate percentage of remaining amount

    'End Sub

    ' Helper method to safely get decimal values
    Private Function GetDecimalValue(dataItem As Object, fieldName As String) As Decimal
        Dim value As Object = DataBinder.Eval(dataItem, fieldName)
        If value IsNot Nothing AndAlso value IsNot DBNull.Value Then
            Return Convert.ToDecimal(value)
        End If
        Return 0
    End Function

    Protected Sub Calculated_DataBound(sender As Object, e As EventArgs)
        '    'Dim TlabelV As ASPxLabel = CType(FindControlRecursive(sender, "Calculated"), ASPxLabel)
        '    Dim grid = GridData

        '------------------------------

        Dim TempPayed
        Dim label As ASPxLabel = DirectCast(sender, ASPxLabel)
        Dim container As GridViewDataItemTemplateContainer = DirectCast(label.NamingContainer, GridViewDataItemTemplateContainer)
        Dim dataItem As Object = container.Grid.GetRow(container.VisibleIndex)

        ' Get multiple column values
        Dim remain As Decimal = GetDecimalValue(dataItem, "Remain")
        Dim totPrm As Decimal = GetDecimalValue(dataItem, "TOTPRM")
        Dim inBox As Decimal = GetDecimalValue(dataItem, "InBox")
        '    grid.Selection.SelectAll()

        '    Dim selectItems As List(Of Object) = grid.GetSelectedFieldValues("PolNo", "EndNo", "LoadNo", "TOTPRM", "OrderNo", "Remain", "InBox", "IssuDate")
        '    ' ORDER No selectItems(0)(4)
        '    ' Total Prm selectItems(0)(3)
        '    ' Remain selectItems(0)(5)
        '    ' Payed (Inbox) selectItems(0)(6)

        If Not IsNothing(Method.Value) And Payed.Value <> 0 Then
            Select Case Method.Value
                Case 1 ' Priority

                    'For i As Integer = 0 To selectItems.Count - 1
                    ' Dim TlabelV As ASPxLabel = CType(FindControlRecursive(sender, "Calculated"), ASPxLabel)
                    'PVal = (IIf(i = 0, Payed.Value, PVal) - (selectItems(i)(3) - selectItems(i)(6)))
                    'TlabelV.Value = IIf(PVal > 0, selectItems(i)(5), 0)
                    label.Value = totPrm - inBox
                    TempPayed = Payed.Value - label.Value
                    'Next
                        'For Each selectedItem As Object In selectItems
                        '    'FindControlRecursive(grid, "Calculated")
                        '    labelV.Value = 55
                        'Next
                Case 2  ' deviding value

                    'For Each selectedItem As Object In selectItems
                    '    Dim TlabelV As ASPxLabel = CType(FindControlRecursive(sender, "Calculated"), ASPxLabel)
                    '    TlabelV.Value = selectItems(0)(5)
                    'Next
                Case Else
                    Dim TlabelV As ASPxLabel = CType(FindControlRecursive(sender, "Calculated"), ASPxLabel)
                    TlabelV.Value = 0
            End Select
        End If
    End Sub

    Private Function GetTotalPaid() As Decimal

        Dim total As Decimal = 0
        For i As Integer = 0 To GridData.VisibleRowCount - 1
            Dim dataItem As Object = GridData.GetRow(i)
            Dim Paid As Decimal = GetDecimalValue(dataItem, "Calculated")
            If Paid > 0 Then
                total += Paid
            End If
        Next
        Return total
    End Function

    Protected Sub GridData_DataBound(sender As Object, e As EventArgs)
        Dim col As GridViewDataTextColumn = CType(GridData.Columns("التوزيع"), GridViewDataTextColumn)

        If col IsNot Nothing Then
            Dim footerTemplateContainer = TryCast(col.FooterTemplate, Control)
            If footerTemplateContainer IsNot Nothing Then
                Dim lblTotalPercent As ASPxLabel = CType(footerTemplateContainer.FindControl("lblTotalPercent"), ASPxLabel)
                If lblTotalPercent IsNot Nothing Then
                    If TotalTOTPRM <> 0 Then
                        Dim overallPercent As Decimal = (TotalRemain / TotalTOTPRM) * 100
                        lblTotalPercent.Text = "الإجمالي: " & overallPercent.ToString("n3") & " %"
                    Else
                        lblTotalPercent.Text = "الإجمالي: 0.000 %"
                    End If
                End If
            End If
        End If
        ' Reset accumulators after use (important if grid rebinds)
        TotalRemain = 0
        TotalTOTPRM = 0
    End Sub

    'Protected Sub GridData_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles GridData.CustomCallback
    '    'Dim grid = TryCast(sender, ASPxGridView)
    ' FindControlRecursive(grid, "Calculated")
    '    'grid.Selection.SelectAll()

    '    'Dim selectItems As List(Of Object) = grid.GetSelectedFieldValues("PolNo", "EndNo", "LoadNo", "TOTPRM", "OrderNo")
    '    'For Each selectedItem As Object In selectItems
    '    '    FindControlRecursive(grid, "Calculated")
    '    '    Dim TlabelV As ASPxLabel = CType(FindControlRecursive(grid, "Calculated"), ASPxLabel)
    '    'Next

    '    'grid.Selection.SelectRow(visibleIndex)
    '    'End If

    '    'GridData.DataBind()
    'End Sub
End Class