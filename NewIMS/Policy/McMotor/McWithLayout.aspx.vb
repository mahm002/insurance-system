Imports System.Data.SqlClient
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class McWithLayout
    Inherits Page

    Private Diff As Double
    Private ShortTimes As Boolean = False
    Private PassMore As Short = 0
    Private CarryMore As Short = 0
    'Private IsCallbackInProgress As Boolean = False
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
        If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
        Else
            BudyNo.Enabled = False
            CarType.Enabled = False
        End If
        If Request("EndNo") = 0 Then
        Else
            BudyNo.Enabled = False
            CarType.Enabled = False
        End If
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))


        If Session("Order") Is Nothing Or Session("Order") = "0" And Not IsCallback Then
            Select Case Left(Session("Branch"), 2)
                Case "01"
                    PermZone.Value = "طرابلس"
                Case "02"
                    PermZone.Value = "بنغازي"
                Case Else
                    PermZone.Value = "طرابلس"
            End Select
        End If
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        'If IsCallbackInProgress Then Exit Sub
        ' Disable controls to prevent double submission
        issue.Enabled = False
        'IsCallbackInProgress = True

        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
        'Dim myList = CType(Session("UserInfo"), List(Of String))
        Dim callbackPanel As ASPxCallbackPanel = DirectCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

        If Getspinedit(FindControlRecursive(PolicyControl, "Interval")) = 0 Or Not isValid Then
            Exit Sub
        End If


        If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
            Exit Sub
        Else
            Try
                ' IsCallbackInProgress = True
                Dim daysCalc As Short = DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), GetDateedit(FindControlRecursive(PolicyControl, "CoverTo"))) + 1

                Select Case daysCalc
                    Case 1 To 15
                        Diff = 0.1
                        ShortTimes = True
                    Case 16 To 31
                        Diff = 0.2
                        ShortTimes = True
                    Case 32 To 61
                        Diff = 0.3
                        ShortTimes = True
                    Case 62 To 92
                        Diff = 0.4
                        ShortTimes = True
                    Case Else
                        Diff = Val(Format(DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), GetDateedit(FindControlRecursive(PolicyControl, "CoverTo"))) / DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), DateAdd(DateInterval.Year, 1, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")))), "0.000"))
                End Select

                Dim T As Boolean

                Select Case e.Parameter

                    Case "Calc"

                        If isValid Then
                            If ShortTimes Then
                                Premium.Text = Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000")
                            Else
                                Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000"))
                            End If

                            If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                                T = True
                                If ShortTimes Then
                                    Premium.Text = Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000")
                                Else
                                    Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000"))
                                End If

                                Select Case daysCalc
                                    Case 1 To 92
                                        SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff) + PassMore + CarryMore, "0.000"))
                                    Case Else
                                        SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000")))
                                End Select
                                'SetTextValue(pageFooter.FindControl("NETPRM"), Format(Net - GetLastNet(GetTextValue(pageFooter.FindControl("PolNo")), GetTextValue(pageFooter.FindControl("EndNo"))), "###,###,##.000"))
                            Else
                                Diff = Val(Format(DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), GetDateedit(FindControlRecursive(PolicyControl, "CoverTo"))) / DateDiff(DateInterval.Day, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), DateAdd(DateInterval.Year, 1, GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")))), "0.000"))
                                T = False
                                Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000"))
                                'GetNet(GetEbaValue(PermType, 1), Val(Power.Text), Val(PassNo.Text), Val(Carry.Text))
                                If Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000")) <> GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) And Diff < 2.8 Then ' 2.8 لوثائق المؤسسة بالتسعيرة الديمة ل 3 سنوات
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(Premium.Text) - GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), "###,##.000"))
                                Else
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(0, "###,##.000"))
                                End If
                                'SetTextValue(pageFooter.FindControl("NETPRM"), Format(0, "###,###,##.000"))
                            End If
                            If GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) <= 0 And GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) <> 0 Then
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), 0)
                            End If
                            MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                         , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                         , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                         , False)
                            ''If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" And MyBase.Session("Order") = "0" Then
                            If Session("Order").ToString = "0" Or GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")).ToString.TrimEnd = "0" Then
                                If String.IsNullOrEmpty(Session("Order")) OrElse Session("Order").ToString = "0" Then
                                    Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                                    SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                                    SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))
                                    Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
                                End If
                                'Threading.Thread.Sleep(1000)
                                'insert new data requested
                                ExecConn(InsertData(Callback, Request("Sys"),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) _
                                        + "; " + InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")),
                             Conn)
                                '&OrderNo=2025-04-0100-01-01000&EndNo=0&LoadNo=0
                                'ASPxWebControl.RedirectOnCallback("~/Policy/McMotor/McWithLayout.aspx?Sys=01&OrderNo=" & Session("Order") & "&EndNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) & "&LoadNo=0")
                            Else
                                'Update data requested

                                ExecConn(UpdateData(Callback, Request("Sys"),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) + "; " +
                                        UpdatePolicyData(PolicyControl, Request("Sys"), Session("Branch"),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)
                            End If
                            If CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "TAXPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "CONPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "STMPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "ISSPRM"))) = CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "TOTPRM"))) And CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "TOTPRM"))) <> 0 Then
                                issue.Enabled = True
                            Else
                                issue.Enabled = False
                            End If
                        Else
                            issue.Enabled = False
                        End If
                    Case "Issue"
                        If Format(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")), "yyyy/MM/dd") = Format(Today.Date(), "yyyy/MM/dd") And GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                            issue.Enabled = False
                            Exit Sub
                        End If

                        If CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "TAXPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "CONPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "STMPRM"))) + CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "ISSPRM"))) = CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "TOTPRM"))) And CDbl(GetDxTextValue(FindControlRecursive(PolicyControl, "TOTPRM"))) <> 0 Then
                            issue.Enabled = True
                        Else
                            Exit Sub
                        End If
                        If isValid Then
                            If ShortTimes Then
                                Premium.Text = Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff) + PassMore + CarryMore, "0.000")
                            Else
                                Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000"))
                            End If

                            If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 Then
                                T = True
                                If ShortTimes Then
                                    Premium.Text = Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff) + PassMore + CarryMore, "0.000")
                                Else
                                    Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000"))
                                End If

                                Select Case daysCalc
                                    Case 1 To 92
                                        SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format((GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff) + PassMore + CarryMore, "0.000"))
                                    Case Else
                                        SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000")))
                                End Select
                                'SetTextValue(pageFooter.FindControl("NETPRM"), Format(Net - GetLastNet(GetTextValue(pageFooter.FindControl("PolNo")), GetTextValue(pageFooter.FindControl("EndNo"))), "###,###,##.000"))
                            Else
                                'T = False
                                'Premium.Text = Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000")) 'GetNet(GetEbaValue(PermType, 1), Val(Power.Text), Val(PassNo.Text), Val(Carry.Text))
                                'If Rndqtr(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value, ShortTimes) * Diff, "0.000")) <> GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) Then
                                '    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Val(Premium.Text) - GetLastNet(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) - 1, GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), "###,##.000"))
                                'Else
                                '    SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(0, "###,##.000"))
                                'End If
                                T = False
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(0, "###,##.000"))
                            End If
                            'SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Net.Text)
                            MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                             , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                             , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))), False)

                            ''If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" And Session("Order") = "0" Then
                            If Session("Order").ToString = "0" Or GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")).ToString.TrimEnd = "0" Then
                                If String.IsNullOrEmpty(Session("Order")) OrElse Session("Order").ToString = "0" Then
                                    Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                                    SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                                    SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                                    SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))
                                    Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
                                End If

                                'Threading.Thread.Sleep(1000)
                                'insert new data requested
                                ExecConn(InsertData(Callback, Request("Sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) _
                                        + "; " + InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")), Conn)
                                'ASPxWebControl.RedirectOnCallback("~/Policy/McMotor/McWithLayout.aspx?Sys=01&OrderNo=" & Session("Order") & "&EndNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) & "&LoadNo=0")
                            Else
                                'Update data requested

                                ExecConn(UpdateData(Callback, Request("Sys"),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) + "; " +
                                        UpdatePolicyData(PolicyControl, Request("Sys"), Session("Branch"),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)
                            End If
                            IssuePolicy(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys"), Session("Branch"), Session("UserID"), 0)
                            Dim P As New List(Of ReportParameter) From {
                        New ReportParameter("PolicyNo", GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), False),
                        New ReportParameter("EndNo", GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), False),
                        New ReportParameter("Sys", Request("Sys"), False)
                    }

                            Session.Add("Parms", P)

                            ASPxWebControl.RedirectOnCallback("~/Reporting/PreviewPDF.aspx?Report=" & ReportsPath & PolRep(Request("Sys")) & "")
                            'ASPxWebControl.RedirectOnCallback(("../../Policy/IssueWithSerial.aspx?Sys=" + Request("Sys") + "&OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) + "&Br=" + Session("Branch")) & "")
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
                        ExecConn("Delete PolicyFile Where PolNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")) & "' and IssuDate Is Null", Conn)
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "EndNo"), GetLastEnd(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), 0) + 1)

                        If Format(GetCoverFrom(Request("OrderNo"), Request("EndNo"), Request("Sys")), "yyyy/MM/dd") <= Format(Today.Date, "yyyy/MM/dd") Then
                            SetDateEdit(FindControlRecursive(PolicyControl, "CoverFrom"), Today.Date())
                        Else
                            SetDateEdit(FindControlRecursive(PolicyControl, "CoverFrom"), GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom")))
                        End If

                        FindControlRecursive(PolicyControl, "Endorsment").Visible = False

                        BudyNo.Enabled = False
                        CarType.Enabled = False
                        'PermType.Enabled = False
                        'PassNo.Enabled = False
                        'Power.Enabled = False
                        Carry.Enabled = False

                        Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                        SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                        SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                        SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))
                        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))

                        'DataGrid.DataSourceID = "DataS1"
                        ExecConn(InsertData(Callback, Request("Sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                        GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) _
                                        + "; " + InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")), Conn)

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
            Finally
                Dim chck As New DataSet
                Using oCon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    Else

                    End If
                    oCon.Open()
                    Dim SrlAdptr5 = New SqlDataAdapter("Select OrderNo From PolicyFile Where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'", oCon)
                    SrlAdptr5.Fill(chck)
                    oCon.Close()
                End Using
                If chck.Tables(0).Rows.Count = 0 Then
                    issue.Enabled = False
                Else
                    If Format(GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) + GetDxTextValue(FindControlRecursive(PolicyControl, "TAXPRM")) + GetDxTextValue(FindControlRecursive(PolicyControl, "CONPRM")) + GetDxTextValue(FindControlRecursive(PolicyControl, "STMPRM")) + GetDxTextValue(FindControlRecursive(PolicyControl, "ISSPRM")), "###,#0.00") = Format(GetDxTextValue(FindControlRecursive(PolicyControl, "TOTPRM")), "###,#0.00") And GetDxTextValue(FindControlRecursive(PolicyControl, "TOTPRM")) <> 0 Then
                        issue.Enabled = True
                    Else
                        issue.Enabled = False
                    End If
                End If
            End Try
        End If
    End Sub

    Protected Sub ASPxCallbackPanel1_Callback(source As Object, e As CallbackEventArgs)
        Select Case e.Parameter
            Case "CheckCarIsCovered"
                If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
                    Exit Sub
                Else

                End If
                Dim ChckIsCovered As New DataSet

                Dim Oldadptr = New SqlDataAdapter("SELECT TOP(1) PolNo,policyFile.EndNo,isnull([dbo].[policyreport]('01'),0) As Report,policyFile.Branch,
                                                    BranchInfo.BranchName,policyFile.IssuDate,policyFile.CoverFrom,policyFile.
                                                    CoverTo,CustomerFile.CustName,motorfile.* FROM PolicyFile
                 inner join BranchInfo on PolicyFile.Branch=BranchInfo.BranchNo
                 inner join CustomerFile on policyfile.CustNo=CustomerFile.custno
                 inner join motorfile on policyfile.orderno=motorfile.Orderno AND policyfile.endno=motorfile.endno AND motorfile.SubIns='01'
                 AND policyfile.loadno=motorfile.loadno AND policyfile.SubIns=motorfile.SubIns
                 WHERE motorfile.TableNo='" & TableNo.Value & "' and right(rtrim(motorfile.BudyNo),5)='" & Right(RTrim(BudyNo.Value), 5) & "' and policyfile.CoverTo>getdate() and PolicyFile.IssuDate is not null and PolicyFile.stop=0 order by coverto desc", Conn)
                Oldadptr.Fill(ChckIsCovered)
                If ChckIsCovered.Tables(0).Rows.Count <> 0 Then
                    e.Result = "هذه المركبة لديها وثيقة سارية حتى " & Format(ChckIsCovered.Tables(0).Rows.Item(0)("CoverTo"), "yyyy/MM/dd") & " باسم /" & ChckIsCovered.Tables(0).Rows.Item(0)("CustName") & " برقم /" & ChckIsCovered.Tables(0).Rows.Item(0)("PolNo").ToString & ""
                    'Checklable.ClientVisible = True
                    'Checklable.Text = "هذه المركبة لديها وثيقة سارية حتى " & ChckIsCovered.Tables(0).Rows.Item(0)("PolNo").ToString & " برقم"
                Else
                    'e.Result.DefaultIfEmpty()
                End If
                'e.Result = (Convert.ToDecimal(ProjectPdG.Value) + Convert.ToDecimal(ProjectTool.Value) + Convert.ToDecimal(ProjectMachine.Value) + Convert.ToDecimal(Machines.Value) + Convert.ToDecimal(OwnerProperties.Value) + Convert.ToDecimal(DebrisRubble.Value)) - OldHist.Tables(0).Rows.Item(0).Item("OldSumIns")
                'Case "IssuanceAlert"
                '    e.Result =
            Case Else
        End Select
    End Sub

    'Protected Function GetNet(Perm As Integer, Power As Integer, PassNo As Integer, Carry As Double, ShortT As Boolean) As Double
    '    Dim Net As New DataSet
    '    Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
    '        If con.State = ConnectionState.Open Then
    '            con.Close()
    '        Else

    '        End If
    '        con.Open()
    '        Dim dbadapter = New SqlDataAdapter("Select NETPRM,PassNoMor,CarryMor, PassNoTo,CarryTo,RateNo, PermTypeNo, PowerFrom, PowerTo, PassNoFrom, CarryFrom" _
    '           & " From MotorRate " _
    '           & "Where (RateNo = @Pr) " _
    '           & "And (@Pw BETWEEN PowerFrom And PowerTo) " _
    '           & "And ((@Cr BETWEEN CarryFrom And CarryTo) Or CarryMor<>0 ) " _
    '           & "And ((@Ps BETWEEN PassNoFrom And PassNoTo) Or PassNoMor<>0)", con)

    '        Select Case Perm
    '            Case 7, 10, 22, 23
    '                If Carry <= 2 Then
    '                    Carry = 2
    '                Else

    '                End If
    '            Case 1
    '                If PassNo <= 4 Then
    '                    PassNo = 4
    '                Else

    '                End If
    '            Case Else

    '        End Select

    '        If ShortT Then
    '            Power = 0
    '            PassNo = 4
    '        End If

    '        dbadapter.SelectCommand.Parameters.AddWithValue("@Pr", Perm)
    '        dbadapter.SelectCommand.Parameters.AddWithValue("@Pw", Power)
    '        dbadapter.SelectCommand.Parameters.AddWithValue("@Ps", PassNo)
    '        dbadapter.SelectCommand.Parameters.AddWithValue("@Cr", Carry)

    '        dbadapter.Fill(Net)
    '        If ShortT And Nation.Value <> 1 Then
    '            If Net.Tables(0).Rows.Count <> 0 Then
    '                GetNet = Net.Tables(0).Rows.Item(0).Item(0)
    '                PassMore = IIf(PassNo >= Net.Tables(0).Rows(0)(3), Net.Tables(0).Rows(0)(1) * (PassNo - Net.Tables(0).Rows(0)(3)), 0)
    '                CarryMore = IIf(Carry >= Net.Tables(0).Rows(0)(4), Net.Tables(0).Rows(0)(2) * (IIf(Carry - Fix(Carry) = 0, Carry, Fix(Carry) + 1) - Net.Tables(0).Rows(0)(4)), 0)
    '            Else
    '                Return "لا توجد تسعيرة حسب المدخلات"
    '            End If
    '        Else
    '            If Net.Tables(0).Rows.Count <> 0 Then

    '                Return Net.Tables(0).Rows.Item(0).Item(0) + IIf(PassNo >= Net.Tables(0).Rows(0)(3), Net.Tables(0).Rows(0)(1) * (PassNo - Net.Tables(0).Rows(0)(3)), 0) _
    '                                                        + IIf(Carry >= Net.Tables(0).Rows(0)(4), Net.Tables(0).Rows(0)(2) * (IIf(Carry - Fix(Carry) = 0, Carry, Fix(Carry) + 1) - Net.Tables(0).Rows(0)(4)), 0)
    '            Else
    '                Return "لا توجد تسعيرة حسب المدخلات"
    '            End If
    '        End If
    '        con.Close()
    '    End Using
    '    'Conn.Close()
    'End Function
    Protected Function GetNet(Perm As Integer, Power As Integer, PassNo As Integer, Carry As Double, ShortT As Boolean) As Double
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
                Case 7, 10, 22, 23, 24
                    If Carry = 0 Then
                        Carry = 20
                    Else
                        If Carry <= 2 Then
                            Carry = 2
                        Else

                        End If
                    End If
                Case 1
                    If PassNo <= 4 Then
                        PassNo = 4
                    Else

                    End If
                    If Power = 0 Then
                        Power = 30
                    Else

                    End If
                Case Else

            End Select

            dbadapter.SelectCommand.Parameters.AddWithValue("@Pr", Perm)
            dbadapter.SelectCommand.Parameters.AddWithValue("@Pw", Power)
            dbadapter.SelectCommand.Parameters.AddWithValue("@Ps", PassNo)
            dbadapter.SelectCommand.Parameters.AddWithValue("@Cr", Carry)

            dbadapter.Fill(Net)

            If ShortT And Nation.Value = 1 And Session("UserID") = 12108 Then
                Select Case Diff
                    Case <= 0.1
                        Return 128
                    Case <= 0.2
                        Return 64
                    Case Else
                        Exit Select
                End Select
            End If

            If ShortT And IsForignCar() Then

                If Net.Tables(0).Rows.Count <> 0 Then
                    GetNet = Net.Tables(0).Rows.Item(0).Item(0)
                    PassMore = IIf(PassNo >= Net.Tables(0).Rows(0)(3), Net.Tables(0).Rows(0)(1) * (PassNo - Net.Tables(0).Rows(0)(3)), 0)
                    CarryMore = IIf(Carry >= Net.Tables(0).Rows(0)(4), Net.Tables(0).Rows(0)(2) * (IIf(Carry - Fix(Carry) = 0, Carry, Fix(Carry) + 1) - Net.Tables(0).Rows(0)(4)), 0)
                Else
                    Return 200
                End If
            Else
                If IsForignCar() Then
                    If Net.Tables(0).Rows.Count <> 0 Then
                        Return 3 * (Net.Tables(0).Rows.Item(0).Item(0) + IIf(PassNo >= Net.Tables(0).Rows(0)(3), Net.Tables(0).Rows(0)(1) * (PassNo - Net.Tables(0).Rows(0)(3)), 0) _
                                                            + IIf(Carry >= Net.Tables(0).Rows(0)(4), Net.Tables(0).Rows(0)(2) * (IIf(Carry - Fix(Carry) = 0, Carry, Fix(Carry) + 1) - Net.Tables(0).Rows(0)(4)), 0))
                    Else
                        Return "لا توجد تسعيرة حسب المدخلات"
                    End If
                Else
                    If Net.Tables(0).Rows.Count <> 0 Then

                        Return Net.Tables(0).Rows.Item(0).Item(0) + IIf(PassNo >= Net.Tables(0).Rows(0)(3), Net.Tables(0).Rows(0)(1) * (PassNo - Net.Tables(0).Rows(0)(3)), 0) _
                                                                + IIf(Carry >= Net.Tables(0).Rows(0)(4), Net.Tables(0).Rows(0)(2) * (IIf(Carry - Fix(Carry) = 0, Carry, Fix(Carry) + 1) - Net.Tables(0).Rows(0)(4)), 0)
                    Else
                        Return "لا توجد تسعيرة حسب المدخلات"
                    End If
                End If
                'If Net.Tables(0).Rows.Count <> 0 Then

                '    Return Net.Tables(0).Rows.Item(0).Item(0) + IIf(PassNo >= Net.Tables(0).Rows(0)(3), Net.Tables(0).Rows(0)(1) * (PassNo - Net.Tables(0).Rows(0)(3)), 0) _
                '                                            + IIf(Carry >= Net.Tables(0).Rows(0)(4), Net.Tables(0).Rows(0)(2) * (IIf(Carry - Fix(Carry) = 0, Carry, Fix(Carry) + 1) - Net.Tables(0).Rows(0)(4)), 0)
                'Else
                '    Return "لا توجد تسعيرة حسب المدخلات"
                'End If
            End If
            con.Close()
        End Using
        'Conn.Close()
    End Function
    Private Function IsForignCar() As Object
        Return (TableNo.Value.ToString.Contains("تونس") Or PermZone.Value.ToString.Contains("تونس") Or TableNo.Value.ToString.Contains("الجزائر") Or PermZone.Value.ToString.Contains("الجزائر") Or Nation.Value <> 1)
    End Function
End Class