Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Utils
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports Microsoft.Reporting.WebForms

Public Class DefaultIW
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If myList Is Nothing Then
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenWindow", "<script> window.open('SystemManage/LogIn.aspx','_self'); </script>")
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 4)

            TryCast(FindControlRecursive(Form, "RightPane"), ASPxPanel).FixedPosition = PanelFixedPosition.WindowLeft

            TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar).RightToLeft = DefaultBoolean.False

        End If
        'Session("Order") = ""
        Session.Remove("Parms")
        Session.Remove("Order")
        'cmbReports.DataBind()
    End Sub

    Private Sub MainGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles MainGrid.CustomButtonCallback

        Select Case e.ButtonID
            Case "Print"
                Dim Report = ReportsPath & PolRep(Request("Sys"))
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = Request("Sys")
                Dim Br = Right(MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString(), 4)

                Dim Pol = MainGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString()
                If IsStoped(Pol, EndNo, LoadNo) Then
                    Report = ReportsPath & "ReIssu"
                Else
                    'SelPolicyRep = SelectReport(Mid(PolNo.Text, 12, 2), Request("sys"))
                End If

                'Dim p As ReportParameter() = New ReportParameter(0) {}
                'p.SetValue(New ReportParameter("PolicyNo", PolicyNo, False), 0)
                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", PolicyNo, False)
                }

                Session.Add("Parms", P)

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "PRINT"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    Select Case Sys
                        Case "02", "03"
                            MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                        Case "OR"
                            If IsIssued(PolicyNo, EndNo, LoadNo, Sys) Then
                                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                            Else
                                '"This menu will Undo an Operation", "Undo"
                                ' MsgBox("هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها", 48, "TIBESTY System")
                                'ClientScriptManager.RegisterClientScriptBlock(GetType(IAsyncResult), "CounterScript", "alert('هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها')", True)
                                'ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها")
                                'Me.ClientScript.RegisterStartupScript(Me.GetType(), "msgbox", "alert('FiveDot File uploaded successfully'); alert('TwoDot File uploaded successfully');", True)
                                ' MsgBob(Me, " هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها ")
                                'MsgBox("هذه الفاتورة لم تصدر - لا يمكن طباعتها قيل إصدارها")
                            End If
                        Case Else
                            MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                    End Select

                    'If Sys = "02" Or Sys = "03" Then
                    '    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                    'Else

                    'End If
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "Distribute"

                Dim Order = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "Sys").ToString() 'Request("Sys")
                Dim Br = Request("Sys") 'Session("Branch")

                If Page.IsCallback Then

                    'Dim PolicyNo = GetPolNo(Sys, Order, EndNo, LoadNo)
                    MainGrid.JSProperties("cpMyAttribute") = "Distribute"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    MainGrid.JSProperties("cpNewWindowUrl") = "../Reins/DistPolicy.aspx?OrderNo=" + Order + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo + "&Branch=" + Br + "&Sys=" + Sys + "&PolNo=" + Order
                    'End If
                Else

                End If
            Case "AgentNote"
                Dim Report = ReportsPath & "ForAgent"
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = Right(MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString(), 4)

                Dim p As ReportParameter() = New ReportParameter(3) {}
                p.SetValue(New ReportParameter("PolicyNo", PolicyNo, False), 0)
                p.SetValue(New ReportParameter("EndNo", EndNo, False), 1)
                p.SetValue(New ReportParameter("LoadNo", LoadNo, False), 2)
                p.SetValue(New ReportParameter("Sys", Sys, False), 3)
                Session.Add("Parms", p)

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "AgentNote"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys) & " - إشعار عميل - "

                    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""

                    'If Sys = "02" Or Sys = "03" Then
                    '    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                    'Else

                    'End If
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "Edit"
                Dim EditForm = GetEditForm(Request("Sys"))
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim
                Dim Order = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = Request("Sys")
                'Dim Br = Right(MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString(), 4)

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Edit"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    Select Case Sys
                        Case "01", "27", "OR"
                            MainGrid.JSProperties("cpSize") = 900
                        Case Else
                            MainGrid.JSProperties("cpSize") = 1100
                    End Select

                    MainGrid.JSProperties("cpNewWindowUrl") = EditForm + "?Mode=Edit&OrderNo=" + Order + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo + "&Sys=" + Sys + "&PolNo=" + PolicyNo
                Else
                    'Response.Redirect("../ OutPut / Viewer.aspx?Report=" & Report & "")
                End If
            Case "New"
                Dim EditForm = GetEditForm(Request("Sys"))
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = Right(MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString(), 4)

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Edit"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    MainGrid.JSProperties("cpNewWindowUrl") = EditForm + "?Sys=" + Sys + "&OrderNo=" + PolicyNo + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If

            Case "Issuance"
                MainGrid.JSProperties("cpRowIndex") = e.VisibleIndex
                MainGrid.JSProperties("cpMyAttribute") = "Issuance"
                MainGrid.JSProperties("cpShowIssueConfirmBox") = True

            Case "Renew"
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim OrderNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim
                Dim EditForm = MainGrid.GetRowValues(e.VisibleIndex, "EditForm").ToString()
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
                    Dim dbadapter1 = New SqlDataAdapter("Select Currency,dbo.GetExtraCatName('Cur',Currency) As Name,Interval, CONVERT(varchar, CoverFrom, 111) AS CoverFrom, " _
                    & "CONVERT(varchar, CoverTo, 111) AS CoverTo from Policyfile where polno='" & PolicyNo & "' and endno=" & EndNo & " and loadno=" & LoadNo & "", oCon)
                    dbadapter1.Fill(DBOld)
                    'oCon.Close()
                End Using
                If DBOld.Tables(0).Rows.Item(0).Item("Currency") <> 1 Then
                    Currrate = GetExrate(DBOld.Tables(0).Rows.Item(0).Item("Currency"), DBOld.Tables(0).Rows.Item(0).Item("Name"))
                Else
                    Currrate = 1
                End If

                Select Case DBOld.Tables(0).Rows.Item(0).Item("Interval")
                    Case 3
                        Itrv = DateInterval.Year
                    Case 2
                        Itrv = DateInterval.Month
                    Case 3
                        Itrv = DateInterval.Day
                End Select

                If DBOld.Tables(0).Rows.Item(0).Item("CoverTo") <= Today.Date Then
                    CoverOld = IIf(DateDiff(Itrv, DBOld.Tables(0).Rows.Item(0).Item("CoverFrom"), DBOld.Tables(0).Rows.Item(0).Item("CoverTo")) = 0, 1, DateDiff(Itrv, DBOld.Tables(0).Rows.Item(0).Item("CoverFrom"), DBOld.Tables(0).Rows.Item(0).Item("CoverTo")))
                    CoverStart = DateAdd(DateInterval.Day, 1, Today.Date)
                Else
                    CoverOld = IIf(DateDiff(Itrv, DBOld.Tables(0).Rows.Item(0).Item("CoverFrom"), DBOld.Tables(0).Rows.Item(0).Item("CoverTo")) = 0, 1, DateDiff(Itrv, DBOld.Tables(0).Rows.Item(0).Item("CoverFrom"), DBOld.Tables(0).Rows.Item(0).Item("CoverTo")))
                    CoverStart = DateAdd(DateInterval.Day, 1, DBOld.Tables(0).Rows.Item(0).Item("CoverTo"))
                End If

                'Request("Renew_request").Replace("1", "0")
                Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                SetPm("@TP", DbType.String, Sys, Parm, 0)
                SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 1)
                NewOrder = CallSP("LastOrderNo", Conn, Parm)

                ExecConn("INSERT INTO PolicyFile " _
                    & "(OrderNo, EndNo, LoadNo, EntryDate, CustNo, AgentNo, OwnNo, Broker, SubIns, Currency " _
                    & ", ExcRate, PayType, AccNo, CoverType, Measure, Interval, CoverFrom, CoverTo, LASTNET, NETPRM, NETPRM1, NETPRM2, TAXPRM, CONPRM, STMPRM " _
                    & ", ISSPRM, EXTPRM, TOTPRM, ExtraRate, Stat, Inbox, ForLoss,Stop, Printed, financed, Discount, Branch, UserName, Wakala, Commision) " _
                    & " Select '" & NewOrder & "'," & 0 & "," & 0 & ",'" & Format(Date.Today, "yyyy/MM/dd") & "',CustNo,AgentNo,OwnNo," & 0 & ",'" & Sys & "',Currency" _
                    & "," & Currrate & ",PayType,AccNo,CoverType,Measure,Interval, " _
                    & "" & "CONVERT(DATETIME,'" & Format(CDate(CoverStart), "yyyy-MM-dd") & "',111)," _
                    & "" & "CONVERT(DATETIME,'" & Format(DateAdd(Itrv, CoverOld, CoverStart), "yyyy-MM-dd") & "',111)," _
                    & "LASTNET, NETPRM, NETPRM1,NETPRM2,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM, ExtraRate, Stat," & 0 & ",ForLoss,Stop,Printed,financed,Discount,'" & Session("Branch") & "','" & Session("UserID") & "', Wakala, Commision " _
                    & "from policyfile where polno='" & PolicyNo & "' and endno=0 and loadno=0", Conn)

                Parm = Array.CreateInstance(GetType(SqlParameter), 4)
                SetPm("@table", DbType.String, GetGroupFile(Sys), Parm, 0)
                SetPm("@OldOrder", DbType.String, OrderNo, Parm, 1)
                SetPm("@NewOrder", DbType.String, NewOrder, Parm, 2)
                SetPm("@Sys", DbType.String, Sys, Parm, 3)
                ExecConn(CallSP("BuildInsert", Conn, Parm), Conn)

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Renew"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    MainGrid.JSProperties("cpNewWindowUrl") = EditForm + "?Sys=" + Sys + "&OrderNo=" + NewOrder + "&EndNo=0&LoadNo=0"
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "Delete"
                MainGrid.JSProperties("cpRowIndex") = e.VisibleIndex
                MainGrid.JSProperties("cpMyAttribute") = "Delete"
                MainGrid.JSProperties("cpShowDeleteConfirmBox") = True
            Case Else
                Exit Select
        End Select

    End Sub

    Protected Sub MainGrid_RowDeleting(sender As Object, e As ASPxDataDeletingEventArgs)

        Dim SelectedIndx = MainGrid.FindVisibleIndexByKeyValue(e.Keys("OrderNo"))

        'Dim OrderNo = MainGrid.GetRowValues(e.Values("OrderNo"))
        Dim OrderNo = MainGrid.GetRowValues(SelectedIndx, "OrderNo").ToString().Trim
        'Dim EndNo = MainGrid.GetRowValues(SelectedIndx, "EndNo").ToString()
        'Dim LoadNo = MainGrid.GetRowValues(SelectedIndx, "LoadNo").ToString()
        'Dim Sys = MainGrid.GetRowValues(SelectedIndx, "SubIns").ToString()
        e.Cancel = True
        Parm = Array.CreateInstance(GetType(SqlParameter), 1)
        'SetPm("@TP", DbType.String, Sys, Parm, 0)
        SetPm("@OrderNo", DbType.String, OrderNo, Parm, 0)
        'SetPm("@EndNo", DbType.Int16, EndNo, Parm, 2)
        'SetPm("@LoadNo", DbType.Int16, LoadNo, Parm, 3)
        'SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 4)
        'SetPm("@UserName", DbType.String, Session("User"), Parm, 5)
        CallSP("MoveRequestIW", Conn, Parm)
        MainGrid.DataBind()

    End Sub

    Protected Sub MainGrid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        Select Case e.Parameters
            Case "Issue"
                Dim Report = ReportsPath & PolRep(Request("Sys"))
                Dim Order = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "LoadNo").ToString()
                Dim Sys = Request("Sys")
                Dim Br = Session("Branch")
                ' IssueInWardPolicy(Order, "", EndNo, LoadNo, Sys, Br, Session("User"))
                IssueInWardPolicy(Order, EndNo, LoadNo)

                Dim p As ReportParameter() = New ReportParameter(0) {}

                p.SetValue(New ReportParameter("PolicyNo", Order, False), 0)
                'p.SetValue(New ReportParameter("EndNo", EndNo, False), 1)
                'p.SetValue(New ReportParameter("Sys", Sys, False), 2)

                Session.Add("Parms", p)

                If Page.IsCallback Then

                    'Dim PolicyNo = GetPolNo(Sys, Order, EndNo, LoadNo)
                    MainGrid.JSProperties("cpMyAttribute") = "Distribute"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    MainGrid.JSProperties("cpNewWindowUrl") = "../Reins/DistPolicy.aspx?OrderNo=" + Order + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo + "&Branch=" + Br + "&Sys=" + Sys + "&PolNo=" + Order
                Else

                End If
            Case Else
                MainGrid.DataBind()
                cmbReports.DataBind()

        End Select

    End Sub

    Private Sub MainGrid_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles MainGrid.CustomButtonInitialize
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim Field = IIf(grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString <> "", grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString, "")
        Dim Expired = IsExpired(grid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString())
        Dim Stopped = IsStoped(grid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString(), grid.GetRowValues(e.VisibleIndex, "LoadNo").ToString())
        Dim issued = IIf(grid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim = "", False, True)
        'Dim Sys = grid.GetRowValues(e.VisibleIndex, "SubIns").ToString().Trim
        Select Case e.ButtonID
            Case "Issuance"
                If Field <> "" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "Delete"
                If Field <> "" Then
                    e.Enabled = False
                    e.Visible = DevExpress.Utils.DefaultBoolean.False
                End If
            Case "Renew"
                If Trim(Field) = "" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "Edit"
                'If Not Expired Then
                '    e.Enabled = False
                '    e.Visible = DefaultBoolean.False
                'End If
                'If issued Then
                '    e.Enabled = False
                '    e.Visible = DefaultBoolean.False
                'Else
                '    e.Enabled = True
                '    e.Visible = DefaultBoolean.True
                'End If
                'If Stopped Then
                '    e.Enabled = False
                '    e.Visible = DefaultBoolean.False
                'End If
                'If Sys = "05" Or Sys = "04" Or Sys = "14" Or Sys = "15" Then
                '    e.Enabled = True
                '    e.Visible = DevExpress.Utils.DefaultBoolean.True
                'End If
            Case "AgentNote"
                'If Sys <> "04" And Sys <> "05" Then
                '    e.Enabled = False
                '    e.Visible = DefaultBoolean.False
                'End If
            Case "Distribute"
                'And issued
                'If Sys <> "01" And Sys <> "OR" And Sys <> "02" And Sys <> "03" Then
                '    If Field = "" Then
                '        e.Enabled = False
                '        e.Visible = DevExpress.Utils.DefaultBoolean.False
                '    End If
                'Else
                '    e.Enabled = False
                '    e.Visible = DefaultBoolean.False
                'End If
        End Select
    End Sub

    Private Sub CmbReports_Callback(sender As Object, e As CallbackEventArgsBase) Handles cmbReports.Callback
        Dim cmbsplited = e.Parameter.Split("|")

        Select Case cmbsplited(0)
            Case "IssuClip3", "DailyJournalS"
                Dim br As Boolean = IIf(Right(Branch.Text, 2) <> "00", False, True)
                Dim p As ReportParameter() = New ReportParameter(3) {}
                p.SetValue(New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True), 0)
                p.SetValue(New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True), 1)
                p.SetValue(New ReportParameter("SubSystem", Sys.Text, True), 2)
                p.SetValue(New ReportParameter("BranchNo", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 3)
                'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                Session.Add("Parms", p)
                cmbReports.JSProperties("cpResult") = cmbsplited(1)
                cmbReports.JSProperties("cpMyAttribute") = "IssuClip"
                cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
            Case "IssuClipOverall"
                Dim br As Boolean = IIf(Right(Branch.Text, 2) <> "00", False, True)
                Dim p As ReportParameter() = New ReportParameter(3) {}

                p.SetValue(New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True), 0)
                p.SetValue(New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True), 1)
                p.SetValue(New ReportParameter("Sys", Sys.Text, True), 2)
                p.SetValue(New ReportParameter("BranchNo", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 3)
                'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                Session.Add("Parms", p)
                cmbReports.JSProperties("cpResult") = cmbsplited(1)
                cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
            Case "IssuClipOverallUsers"
                Dim br As Boolean = IIf(Right(Branch.Text, 2) <> "00", False, True)

                Dim p As ReportParameter() = New ReportParameter(3) {}

                p.SetValue(New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True), 0)
                p.SetValue(New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True), 1)
                p.SetValue(New ReportParameter("Sys", Sys.Text, True), 2)
                p.SetValue(New ReportParameter("BranchNo", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 3)
                'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                Session.Add("Parms", p)
                If e.Parameter <> "" And Right(Branch.Text, 2) = "00" Then
                    cmbReports.JSProperties("cpResult") = cmbsplited(1)
                    cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                    cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                Else

                End If

            Case "AllIssues"
                If Right(Branch.Text, 2) = "00" Then
                    Dim br As Boolean = IIf(Right(Branch.Text, 2) <> "00", False, True)

                    Dim p As ReportParameter() = New ReportParameter(4) {}

                    p.SetValue(New ReportParameter("FDate", Format(Today.Date, "yyyy/MM/dd"), True), 0)
                    p.SetValue(New ReportParameter("TDate", Format(Today.Date, "yyyy/MM/dd"), True), 1)
                    p.SetValue(New ReportParameter("BranchNo", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 2)
                    p.SetValue(New ReportParameter("Sys", Sys.Text, True), 3)
                    p.SetValue(New ReportParameter("Agents", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)

                    'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                    Session.Add("Parms", p)
                    cmbReports.JSProperties("cpResult") = cmbsplited(1)
                    cmbReports.JSProperties("cpMyAttribute") = "AllIssues"
                    cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                Else

                End If

            Case Else
                Dim br As Boolean = IIf(Right(Branch.Text, 2) <> "00", False, True)
                'Dim p As ReportParameter() = New ReportParameter(3) {}

                'p.SetValue(New ReportParameter("FDate", Format(Today.Date, "yyyy/MM/dd"), True), 0)
                'p.SetValue(New ReportParameter("TDate", Format(Today.Date, "yyyy/MM/dd"), True), 1)
                'p.SetValue(New ReportParameter("Branch", Branch.Text, False), 2)
                'p.SetValue(New ReportParameter("Sys", Sys.Text, False), 3)

                'Session.Add("Parms", p)

                If e.Parameter <> "" Then
                    cmbReports.JSProperties("cpResult") = cmbsplited(1)
                    cmbReports.JSProperties("cpMyAttribute") = ""
                    cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                Else

                End If
        End Select
    End Sub

    Protected Sub MainGrid_ToolbarItemClick(source As Object, e As ASPxGridViewToolbarItemClickEventArgs)
        Select Case e.Item.Name
            'Case "CustomExportToXLS"
            '    grid.ExportXlsToResponse(New DevExpress.XtraPrinting.XlsExportOptionsEx With {.ExportType = ExportType.WYSIWYG})
            'Case "CustomExportToXLSX"
            '    grid.ExportXlsxToResponse(New DevExpress.XtraPrinting.XlsxExportOptionsEx With {.ExportType = ExportType.WYSIWYG})
            Case "NewPolicy"
                'If Page.IsCallback Then
                MainGrid.JSProperties("cpMyAttribute") = "New"
                MainGrid.JSProperties("cpResult") = GetSysName(Request("Sys"))
                Select Case Sys.Value
                    Case "01", "27", "OR"
                        MainGrid.JSProperties("cpSize") = 900
                    Case Else
                        MainGrid.JSProperties("cpSize") = 1100
                End Select

                MainGrid.JSProperties("cpNewWindowUrl") = GetEditForm(Request("Sys"))
            Case "ExtraSearch"
                MainGrid.JSProperties("cpMyAttribute") = "Search"
                MainGrid.JSProperties("cpResult") = GetSysName(Request("Sys"))
                MainGrid.JSProperties("cpNewWindowUrl") = "../Policy/SearchForm.aspx?SystemTable=" + GetGroupFile(Request("Sys")) + "&Sys=" + Request("Sys")
            Case Else
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
                    'If IsIssued(e.GetValue("OrderNo"), e.GetValue("EndNo"), e.GetValue("LoadNo"), e.GetValue("SubIns")) Then
                    e.Cell.ForeColor = Color.DarkGreen
                    e.Cell.HorizontalAlign = HorizontalAlign.Center
                    'Else
                    '    e.Cell.BackColor = Drawing.Color.LightGray
                    '    e.Cell.ForeColor = Drawing.Color.DarkGreen
                    '    e.Cell.HorizontalAlign = HorizontalAlign.Center
                    '    e.Cell.Text = e.GetValue("PolNo") + " / عرض / ملحق / إشعار"
                    'End If
                End If
            Else
                e.Cell.BackColor = Color.Red
                e.Cell.ForeColor = Color.White
                e.Cell.Text = "فاتورة مبدئية"
                e.Cell.HorizontalAlign = HorizontalAlign.Center
            End If
            'Dim PolicyRow As DevExpress.Web.Rendering.GridViewTableDataRow = e.Cell.Parent
        End If

    End Sub

    Private Sub MainGrid_DataBound(sender As Object, e As EventArgs) Handles MainGrid.DataBound
        'If Sys.Value <> "05" Then
        '    MainGrid.Columns("LoadNo").Visible = False
        'Else
        '    MainGrid.Columns("LoadNo").Visible = True
        'End If

    End Sub

End Class