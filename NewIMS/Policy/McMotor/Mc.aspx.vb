Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class Mc
    Inherits Page

    Private ShortTimes As Boolean = False
    Private PassMore As Short = 0
    Private CarryMore As Short = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
        'Dim myList = CType(Session("UserInfo"), List(Of String))

        Dim diff As Double
        If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
            Exit Sub
        Else
            Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
            Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)
            Dim daysCalc As Short = DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), GetDateedit(FindControlRecursive(PolicyControl, "CoverTo"))) + 1

            Select Case daysCalc
                Case 1 To 15
                    diff = 0.1
                    ShortTimes = True
                Case 16 To 30
                    diff = 0.2
                    ShortTimes = True
                Case 31 To 60
                    diff = 0.3
                    ShortTimes = True
                Case 61 To 92
                    diff = 0.4
                    ShortTimes = True
                Case Else
                    diff = Val(Format(DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), GetDateedit(FindControlRecursive(PolicyControl, "CoverTo"))) / DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), DateAdd(DateInterval.Year, 1, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")))), "0.000"))
            End Select

            Dim T As Boolean

            Select Case e.Parameter

                Case "Calc"

                    If isValid Then

                        If ShortTimes Then
                            Premium.Text = Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff) + IIf(Nation.Value = 1, 0, PassMore + CarryMore), "0.000")
                        Else
                            Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000"))
                        End If

                        If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                            T = True
                            If ShortTimes Then
                                Premium.Text = Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff) + IIf(Nation.Value = 1, 0, PassMore + CarryMore), "0.000")
                            Else
                                Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000"))
                            End If

                            Select Case daysCalc
                                Case 1 To 92
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff) + PassMore + CarryMore, "0.000"))
                                Case Else
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000")))
                            End Select
                            'SetTextValue(pageFooter.FindControl("NETPRM"), Format(Net - GetLastNet(GetTextValue(pageFooter.FindControl("PolNo")), GetTextValue(pageFooter.FindControl("EndNo"))), "###,###,##.000"))
                        Else
                            diff = Val(Format(DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), GetDateedit(FindControlRecursive(PolicyControl, "CoverTo"))) / DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), DateAdd(DateInterval.Year, 1, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")))), "0.000"))
                            T = False
                            Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000"))
                            'GetNet(GetEbaValue(PermType, 1), Val(Power.Text), Val(PassNo.Text), Val(Carry.Text))
                            If Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000")) <> GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) And diff < 2.8 Then ' 2.8 لوثائق المؤسسة بالتسعيرة الديمة ل 3 سنوات
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(Premium.Text) - GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), "###,##.000"))
                            Else
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(0, "###,##.000"))
                            End If
                            'SetTextValue(pageFooter.FindControl("NETPRM"), Format(0, "###,###,##.000"))
                        End If
                        'SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Net.Text)
                        MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                         , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                         , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                         , T)
                        ''If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" And MyBase.Session("Order") = "0" Then
                        If Session("Order") = "0" Then
                            Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                            SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                            SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))
                            Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
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
                                    UpdatePolicyData(PolicyControl,
                                    Request("Sys"), Session("Branch"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)
                        End If
                        issue.Enabled = True
                    Else
                        issue.Enabled = False
                    End If
                Case "Issue"

                    If isValid Then

                        If ShortTimes Then
                            Premium.Text = Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff) + IIf(Nation.Value = 1, 0, PassMore + CarryMore), "0.000")
                        Else
                            Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000"))
                        End If

                        If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                            T = True
                            If ShortTimes Then
                                Premium.Text = Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff) + IIf(Nation.Value = 1, 0, PassMore + CarryMore), "0.000")
                            Else
                                Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000"))
                            End If

                            Select Case daysCalc
                                Case 1 To 92
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff) + IIf(Nation.Value = 1, 0, PassMore + CarryMore), "0.000"))
                                Case Else
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000")))
                            End Select
                            'SetTextValue(pageFooter.FindControl("NETPRM"), Format(Net - GetLastNet(GetTextValue(pageFooter.FindControl("PolNo")), GetTextValue(pageFooter.FindControl("EndNo"))), "###,###,##.000"))
                        Else
                            T = False
                            Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000")) 'GetNet(GetEbaValue(PermType, 1), Val(Power.Text), Val(PassNo.Text), Val(Carry.Text))
                            If Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * diff, "0.000")) <> GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) Then
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(Premium.Text) - GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), "###,##.000"))
                            Else
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(0, "###,##.000"))
                            End If
                        End If
                        'SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Net.Text)
                        MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                         , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                         , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                         , T)
                        ''If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" And Session("Order") = "0" Then
                        If Session("Order") = "0" Then
                            Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                            SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                            SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))
                            Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
                            'insert new data requested
                            ExecConn(InsertData(Callback,
                                    Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) _
                                    + "; " +
                                    InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")),
                         Conn)
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
                        ASPxWebControl.RedirectOnCallback("../../Policy/IssueWithSerial.aspx?Sys=" + Request("Sys") + "&OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) + "&Br=" + Session("Branch") & "")
                    Else
                        issue.Enabled = False
                    End If
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

                Case "Endorsment"
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "EndNo"), GetLastEnd(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), 0) + 1)
                    FindControlRecursive(PolicyControl, "Endorsment").Visible = False
                    'Notes.Text = ""
                Case Else

            End Select
            'If e.Parameter = "Calc" Then
            '    'LoadUserControl()
            '    'MsgBox("hhh")

            '    'MsgBox(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value).ToString)
            'Else
            '    'UnloadUserControl()
            '    Session.Remove("Loaded")
            'End If
        End If
    End Sub

    Protected Function GetNet(ByVal Perm As Integer, ByVal Power As Integer, ByVal PassNo As Integer, ByVal Carry As Double, ByVal ShortT As Boolean) As Double
        Dim Net As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter("Select NETPRM,PassNoMor,CarryMor, PassNoTo,CarryTo,RateNo, PermTypeNo, PowerFrom, PowerTo, PassNoFrom, CarryFrom" _
               & " From MotorRate " _
               & "Where (RateNo = @Pr) " _
               & "And (@Pw BETWEEN PowerFrom And PowerTo) " _
               & "And ((@Cr BETWEEN CarryFrom And CarryTo) Or CarryMor<>0 ) " _
               & "And ((@Ps BETWEEN PassNoFrom And PassNoTo) Or PassNoMor<>0)", con)
            Select Case Perm
                Case 7, 10, 22, 23
                    If Carry = 0 Then
                        Carry = 10
                    Else

                    End If
                Case 1
                    If PassNo = 0 Then
                        PassNo = 10
                    Else

                    End If
                Case Else

            End Select

            dbadapter.SelectCommand.Parameters.AddWithValue("@Pr", Perm)
            dbadapter.SelectCommand.Parameters.AddWithValue("@Pw", Power)
            dbadapter.SelectCommand.Parameters.AddWithValue("@Ps", PassNo)
            dbadapter.SelectCommand.Parameters.AddWithValue("@Cr", Carry)

            dbadapter.Fill(Net)
            If ShortT And Nation.Value <> 1 Then
                If Net.Tables(0).Rows.Count <> 0 Then
                    GetNet = Net.Tables(0).Rows.Item(0).Item(0)
                    PassMore = IIf(PassNo >= Net.Tables(0).Rows(0)(3), Net.Tables(0).Rows(0)(1) * (PassNo - Net.Tables(0).Rows(0)(3)), 0)
                    CarryMore = IIf(Carry >= Net.Tables(0).Rows(0)(4), Net.Tables(0).Rows(0)(2) * (IIf(Carry - Fix(Carry) = 0, Carry, Fix(Carry) + 1) - Net.Tables(0).Rows(0)(4)), 0)
                Else
                    GetNet = "لا توجد تسعيرة حسب المدخلات"
                End If
            Else
                If Net.Tables(0).Rows.Count <> 0 Then

                    GetNet = Net.Tables(0).Rows.Item(0).Item(0) + IIf(PassNo >= Net.Tables(0).Rows(0)(3), Net.Tables(0).Rows(0)(1) * (PassNo - Net.Tables(0).Rows(0)(3)), 0) _
                                                            + IIf(Carry >= Net.Tables(0).Rows(0)(4), Net.Tables(0).Rows(0)(2) * (IIf(Carry - Fix(Carry) = 0, Carry, Fix(Carry) + 1) - Net.Tables(0).Rows(0)(4)), 0)
                Else
                    GetNet = "لا توجد تسعيرة حسب المدخلات"
                End If
            End If
            con.Close()
        End Using
        'Conn.Close()
    End Function

End Class