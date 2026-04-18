Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class ExpandCover
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback

        If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
            Exit Sub
        Else
            Dim callbackPanel As ASPxCallbackPanel = CType(sender, ASPxCallbackPanel)
            Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

            Dim T As Boolean
            Dim diff As Double = DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverTo")))) ' / DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), DateAdd(DateInterval.Year, 1, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))))), "0.000"))

            Select Case diff
                Case 1 To 10
                    Premium.Text = Format(Request("BasePrm") * 0.1, "0.000")
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Request("BasePrm") * 0.1, "0.000"))
                    'NETPRM.Text = TMM.Tables(0).Rows(0)("Premium") * 0.1
                Case 11 To 20
                    Premium.Text = Format(Request("BasePrm") * 0.2, "0.000")
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Request("BasePrm") * 0.2, "0.000"))
                Case 21 To 30
                    Premium.Text = Format(Request("BasePrm") * 0.3, "0.000")
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Request("BasePrm") * 0.3, "0.000"))
                Case 31 To 90
                    Premium.Text = Format(Request("BasePrm") * 0.4, "0.000")
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Request("BasePrm") * 0.4, "0.000"))
            End Select

            Select Case e.Parameter

                Case "Calc"

                    If isValid Then

                        ''Premium.Text = Format(GetNet(PermType.Value, AreaCover.Tokens.Count) * diff, "0.000")
                        'If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                        T = False
                        ''Premium.Text = Format(GetNet(PermType.Value, AreaCover.Tokens.Count) * diff, "0.000")

                        'SetTextValue(pageFooter.FindControl("NETPRM"), Format(net - GetLastNet(GetTextValue(pageFooter.FindControl("PolNo")), GetTextValue(pageFooter.FindControl("EndNo"))), "###,###,##.000"))
                        'Else
                        '    T = False
                        '    Net.Text = 0 'GetNet(GetEbaValue(PermType, 1), Val(Power.Text), Val(PassNo.Text), Val(Carry.Text))
                        '    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(0, "###,##.000"))
                        '    'SetTextValue(pageFooter.FindControl("NETPRM"), Format(0, "###,###,##.000"))
                        'End If
                        'SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Net.Text)
                        MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                             , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                             , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                             , T)
                        'If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" Then
                        '    Parm = Array.CreateInstance(GetType(SqlClient.SqlParameter), 2)
                        '    SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                        '    SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                        '    SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))

                        '    'insert new data requested
                        '    ExecConn(InsertData(Callback,
                        '            Request("Sys"),
                        '            GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                        '            GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                        '            GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) _
                        ' + "; " + InsertPolicyData(PolicyControl,
                        '                           Request("Sys"),
                        '                           Session("Branch"), Session("User")),
                        ' Conn)

                        'Else
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
                        'End If
                    Else
                    End If
                    GoTo PP
                Case "ExRate"

                    SetDxtxtValue(FindControlRecursive(PolicyControl, "ExcRate"), GetExrate(GetComboValue(FindControlRecursive(PolicyControl, "Currency")), GetComboText(FindControlRecursive(PolicyControl, "Currency"))))

                Case "PayType"
PP:
                    If GetComboValue(FindControlRecursive(PolicyControl, "PayType")) = 2 And GethiddenField(FindControlRecursive(PolicyControl, "CustNo")) <> 0 Then
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
                    Exit Select
            End Select
            If e.Parameter = "Calc" Then
                'LoadUserControl()
                'MsgBox("hhh")

                'MsgBox(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value).ToString)
            Else
                'UnloadUserControl()
                Session.Remove("Loaded")
            End If
        End If
    End Sub

    Protected Function GetNet(Perm As Integer, Count As Integer) As Double
        Dim Net As New DataSet

        Dim dbadapter = New SqlDataAdapter("select * from OrangCard where " _
                                     & " PerNo=" & Perm & "", Conn)
        ' & " And PasFrom <= " & PassNo & " And PasTo >= " & PassNo & "" _
        '& " and CaryFrom<=" & Carry & " and CaryTo>=" & Carry, Conn)
        dbadapter.Fill(Net)
        If Net.Tables(0).Rows.Count <> 0 Then
            Return Format((Net.Tables(0).Rows(0)("Rate") + ((Count - 1) * 3)), "###,0.000")
        Else
            Return 0
        End If
    End Function

End Class