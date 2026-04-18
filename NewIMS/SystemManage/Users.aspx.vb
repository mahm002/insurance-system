Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Utils
Imports DevExpress.Web
Imports DevExpress.Web.Data

Partial Class Users
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        If myList Is Nothing Then
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 6)
        End If

        If IsHeadQuarter(Session("Branch")) And Not IsPostBack Then
            'BranchNo.DataSourceID = "UsersDS"
            BranchNo.ClientEnabled = True
            BranchNo.Value = Session("Branch")
        Else
            If Not IsHeadQuarter(Session("Branch")) Then
                BranchNo.Value = Session("Branch")
                BranchNo.ClientEnabled = False
            Else

            End If

            If IsBranch(Session("Branch")) Then
                'BranchNo.DataSourceID = "BranchUsersDS"
            Else
                'BranchNo.DataSourceID = "AgentUsersDS"
            End If

        End If
        SqlDataSource2.ConnectionString = Conn.ConnectionString
        'If Request("Pm") = 3 Then
        '    Dim DeptUsers As New DataSet

        '    Dim dbadapter = New SqlDataAdapter("select * from AccountFile where AccountLogIn='" & Lo(0) & "' and left(Branch,3)='" & Left(Session("Branch"), 3) & "' and AccountName<>'branchAdmin'", Conn)
        '    dbadapter.Fill(DeptUsers)
        '    Dim Dept As New DataSet

        '    Dim dbadapter1 = New SqlDataAdapter("select * from Department where nParentId='" & Lo(4) & "' ", Conn)
        '    dbadapter.Fill(Dept)

        '    SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,3)='" & Left(Session("Branch"), 3) & "' and Dept=" & DeptUsers.Tables(0).Rows.Item(0).Item(11) & " and AccountName<>'branchAdmin'"
        '    SqlDataSource2.Select(DataSourceSelectArguments.Empty)
        '    'GridView1.DataBind()
        'Else
        If IsHeadQuarter(BranchNo.Value) Then
            If Session("User") = "branchAdmin" Then
                SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "' "
                SqlDataSource2.Select(DataSourceSelectArguments.Empty)
                SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000'"
                SqlDataSource1.Select(DataSourceSelectArguments.Empty)
            Else
                SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "' and AccountName<>'branchAdmin'"
                SqlDataSource2.Select(DataSourceSelectArguments.Empty)
                SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000'"
                SqlDataSource1.Select(DataSourceSelectArguments.Empty)
            End If
        Else
            If IsBranch(BranchNo.Value) Then
                If Session("User") = "branchAdmin" Then
                    SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "'"
                    SqlDataSource2.Select(DataSourceSelectArguments.Empty)
                    SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000' AND LEFT(BranchNo,2)='" & Left(Session("Branch"), 2) & "'"
                    SqlDataSource1.Select(DataSourceSelectArguments.Empty)
                Else
                    SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "' and AccountName<>'branchAdmin'"
                    SqlDataSource2.Select(DataSourceSelectArguments.Empty)
                    SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000' AND LEFT(BranchNo,2)='" & Left(Session("Branch"), 2) & "' "
                    SqlDataSource1.Select(DataSourceSelectArguments.Empty)
                End If
            Else
                SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "' and AccountName<>'branchAdmin' AND RIGHT(Branch,3)='" & Right(Session("Branch"), 3) & "' "
                SqlDataSource2.Select(DataSourceSelectArguments.Empty)
                SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where BranchNo='" & Session("Branch") & "' "
                SqlDataSource1.Select(DataSourceSelectArguments.Empty)
            End If
        End If

        'If Right(Session("Branch"), 2) = "00" Then
        '        SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,3)='" & Left(Session("Branch"), 3) & "' and AccountName<>'branchAdmin'"
        '        SqlDataSource2.Select(DataSourceSelectArguments.Empty)
        '        'GridView1.DataBind()
        '    Else
        '        SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where Branch='" & Session("Branch") & "' and AccountName<>'branchAdmin'"
        '        SqlDataSource2.Select(DataSourceSelectArguments.Empty)
        '        'GridView1.DataBind()
        '    End If
        'End If
        MainGrid.DataBind()

    End Sub
    Protected Function GetBranchName(Br As String) As String
        Dim GetName As New DataSet
        Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
            If con.State = ConnectionState.Open Then
                con.Close()
            Else

            End If
            con.Open()
            Dim dbadapter = New SqlDataAdapter(String.Format("select BranchName from BranchInfo where BranchNo='{0}'", Br), con)
            dbadapter.Fill(GetName)
            'GetName = RecSet("select BranchName from BranchInfo where BranchNo='" & SysNo & "'", Conn)
            If GetName.Tables(0).Rows.Count <> 0 Then
                GetBranchName = Trim(GetName.Tables(0).Rows(0)(0))
            Else
                GetBranchName = "«·›—⁄ €Ì— „÷«›"
            End If
            con.Close()
        End Using
    End Function


    Private Sub MainGrid_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles MainGrid.CustomButtonInitialize
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        Dim Stopped As Boolean = grid.GetRowValues(e.VisibleIndex, "Stop") 'IsStoped(grid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString(), grid.GetRowValues(e.VisibleIndex, "LoadNo").ToString())
        Dim TT As Integer = CType(Mid(myList(6), InStr(1, myList(6), "S1") + 3, 1), Short)
        Select Case e.ButtonID
            Case "Activate"
                If Stopped Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "Delete"
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If

            Case "Edit"
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If
            Case "ResetPass"
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                    Exit Sub
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                    Exit Sub
                End If

                If TT < 5 Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If

            Case Else
                Exit Select
        End Select
    End Sub
    Protected Sub MainGrid_ToolbarItemClick(source As Object, e As ASPxGridViewToolbarItemClickEventArgs)
        Select Case e.Item.Name
            Case "NewUser"
                MainGrid.JSProperties("cpMyAttribute") = "New"
                MainGrid.JSProperties("cpResult") = "≈÷«›…"
                MainGrid.JSProperties("cpSize") = 1100
                MainGrid.JSProperties("cpNewWindowUrl") = "../SystemManage/UserForm.aspx?Br=" & IIf(IsHeadQuarter(Session("Branch")), BranchNo.Value, IIf(IsBranch(Session("Branch")), BranchNo.Value, Session("Branch")))
            Case Else
                Exit Select
        End Select
    End Sub
    Protected Sub MainGrid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        If (e.DataColumn.FieldName = "Stop") Then
            '    If e.GetValue("PolNo") <> "" Then
            '        If IsStoped(e.GetValue("PolNo"), e.GetValue("EndNo"), e.GetValue("LoadNo")) Then
            '            e.Cell.ForeColor = Color.Red
            '            e.Cell.Text = e.GetValue("PolNo") + " / Cancelled"
            '            e.Cell.HorizontalAlign = HorizontalAlign.Center
            '        Else
            '            If IsIssued(e.GetValue("OrderNo"), e.GetValue("EndNo"), e.GetValue("LoadNo"), e.GetValue("SubIns")) Then
            '                e.Cell.ForeColor = Color.DarkGreen
            '                e.Cell.HorizontalAlign = HorizontalAlign.Center
            '            Else
            '                e.Cell.BackColor = Color.Red
            '                e.Cell.ForeColor = Color.DarkGreen
            '                e.Cell.HorizontalAlign = HorizontalAlign.Center
            '                e.Cell.Text = e.GetValue("PolNo") + " / ⁄—÷ / „·Õﬁ / ≈‘⁄«—"
            '            End If
            '        End If
        Else
            '        e.Cell.BackColor = Color.Red
            '        e.Cell.ForeColor = Color.White
            '        e.Cell.Text = " ›« Ê—… „»œ∆Ì… (ÿ·»  √„Ì‰) "
            '        e.Cell.HorizontalAlign = HorizontalAlign.Center
            '    End If
            '    'Dim PolicyRow As DevExpress.Web.Rendering.GridViewTableDataRow = e.Cell.Parent
        End If

    End Sub
    Protected Sub MainGrid_RowDeleting(sender As Object, e As ASPxDataDeletingEventArgs)

        Dim SelectedIndx = MainGrid.FindVisibleIndexByKeyValue(e.Keys("AccountNo"))

        Dim OrderNo = MainGrid.GetRowValues(SelectedIndx, "AccountNo")

        ExecConn("Update AccountFile Set Stop=1 where AccountNo=" & OrderNo & "", Conn)

        'Dim OrderNo = MainGrid.GetRowValues(SelectedIndx, "OrderNo").ToString().Trim
        'Dim EndNo = MainGrid.GetRowValues(SelectedIndx, "EndNo").ToString()
        'Dim LoadNo = MainGrid.GetRowValues(SelectedIndx, "LoadNo").ToString()
        'Dim Sys = MainGrid.GetRowValues(SelectedIndx, "SubIns").ToString()
        'e.Cancel = True
        'Parm = Array.CreateInstance(GetType(SqlParameter), 6)
        'SetPm("@TP", DbType.String, Sys, Parm, 0)
        'SetPm("@OrderNo", DbType.String, OrderNo, Parm, 1)
        'SetPm("@EndNo", DbType.Int16, EndNo, Parm, 2)
        'SetPm("@LoadNo", DbType.Int16, LoadNo, Parm, 3)
        'SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 4)
        'SetPm("@UserName", DbType.String, Session("User"), Parm, 5)
        'CallSP("MoveRequest", Conn, Parm)
        MainGrid.DataBind()

    End Sub
    Protected Sub MainGrid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        Select Case e.Parameters
            Case "Activate"

                Dim Order = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "AccountNo").ToString()
                ExecConn("Update AccountFile Set Stop=0 where AccountNo=" & Order & "", Conn)

            Case Else
                MainGrid.DataBind()
                'cmbReports.DataBind()

        End Select
        MainGrid.DataBind()
    End Sub
    Private Sub MainGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles MainGrid.CustomButtonCallback
        Select Case e.ButtonID
            Case "Edit"
                Dim Log = MainGrid.GetRowValues(e.VisibleIndex, "AccountNo").ToString()
                Dim logname = MainGrid.GetRowValues(e.VisibleIndex, "AccountName").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Branch").ToString()

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Edit"
                    MainGrid.JSProperties("cpResult") = logname

                    MainGrid.JSProperties("cpSize") = 1100

                    MainGrid.JSProperties("cpNewWindowUrl") = "../SystemManage/UserForm.aspx?log=" + Log + "&Br=" + Br
                    'Response.Redirect("../SystemManage/UserForm.aspx? " + "?Log=" + Log + "&Br=" + Br & "")
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "Activate"
                Dim Log = MainGrid.GetRowValues(e.VisibleIndex, "AccountNo").ToString()
                Dim logname = MainGrid.GetRowValues(e.VisibleIndex, "AccountName").ToString().Trim
                MainGrid.JSProperties("cpRowIndex") = e.VisibleIndex
                MainGrid.JSProperties("cpMyAttribute") = "Activation"
                MainGrid.JSProperties("cpCust") = logname
                MainGrid.JSProperties("cpShowIssueConfirmBox") = True
                'End If

            Case "Delete"
                Dim Log = MainGrid.GetRowValues(e.VisibleIndex, "AccountNo").ToString()
                Dim logname = MainGrid.GetRowValues(e.VisibleIndex, "AccountName").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)

                MainGrid.JSProperties("cpRowIndex") = e.VisibleIndex
                MainGrid.JSProperties("cpMyAttribute") = "Delete"
                MainGrid.JSProperties("cpCust") = logname
                MainGrid.JSProperties("cpShowDeleteConfirmBox") = True
            Case "ResetPass"
                Dim Log = MainGrid.GetRowValues(e.VisibleIndex, "AccountNo").ToString()
                Dim logname = MainGrid.GetRowValues(e.VisibleIndex, "AccountName").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Branch").ToString()
                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "ResetPass"
                    MainGrid.JSProperties("cpResult") = logname

                    MainGrid.JSProperties("cpSize") = 550

                    MainGrid.JSProperties("cpNewWindowUrl") = "../SystemManage/ResetPassword.aspx?log=" + Log + "&Br=" + Br
                    'Response.Redirect("../SystemManage/UserForm.aspx? " + "?Log=" + Log + "&Br=" + Br & "")
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If

            Case Else
                Exit Select
        End Select

    End Sub

    Private Sub MainGrid_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles MainGrid.HtmlRowPrepared
        If MainGrid.GetRowValues(e.VisibleIndex, "Stop") Then
            e.Row.BackColor = Color.DarkGray
            e.Row.ForeColor = Color.White
        Else

        End If
    End Sub
End Class
