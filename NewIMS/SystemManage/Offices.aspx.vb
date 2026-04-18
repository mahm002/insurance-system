Imports DevExpress.Web
Imports DevExpress.Web.Data

Partial Public Class Offices
    Inherits Page

    Private Lo() As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim myList = TryCast(Session("UserInfo"), List(Of String))
        Session("AgentNo") = ""
        If myList Is Nothing Then
            'ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            FormsAuthentication.RedirectToLoginPage()
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
                BranchNo.Value = BranchNo.Value
                BranchNo.ClientEnabled = True
            End If
        End If
        If IsHeadQuarter(BranchNo.Value) Then
            SqlDataSource2.ConnectionString = Conn.ConnectionString
            SqlDataSource2.SelectCommand = "select * from BranchInfo where left(BranchNo,2)='" & Left(Session("Branch"), 2) & "' and right(BranchNo,3)<>'000' and agent=0 order by BranchNo "
            SqlDataSource2.Select(DataSourceSelectArguments.Empty)

            'SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "' and AccountName<>'branchAdmin'"
            'SqlDataSource2.Select(DataSourceSelectArguments.Empty)
            SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000'"
            SqlDataSource1.Select(DataSourceSelectArguments.Empty)

        Else
            SqlDataSource2.ConnectionString = Conn.ConnectionString
            SqlDataSource2.SelectCommand = "select * from BranchInfo where left(BranchNo,2)='" & Left(BranchNo.Value, 2) & "' and right(BranchNo,3)<>'000' and agent=0 order by BranchNo "
            SqlDataSource2.Select(DataSourceSelectArguments.Empty)

            'SqlDataSource2.SelectCommand = "select *,dbo.[BranchName](Branch) As BrName from AccountFile where left(Branch,2)='" & Left(BranchNo.Value, 2) & "' and AccountName<>'branchAdmin'"
            'SqlDataSource2.Select(DataSourceSelectArguments.Empty)
            SqlDataSource1.SelectCommand = "SELECT DISTINCT BranchNo, RTRIM(BranchName) As BranchName FROM BranchInfo where right(BranchNo,3)='000'"
            SqlDataSource1.Select(DataSourceSelectArguments.Empty)

        End If

        'SqlDataSource2.ConnectionString = Conn.ConnectionString
        'SqlDataSource2.SelectCommand = "select * from BranchInfo where left(BranchNo,2)='" & Left(Session("Branch"), 2) & "' and right(BranchNo,3)<>'000' and agent=0 order by BranchNo "
        'SqlDataSource2.Select(DataSourceSelectArguments.Empty)
        ''GridView1.DataBind()
        MainGrid.DataBind()
    End Sub

    'Protected Sub Insert_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles Insert.Click
    '    Response.Write("<script> window.open('../SystemManage/AgentForm.aspx','_new'); </script>")
    'End Sub

    'Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
    '    Select Case e.CommandName
    '        Case "Special"
    '            GridView1.SelectedIndex = Val(e.CommandArgument.ToString)
    '            Response.Write("<script> window.open('../SystemManage/AgentForm.aspx?Agent=" & Right(GridView1.SelectedRow.Cells(2).Text, 2) & "','_new'); </script>")
    '            'MsgBox1.confirm("! سيتم الغاءالطلب رقم :" & OrderNo.Text & " هل تريد الإستمرار ?", "delete_request")
    '    End Select
    'End Sub

    Private Sub MainGrid_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles MainGrid.CustomButtonInitialize
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        'Dim Stopped As Boolean = grid.GetRowValues(e.VisibleIndex, "Stop") 'IsStoped(grid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString(), grid.GetRowValues(e.VisibleIndex, "LoadNo").ToString())

        'Select Case e.ButtonID
        '    Case "Activate"
        '        If Stopped Then
        '            e.Enabled = True
        '            e.Visible = DefaultBoolean.True
        '        Else
        '            e.Enabled = False
        '            e.Visible = DefaultBoolean.False
        '        End If
        '    Case "Delete"
        '        If Stopped Then
        '            e.Enabled = False
        '            e.Visible = DefaultBoolean.False
        '        Else
        '            e.Enabled = True
        '            e.Visible = DefaultBoolean.True
        '        End If

        '    Case "Edit"
        '        If Stopped Then
        '            e.Enabled = False
        '            e.Visible = DefaultBoolean.False
        '        Else
        '            e.Enabled = True
        '            e.Visible = DefaultBoolean.True
        '        End If
        'End Select
    End Sub

    Protected Sub MainGrid_ToolbarItemClick(source As Object, e As ASPxGridViewToolbarItemClickEventArgs)
        Dim grid As ASPxGridView = CType(source, ASPxGridView)
        Select Case e.Item.Name
            Case "NewAgent"
                MainGrid.JSProperties("cpMyAttribute") = "New"
                MainGrid.JSProperties("cpResult") = "إضافة" + "-" + BranchNo.Text.Trim
                MainGrid.JSProperties("cpSize") = 1100
                MainGrid.JSProperties("cpNewWindowUrl") = "../SystemManage/OfficeForm.aspx?Branch=" + BranchNo.Value
        End Select

    End Sub

    Protected Sub MainGrid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        'If (e.DataColumn.FieldName = "Stop") Then
        '    '    If e.GetValue("PolNo") <> "" Then
        '    '        If IsStoped(e.GetValue("PolNo"), e.GetValue("EndNo"), e.GetValue("LoadNo")) Then
        '    '            e.Cell.ForeColor = Color.Red
        '    '            e.Cell.Text = e.GetValue("PolNo") + " / Cancelled"
        '    '            e.Cell.HorizontalAlign = HorizontalAlign.Center
        '    '        Else
        '    '            If IsIssued(e.GetValue("OrderNo"), e.GetValue("EndNo"), e.GetValue("LoadNo"), e.GetValue("SubIns")) Then
        '    '                e.Cell.ForeColor = Color.DarkGreen
        '    '                e.Cell.HorizontalAlign = HorizontalAlign.Center
        '    '            Else
        '    '                e.Cell.BackColor = Color.Red
        '    '                e.Cell.ForeColor = Color.DarkGreen
        '    '                e.Cell.HorizontalAlign = HorizontalAlign.Center
        '    '                e.Cell.Text = e.GetValue("PolNo") + " / عرض / ملحق / إشعار"
        '    '            End If
        '    '        End If
        'Else
        '    '        e.Cell.BackColor = Color.Red
        '    '        e.Cell.ForeColor = Color.White
        '    '        e.Cell.Text = " فاتورة مبدئية (طلب تأمين) "
        '    '        e.Cell.HorizontalAlign = HorizontalAlign.Center
        '    '    End If
        '    '    'Dim PolicyRow As DevExpress.Web.Rendering.GridViewTableDataRow = e.Cell.Parent
        'End If

    End Sub

    Protected Sub MainGrid_RowDeleting(sender As Object, e As ASPxDataDeletingEventArgs)

        'Dim SelectedIndx = MainGrid.FindVisibleIndexByKeyValue(e.Keys("AccountNo"))

        'Dim OrderNo = MainGrid.GetRowValues(SelectedIndx, "AccountNo")

        'ExecConn("Update AccountFile Set Stop=1 where AccountNo=" & OrderNo & "", Conn)

        ''Dim OrderNo = MainGrid.GetRowValues(SelectedIndx, "OrderNo").ToString().Trim
        ''Dim EndNo = MainGrid.GetRowValues(SelectedIndx, "EndNo").ToString()
        ''Dim LoadNo = MainGrid.GetRowValues(SelectedIndx, "LoadNo").ToString()
        ''Dim Sys = MainGrid.GetRowValues(SelectedIndx, "SubIns").ToString()
        ''e.Cancel = True
        ''Parm = Array.CreateInstance(GetType(SqlParameter), 6)
        ''SetPm("@TP", DbType.String, Sys, Parm, 0)
        ''SetPm("@OrderNo", DbType.String, OrderNo, Parm, 1)
        ''SetPm("@EndNo", DbType.Int16, EndNo, Parm, 2)
        ''SetPm("@LoadNo", DbType.Int16, LoadNo, Parm, 3)
        ''SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 4)
        ''SetPm("@UserName", DbType.String, Session("User"), Parm, 5)
        ''CallSP("MoveRequest", Conn, Parm)
        'MainGrid.DataBind()

    End Sub

    Protected Sub MainGrid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        'Select Case e.Parameters
        '    Case "Activate"

        '        Dim Order = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "AccountNo").ToString()
        '        ExecConn("Update AccountFile Set Stop=0 where AccountNo=" & Order & "", Conn)

        '    Case Else
        '        MainGrid.DataBind()
        '        'cmbReports.DataBind()

        'End Select
        'MainGrid.DataBind()
    End Sub

    Private Sub MainGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles MainGrid.CustomButtonCallback
        Select Case e.ButtonID
            Case "Edit"
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "BranchNo").ToString()

                Dim logname = MainGrid.GetRowValues(e.VisibleIndex, "BranchName").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                'Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Branch").ToString()
                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Edit"
                    MainGrid.JSProperties("cpResult") = logname

                    MainGrid.JSProperties("cpSize") = 1100

                    MainGrid.JSProperties("cpNewWindowUrl") = "../SystemManage/OfficeForm.aspx?Branch=" + BranchNo.Value + "&Agent=" + Br
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
        End Select

    End Sub

    Private Sub MainGrid_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles MainGrid.HtmlRowPrepared
        'If MainGrid.GetRowValues(e.VisibleIndex, "Stop") Then
        '    e.Row.BackColor = Color.DarkGray
        '    e.Row.ForeColor = Color.White
        'Else

        'End If
    End Sub

End Class