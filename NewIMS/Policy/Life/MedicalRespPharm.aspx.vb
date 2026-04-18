Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class MedicalRespPharm
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))

        Dim diff, Prmm As Double
        If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
            Exit Sub
        Else
            Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
            Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

            Dim T As Boolean
            diff = Val(Format(DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), GetDateedit(FindControlRecursive(PolicyControl, "CoverTo"))) / DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), DateAdd(DateInterval.Year, 1, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")))), "0.000")) 'DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), DateAdd(DateInterval.Year, 1, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")))))
            ' DateDiff(DateInterval.Year, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverTo")))) ' /

            Select Case e.Parameter

                Case "Calc"
                    If diff = 0.997 Then
                        Prmm = 172
                    Else
                        Prmm = 172 * diff
                    End If
                    If isValid Then

                        Premium.Text = Format(Prmm, "0.000")
                        If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                            T = True
                            Premium.Text = Format(Prmm, "0.000")
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Prmm, "0.000"))
                            'SetTextValue(pageFooter.FindControl("NETPRM"), Format(Net - GetLastNet(GetTextValue(pageFooter.FindControl("PolNo")), GetTextValue(pageFooter.FindControl("EndNo"))), "###,###,##.000"))
                        Else
                            T = False
                            Premium.Text = Format(Prmm, "0.000") 'GetNet(GetEbaValue(PermType, 1), Val(Power.Text), Val(PassNo.Text), Val(Carry.Text))
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(Premium.Text) - GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), "###,##.000"))
                            'SetTextValue(pageFooter.FindControl("NETPRM"), Format(0, "###,###,##.000"))
                        End If

                        MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                             , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                             , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                             , T)
                        If Session("Order") = "0" Or Session("Order") = "" Then
                            Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                            SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                            SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))

                            'insert new data requested
                            ExecConn(InsertData(Callback, Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) _
                         + "; " + InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")),
                         Conn)
                        Else
                            'Update data requested

                            ExecConn(UpdateData(Callback,
                                    Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) + "; " +
                           UpdatePolicyData(PolicyControl, Request("Sys"), Session("Branch"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)
                        End If
                    Else
                    End If
                    GoTo PP
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

    Protected Function GetNet(ByVal Count As Integer) As Double

        Select Case Count
            Case 0
                GetNet = Format(0, "###,0.000")
            Case 1
                GetNet = Format(107.25, "###,0.000")
            Case > 1
                GetNet = Format(137, "###,0.000")
            Case Else
                GetNet = Format(0, "###,0.000")
        End Select

    End Function

End Class