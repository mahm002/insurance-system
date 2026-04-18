Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class EngRisks
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'DataGrid.DataBind()
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        'Dim myList = CType(Session("UserInfo"), List(Of String))
        'Dim callback = CType(sender, ASPxCallback)
        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
        If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) And Not IsSameMonth(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
            Exit Sub
        Else
            Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
            Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)
            'Dim diff As Double = DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverTo")))) ' / DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), DateAdd(DateInterval.Year, 1, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))))), "0.000"))
            Select Case e.Parameter

                Case "Calc"

                    If isValid Then

                        If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), CDbl(Premium.Value))
                        Else
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), CDbl(Premium.Value))
                        End If

                        MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                         , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                         , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                         , False)
                        If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" Then
                            Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                            SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                            SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))

                            'insert new data requested
                            ExecConn(InsertData(Callback,
                                    Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) _
                                    + "; " + InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")), Conn)
                        Else
                            'Update data requested

                            ExecConn(UpdateData(Callback,
                                    Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) + "; " +
                                    UpdatePolicyData(PolicyControl,
                                    Request("Sys"), Session("Branch"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)
                        End If
                    Else

                    End If
                    GoTo PP
                    'Case "ExRate"
                    '    SetDxtxtValue(FindControlRecursive(PolicyControl, "ExcRate"), GetExrate(GetComboValue(FindControlRecursive(PolicyControl, "Currency")), GetComboText(FindControlRecursive(PolicyControl, "Currency"))))

                Case "LocalExRate"
                    ''SetDxtxtValue(ExcRate, GetExrate(Currency.SelectedItem.Value, Currency.SelectedItem.Text))
                    'GoTo 1
                Case "SumInsCalc"
                    ''SumIns.Text = Format(ExcRate.Value * DocVal.Value + (((Margin.Value / 100) * DocVal.Value) * ExcRate.Value), "0.000")
                    If isValid Then
                        '' GoTo 10
                    Else
                    End If

                Case "Endorsment"
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "EndNo"), GetLastEnd(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), 0) + 1)
                    FindControlRecursive(PolicyControl, "Endorsment").Visible = False
                Case "Dist"
                    Session("RtnPg") = GetEditForm(Request("Sys")) + "?Sys=" + Request("Sys") + "&OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))

                    ASPxWebControl.RedirectOnCallback("../../Reins/DistPolicy.aspx?OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) + "&Branch=" + GetBranchbyOrderNo(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))) + "&Sys=" + Request("sys") + "&PolNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")))

                Case "ExRate"

                    SetDxtxtValue(FindControlRecursive(PolicyControl, "ExcRate"), GetExrate(GetComboValue(FindControlRecursive(PolicyControl, "Currency")), GetComboText(FindControlRecursive(PolicyControl, "Currency"))))
                Case "PayType"

PP:                 If GetComboValue(FindControlRecursive(PolicyControl, "PayType")) = 2 And GethiddenField(FindControlRecursive(PolicyControl, "CustNo")) <> 0 Then
                        Dim AccountNo As New DataSet

                        Dim dbadapter = New SqlDataAdapter("select AccNo from Customerfile where CustNo=" & GethiddenField(FindControlRecursive(PolicyControl, "CustNo")), Conn)
                        dbadapter.Fill(AccountNo)
                        'AccountNo = RecSet("select AccNo from Customerfile where CustNo=" & CustNo.Text, Conn)
                        If Not AccountNo.Tables(0).Rows.Item(0).IsNull(0) Then
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "AccountNo"), AccountNo.Tables(0).Rows(0)(0))
                            'AccNo.Text = AccountNo.Tables(0).Rows(0)(0)
                        Else
                            SetComboIndex(FindControlRecursive(PolicyControl, "PayType"), 0)
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "AccountNo"), 0)
                            'MsgBox.confirm("! لايوجد رقم حساب لهذا الزبون :" & OrderNo.Text & " هل تريد تسجيل رقم حساب له ?", "AccNo_request")
                        End If
                    Else
                        SetComboIndex(FindControlRecursive(PolicyControl, "PayType"), 0)
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "AccountNo"), 0)
                    End If

                Case Else

            End Select

        End If
    End Sub

    Protected Sub ASPxCallbackPanel1_Callback(source As Object, e As CallbackEventArgs)
        Select Case e.Parameter
            Case "SuminsCHANGE"
                If Session("End") = 0 Then
                    e.Result = Convert.ToDecimal(ProjectPdG.Value) + Convert.ToDecimal(ProjectTool.Value) + Convert.ToDecimal(ProjectMachine.Value) + Convert.ToDecimal(Machines.Value) + Convert.ToDecimal(OwnerProperties.Value) + Convert.ToDecimal(DebrisRubble.Value)
                Else
                    Dim OldHist As New DataSet

                    Dim Oldadptr = New SqlDataAdapter("select sum(SumIns) as OldSumIns from Policyfile left join ProjectFile on PolicyFile.OrderNo=ProjectFile.OrderNo where PolNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")) & "' and PolicyFile.EndNo<" & CInt(Session("End")) & " and IssuDate is not null", Conn)
                    Dim unused = Oldadptr.Fill(OldHist)
                    e.Result = Convert.ToDecimal(ProjectPdG.Value) + Convert.ToDecimal(ProjectTool.Value) + Convert.ToDecimal(ProjectMachine.Value) + Convert.ToDecimal(Machines.Value) + Convert.ToDecimal(OwnerProperties.Value) + Convert.ToDecimal(DebrisRubble.Value) - OldHist.Tables(0).Rows.Item(0).Item("OldSumIns")
                End If

            Case Else
        End Select
    End Sub

End Class