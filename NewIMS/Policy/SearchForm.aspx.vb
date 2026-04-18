Imports System.Data.SqlClient

Imports DevExpress.Utils
Imports DevExpress.Web
Imports Microsoft.Reporting.WebForms

Public Class SearchForm
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            SearchGrid.DataBind()
            SearchClm.DataBind()
        End If
    End Sub

    Protected Sub ASPxButton1_Click(sender As Object, e As EventArgs) Handles ASPxButton1.Click
        ViewState("needBind") = True
        SearchGrid.DataBind()
        SearchClm.DataBind()
    End Sub

    Protected Sub SearchGrid_DataBinding(sender As Object, e As EventArgs) Handles SearchGrid.DataBinding
        If ViewState("needBind") IsNot Nothing AndAlso CBool(ViewState("needBind")) Then
            SearchGrid.DataSource = GetData()
        End If

    End Sub

    Private Sub SearchClm_DataBinding(sender As Object, e As EventArgs) Handles SearchClm.DataBinding
        If ViewState("needBind") IsNot Nothing AndAlso CBool(ViewState("needBind")) Then
            SearchClm.DataSource = GetClmData()
        End If
    End Sub

    Private Function GetData() As DataTable
        Try
            Dim connection As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ToString()
            Dim da As New SqlDataAdapter()
            Dim dt As New DataTable()
            Parm = Array.CreateInstance(GetType(SqlParameter), 3)
            SetPm("@searchText", DbType.String, searchtxt.Text.TrimEnd, Parm, 0)
            SetPm("@tableName", DbType.String, Request("SystemTable"), Parm, 1)
            SetPm("@Sys", DbType.String, Request("Sys"), Parm, 2)
            Dim Sql As String = CallSP("BulkSearch", Conn, Parm)
            SqlDataSource2.SelectCommand = Sql

            Using sqlCon = New SqlConnection(connection)
                Using cmd As New SqlCommand(Sql, sqlCon)
                    If sqlCon.State = ConnectionState.Open Then
                        sqlCon.Close()
                    Else

                    End If
                    sqlCon.Open()

                    da.SelectCommand = cmd
                    Dim unused = da.Fill(dt)
                    sqlCon.Close()
                End Using
                Return dt
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetClmData() As DataTable
        Dim sql1 As String
        Try
            Dim connection As String = ConfigurationManager.ConnectionStrings("IMSDBConnectionString").ToString()
            Dim da As New SqlDataAdapter()
            Dim dt As New DataTable()
            Parm = Array.CreateInstance(GetType(SqlParameter), 3)
            SetPm("@searchText", DbType.String, searchtxt.Text.TrimEnd, Parm, 0)
            SetPm("@tableName", DbType.String, Request("SystemTable"), Parm, 1)
            SetPm("@Sys", DbType.String, Request("Sys"), Parm, 2)
            sql1 = If(IsGroupedSys(Request("Sys")), CallSP("BulkSearchClmGrouped", Conn, Parm), CallSP("BulkSearchClm", Conn, Parm))

            SqlDataSource2.SelectCommand = sql1
            Using sqlCon = New SqlConnection(connection)
                Using cmd As New SqlCommand(sql1, sqlCon)
                    If sqlCon.State = ConnectionState.Open Then
                        sqlCon.Close()
                    Else

                    End If
                    sqlCon.Open()
                    da.SelectCommand = cmd
                    Dim unused = da.Fill(dt)
                    sqlCon.Close()
                End Using
                Return dt
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub SearchGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles SearchGrid.CustomButtonCallback
        Select Case e.ButtonID
            Case "Print"
                Dim PolicyRef = SearchGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim
                Dim PolicyNo = SearchGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = SearchGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = SearchGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = SearchGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Report = IIf(Sys = "OR", SearchGrid.GetRowValues(e.VisibleIndex, "Report").ToString() & "?policy=" & PolicyNo, ReportsPath & SearchGrid.GetRowValues(e.VisibleIndex, "Report").ToString())
                Dim Br = GetBranchbyOrderNo(PolicyNo)

                Dim Pol = SearchGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString()

                If Sys <> "OC" Then

                    Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", PolicyNo, False),
                    New ReportParameter("EndNo", EndNo, False),
                    New ReportParameter("Sys", Sys, False)
                     }

                    Session.Add("Parms", P)
                Else

                    Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", PolicyNo, False),
                    New ReportParameter("EndNo", EndNo, False),
                    New ReportParameter("LoadNo", LoadNo, False),
                    New ReportParameter("Sys", Sys, False)
                     }

                    Session.Add("Parms", P)
                End If

                'p.SetValue(New ReportParameter("PolicyNo", PolicyNo, False), 0)
                'p.SetValue(New ReportParameter("EndNo", EndNo, False), 1)
                'p.SetValue(New ReportParameter("Sys", Sys, False), 2)

                'Session.Add("Parms", p)

                Select Case Sys
                    Case "01", "27", "PH", "MN"
                        If IsIssued(PolicyNo, EndNo, LoadNo, Sys) Then
                            If Sys = "PH" Then
                                'SearchGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                                ASPxWebControl.RedirectOnCallback("~/Reporting/Previewer.aspx?Report=" & Report & "")
                            Else
                                ASPxWebControl.RedirectOnCallback("~/Reporting/PreviewPDF.aspx?Report=" & Report & "")
                            End If
                        Else
                            '"This menu will Undo an Operation", "Undo"
                            ' MsgBox("هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها", 48, "TIBESTY System")
                            'ClientScriptManager.RegisterClientScriptBlock(GetType(IAsyncResult), "CounterScript", "alert('هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها')", True)
                            'ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها")
                            'Me.ClientScript.RegisterStartupScript(Me.GetType(), "msgbox", "alert('FiveDot File uploaded successfully'); alert('TwoDot File uploaded successfully');", True)
                            ' MsgBob(Me, " هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها ")
                            'MsgBox("هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها")
                        End If
                    Case "OR"
                        If PolicyNo = PolicyRef Then
                            ASPxWebControl.RedirectOnCallback(Report)
                        Else
                            ASPxWebControl.RedirectOnCallback("~/Reporting/PreviewPDF.aspx?Report=/IMSReports/OrangeCard")
                        End If
                        'ASPxWebControl.RedirectOnCallback(SearchGrid.GetRowValues(e.VisibleIndex, "Report").ToString() & "?policyNumber=" & PolicyNo & "&UserName=" & Session("UserLogin") & "&Branch=" & Session("Branch"))
                        'SearchGrid.JSProperties("cpNewWindowUrl") = SearchGrid.GetRowValues(e.VisibleIndex, "Report").ToString() & "?policyNumber=" & PolicyNo
                    Case Else
                        ASPxWebControl.RedirectOnCallback("~/Reporting/Previewer.aspx?Report=" & Report & "")
                End Select
                'ASPxWebControl.RedirectOnCallback("~/Reporting/Previewer.aspx?Report=" & Report & "")

            Case "Renew"
                'Dim SelectedIndx = SearchGrid.FindVisibleIndexByKeyValue(SearchGrid.KeyFieldName())

                Dim PolicyNo = SearchGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim
                Dim EndNo = SearchGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()

                Dim LoadNo = SearchGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = SearchGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString().Trim
                Dim OrderNo = SearchGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim
                Dim EditForm = SearchGrid.GetRowValues(e.VisibleIndex, "EditForm").ToString().Trim
                Dim DBOld As New DataSet
                Dim CoverOld As Integer
                Dim Currrate As Double
                Dim CoverStart As Date
                Dim NewOrder As String
                Dim Itrv As DateInterval
                'Dim Endno As Integer
                Using oCon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    Else

                    End If
                    oCon.Open()
                    Dim dbadapter1 = New SqlDataAdapter("select Currency,EndNo,dbo.GetExtraCatName('Cur',Currency) As Name,Interval,Measure, CONVERT(varchar, CoverFrom, 111) AS CoverFrom, " _
                    & "CONVERT(varchar, CoverTo, 111) AS CoverTo from Policyfile where polno='" & PolicyNo & "' and endno=" & EndNo & " and loadno=" & LoadNo & "", oCon)
                    dbadapter1.Fill(DBOld)
                    oCon.Close()
                End Using
                If DBOld.Tables(0).Rows.Item(0).Item("Currency") <> 1 Then
                    Currrate = GetExrate(DBOld.Tables(0).Rows.Item(0).Item("Currency"), DBOld.Tables(0).Rows.Item(0).Item("Name"))
                Else
                    Currrate = 1
                End If
                If DBOld.Tables(0).Rows.Item(0).Item("EndNo") = 0 Then
                    Select Case DBOld.Tables(0).Rows.Item(0).Item("Measure")
                        Case 3
                            Itrv = DateInterval.Year
                        Case 2
                            Itrv = DateInterval.Month
                        Case 1
                            Itrv = DateInterval.Day
                    End Select
                Else
                    Itrv = DateInterval.Year
                End If

                If DBOld.Tables(0).Rows.Item(0).Item("CoverTo") <= Today.Date Then
                    CoverOld = IIf(DateDiff(Itrv, DBOld.Tables(0).Rows.Item(0).Item("CoverFrom"), DBOld.Tables(0).Rows.Item(0).Item("CoverTo")) = 0, 1, DateDiff(Itrv, DBOld.Tables(0).Rows.Item(0).Item("CoverFrom"), DBOld.Tables(0).Rows.Item(0).Item("CoverTo")))
                    CoverStart = DateAdd(DateInterval.Day, 1, Today.Date)
                Else
                    CoverOld = IIf(DateDiff(Itrv, DBOld.Tables(0).Rows.Item(0).Item("CoverFrom"), DBOld.Tables(0).Rows.Item(0).Item("CoverTo")) = 0, 1, DateDiff(Itrv, DBOld.Tables(0).Rows.Item(0).Item("CoverFrom"), DBOld.Tables(0).Rows.Item(0).Item("CoverTo")))
                    CoverStart = DBOld.Tables(0).Rows.Item(0).Item("CoverTo") 'DateAdd(DateInterval.Day, 1, DBOld.Tables(0).Rows.Item(0).Item("CoverTo"))
                End If

                'Request("Renew_request").Replace("1", "0")
                Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                SetPm("@TP", DbType.String, Sys, Parm, 0)
                SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                NewOrder = CallSP("LastOrderNo", Conn, Parm)
                EndNo = 0

                ExecConn("INSERT INTO PolicyFile " _
                    & "(OrderNo, EndNo, LoadNo, EntryDate, CustNo, OldPolicy, AgentNo, OwnNo, Broker, SubIns, Currency " _
                    & ", ExcRate, PayType, AccountNo, CoverType, Measure, Interval, CoverFrom, CoverTo, LASTNET, NETPRM, TAXPRM, CONPRM, STMPRM " _
                    & ", ISSPRM, EXTPRM, TOTPRM, ExtraRate, Stat, Inbox, ForLoss,Stop, Printed, financed, Discount, Branch, UserName, Commision) " _
                    & " Select '" & NewOrder & "'," & 0 & "," & 0 & ",CONVERT(DATETIME,getdate(),111), CustNo,'" & "تجديد على الوثيقة رقم /" & PolicyNo + "//" + EndNo & "/" & MyBase.Session("User") & "', AgentNo, OwnNo," & 0 & ",'" & Sys & "',Currency" _
                    & "," & Currrate & ",1, '0', CoverType,Measure,Interval, " _
                    & "" & "CONVERT(DATETIME,'" & Format(CoverStart, "yyyy-MM-dd") & "',111)," _
                    & "" & "CONVERT(DATETIME,'" & Format(DateAdd(DateInterval.Day, -1, DateAdd(Itrv, CoverOld, CoverStart)), "yyyy-MM-dd") & "',111)," _
                    & "LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM," & GetIssuVal(Sys, 0) & ",EXTPRM,TOTPRM, ExtraRate, Stat," & 0 & ",ForLoss,Stop,Printed,0,0,'" & MyBase.Session("Branch") & "','" & MyBase.Session("UserID") & "',0 " _
                    & "from policyfile where polno='" & PolicyNo & "' and endno=" & DBOld.Tables(0).Rows.Item(0).Item("EndNo") & " and loadno=0", Conn)

                If IsGroupedSys(Sys) Then
                    Parm = Array.CreateInstance(GetType(SqlParameter), 4)
                    SetPm("@table", DbType.String, GetGroupFile(Sys), Parm, 0)
                    SetPm("@OldPol", DbType.String, PolicyNo, Parm, 1)
                    SetPm("@NewOrder", DbType.String, NewOrder, Parm, 2)
                    SetPm("@Sys", DbType.String, Sys, Parm, 3)
                    ExecConn(CallSP("BuildInsertRenew", Conn, Parm), Conn)
                Else
                    Parm = Array.CreateInstance(GetType(SqlParameter), 5)
                    SetPm("@table", DbType.String, GetGroupFile(Sys), Parm, 0)
                    SetPm("@OldOrder", DbType.String, OrderNo, Parm, 1)
                    SetPm("@NewOrder", DbType.String, NewOrder, Parm, 2)
                    SetPm("@Sys", DbType.String, Sys, Parm, 3)
                    SetPm("@End", DbType.String, DBOld.Tables(0).Rows.Item(0).Item("EndNo"), Parm, 4)

                    ExecConn(CallSP("BuildInsert", Conn, Parm), Conn)
                End If

                ASPxWebControl.RedirectOnCallback(EditForm + "?Sys=" + Sys + "&OrderNo=" + NewOrder + "&EndNo=0&LoadNo=0")
            Case "SpecialProcedure"
                'Dim Report = ReportsPath & SearchGrid.GetRowValues(e.VisibleIndex, "Report").ToString()
                Dim Order = SearchGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = SearchGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = SearchGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = SearchGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = SearchGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString()
                Dim Pol = SearchGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString()

                ASPxWebControl.RedirectOnCallback("~/SystemManage/SpecialProcedures.aspx?OrderNo=" & Order & "&PolNo=" & Pol & "&EndNo=" & EndNo & "&LoadNo=" & LoadNo & "&sys=" & Sys & "")
            Case "Edit"
                Dim EditForm = SearchGrid.GetRowValues(e.VisibleIndex, "EditForm").ToString()
                Dim PolicyNo = SearchGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = SearchGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = SearchGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = SearchGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = SearchGrid.GetRowValues(e.VisibleIndex, "Branch").ToString()

                If Page.IsCallback Then
                    SearchGrid.JSProperties("cpMyAttribute") = "Edit"
                    SearchGrid.JSProperties("cpResult") = GetSysName(Sys)
                    Select Case Sys
                        Case "01", "27", "OR"
                            SearchGrid.JSProperties("cpSize") = 900
                        Case Else
                            SearchGrid.JSProperties("cpSize") = 1100
                    End Select
                    ASPxWebControl.RedirectOnCallback(EditForm + "?Sys=" + Sys + "&OrderNo=" + PolicyNo + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo)
                    'SearchGrid.JSProperties("cpNewWindowUrl") = EditForm + "?Sys=" + Sys + "&OrderNo=" + PolicyNo + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If

            Case Else
                Exit Select
        End Select
    End Sub

    Protected Sub SearchGrid_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles SearchGrid.CustomButtonInitialize
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim Field = IIf(grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString <> "", grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString, "")
        Dim Expired = IsExpired(grid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString())
        Dim Stopped = IsStoped(grid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString(), grid.GetRowValues(e.VisibleIndex, "LoadNo").ToString())
        Dim PermLevel = CType(Mid(myList(1), InStr(1, myList(1), Request("Sys")) + 3, 1), Short)
        'Dim IsSameMonth = IIf(grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString = "", False, IIf((Month(grid.GetRowValues(e.VisibleIndex, "IssuDate")) = Month(Today.Date) And Year(grid.GetRowValues(e.VisibleIndex, "IssuDate")) = Year(Today.Date)) Or IsHeadQuarter(Session("Branch")), True, False))
        Select Case e.ButtonID
            Case "Renew"
                If Trim(Field) = "" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Request("Sys") = "OR" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "SpecialProcedure"
                If Not Expired And Request("Sys") <> "MC" And Request("Sys") <> "MB" And Request("Sys") <> "MA" And Request("Sys") <> "OC" And Request("Sys") <> "ER" And Request("Sys") <> "CR" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If PermLevel < 3 Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Trim(Field) = "" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                'If Request("Sys") = "OR" Then
                '    If DateDiff(DateInterval.Minute, grid.GetRowValues(e.VisibleIndex, "IssuTime"), Now()) > 1440 And Request("Sys") = "OR" Then
                '        e.Enabled = False
                '        e.Visible = DefaultBoolean.False
                '    End If
                'End If

            Case "Edit"
                If Not Expired And Field <> "" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If

                If Request("Sys") = "OC" Or Request("Sys") = "MC" Or Request("Sys") = "MB" Or Request("Sys") = "MA" Or Request("Sys") = "ER" Or Request("Sys") = "CR" Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If
                If Request("Sys") = "OR" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case Else
                Exit Select
        End Select
    End Sub

End Class