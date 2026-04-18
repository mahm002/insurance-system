Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Utils
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports Microsoft.Reporting.WebForms

Public Class IssuDaily
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = CType(Session("UserInfo"), List(Of String))
        If myList Is Nothing Then Exit Sub
        If myList Is Nothing Then
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 3)
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                If con.State = ConnectionState.Open Then
                    con.Close()
                Else

                End If
                con.Open()

                SqlDataSource.ConnectionString = con.ConnectionString

                con.Close()
            End Using
        End If

        If Not IsPostBack Then
            If DatePart("M", Today.Date) = 1 Then
                Months.SelectedIndex = 0
                Months.Value = 1
                Years.Value = Year(Today.Date)
            Else
                Months.SelectedIndex = DatePart("M", Today.Date) - 1
                Months.Value = DatePart("M", Today.Date)
                Years.Value = Year(Today.Date)
            End If

            'Else
            'Months.Text = "Q" + CStr(DatePart("M", Today.Date) - 1)
            'Years.Value = Year(Today.Date)
            'End If
        End If

        If IsCallback Then
            'Session("Year") = Years.Value()
            'Session("Q") = Months.Value()
        Else
            'Session("Year") = Years.Value()
            'Session("Q") = Months.Value()
        End If

        MainGrid.DataBind()
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Select Case e.Parameter
            Case "Transfer"
                'Dim cn As New SqlConnection(CAccConn.ConnectionString)
                'Dim cmd As New SqlCommand()
                'cmd.CommandType = CommandType.StoredProcedure
                'cmd.Parameters.AddWithValue("@Year", Years.Text)
                'cmd.Parameters.AddWithValue("@Month", Mfrom.Value)
                'cmd.CommandText = "UpdateAccounts"
                'cmd.CommandTimeout = 0
                'cmd.Connection = cn
                'Try
                '    cn.Open()
                '    cmd.ExecuteNonQuery()
                '    ASPxButton1.Text = "تم ترحيل أرصدة الحسابات لسنة !!!" & Years.Text
                '    Group.SelectedIndex = -1
                '    grid.DataBind()
                'Catch ex As Exception
                '    Throw ex
                'Finally
                '    cn.Close()
                '    cn.Dispose()
                'End Try

            Case "DelMain"
            Case Else
                Exit Select
                'ExecConn("Delete [dbo].[BALANCE] where [Group]=" & Group.Value & "; Delete [dbo].[Groups] where GroupNo=" & Group.Value & "", CAccConn)
                'Group.SelectedIndex = "-1"
                'Groups.DataBind()
                'SqlDataSource.DataBind()
                'grid.DataBind()
                'Group.DataBind()

        End Select
        MainGrid.DataBind()
    End Sub

    Private Sub MainGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles MainGrid.CustomButtonCallback

        Select Case e.ButtonID
            Case "Print"
                Dim Report = ReportsPath & "GeneralDaily"
                Dim DailyNo = MainGrid.GetRowValues(e.VisibleIndex, "DAILYNUM").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim Sys = Request("Sys")

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("Daily", DailyNo, False),
                    New ReportParameter("Typ", Sys, False)
                }

                Session.Add("Parms", P)

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "PRINT"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    'Select Case Sys
                    '    Case "02", "03"
                    '        MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                    '    Case "OR"
                    '        If IsIssued(PolicyNo, EndNo, LoadNo, Sys) Then
                    '            MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                    '        Else

                    '        End If
                    'Case Else
                    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                    'End Select

                    'If Sys = "02" Or Sys = "03" Then
                    '    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                    'Else

                    'End If
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "Edit"
                Dim DailyNo = MainGrid.GetRowValues(e.VisibleIndex, "DAILYNUM").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim Sys = Request("Sys")

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Edit"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    Select Case Request("Sys")
                        Case 4
                            MainGrid.JSProperties("cpNewWindowUrl") = "../Finance/DailySarfSD.aspx?daily=" + DailyNo + "&Sys=" + Sys
                        Case Else
                            MainGrid.JSProperties("cpNewWindowUrl") = "../Finance/DailySarf.aspx?daily=" + DailyNo + "&Sys=" + Sys
                    End Select
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If

            Case Else
                Exit Select

        End Select

    End Sub

    Protected Sub MainGrid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        Select Case e.Parameters
            Case "Issue"
            Case Else
                Exit Select
        End Select

    End Sub

    Protected Sub MainGrid_ToolbarItemClick(source As Object, e As ASPxGridViewToolbarItemClickEventArgs)
        Select Case e.Item.Name
            Case "NewDaily"

                MainGrid.JSProperties("cpMyAttribute") = "New"
                MainGrid.JSProperties("cpResult") = GetSysName(Request("Sys"))

                Select Case Request("Sys")
                    Case 4
                        MainGrid.JSProperties("cpNewWindowUrl") = "../Finance/DailySarfSD.aspx?Sys=" + Request("Sys")
                    Case Else
                        MainGrid.JSProperties("cpNewWindowUrl") = "../Finance/DailySarf.aspx?Sys=" + Request("Sys")
                End Select

            Case "ExtraSearch"

                MainGrid.JSProperties("cpMyAttribute") = "Search"
                MainGrid.JSProperties("cpResult") = GetSysName(Sys.Value)
                MainGrid.JSProperties("cpNewWindowUrl") = "../Finance/JournalSearchForm.aspx"

            Case "BalanceSheet"

                Dim Report = ReportsPath & "BalanceSheet"

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("DFrom", Format(Today.AddMonths(-1), "yyyy/MM/dd"), True),
                    New ReportParameter("DTo", Format(Today.Date, "yyyy/MM/dd"), True),
                    New ReportParameter("lvl", 20, True),
                    New ReportParameter("B1", Session("Branch").ToString, IsHeadQuarter(Session("Branch")))
                }

                Session.Add("Parms", P)

                MainGrid.JSProperties("cpResult") = "ميزان المراجعة"
                MainGrid.JSProperties("cpMyAttribute") = "PRINT"

                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""

            Case "MJournal"

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("Month", Month(Today.Date), True),
                    New ReportParameter("Year", Year(Today.Date), True),
                    New ReportParameter("reporttype", "M", True),
                    New ReportParameter("B1", CStr(Session("Branch")), IsHeadQuarter(Session("Branch")))
                }
                Session.Add("Parms", P)

                Dim Report = ReportsPath & "Monthjornal"
                MainGrid.JSProperties("cpResult") = "القيد الشهري"
                MainGrid.JSProperties("cpMyAttribute") = "PRINT"

                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case "DJournal"
                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("DFrom", Today.Date, True),
                    New ReportParameter("B1", CStr(Session("Branch")), IsHeadQuarter(Session("Branch")))
                }
                Session.Add("Parms", P)

                Dim Report = ReportsPath & "Djornal"
                MainGrid.JSProperties("cpResult") = "القيد اليومي"
                MainGrid.JSProperties("cpMyAttribute") = "PRINT"

                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
            Case Else
                Exit Select
                Exit Select
        End Select

    End Sub

    Protected Sub MainGrid_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs)
        If e.DataColumn.FieldName = "PolNo" Then
            If e.GetValue("PolNo") <> "" Then
                If IsStoped(e.GetValue("PolNo"), e.GetValue("EndNo"), e.GetValue("LoadNo")) Then
                    e.Cell.ForeColor = Color.Red
                    e.Cell.Text = e.GetValue("PolNo") + " / Cancelled"
                    e.Cell.HorizontalAlign = HorizontalAlign.Center
                Else
                    If IsIssued(e.GetValue("OrderNo"), e.GetValue("EndNo"), e.GetValue("LoadNo"), e.GetValue("SubIns")) Then
                        e.Cell.ForeColor = Color.DarkGreen
                        e.Cell.HorizontalAlign = HorizontalAlign.Center
                    Else
                        e.Cell.BackColor = Color.LightGray
                        e.Cell.ForeColor = Color.DarkGreen
                        e.Cell.HorizontalAlign = HorizontalAlign.Center
                        e.Cell.Text = e.GetValue("PolNo") + " / عرض / ملحق / إشعار"
                    End If
                End If
            Else
                e.Cell.BackColor = Color.Red
                e.Cell.ForeColor = Color.White
                e.Cell.Text = "فاتورة مبدئية"
                e.Cell.HorizontalAlign = HorizontalAlign.Center
            End If
        End If
    End Sub

    Private Sub MainGrid_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles MainGrid.CustomButtonInitialize
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)

        Dim MonthIsOpened As Boolean = IsValidDate(grid.GetRowValues(e.VisibleIndex, "DAILYDTE").ToString)
        Dim IsValidChk As Boolean = grid.GetRowValues(e.VisibleIndex, "DailyChk")
        Dim IsApproved As Boolean = grid.GetRowValues(e.VisibleIndex, "DailyChk") And grid.GetRowValues(e.VisibleIndex, "DailyPrv")
        Dim IsErrorandNotAppreoved As Boolean = Not grid.GetRowValues(e.VisibleIndex, "DailyChk") And Not grid.GetRowValues(e.VisibleIndex, "DailyChk")

        Select Case e.ButtonID
            Case "Edit"
                If MonthIsOpened Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "Errorlbl"
                If IsValidChk Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If

            Case "Approvable"
                If IsValidChk And Not IsApproved Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "Approved"
                If IsApproved Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If

            Case Else
                Exit Select
        End Select

        'Dim Field = IIf(grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString <> "", grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString, "")
        'Dim Expired As Boolean = grid.GetRowValues(e.VisibleIndex, "Expired") 'IsExpired(grid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString())
        'Dim Stopped As Boolean = grid.GetRowValues(e.VisibleIndex, "Stoped") 'IsStoped(grid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString(), grid.GetRowValues(e.VisibleIndex, "LoadNo").ToString())
        'Dim PayTp = grid.GetRowValues(e.VisibleIndex, "PayAs").ToString
        'Dim Sys = grid.GetRowValues(e.VisibleIndex, "SubIns").ToString().Trim
    End Sub

    Private Sub MainGrid_HtmlRowPrepared(sender As Object, e As ASPxGridViewTableRowEventArgs) Handles MainGrid.HtmlRowPrepared

        If e.RowType <> GridViewRowType.Data Then
            Return
        End If
        Dim Approved As Integer = Convert.ToInt32(e.GetValue("DailyPrv"))

        If Approved = 1 Then
            'e.Row.BackColor = Color.LightCyan
        Else
            e.Row.ForeColor = Color.DarkOrange
            e.Row.BackColor = Color.Beige

        End If

    End Sub

End Class