Imports System.Data.SqlClient
Imports System.Web.UI
Imports DevExpress.Web

Public Class MarineCargo
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
        Session("Load") = GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))

        Dim Curr = GetComboValue(FindControlRecursive(PolicyControl, "Currency"))
        Dim Formating As String = IIf(Curr = 1, "###,#0.000", "###,#0.00")

        If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
            If IsSameMonth(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
                If e.Parameter = "ExRate" Then
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "ExcRate"), GetExrate(GetComboValue(FindControlRecursive(PolicyControl, "Currency")), GetComboText(FindControlRecursive(PolicyControl, "Currency"))))
                Else

                End If
                SumIns.Text = Format(ExcRate.Value * DocVal.Value + (Margin.Value / 100 * DocVal.Value * ExcRate.Value), "0.000")
                If Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Then
                    'Normal Cargo Policy
                    MarPrm.Text = Format(MarRate.Value / 1000 * CDbl(SumIns.Value), Formating)

                    ExtraPrm.Text = Format(ExtraRate.Value / 1000 * CDbl(SumIns.Value), Formating)

                    ShipPrm.Text = Format(ShipRate.Value / 1000 * CDbl(SumIns.Value), Formating)

                    WarPrm.Text = Format(WarRate.Value / 1000 * CDbl(SumIns.Value), Formating)

                    If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value))
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "LASTNET"), CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value))
                    Else

                        SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"),
                        Format(Val(CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value)) -
                        GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")),
                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1,
                        GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Formating))

                        NewSumIns.Value = SumIns.Value - Format(GetLastSi(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")),
                                           GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), Request("Sys")), Formating)
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "LASTNET"), Format(GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Formating))
                        'SetTextValue(pageFooter.FindControl("NETPRM"), Format(0, "###,###,##.000"))
                    End If

                    MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                             , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                             , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                             , False)

                    'Update data requested

                    ExecConn(UpdateData(Callback,
                                    Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) + "; " +
                                    UpdatePolicyData(PolicyControl,
                                    Request("Sys"), GetBranchbyOrderNo(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)
                    'End If
                Else
                    'Open Cover Cargo Policy
                    MarPrm.Text = Format((MarRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                    ExtraPrm.Text = Format((ExtraRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                    ShipPrm.Text = Format((ShipRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                    WarPrm.Text = Format((WarRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                    If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 And GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) = 0 Then
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), 0)
                    Else
                        If GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) <> 0 Then
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value)), Formating))
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "LASTNET"), Format(Val(CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value)), Formating))
                        Else

                        End If
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value)) - GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Formating))
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "LASTNET"), Format(GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Formating))
                    End If

                    MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                             , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                             , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))), False)
                    'If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" Then
                    '    Parm = Array.CreateInstance(GetType(SqlClient.SqlParameter), 2)
                    '    SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                    '    SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                    '    SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))

                    '    'insert new data requested
                    '    ExecConn(InsertData(Callback,
                    '                Request("Sys"),
                    '                GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                    '                GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                    '                GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) _
                    '     + "; " + InsertPolicyData(PolicyControl,
                    '                               Request("Sys"),
                    '                               Session("Branch"), Session("User")), Conn)

                    'Else
                    'Update data requested

                    ExecConn(UpdateData(Callback,
                                    Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) + "; " +
                            UpdatePolicyData(PolicyControl,
                                    Request("Sys"), GetBranchbyOrderNo(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)
                    'End If

                End If
                'ASPxWebControl.RedirectOnCallback("../../Reins/DistBulkPol.aspx?OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) + "&Branch=" + GetBranchbyOrderNo(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))) + "&Sys=" + Request("sys") + "&PolNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")) + "")
            Else
                Exit Sub
            End If
        Else
            Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
            Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)
            'Dim diff As Double = DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverTo")))) ' / DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), DateAdd(DateInterval.Year, 1, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))))), "0.000"))
            Select Case e.Parameter
                Case "Calc"
10:                 If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = "0" Then
                        SumIns.Text = Format(ExcRate.Value * DocVal.Value + (((Margin.Value / 100) * DocVal.Value) * ExcRate.Value), Formating)
                    Else

                        Dim NewDocVal As Double = DocVal.Value - GetLastDocValue(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")),
                                               GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), Request("Sys"))

                        SumIns.Text = CDbl(ExcRate.Value * NewDocVal + (((Margin.Value / 100) * NewDocVal) * ExcRate.Value)) +
                        CDbl(GetLastSi(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")),
                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), Request("Sys")))
                    End If

                    If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = "0" Then

                    Else
                        NewSumIns.Value = SumIns.Value - Format(GetLastSi(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")),
                                               GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), Request("Sys")), Formating)
                    End If

                    If isValid Then
                        If Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Then
                            'Normal Cargo Policy
                            MarPrm.Text = Format((MarRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                            ExtraPrm.Text = Format((ExtraRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                            ShipPrm.Text = Format((ShipRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                            WarPrm.Text = Format((WarRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                            If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value))
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "LASTNET"), CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value))
                            Else
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value)) _
                                    - IIf(MarRate.Value = 0, 0, GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1,
                                                 GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")))), Formating))
                                NewSumIns.Value = SumIns.Value - Format(GetLastSi(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                                                   Request("Sys")), Formating)
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "LASTNET"),
                                              Format(IIf(MarRate.Value = 0, 0, GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")),
                                                                GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                                                GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")))), Formating))
                                'SetTextValue(pageFooter.FindControl("NETPRM"), Format(0, "###,###,##.000"))
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
                                    UpdatePolicyData(PolicyControl, Request("Sys"), Session("Branch"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)
                            End If
                        Else
                            'Open Cover Cargo Policy
                            MarPrm.Text = Format((MarRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                            ExtraPrm.Text = Format((ExtraRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                            ShipPrm.Text = Format((ShipRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                            WarPrm.Text = Format((WarRate.Value / 1000) * CDbl(SumIns.Value), Formating)

                            If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 And GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) = 0 Then
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), 0)
                            Else
                                If GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) <> 0 Then
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value)), Formating))
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "LASTNET"), Format(Val(CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value)), Formating))
                                Else

                                End If
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(CDbl(MarPrm.Value) + CDbl(ExtraPrm.Value) + CDbl(ShipPrm.Value) + CDbl(WarPrm.Value)) - GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Formating))
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "LASTNET"), Format(GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Formating))
                            End If

                            MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                             , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                             , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                             , False)
                            If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" Then
                                Parm = Array.CreateInstance(GetType(SqlClient.SqlParameter), 2)
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

                        End If
                    Else

                    End If

                    'Case "ExRate"
                    '    SetDxtxtValue(FindControlRecursive(PolicyControl, "ExcRate"), GetExrate(GetComboValue(FindControlRecursive(PolicyControl, "Currency")), GetComboText(FindControlRecursive(PolicyControl, "Currency"))))
                Case "Dist"
                    Session("RtnPg") = GetEditForm(Request("Sys")) + "?Sys=" + Request("Sys") + "&OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))

                    ASPxWebControl.RedirectOnCallback("../../Reins/DistPolicy.aspx?OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) + "&Branch=" + GetBranchbyOrderNo(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))) + "&Sys=" + Request("sys") + "&PolNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "")
                Case "LocalExRate"
                    SetDxtxtValue(ExcRate, GetExrate(Currency.SelectedItem.Value, Currency.SelectedItem.Text))
                    GoTo 1
                Case "SumInsCalc"
                    If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = "0" Then
                        SumIns.Text = Format(ExcRate.Value * DocVal.Value + (((Margin.Value / 100) * DocVal.Value) * ExcRate.Value), "0.000")
                    Else
                        Dim NewDocVal As Double = DocVal.Value - GetLastDocValue(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")),
                                               GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), Request("Sys"))

                        SumIns.Value = CDbl(ExcRate.Value * NewDocVal + (((Margin.Value / 100) * NewDocVal) * ExcRate.Value)) +
                        CDbl(GetLastSi(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")),
                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), Request("Sys")))
                    End If
1:
                    If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = "0" Then

                    Else
                        NewSumIns.Value = SumIns.Value - Format(GetLastSi(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")),
                                          GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), Request("Sys")), Formating)
                    End If

                    If isValid Then
                        GoTo 10
                    Else

                    End If
                    'Case "PayType"

                    '    If GetComboValue(FindControlRecursive(PolicyControl, "PayType")) = 2 Then
                    '        Dim AccountNo As New DataSet

                    '        Dim dbadapter = New System.Data.SqlClient.SqlDataAdapter("select AccNo from Customerfile where CustNo=" & GetlookuptValue(FindControlRecursive(PolicyControl, "Customer")), Conn)
                    '        dbadapter.Fill(AccountNo)

                    '        If Not AccountNo.Tables(0).Rows.Item(0).IsNull(0) Then
                    '            SetDxtxtValue(FindControlRecursive(PolicyControl, "AccountNo"), AccountNo.Tables(0).Rows(0)(0))

                    '        Else
                    '            SetComboValue(FindControlRecursive(PolicyControl, "PayType"), 0)
                    '            SetDxtxtValue(FindControlRecursive(PolicyControl, "AccountNo"), 0)
                    '            'MsgBox.confirm("! لايوجد رقم حساب لهذا الزبون :" & OrderNo.Text & " هل تريد تسجيل رقم حساب له ?", "AccNo_request")
                    '        End If
                    '    End If
                    'Case Else
                Case "Endorsment"
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "EndNo"), GetLastEnd(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), 0) + 1)
                    FindControlRecursive(PolicyControl, "Endorsment").Visible = False
                Case "Shipment"
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "LoadNo"), GetLastLoad(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo"))) + 1)
                    FindControlRecursive(PolicyControl, "Shipment").Visible = False

                    'FindControlRecursive(PolicyControl, "Shipment").Visible = False
                    'SetDxtxtValue(FindControlRecursive(PolicyControl, "LoadNo"), GetLastLoad(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo"))) + 1)
                    'DocVal.Value = 0
                    'SumIns.Value = 0
                    'Loads.ClientEnabled = False
                Case "ExRate"

                    SetDxtxtValue(FindControlRecursive(PolicyControl, "ExcRate"), GetExrate(GetComboValue(FindControlRecursive(PolicyControl, "Currency")), GetComboText(FindControlRecursive(PolicyControl, "Currency"))))

                Case "PayType"

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

        End If
        If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = "0" Then
        Else

            NewSumIns.Value = SumIns.Value - Format(GetLastSi(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), Request("Sys")), Formating)
        End If
    End Sub

End Class