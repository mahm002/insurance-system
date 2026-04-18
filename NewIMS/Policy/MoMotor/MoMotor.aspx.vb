Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.Web

Public Class MoMotor
    Inherits Page

    Private Addflag As String = ""
    Private DelFlag As String = ""

    Private Sub MoMotor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))

        If GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")) <> "" And GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")).ToString().Trim <> "0" Then
            Session("Pol") = GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo"))
            If Addflag = "" Then
                DataGrid.DataSourceID = "DataS1"
            Else
                DataGrid.DataSourceID = "DataS"
            End If
            DataGrid.DataBind()
        Else
            DataGrid.DataSourceID = "DataS"
            DataGrid.DataBind()
        End If

    End Sub

    Private Sub MoMotor_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))

    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        'Dim myList = CType(Session("UserInfo"), List(Of String))
        Session("End") = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo"))
        Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))
        Dim COVERTYP = GetComboValue(FindControlRecursive(PolicyControl, "CoverType"))
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
                If diff < 0.997 Then
                Else
                    diff = Math.Ceiling(diff)
                End If

            End If

            Select Case e.Parameter

                Case "Endorsment"
                    FindControlRecursive(PolicyControl, "Endorsment").Visible = False
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "EndNo"), GetLastEnd(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")), 0) + 1)
                    FindControlRecursive(PolicyControl, "Endorsment").Visible = False

                    Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                    SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                    SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                    SetDxtxtValue(FindControlRecursive(PolicyControl, "OrderNo"), CallSP("LastOrderNo", Conn, Parm))
                    Session("Order") = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo"))

                    Session("Pol") = GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo"))
                    If Addflag = "" Then
                        DataGrid.DataSourceID = "DataS1"
                    Else
                        DataGrid.DataSourceID = "DataS"
                    End If
                    'DataGrid.DataSourceID = "DataS1"

                    ExecConn(InsertPolicyData(PolicyControl, Request("Sys"), Session("Branch")), Conn)
                    DataGrid.DataBind()
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
                                If RateAll.Checked Then
                                    If COVERTYP = 2 Then
                                        ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=" & Rate.Value & ", Premium=Round(" & Rate.Value & " * " & diff & ",3)" _
                                                                        & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                                                        & " AND EndNo=" & Session("End") & " ", con)
                                        RateAll.Checked = False
                                    Else
                                        ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=" & Rate.Value & ", Premium=Round(" & GetEndFile(Request("Sys")) & " * (" & Rate.Value & " /100) * " & diff & ",3)" _
                                                                        & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                                                        & " AND EndNo=" & Session("End") & " ", con)
                                        RateAll.Checked = False
                                    End If
                                Else
                                    If COVERTYP = 2 Then
                                        ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=Rate, Premium=Round(Rate *" & diff & ",3)" _
                                                                        & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                                                        & " AND EndNo=" & Session("End") & " ", con)
                                    Else
                                        ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=Rate, Premium=Round(" & GetEndFile(Request("Sys")) & " * (Rate /100) * " & diff & ",3)" _
                               & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                               & " AND EndNo=" & Session("End") & " ", con)
                                    End If

                                End If

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
                                    If RateAll.Checked Then
                                        If COVERTYP = 2 Then
                                            ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=" & Rate.Value & ", Premium=Round(" & Rate.Value & " * " & diff & ",3)" _
                                                                       & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                                                       & " AND EndNo=" & Session("End") & " ", con)
                                        Else
                                            ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=" & Rate.Value & ", Premium=Round(" & GetEndFile(Request("Sys")) & " * (" & Rate.Value & " /100) * " & diff & ",3)" _
                                    & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                    & " AND EndNo=" & Session("End") & " ", con)
                                            RateAll.Checked = False
                                        End If
                                    Else
                                        If COVERTYP = 2 Then
                                            ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=Rate, Premium=Round(Rate * " & diff & ",3)" _
                                                                       & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                                                       & " AND EndNo=" & Session("End") & " ", con)
                                        Else
                                            ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=Rate, Premium=Round(" & GetEndFile(Request("Sys")) & " * (Rate /100) * " & diff & ",3)" _
                                   & " where OrderNo='" & Session("Order") & "'" _
                                   & " AND EndNo=" & Session("End") & " ", con)
                                        End If

                                    End If
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
                                    If RateAll.Checked Then
                                        If COVERTYP = 2 Then
                                            ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=" & Rate.Value & ", Premium=Round(" & Rate.Value & " * " & diff & ",3)" _
                                                                       & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                                                       & " AND EndNo=" & Session("End") & " ", con)
                                        Else
                                            ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=" & Rate.Value & ", Premium=Round(" & GetEndFile(Request("Sys")) & " * (" & Rate.Value & " /100) * " & diff & ",3)" _
                                   & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                   & " AND EndNo=" & Session("End") & " ", con)
                                            RateAll.Checked = False
                                        End If
                                    Else
                                        If COVERTYP = 2 Then
                                            ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=Rate, Premium=Round(Rate * " & diff & ",3)" _
                                                                       & " where OrderNo='" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "'" _
                                                                       & " AND EndNo=" & Session("End") & " ", con)
                                        Else
                                            ExecConn("Update " & GetGroupFile(Request("Sys")) & " Set Rate=Rate, Premium=Round(SumIns * (Rate /100) * " & diff & ",3)" _
                                            & " where OrderNo='" & Session("Order") & "'" _
                                            & " AND EndNo=" & Session("End") & " ", con)
                                        End If

                                    End If
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
                                Callback.JSProperties("cpNewWindowUrl") = "../MoMotor/MotoImport.aspx?OrderNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "&EndNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) & "&LoadNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) & "&Sys=" & Request("Sys") & ""
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
                Case "Import"
                    If IsIssued(GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
                        Exit Sub
                    Else
                        Session("FilePath") = ""
                        Callback.JSProperties("cpMyAttribute") = "ImportExcel"
                        Callback.JSProperties("cpNewWindowUrl") = "../MoMotor/MotoImport.aspx?OrderNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) & "&EndNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) & "&LoadNo=" & GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) & "&Sys=" & Request("Sys") & ""
                    End If
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

    Private Sub DataS1_Inserted(sender As Object, e As SqlDataSourceStatusEventArgs) Handles DataS1.Inserted
        Addflag = "Add"
        DataGrid.DataSourceID = "DataS"
        DataGrid.DataBind()

    End Sub

    Private Sub DataS1_Inserting(sender As Object, e As SqlDataSourceCommandEventArgs) Handles DataS1.Inserting
        Addflag = "Add"
        DataGrid.DataSourceID = "DataS"
        DataGrid.DataBind()
    End Sub

    Private Sub DataS_Updating(sender As Object, e As SqlDataSourceCommandEventArgs) Handles DataS.Updating
        DataS.UpdateParameters.Add("Period", "1")
        'MsgBox(MotorSqlDataSource.UpdateCommand)
    End Sub

    Private Sub DataGrid_CommandButtonInitialize(sender As Object, e As ASPxGridViewCommandButtonEventArgs) Handles DataGrid.CommandButtonInitialize

        If e.VisibleIndex < 0 Then
            Exit Sub
        Else
            If Not IsIssued(DataGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString(), CInt(DataGrid.GetRowValues(e.VisibleIndex, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys")) Then
                Select Case e.ButtonType
                    Case ColumnCommandButtonType.Edit
                        e.Visible = True
                    Case ColumnCommandButtonType.Delete
                        e.Visible = True
                    Case ColumnCommandButtonType.New
                        e.Visible = True
                    Case ColumnCommandButtonType.Update
                        e.Visible = True
                    Case ColumnCommandButtonType.Cancel
                        e.Visible = True
                    Case Else
                        If Session("End") = CInt(DataGrid.GetRowValues(e.VisibleIndex, "EndNo")).ToString() Then
                            e.Visible = False
                        Else
                            e.Visible = True
                        End If
                End Select
            Else
                Select Case e.ButtonType
                    Case ColumnCommandButtonType.Edit
                        e.Visible = False
                    Case ColumnCommandButtonType.Delete
                        e.Visible = False
                    Case ColumnCommandButtonType.New
                        e.Visible = True
                    Case ColumnCommandButtonType.Update
                        e.Visible = True
                    Case ColumnCommandButtonType.Cancel
                        e.Visible = True
                    Case Else
                        If Session("End") = CInt(DataGrid.GetRowValues(e.VisibleIndex, "EndNo")).ToString() Then
                            e.Visible = False
                        Else
                            e.Visible = True
                        End If
                End Select
            End If

        End If

    End Sub

    Private Sub DataGrid_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles DataGrid.CustomButtonInitialize
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim Field = IsIssued(DataGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString(), CInt(DataGrid.GetRowValues(e.VisibleIndex, "EndNo")), GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")), Request("Sys"))
        Dim MoveType = CDbl(DataGrid.GetRowValues(e.VisibleIndex, "SumIns"))
        Select Case e.ButtonID

            Case "ExpandBtn"
                If Field Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "DelButton"
                If Field Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "تعديل", "إضافة"
                If MoveType > 0 Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
        End Select
    End Sub

    Private Sub DataGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles DataGrid.CustomButtonCallback
        Dim diff As Double = Val(Format(DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverTo")))) / DateDiff(DateInterval.Day, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))), DateAdd(DateInterval.Year, 1, CDate(GetDateedit(FindControlRecursive(PolicyControl, "CoverFrom"))))), "0.000"))
        Select Case e.ButtonID

            Case "ExpandBtn"
                Dim Ord = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")).ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")).ToString().Trim
                Dim SubIns = Request("Sys")
                Dim CarType = DataGrid.GetRowValues(e.VisibleIndex, "CarType").ToString()
                Dim TableNo = DataGrid.GetRowValues(e.VisibleIndex, "TableNo").ToString()
                Dim BudyNo = DataGrid.GetRowValues(e.VisibleIndex, "BudyNo").ToString()
                Dim PermType = DataGrid.GetRowValues(e.VisibleIndex, "PermType").ToString()
                Dim PassNo = DataGrid.GetRowValues(e.VisibleIndex, "PassNo")
                Dim Power = DataGrid.GetRowValues(e.VisibleIndex, "Power")
                Dim CarColor = DataGrid.GetRowValues(e.VisibleIndex, "CarColor").ToString()
                Dim MadeYear = DataGrid.GetRowValues(e.VisibleIndex, "MadeYear")
                Dim SumIns = DataGrid.GetRowValues(e.VisibleIndex, "SumIns").ToString()
                Dim Premium = DataGrid.GetRowValues(e.VisibleIndex, "Premium").ToString()

                ExecConn("INSERT INTO [MOMOTORFILE] (OrderNo, EndNo, LoadNo, SubIns, CarType, TableNo, BudyNo, PermType, PassNo, Power, CarColor, MadeYear, SumIns, Premium, Zone)
                VALUES('" & Ord & "'," & EndNo & ", 0,'" & Request("Sys") & "','" & CarType & "' ,'" & TableNo & "', '" & BudyNo & "', '" & PermType & "',  " & PassNo & ", " & Power & ", '" & CarColor & "', " & MadeYear & "," & SumIns & ", " & Premium & ",'تونس')", Conn)

                ASPxWebControl.RedirectOnCallback("../../Policy/MoMotor/ExpandCover.aspx.aspx?Sys=" + Request("Sys") + "&OrderNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")) + "&EndNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")) + "&LoadNo=" + GetDxTextValue(FindControlRecursive(PolicyControl, "LoadNo")) + "&EndType=Expand&BasePrm=" + Premium + "")

            Case "DelButton"
                Dim covertp = GetComboValue(FindControlRecursive(PolicyControl, "CoverType"))
                Dim Ord = GetDxTextValue(FindControlRecursive(PolicyControl, "OrderNo")).ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim Ser = DataGrid.GetRowValues(e.VisibleIndex, "SerNo").ToString()
                Dim EndNo = GetDxTextValue(FindControlRecursive(PolicyControl, "EndNo")).ToString().Trim
                Dim SubIns = Request("Sys")
                Dim CarType = DataGrid.GetRowValues(e.VisibleIndex, "CarType").ToString()
                Dim TableNo = DataGrid.GetRowValues(e.VisibleIndex, "TableNo").ToString()
                Dim BudyNo = DataGrid.GetRowValues(e.VisibleIndex, "BudyNo").ToString()
                Dim PermType = DataGrid.GetRowValues(e.VisibleIndex, "PermType").ToString()
                Dim PassNo = DataGrid.GetRowValues(e.VisibleIndex, "PassNo")
                Dim Power = DataGrid.GetRowValues(e.VisibleIndex, "Power")
                Dim CarColor = DataGrid.GetRowValues(e.VisibleIndex, "CarColor").ToString()
                Dim MadeYear = DataGrid.GetRowValues(e.VisibleIndex, "MadeYear")
                Dim SumIns = DataGrid.GetRowValues(e.VisibleIndex, "SumIns").ToString()
                Dim Premium = DataGrid.GetRowValues(e.VisibleIndex, "Premium").ToString()
                Dim Rt = DataGrid.GetRowValues(e.VisibleIndex, "Rate") * -1

                Dim OriginCover = GetCoverDays(GetDxTextValue(FindControlRecursive(PolicyControl, "PolNo")).ToString(), Request("EndNo"), Request("Sys"))
                Dim newcover = diff * 100

                If covertp = 2 Then
                    ExecConn("INSERT INTO [MOMOTORFILE] (OrderNo, EndNo, LoadNo, SubIns, CarType, TableNo, BudyNo, PermType, PassNo, Power, CarColor, MadeYear, SumIns, Premium,Rate, State)
                VALUES('" & Ord & "'," & EndNo & ", 0,'" & Request("Sys") & "','" & Trim(CarType) & "' ,'" & Trim(TableNo) & "', '" & Trim(BudyNo) & "', '" & Trim(PermType) & "',  " & PassNo & ", " & Power & ", '" & Trim(CarColor) & "', " & MadeYear & "," & SumIns & ", " & (newcover / OriginCover) * Premium * -1 & "," & (newcover / OriginCover) * Premium * -1 & ",'0')", Conn)

                    ExecConn("Update MoMotorFile Set State=0 where SerNo=" & Ser & "", Conn)

                    ExecConn("Update MomotorFile Set State=0 where BudyNo='" & BudyNo & "' and TableNo='" & TableNo & "'", Conn)
                Else
                    ExecConn("INSERT INTO [MOMOTORFILE] (OrderNo, EndNo, LoadNo, SubIns, CarType, TableNo, BudyNo, PermType, PassNo, Power, CarColor, MadeYear, SumIns, Premium,Rate, State)
                VALUES('" & Ord & "'," & EndNo & ", 0,'" & Request("Sys") & "','" & Trim(CarType) & "' ,'" & Trim(TableNo) & "', '" & Trim(BudyNo) & "', '" & Trim(PermType) & "',  " & PassNo & ", " & Power & ", '" & Trim(CarColor) & "', " & MadeYear & "," & SumIns * -1 & ", " & SumIns * (Rt / 100) * diff & "," & Rt * -1 & ",'0')", Conn)

                    ExecConn("Update MoMotorFile Set State=0 where SerNo=" & Ser & "", Conn)

                    ExecConn("Update MomotorFile Set State=0 where BudyNo='" & BudyNo & "' and TableNo='" & TableNo & "'", Conn)
                End If

        End Select
    End Sub

End Class