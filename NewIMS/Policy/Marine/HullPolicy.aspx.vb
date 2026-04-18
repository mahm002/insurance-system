Imports System.Data.SqlClient
Imports DevExpress.Web

Public Class HullPolicy
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        DataGrid.DataBind()
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        'Dim myList = CType(Session("UserInfo"), List(Of String))
        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
        If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
            Exit Sub
        Else
            Dim Netpremium As Double = 0
            Dim callbackPanel As ASPxCallbackPanel = CType(sender, ASPxCallbackPanel)
            Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

            Dim T As Boolean
            Dim diff As Double = Val(Format(DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverTo")))) / DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), DateAdd(DateInterval.Year, 1, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))))), "0.000"))
            If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) <> 0 Then
                diff = diff
            Else
                diff = Math.Ceiling(diff)
            End If

            Select Case e.Parameter
                Case "Endorsment"
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "EndNo"), GetLastEnd(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), 0) + 1)

                Case "Calc"

                Case "ImportExcel"
                    If isValid Then
                        ''''Premium.Text = RndTax25(Format(GetNet(PermType.Value, Power.Value, PassNo.Value, Carry.Value) * diff, "0.000"))
                        If GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) = 0 AndAlso Session("Order") <> "" Then
                            T = False
                            Dim Net As New DataSet
                            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                                If con.State = ConnectionState.Open Then
                                    con.Close()
                                Else

                                End If
                                con.Open()
                                'If RateAll.Checked Then
                                '    ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=" & Rate.Value & ", Premium=Round(" & GetEndFile(Request("Sys")) & " * (" & Rate.Value & " /100) * " & diff & ",3)" _
                                '& " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                '& " AND EndNo=" & Session("End") & " ", con)
                                '    RateAll.Checked = False
                                'Else
                                ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=Rate, Premium=Round((" & GetEndFile(Request("Sys")) & ") * (Rate /100)  * " & diff & ",3)" _
                                & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                & " AND EndNo=" & Session("End") & " ", con)
                                'End If

                                Dim dbadapter = New SqlDataAdapter("select Sum(Premium) as Net from " & GetGroupFile(Request("Sys")) & " where OrderNo='" & Session("Order") & "' " _
                                                                    & "And EndNo=" & Session("End") & " ", con)
                                dbadapter.Fill(Net)
                                Netpremium = IIf(Net.Tables(0).Rows(0).IsNull(0), 0, Net.Tables(0).Rows(0)(0))
                                con.Close()
                            End Using

                            SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Netpremium, "###,##.000"))
                        Else
                            If Session("Order") = "0" Then
                                Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                                SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                                SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                                SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))
                                Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
                                'Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
                                'insert new data requested
                                ExecConn(InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")), Conn)
                                T = False
                                Dim net As New DataSet
                                Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                                    If con.State = ConnectionState.Open Then
                                        con.Close()
                                    Else

                                    End If
                                    con.Open()
                                    'If RateAll.Checked Then
                                    '    ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=" & Rate.Value & ", Premium=Round(" & GetEndFile(Request("Sys")) & " * (" & Rate.Value & " /100) * " & diff & ",3)" _
                                    '& " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                    '& " AND EndNo=" & Session("End") & " ", con)
                                    '    RateAll.Checked = False
                                    'Else
                                    ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=Rate, Premium=Round((" & GetEndFile(Request("Sys")) & ") * (Rate /100)  * " & diff & ",3)" _
                                    & " where OrderNo='" & Session("Order") & "'" _
                                    & " AND EndNo=" & Session("End") & " ", con)
                                    'End If
                                    Dim dbadapter = New SqlDataAdapter("select Sum(Premium) as Net from " & GetGroupFile(Request("Sys")) & "  where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "' And SubIns='" & Request("Sys") & "' " _
                                                                        & "And EndNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) & " ", con)
                                    dbadapter.Fill(net)
                                    Netpremium = IIf(net.Tables(0).Rows(0).IsNull(0), 0, net.Tables(0).Rows(0)(0))
                                    con.Close()
                                End Using
                            Else
                                T = False
                                Dim net As New DataSet
                                Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                                    If con.State = ConnectionState.Open Then
                                        con.Close()
                                    Else

                                    End If
                                    con.Open()
                                    'If RateAll.Checked Then
                                    '    ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=" & Rate.Value & ", Premium=Round(" & GetEndFile(Request("Sys")) & " * (" & Rate.Value & " /100) * " & diff & ",3)" _
                                    '& " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                    '& " AND EndNo=" & Session("End") & " ", con)
                                    '    RateAll.Checked = False
                                    'Else
                                    ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=Rate, Premium=Round((" & GetEndFile(Request("Sys")) & ") * (Rate /100)  * " & diff & ",3)" _
                                    & " where OrderNo='" & Session("Order") & "'" _
                                    & " AND EndNo=" & Session("End") & " ", con)
                                    'End If
                                    Dim dbadapter = New SqlDataAdapter("select Sum(Premium) as Net from " & GetGroupFile(Request("Sys")) & "  where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "' And SubIns='" & Request("Sys") & "' " _
                                                                        & "And EndNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) & " ", con)
                                    dbadapter.Fill(net)
                                    Netpremium = IIf(net.Tables(0).Rows(0).IsNull(0), 0, net.Tables(0).Rows(0)(0))
                                    con.Close()
                                End Using

                            End If

                            'Premium.Text = 0 'GetNet(GetEbaValue(PermType, 1), Val(Power.Text), Val(PassNo.Text), Val(Carry.Text))
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Format(Netpremium, "###,##.000"))
                            'SetTextValue(pageFooter.FindControl("NETPRM"), Format(0, "###,###,##.000"))
                        End If
                        'SetDxtxtValue(FindControlRecursive(PolicyControl, "NETPRM"), Net.Text)
                        MainCalcDx(PolicyControl, GetDxTextValue(FindControlRecursive(PolicyControl, "NETPRM")) _
                             , Request("sys"), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) _
                             , Val(GetSpCase(GethiddenField(FindControlRecursive(PolicyControl, "CustNo")))) _
                             , T)
                        If GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) = "0" And Request("Order") = "" And MyBase.Session("Order") = "0" Then
                            Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                            SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                            SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                            SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))
                            Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
                            'Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
                            'insert new data requested
                            ExecConn(InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")), Conn)

                            ExecConn("Update " & GetGroupFile(Request("Sys")) & "
                                    Set " & GetGroupFile(Request("Sys")) & ".GroupNo=D.GroupNoT
                                    From(select serno,ROW_NUMBER() over (partition by S.orderno order by S.serno) as GroupNoT
                                    From " & GetGroupFile(Request("Sys")) & " S) D
                                    Where " & GetGroupFile(Request("Sys")) & ".serno=D.serno
                                    and " & GetGroupFile(Request("Sys")) & ".orderno='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'", Conn)

                            If Page.IsCallback Then
                                Session("FilePath") = ""
                                Callback.JSProperties("cpMyAttribute") = "ImportExcel"
                                Callback.JSProperties("cpNewWindowUrl") = "../Marine/HullImport.aspx?OrderNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "&EndNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) & "&LoadNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) & "&Sys=" & Request("Sys") & ""
                            Else
                                'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                            End If
                        Else
                            'Update data requested

                            ExecConn(UpdateData(Callback,
                                    Request("Sys"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))) + "; " + 'Sub Sysytem Data
                                    UpdatePolicyData(PolicyControl, ' PolicyFile Data
                                    Request("Sys"), Session("Branch"),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")),
                                    GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))), Conn)

                            ExecConn("Update " & GetGroupFile(Request("Sys")) & "
                                    Set " & GetGroupFile(Request("Sys")) & ".GroupNo=D.GroupNoT
                                    From(select serno,ROW_NUMBER() over (partition by S.orderno order by S.serno) as GroupNoT
                                    From " & GetGroupFile(Request("Sys")) & " S) D
                                    Where " & GetGroupFile(Request("Sys")) & ".serno=D.serno
                                    and " & GetGroupFile(Request("Sys")) & ".orderno='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'", Conn)
                        End If
                    Else

                    End If
                    GoTo PP
                Case "Dist"
                    Session("RtnPg") = GetEditForm(Request("Sys")) + "?Sys=" + Request("Sys") + "&OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo"))

                    ASPxWebControl.RedirectOnCallback("../../Reins/DistPolicy.aspx?OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) + "&Branch=" + GetBranchbyOrderNo(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))) + "&Sys=" + Request("sys") + "&PolNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "")
                Case "Import"
                    If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
                        Exit Sub
                    Else
                        Session("FilePath") = ""
                        Callback.JSProperties("cpMyAttribute") = "ImportExcel"
                        Callback.JSProperties("cpNewWindowUrl") = "../Marine/HullImport.aspx?OrderNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "&EndNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) & "&LoadNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) & "&Sys=" & Request("Sys") & ""
                    End If
                Case "ExRate"

                    SetDxtxtValue(FindControlRecursive(PolicyControl, "ExcRate"), GetExrate(GetComboValue(FindControlRecursive(PolicyControl, "Currency")), GetComboText(FindControlRecursive(PolicyControl, "Currency"))))

PP:             Case "PayType"

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
                    'Premium.Value = Format(GetTotalSummaryValue(), "###,#0.000")
            End Select
        End If
        DataGrid.DataBind()
        'Premium.Value = Format(GetTotalSummaryValue(), "###,#0.000")
    End Sub

    Protected Function GetTotalSummaryValue() As Double
        Dim summaryItem As ASPxSummaryItem = DataGrid.TotalSummary.First(Function(i) i.Tag = "Prm")
        GetTotalSummaryValue = DataGrid.GetTotalSummaryValue(summaryItem)
        'Return value
    End Function

    Protected Function GetNet(ByVal Perm As Integer, ByVal Power As Integer, ByVal PassNo As Integer, ByVal Carry As Double) As Double
        'Dim Net As New DataSet
        'Dim dbadapter = New SqlDataAdapter("select NETPRM,PassNoMor,CarryMor,PassNoTo,CarryTo from MotorRate where" _
        '& " PermTypeNo=" & Perm & " and " _
        '& Power & ">=PowerFrom and " & Power & "<=PowerTo and " _
        '& "((" & PassNo & ">=passNoFrom and " & PassNo & "<=PassNoTo) or PassNoMor<>0) and" _
        '& "((" & Carry & ">=CarryFrom ad " & Carry & "<=CarryTo) or CarryMor<>0)", Conn)
        'dbadapter.Fill(Net)
        'If Net.Tables(0).Rows.Count <> 0 Then

        '    GetNet = Net.Tables(0).Rows.Item(0).Item(0) _
        '    + IIf(PassNo >= Net.Tables(0).Rows(0)(3), Net.Tables(0).Rows(0)(1) * (PassNo - Net.Tables(0).Rows(0)(3)), 0) _
        '    + IIf(Carry >= Net.Tables(0).Rows(0)(4),
        '          Net.Tables(0).Rows(0)(2) *
        '             (IIf(Carry - Fix(Carry) = 0, Carry, Fix(Carry) + 1) - Net.Tables(0).Rows(0)(4)), 0)
        'Else
        GetNet = 0
        'End If
    End Function

    Protected Sub DataGrid_DataBound(sender As Object, e As EventArgs) Handles DataGrid.DataBound
        Dim gridView As New ASPxGridView
        gridView = TryCast(sender, ASPxGridView)
        Dim total As Double = 0

        If gridView.TotalSummary("Importe") Is Nothing Then

            gridView.JSProperties("cpSummary") = total
        Else
            total = gridView.GetTotalSummaryValue(gridView.TotalSummary("Importe"))
            gridView.JSProperties("cpSummary") = total
        End If
    End Sub

    Protected Sub DataGrid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        'Premium.Value = Format(GetTotalSummaryValue(), "###,#0.000")
    End Sub

    Protected Sub DataGrid_DataBinding(sender As Object, e As EventArgs) Handles DataGrid.DataBinding
        Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
        If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
            TryCast(grid.Columns("Commands"), GridViewColumn).Visible = False
            'Button1.Visible = False
        Else
            TryCast(grid.Columns("Commands"), GridViewColumn).Visible = True
            'Button1.Visible = True
        End If
    End Sub

    Private Sub DataS_Updating(sender As Object, e As SqlDataSourceCommandEventArgs) Handles DataS.Updating
        DataS.UpdateParameters.Add("Period", "1")
        'MsgBox(MotorSqlDataSource.UpdateCommand)
    End Sub

    Private Sub MoMotor_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
    End Sub

End Class