Imports System.Data.SqlClient
Imports System.Drawing
Imports DevExpress.Utils
Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports Microsoft.Reporting.WebForms

Public Class Policy_Default
    Inherits Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        If IsNothing(Session("UserInfo")) Then
            FormsAuthentication.SignOut()
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            Exit Sub

            'End If
            'If myList Is Nothing Then
            '    FormsAuthentication.SignOut()
            '    'FormsAuthentication.RedirectToLoginPage()
            '    'ASPxWebControl.RedirectOnCallback(TARGET_URL)
            '    ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
        Else
            Call SetUserPermNAV(TryCast(FindControlRecursive(Form, "SideBar"), ASPxNavBar), myList.ToArray, 1)

            'If Not IsNotificationServiceRunning() Then
            '    StartNotificationService()
            'End If
            'Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMS-DBConnectionString").ConnectionString)
            '    If con.State = ConnectionState.Open Then
            '        con.Close()
            '    Else

            '    End If
            '    con.Open()

            '    SqlDataSource.ConnectionString = con.ConnectionString

            '    con.Close()
            'End Using
        End If
        If Sys.Text = "0" Then

        Else
            If CType(Mid(myList(1), InStr(1, myList(1), Sys.Text) + 3, 1), Short) <= 2 Then
                TryCast(MainGrid.Columns("UserName"), GridViewDataColumn).AutoFilterBy(Session("UserID"))
                'MainGrid.DataSourceID = "SqlDataSource"
            Else
                'MainGrid.DataSourceID = "SqlDataSource"
            End If
        End If
        'If Sys.Text = "0" Then

        'Else
        '    If CType(Mid(myList(1), InStr(1, myList(1), Sys.Text) + 3, 1), Short) <= 2 Then
        '        MainGrid.DataSourceID = "SqlDataSourceLog"
        '    Else
        '        MainGrid.DataSourceID = "SqlDataSource"
        '    End If
        'End If

        'MainGrid.DataBind()

        'Session("Order") = ""
        Session.Remove("Parms")
        Session.Remove("Order")
        Session.Remove("End")
        cmbReports.DataBind()
    End Sub

    Private Sub MainGrid_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles MainGrid.CustomButtonCallback
        Select Case e.ButtonID
            Case "Print"
                'Dim Report = ReportsPath & MainGrid.GetRowValues(e.VisibleIndex, "Report").ToString()
                'Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                'Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                'Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                'Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                'Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()
                'Dim Br = Right(MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString(), 4)
                Dim PolicyRef = MainGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Report = IIf(Sys = "OR", MainGrid.GetRowValues(e.VisibleIndex, "Report").ToString() & "?policy=" & PolicyNo, ReportsPath & MainGrid.GetRowValues(e.VisibleIndex, "Report").ToString())
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()
                Dim Pol = MainGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString()

                If IsStoped(Pol, EndNo, LoadNo) Then
                    Report = ReportsPath & "ReIssu"
                Else
                    'SelPolicyRep = SelectReport(Mid(PolNo.Text, 12, 2), Request("sys"))
                End If

                Dim P As New List(Of ReportParameter)()

                If Sys <> "OC" Then
                    'Dim p As ReportParameter() = New ReportParameter(2) {}
                    'P.SetValue(New ReportParameter("PolicyNo", PolicyNo, False), 0)
                    P.Add(New ReportParameter("PolicyNo", PolicyNo, False))
                    P.Add(New ReportParameter("EndNo", EndNo, False))
                    P.Add(New ReportParameter("Sys", Sys, False))

                    Session.Add("Parms", P)
                Else
                    'Dim p As ReportParameter() = New ReportParameter(3) {}
                    P.Add(New ReportParameter("PolicyNo", PolicyNo, False))
                    P.Add(New ReportParameter("EndNo", EndNo, False))
                    P.Add(New ReportParameter("LoadNo", LoadNo, False))
                    P.Add(New ReportParameter("Sys", Sys, False))

                    Session.Add("Parms", P)
                End If

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "PRINT"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    Select Case Sys
                        Case "PH"
                            If IsIssued(PolicyNo, EndNo, LoadNo, Sys) Then
                                MainGrid.JSProperties("cpNewWindowUrl") = If(Sys = "PH", "../Reporting/Previewer.aspx?Report=" & Report & "", "../Reporting/PreviewPDF.aspx?Report=" & Report & "")
                            Else

                            End If
                            'Case "OR"
                            '    MainGrid.JSProperties("cpNewWindowUrl") = MainGrid.GetRowValues(e.VisibleIndex, "Report").ToString() & "?policyNumber=" & PolicyNo.TrimEnd & "&UserName=" & Session("UserLogin").ToString.TrimEnd & "&Branch=" & Session("Branch").ToString.TrimEnd
                        Case "OR"
                            If PolicyNo = PolicyRef Then
                                MainGrid.JSProperties("cpNewWindowUrl") = Report
                            Else
                                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=/IMSReports/OrangeCard"
                            End If
                        Case Else
                            If Sys = "01" Or Sys = "27" Or Sys = "08" Or Sys = "04" Then
                                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                            Else
                                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                            End If

                    End Select

                    'If Sys = "02" Or Sys = "03" Then
                    '    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                    'Else

                    'End If
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "CreditNote"
                Dim Report = ReportsPath & "CreditNote"
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", PolicyNo, False),
                    New ReportParameter("EndNo", EndNo, False),
                    New ReportParameter("LoadNo", LoadNo, False),
                    New ReportParameter("Sys", Sys, False)
                }

                Session.Add("Parms", P)

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "CreditNote"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys) & " - إشعار دائن - "

                    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                Else

                End If
            Case "Payment"

                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()
                Dim Report = If(IsBranch(Br), ReportsPath & "Cashier", ReportsPath & "AgentsReciept")
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()

                If IsBranch(Br) Then
                    Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", PolicyNo, False),
                    New ReportParameter("EndNo", EndNo, False),
                    New ReportParameter("LoadNo", LoadNo, False),
                    New ReportParameter("Sys", Sys, False)
                }
                    Session.Add("Parms", P)
                Else
                    Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("ReciptNo", PolicyNo, False)
                }
                    Session.Add("Parms", P)
                End If

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Cashier"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys) & If(IsBranch(Br), " - أمر توريد - ", " - إيصال فبض رقم -")
                    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "DebitNote"
                Dim Report = ReportsPath & "DebitNote"
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", PolicyNo, False),
                    New ReportParameter("EndNo", EndNo, False),
                    New ReportParameter("LoadNo", LoadNo, False),
                    New ReportParameter("Sys", Sys, False)
                }

                Session.Add("Parms", P)

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "DebitNote"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys) & " - إشعار مدين - "

                    MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "Edit"
ss:             Dim EditForm = MainGrid.GetRowValues(e.VisibleIndex, "EditForm").ToString()
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Edit"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    Select Case Sys
                        Case "01", "27", "OR", "04"
                            MainGrid.JSProperties("cpSize") = 900
                        Case Else
                            MainGrid.JSProperties("cpSize") = 1100
                    End Select
                    MainGrid.JSProperties("cpNewWindowUrl") = EditForm + "?Sys=" + Sys + "&OrderNo=" + PolicyNo + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "New"
                Dim EditForm = MainGrid.GetRowValues(e.VisibleIndex, "EditForm").ToString()
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()

                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Edit"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    If Sys = "OR" Then
                        MainGrid.JSProperties("cpNewWindowUrl") = EditForm
                    Else
                        MainGrid.JSProperties("cpNewWindowUrl") = EditForm + "?Sys=" + Sys + "&OrderNo=" + PolicyNo + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo
                    End If
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If

            Case "Issuance"
                Dim EditForm = MainGrid.GetRowValues(e.VisibleIndex, "EditForm").ToString()
                Dim PolicyNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()
                Dim Cust = MainGrid.GetRowValues(e.VisibleIndex, "CustName").ToString()
                Dim EntryDte = MainGrid.GetRowValues(e.VisibleIndex, "EntryDate")
                Dim NET = MainGrid.GetRowValues(e.VisibleIndex, "NETPRM")
                Dim TOT = MainGrid.GetRowValues(e.VisibleIndex, "TOTPRM")
                Dim CoverFrom = GetCoverFrom(PolicyNo, EndNo, Sys)

                Using oCon As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                    If oCon.State = ConnectionState.Open Then
                        oCon.Close()
                    Else

                    End If
                    oCon.Open()
                    Dim TotPrm As New DataSet
                    Dim dbadapter = New SqlDataAdapter(String.Format("select NETPRM+TAXPRM+CONPRM+STMPRM+ISSPRM As Total From PolicyFile WHERE OrderNo='{0}' and EndNo={1} and LoadNo={2} and subins='{3}'", PolicyNo, EndNo, LoadNo, Sys), oCon)
                    dbadapter.Fill(TotPrm)
                    If TotPrm.Tables(0).Rows.Item(0).Item("Total") <> TOT Or NET = TOT Then
                        GoTo ss
                    End If
                End Using

                If NET <> GetSysNet(PolicyNo, EndNo, LoadNo, Sys) And IsGroupedSys(Sys) Then
                    GoTo ss
                End If

                Select Case Sys
                    Case "01", "04"
                        If CoverFrom <= Today.Date And EndNo = 0 Then
                            ExecConn("Update PolicyFile Set CoverFrom = DATEADD(day, 1, GETDATE()), CoverTo = DATEADD(year, 1, DATEADD(day, 0, GETDATE())) Where OrderNo='" & PolicyNo & "'", Conn)
                            GoTo ss
                        Else
                            If NET <> GetSysNet(PolicyNo, EndNo, LoadNo, Sys) And EndNo = 0 Then
                                GoTo ss
                            End If
                            'MainGrid.JSProperties("cpMyAttribute") = "IssuSerial"
                            'MainGrid.JSProperties("cpResult") = GetSysName(Sys) & " للزبون  " & Cust
                            'MainGrid.JSProperties("cpNewWindowUrl") = "../Policy/IssueWithSerial.aspx?Sys=" + Sys + "&OrderNo=" + PolicyNo + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo + "&Br=" + Br
                            MainGrid.JSProperties("cpRowIndex") = e.VisibleIndex
                            MainGrid.JSProperties("cpMyAttribute") = "Issuance"
                            MainGrid.JSProperties("cpCust") = "تأكيد إصدار وثيقة للزبون " & Cust & "/" & GetSysName(Sys)
                            MainGrid.JSProperties("cpShowIssueConfirmBox") = True
                        End If

                    Case "27"
                        If CoverFrom < Today.Date And EndNo = 0 Then
                            GoTo ss
                        Else
                            If NET <> GetSysNet(PolicyNo, EndNo, LoadNo, Sys) And EndNo = 0 Then
                                GoTo ss
                            End If
                            'MainGrid.JSProperties("cpMyAttribute") = "IssuSerial"
                            'MainGrid.JSProperties("cpResult") = GetSysName(Sys) & " للزبون  " & Cust
                            'MainGrid.JSProperties("cpNewWindowUrl") = "../Policy/IssueWithSerial.aspx?Sys=" + Sys + "&OrderNo=" + PolicyNo + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo + "&Br=" + Br
                            MainGrid.JSProperties("cpRowIndex") = e.VisibleIndex
                            MainGrid.JSProperties("cpMyAttribute") = "Issuance"
                            MainGrid.JSProperties("cpCust") = "تأكيد إصدار وثيقة للزبون " & Cust & "/" & GetSysName(Sys)
                            MainGrid.JSProperties("cpShowIssueConfirmBox") = True
                        End If
                    Case Else
                        If NET <> GetSysNet(PolicyNo, EndNo, LoadNo, Sys) Then
                            GoTo ss
                        End If
                        MainGrid.JSProperties("cpRowIndex") = e.VisibleIndex
                        MainGrid.JSProperties("cpMyAttribute") = "Issuance"
                        MainGrid.JSProperties("cpCust") = "تأكيد إصدار وثيقة للزبون " & Cust & "/" & GetSysName(Sys)
                        MainGrid.JSProperties("cpShowIssueConfirmBox") = True

                End Select
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
                    Dim dbadapter1 = New SqlDataAdapter("Select Currency,EndNo,dbo.GetExtraCatName('Cur',Currency) As Name,Interval,Measure, CONVERT(varchar, CoverFrom, 111) AS CoverFrom, " _
                    & "CONVERT(varchar, CoverTo, 111) AS CoverTo from Policyfile where polno='" & PolicyNo & "' and endno=" & EndNo & " and loadno=" & LoadNo & "", oCon)
                    dbadapter1.Fill(DBOld)
                    'oCon.Close()
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

                ExecConn("INSERT INTO PolicyFile " _
                    & "(OrderNo, EndNo, LoadNo, EntryDate, CustNo, OldPolicy, AgentNo, OwnNo, Broker, SubIns, Currency " _
                    & ", ExcRate, PayType, AccountNo, CoverType, Measure, Interval, CoverFrom, CoverTo, LASTNET, NETPRM, TAXPRM, CONPRM, STMPRM " _
                    & ", ISSPRM, EXTPRM, TOTPRM, ExtraRate, Stat, Inbox, ForLoss,Stop, Printed, financed, Discount, Branch, UserName, Commision) " _
                    & " Select '" & NewOrder & "'," & 0 & "," & 0 & ",CONVERT(DATETIME,getdate(),111) ,CustNo,'" & "تجديد على الوثيقة رقم /" & PolicyNo + "//" + EndNo & "/" & MyBase.Session("User") & "', AgentNo, OwnNo, " & 0 & ",'" & Sys & "',Currency" _
                    & "," & Currrate & ",1, '0', CoverType,Measure,Interval, " _
                    & "" & "CONVERT(DATETIME,'" & Format(CoverStart, "yyyy-MM-dd") & "',111)," _
                    & "" & "CONVERT(DATETIME,'" & Format(DateAdd(DateInterval.Day, -1, DateAdd(Itrv, CoverOld, CoverStart)), "yyyy-MM-dd") & "',111)," _
                    & "LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM," & GetIssuVal(Sys, 0) & ", EXTPRM, TOTPRM, ExtraRate, Stat, " & 0 & ", ForLoss,Stop, Printed, 0, 0,'" & MyBase.Session("Branch") & "','" & MyBase.Session("UserID") & "',0 " _
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
                If Page.IsCallback Then
                    MainGrid.JSProperties("cpMyAttribute") = "Renew"
                    MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                    MainGrid.JSProperties("cpNewWindowUrl") = EditForm + "?Sys=" + Sys + "&OrderNo=" + NewOrder + "&EndNo=0&LoadNo=0"
                Else
                    'Response.Redirect("../OutPut/Viewer.aspx?Report=" & Report & "")
                End If
            Case "Delete"
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()
                Dim Cust = MainGrid.GetRowValues(e.VisibleIndex, "CustName").ToString()

                MainGrid.JSProperties("cpRowIndex") = e.VisibleIndex
                MainGrid.JSProperties("cpMyAttribute") = "Delete"
                MainGrid.JSProperties("cpCust") = "تأكيد إلغاء الطلب للزبون " & Cust & "/" & GetSysName(Sys)
                MainGrid.JSProperties("cpShowDeleteConfirmBox") = True
            Case "RequestCancel"
                Dim Sys = MainGrid.GetRowValues(e.VisibleIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(e.VisibleIndex, "Br").ToString()
                Dim OrderNo = MainGrid.GetRowValues(e.VisibleIndex, "OrderNo").ToString()
                Dim PolNo = MainGrid.GetRowValues(e.VisibleIndex, "PolNo").ToString()
                Dim EndNo = MainGrid.GetRowValues(e.VisibleIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(e.VisibleIndex, "LoadNo").ToString()

                MainGrid.JSProperties("cpMyAttribute") = "RequestCancel"
                MainGrid.JSProperties("cpResult") = PolNo & " - طلب إلغاء وثيقة رقم  - "
                MainGrid.JSProperties("cpNewWindowUrl") = "../Policy/CancelRequest.aspx?OrderNo=" & OrderNo & "&Sys=" & Sys & "&Br=" & Br & "&PolNo=" & PolNo & "&EndNo=" & EndNo & "&LoadNo=" & LoadNo & ""
            Case Else
                Exit Select
        End Select

    End Sub

    Protected Sub MainGrid_RowDeleting(sender As Object, e As ASPxDataDeletingEventArgs)

        Dim SelectedIndx = MainGrid.FindVisibleIndexByKeyValue(e.Keys("OrderNo"))

        Dim OrderNo = MainGrid.GetRowValues(SelectedIndx, "OrderNo").ToString().Trim
        Dim EndNo = MainGrid.GetRowValues(SelectedIndx, "EndNo").ToString()
        Dim LoadNo = MainGrid.GetRowValues(SelectedIndx, "LoadNo").ToString()
        Dim Sys = MainGrid.GetRowValues(SelectedIndx, "SubIns").ToString()
        e.Cancel = True
        Parm = Array.CreateInstance(GetType(SqlParameter), 6)
        SetPm("@TP", DbType.String, Sys, Parm, 0)
        SetPm("@OrderNo", DbType.String, OrderNo, Parm, 1)
        SetPm("@EndNo", DbType.Int16, EndNo, Parm, 2)
        SetPm("@LoadNo", DbType.Int16, LoadNo, Parm, 3)
        SetPm("@BranchNo", DbType.String, Session("Branch"), Parm, 4)
        SetPm("@UserName", DbType.String, Session("UserID"), Parm, 5)

        CallSP("MoveRequest", Conn, Parm)
        MainGrid.DataBind()

    End Sub

    Protected Sub MainGrid_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs)
        'If String.IsNullOrEmpty(e.Parameters) Then
        '    MainGrid.DataBind()
        '    Exit Sub
        'Else

        'End If
        Select Case e.Parameters
            Case "Retrieve", ""
                MainGrid.DataBind()
            Case "Issue"
                Dim Report = "/IMSReports/" & MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "Report").ToString()
                Dim Order = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "OrderNo").ToString().Trim 'ASPxGridView.GetRowValues(e.VisibleIndex, ASPxGridView.KeyFieldName)
                Dim EndNo = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "EndNo").ToString()
                Dim LoadNo = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "LoadNo").ToString()
                Dim Sys = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "SubIns").ToString()
                Dim Br = MainGrid.GetRowValues(MainGrid.FocusedRowIndex, "Br").ToString()

                IssuePolicy(Order, "", EndNo, LoadNo, Sys, Br, Session("UserID"), 0)
                'Dim p As ReportParameter() = New ReportParameter(2) {}

                'p.SetValue(New ReportParameter("PolicyNo", Order, False), 0)
                'p.SetValue(New ReportParameter("EndNo", EndNo, False), 1)
                'p.SetValue(New ReportParameter("Sys", Sys, False), 2)

                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("PolicyNo", Order, False),
                    New ReportParameter("EndNo", EndNo, False),
                    New ReportParameter("Sys", Sys, False)
                }

                Session.Add("Parms", P)

                If Page.IsCallback Then

                    If Sys = "01" Or Sys = "OR" Or Sys = "27" Or Sys = "02" Or Sys = "03" Or Sys = "08" Or Sys = "07" Or Sys = "PH" Or Sys = "04" Then
                        MainGrid.JSProperties("cpMyAttribute") = "PRINT"
                        MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                        Select Case Sys
                            Case "01", "OR", "27", "07", "08", "PH"
                                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/PreviewPDF.aspx?Report=" & Report & ""
                            Case Else
                                MainGrid.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & Report & ""
                        End Select
                    Else
                        Dim PolicyNo = GetPolNo(Sys, Order, EndNo, LoadNo)
                        MainGrid.JSProperties("cpMyAttribute") = "Distribute"
                        MainGrid.JSProperties("cpResult") = GetSysName(Sys)
                        MainGrid.JSProperties("cpNewWindowUrl") = "../Reins/DistPolicy.aspx?OrderNo=" + Order + "&EndNo=" + EndNo + "&LoadNo=" + LoadNo + "&Branch=" + Br + "&Sys=" + Sys + "&PolNo=" + PolicyNo
                    End If
                Else

                End If
            Case Else
                MainGrid.DataBind()
                'cmbReports.DataBind()
        End Select

    End Sub

    Private Sub MainGrid_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles MainGrid.CustomButtonInitialize
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        If IsNothing(Session("UserInfo")) Then
            FormsAuthentication.SignOut()
            'FormsAuthentication.RedirectToLoginPage()
            'ASPxWebControl.RedirectOnCallback(TARGET_URL)
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            Exit Sub
        End If
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim issued As Boolean = IsIssued(grid.GetRowValues(e.VisibleIndex, "OrderNo").ToString, grid.GetRowValues(e.VisibleIndex, "EndNo"), grid.GetRowValues(e.VisibleIndex, "LoadNo"), grid.GetRowValues(e.VisibleIndex, "SubIns").ToString)
        Dim Field = IIf(grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString <> "", grid.GetRowValues(e.VisibleIndex, "IssuDate").ToString, "")
        Dim Expired As Boolean = grid.GetRowValues(e.VisibleIndex, "Expired") 'IsExpired(grid.GetRowValues(e.VisibleIndex, "OrderNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString())
        Dim Stopped As Boolean = grid.GetRowValues(e.VisibleIndex, "Stoped") 'IsStoped(grid.GetRowValues(e.VisibleIndex, "PolNo").ToString().Trim, grid.GetRowValues(e.VisibleIndex, "EndNo").ToString(), grid.GetRowValues(e.VisibleIndex, "LoadNo").ToString())
        Dim PayTp = grid.GetRowValues(e.VisibleIndex, "PayAs").ToString
        Dim Sys = grid.GetRowValues(e.VisibleIndex, "SubIns").ToString().Trim
        Dim Fianced = grid.GetRowValues(e.VisibleIndex, "Financed").ToString().Trim
        Dim WithCommision = IIf(CDbl(grid.GetRowValues(e.VisibleIndex, "commision")) = 0, False, True)
        Dim PermLevel = CType(Mid(myList(1), InStr(1, myList(1), grid.GetRowValues(e.VisibleIndex, "SubIns").ToString().Trim) + 3, 1), Short)
        Select Case e.ButtonID
            Case "Issuance"
                If Field <> "" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "Delete"
                If Field <> "" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
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
                If Sys = "OR" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "Edit"
                'TEMPORARY CLOSE TO ACTIVATE ENDORSMENTs

                If Not Expired And Field <> "" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If

                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Sys = "OC" Or Sys = "MC" Or Sys = "MB" Or Sys = "MA" Or Sys = "ER" Or Sys = "CR" Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If
                If Sys = "OR" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "CreditNote"
                'If Sys <> "OC" And Sys <> "MC" And Sys <> "MB" And Sys <> "MA" Then
                '    e.Enabled = WithCommision
                '    e.Visible = DefaultBoolean.(WithCommision)
                'End If
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Not issued Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                Else
                    If WithCommision Then
                        e.Enabled = True
                        e.Visible = DefaultBoolean.True
                    Else
                        e.Enabled = False
                        e.Visible = DefaultBoolean.False
                    End If
                End If
            Case "Payment"
                If Not issued Or PayTp <> "نقداً" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
                If Fianced Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If
            Case "DebitNote"
                If Not issued Or PayTp = "نقداً" Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                Else
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                End If
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                End If
            Case "RequestCancel"
                If Stopped Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                    Exit Select
                End If
                If Not issued Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                    Exit Select
                End If
                If Not Expired Then
                    e.Enabled = False
                    e.Visible = DefaultBoolean.False
                    Exit Select
                End If

                If PermLevel <= 3 Then
                    e.Enabled = True
                    e.Visible = DefaultBoolean.True
                Else
                    If IsBranchOffice(Session("Branch")) Or IsBranch(Session("Branch")) Then
                        e.Enabled = False
                        e.Visible = DefaultBoolean.False
                    Else
                        e.Enabled = True
                        e.Visible = DefaultBoolean.True
                    End If

                End If
            Case Else
                Exit Select
        End Select
    End Sub

    Private Sub CmbReports_Callback(sender As Object, e As CallbackEventArgsBase) Handles cmbReports.Callback
        If String.IsNullOrEmpty(e.Parameter) Then Exit Sub
        Dim cmbsplited = e.Parameter.Split("|")
        If cmbsplited.Length = 1 Or String.IsNullOrEmpty(e.Parameter) Then Exit Sub
        Dim LogList = DirectCast(Session("UserInfo"), List(Of String))

        If IsNothing(Session("UserInfo")) Then
            FormsAuthentication.SignOut()
            ASPxWebControl.RedirectOnCallback(String.Format("~/SystemManage/LogIn.aspx?ReturnUrl={0}", AppRelativeVirtualPath))
            Exit Sub
        End If

        If Not IsBranch(Session("Branch")) Then
            Dim br As Boolean = IIf(Not IsBranch(Session("Branch")), False, True)

            If CType(Mid(LogList(1), InStr(1, LogList(1), Sys.Text) + 3, 1), Short) <= 2 Then
                Select Case cmbsplited(0)
                    Case "AgentComissions"
                        Dim P As New List(Of ReportParameter) From {
     New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
     New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
     New ReportParameter("Sys", Sys.Text, False),
     New ReportParameter("BranchNo", GetMainBranch(Session("Branch")), False),
     New ReportParameter("Agents", Branch.Text, False)
 }

                        'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                        Session.Add("Parms", P)
                        If e.Parameter <> "" Then
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0)
                        Else

                        End If
                    Case Else
                        Dim P As New List(Of ReportParameter) From {
      New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
      New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
      New ReportParameter("Sys", Sys.Text, False),
      New ReportParameter("BranchNo", GetMainBranch(Session("Branch")), False),
      New ReportParameter("Agents", Branch.Text, False),
      New ReportParameter("User", Session("UserID").ToString, False)
  }

                        'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                        Session.Add("Parms", P)
                        If e.Parameter <> "" Then
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & "IssuClipOverallUsers"
                        Else

                        End If
                End Select
            Else
                Select Case cmbsplited(1)

                    Case "حافظة الإنتاج"
                        Dim Reprt As String
                        If IsOfficesManager(Session("UserID")) Then
                            Reprt = "IssuClipOverAllOfficeManagers"
                            Dim P As New List(Of ReportParameter) From {
                         New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                         New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                         New ReportParameter("Sys", Sys.Text, True),
                         New ReportParameter("User", Session("UserID").ToString, False),
                         New ReportParameter("BranchNo", Branch.Text, IsOfficesManager(Session("UserID")))
                     }
                            Session.Add("Parms", P)
                        Else
                            Reprt = "IssuClipOverAll"
                            Dim P As New List(Of ReportParameter) From {
                         New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                         New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                         New ReportParameter("Sys", Sys.Text, True),
                         New ReportParameter("BranchNo", GetMainBranch(Branch.Text), False),
                         New ReportParameter("Agents", Branch.Text, False)
                     }
                            Session.Add("Parms", P)
                        End If

                        If e.Parameter <> "" Then
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & Reprt & ""
                        Else

                        End If
                    Case "إحصائية مجمعة (للمستخدمين)"

                        Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                    New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                    New ReportParameter("Sys", Sys.Text, True),
                    New ReportParameter("BranchNo", GetMainBranch(Branch.Text), False),
                    New ReportParameter("Agents", Branch.Text, False),
                    New ReportParameter("User", Session("UserID").ToString, True)
                }

                        'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                        Session.Add("Parms", P)
                        If e.Parameter <> "" Then
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & "IssuClipOverallUsers"
                        Else

                        End If
                    Case "عمولات الوكلاء"
                        Dim P As New List(Of ReportParameter) From {
                          New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                          New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                          New ReportParameter("BranchNo", GetMainBranch(Session("Branch")), False),
                          New ReportParameter("Agents", Branch.Text, False),
                          New ReportParameter("Sys", Sys.Text, True)
                        }
                        Session.Add("Parms", P)

                        'And (IsBranch(Session("Branch")) Or IsBranchOffice(Session("Branch")))
                        If e.Parameter <> "" Then
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = ""
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                        Else

                        End If
                    Case "كشف مفصل بالسيارات"
                        Dim P As New List(Of ReportParameter) From {
                     New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                     New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                        New ReportParameter("BranchNo", GetMainBranch(Branch.Text), False),
                        New ReportParameter("Agents", Branch.Text, False),
                        New ReportParameter("Sys", Sys.Text, False)
                 }
                        Session.Add("Parms", P)

                        If e.Parameter <> "" Then
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = ""
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                        Else

                        End If
                End Select
            End If
        Else
            If CType(Mid(LogList(1), InStr(1, LogList(1), Sys.Text) + 3, 1), Short) <= 2 Then
                Dim P As New List(Of ReportParameter) From {
                    New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                    New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                    New ReportParameter("Sys", Sys.Text, False),
                    New ReportParameter("BranchNo", Branch.Text, False),
                    New ReportParameter("Agents", Branch.Text, False),
                    New ReportParameter("User", Session("UserID").ToString, False)
                }

                Session.Add("Parms", P)
                If e.Parameter <> "" Then
                    cmbReports.JSProperties("cpResult") = cmbsplited(1)
                    cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                    cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & "IssuClipOverallUsers"
                Else

                End If
            Else
                Select Case cmbsplited(0)
                    Case "IssuClipOverall", "IssuClip"
                        Dim br As Boolean = IIf(Not IsBranch(Session("Branch")), False, True)

                        Dim P As New List(Of ReportParameter) From {
                            New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                            New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                            New ReportParameter("Sys", Sys.Text, True),
                            New ReportParameter("BranchNo", Branch.Text, IsHeadQuarter(Session("Branch")))
                        }

                        Session.Add("Parms", P)
                        cmbReports.JSProperties("cpResult") = cmbsplited(1)
                        cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                        cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & "IssuClipOverall" & ""
                    Case "BrokersComissions"
                        Dim br As Boolean = IIf(Not IsBranch(Session("Branch")), False, True)

                        Dim P As New List(Of ReportParameter) From {
                            New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                            New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                            New ReportParameter("Sys", Sys.Text, True),
                            New ReportParameter("BranchNo", Branch.Text, IsHeadQuarter(Session("Branch")))
                        }
                        'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                        Session.Add("Parms", P)
                        cmbReports.JSProperties("cpResult") = cmbsplited(1)
                        cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                        cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                    Case "IssuClipOverallUsers", "IssuClipOverAllUsersTimer"
                        Dim br As Boolean = IIf(Not IsBranch(Session("Branch")), False, True)

                        Dim P As New List(Of ReportParameter) From {
                            New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                            New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                            New ReportParameter("Sys", Sys.Text, True),
                            New ReportParameter("BranchNo", Branch.Text, IsHeadQuarter(Session("Branch")))
                        }

                        'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                        Session.Add("Parms", P)
                        If e.Parameter <> "" Then
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = "IssuClipOverall"
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                        Else

                        End If

                    Case "SumIssues", "AllIssues", "SumIssuesWithSI", "AgentsReport", "CompanyIssues"
                        If IsBranch(Session("Branch")) Then
                            Dim br As Boolean = IIf(Not IsHeadQuarter(Session("Branch")), False, True)

                            If cmbsplited(0) = "AgentsReport" Or cmbsplited(0) = "CompanyIssues" Then
                                Dim P As New List(Of ReportParameter) From {
                              New ReportParameter("FDate", Format(Today.Date, "yyyy/MM/dd"), True),
                              New ReportParameter("TDate", Format(Today.Date, "yyyy/MM/dd"), True),
                              New ReportParameter("BranchNo", Session("Branch").ToString, IsHeadQuarter(Session("Branch").ToString)),
                              New ReportParameter("Agents", Session("Branch").ToString, True),
                              New ReportParameter("Sys", Sys.Text, True)
                              }
                                Session.Add("Parms", P)
                            Else
                                Dim P As New List(Of ReportParameter) From {
                               New ReportParameter("FDate", Format(Today.Date, "yyyy/MM/dd"), True),
                               New ReportParameter("TDate", Format(Today.Date, "yyyy/MM/dd"), True),
                               New ReportParameter("BranchNo", Session("Branch").ToString, IsHeadQuarter(Session("Branch").ToString)),
                               New ReportParameter("SubSystem", GetMainSystem(Sys.Text)),
                               New ReportParameter("Agents", Session("Branch").ToString, True),
                               New ReportParameter("Sys", Sys.Text, True)
                              }
                                Session.Add("Parms", P)
                            End If

                            'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)

                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = "AllIssues"
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                        Else

                        End If
                    Case "IssuesRecord"

                    Case "AgentComissions"
                        If IsHeadQuarter(Session("Branch")) Then

                            Dim P As New List(Of ReportParameter) From {
                                New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                                New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                                 New ReportParameter("BranchNo", Branch.Text, True),
                                 New ReportParameter("Agents", Branch.Text, True),
                                 New ReportParameter("Sys", Sys.Text, True)
                            }

                            Session.Add("Parms", P)
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = "AllIssues"
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                        Else
                            Dim P As New List(Of ReportParameter) From {
                                New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                                New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                                New ReportParameter("BranchNo", Branch.Text, False),
                                New ReportParameter("Agents", Branch.Text, True),
                                New ReportParameter("Sys", Sys.Text, True)
                            }

                            Session.Add("Parms", P)
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = "AllIssues"
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                        End If

                    Case "EarnedPremiums"
                        If IsHeadQuarter(Session("Branch")) Then

                            Dim P As New List(Of ReportParameter) From {
                           New ReportParameter("EarnedToDate", Format(Today.Date, "yyyy/MM/dd"), True)
                       }

                            'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                            Session.Add("Parms", P)
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = "AllIssues"
                            cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                        Else

                        End If
                    Case Else
                        If IsBranch(Session("Branch")) Then

                            If cmbsplited(0) = "AgentsReport" Then
                                If e.Parameter <> "" Then
                                    cmbReports.JSProperties("cpResult") = cmbsplited(1)
                                    cmbReports.JSProperties("cpMyAttribute") = ""
                                    cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                                Else
                                    Dim P As New List(Of ReportParameter) From {
                                 New ReportParameter("Sys", Sys.Text, True)
                             }
                                    'p.SetValue(New ReportParameter("AgentOrBranch", Branch.Text, IIf(Right(Branch.Text, 2) <> "00", False, True)), 4)
                                    Session.Add("Parms", P)
                                End If
                            Else
                                Dim P As New List(Of ReportParameter) From {
                               New ReportParameter("CurDate", Format(Today.Date, "yyyy/MM/dd"), True),
                               New ReportParameter("CurDateTo", Format(Today.Date, "yyyy/MM/dd"), True),
                               New ReportParameter("BranchNo", Branch.Text, IsHeadQuarter(Branch.Text)),
                               New ReportParameter("Agents", Branch.Text, True),
                               New ReportParameter("Sys", Sys.Text, True)
                           }
                                Session.Add("Parms", P)
                            End If
                        Else
                            If e.Parameter <> "" Then
                                cmbReports.JSProperties("cpResult") = cmbsplited(1)
                                cmbReports.JSProperties("cpMyAttribute") = ""
                                cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                            Else

                            End If

                        End If
                        'And (IsBranch(Session("Branch")) Or IsBranchOffice(Session("Branch")))
                        If e.Parameter <> "" Then
                            cmbReports.JSProperties("cpResult") = cmbsplited(1)
                            cmbReports.JSProperties("cpMyAttribute") = ""
                            Dim url As String = Page.ResolveUrl("~/Reporting/Previewer.aspx")
                            MainGrid.JSProperties("cpNewWindowUrl") = url & "?Report=" & ReportsPath & cmbsplited(0)
                            'cmbReports.JSProperties("cpNewWindowUrl") = "../Reporting/Previewer.aspx?Report=" & ReportsPath & cmbsplited(0) & ""
                        Else

                        End If
                End Select
            End If
        End If
        'e.Parameter.Remove(0)

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
                MainGrid.JSProperties("cpResult") = GetSysName(Sys.Value)
                Select Case Sys.Value
                    Case "01", "27", "OR", "04"
                        MainGrid.JSProperties("cpSize") = 900
                    Case Else
                        MainGrid.JSProperties("cpSize") = 1100
                End Select
                MainGrid.JSProperties("cpNewWindowUrl") = If(Sys.Value = "OR", GetEditForm(Sys.Value) + "?UserName=" + Session("UserLogin") + "&Branch=" + Session("Branch"), GetEditForm(Sys.Value) + "?Sys=" + Sys.Value)

            Case "ExtraSearch"
                MainGrid.JSProperties("cpMyAttribute") = "Search"
                MainGrid.JSProperties("cpResult") = GetSysName(Sys.Value)
                Dim url As String = Page.ResolveUrl("~/Policy/SearchForm.aspx")
                MainGrid.JSProperties("cpNewWindowUrl") = url & "?SystemTable=" & GetGroupFile(Sys.Value) & "&Sys=" & Sys.Value
                'MainGrid.JSProperties("cpNewWindowUrl") = "../Policy/SearchForm.aspx?SystemTable=" + GetGroupFile(Sys.Value) + "&Sys=" + Sys.Value
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
                    If IsIssued(e.GetValue("OrderNo"), e.GetValue("EndNo"), e.GetValue("LoadNo"), e.GetValue("SubIns")) Then
                        e.Cell.ForeColor = Color.DarkGreen
                        e.Cell.HorizontalAlign = HorizontalAlign.Center
                    Else
                        e.Cell.BackColor = Color.Red
                        e.Cell.ForeColor = Color.DarkGreen
                        e.Cell.HorizontalAlign = HorizontalAlign.Center
                        e.Cell.Text = e.GetValue("PolNo") + " / عرض / ملحق / إشعار"
                    End If
                End If
            Else
                e.Cell.BackColor = Color.Red
                e.Cell.ForeColor = Color.White
                e.Cell.Text = " فاتورة مبدئية (طلب تأمين) "
                e.Cell.HorizontalAlign = HorizontalAlign.Center
            End If
            'Dim PolicyRow As DevExpress.Web.Rendering.GridViewTableDataRow = e.Cell.Parent
        End If

    End Sub

    Private Sub MainGrid_DataBound(sender As Object, e As EventArgs) Handles MainGrid.DataBound
        MainGrid.Columns("LoadNo").Visible = Not Sys.Value <> "OC"
        MainGrid.Columns("Payment").Visible = Right(Session("Branch"), 3) = "000"
    End Sub

    Protected Sub imgStatus_Init(sender As Object, e As EventArgs)
        Dim img As ASPxImage = CType(sender, ASPxImage)
        Dim container As GridViewDataItemTemplateContainer = CType(img.NamingContainer, GridViewDataItemTemplateContainer)

        Dim statusValue As String = DataBinder.Eval(container.DataItem, "Payment").ToString()
        Dim IssuanceDate As String = DataBinder.Eval(container.DataItem, "IssuDate").ToString()

        ' Safely get TimeElapsed
        Dim TimeElapsed As Long = 0
        Dim timeElapsedObj As Object = DataBinder.Eval(container.DataItem, "IssMinutes")
        If timeElapsedObj IsNot Nothing AndAlso Not IsDBNull(timeElapsedObj) Then
            Long.TryParse(timeElapsedObj.ToString(), TimeElapsed)
        End If

        ' Select image based ONLY on status and time elapsed - NO DATABASE OPERATIONS
        Select Case statusValue
            Case "Pending"
                If String.IsNullOrEmpty(IssuanceDate) Then
                    img.ImageUrl = ""
                Else
                    Select Case TimeElapsed
                        Case <= 1440
                            img.ImageUrl = "~/Content/Images/24-times.png"
                            img.AlternateText = "تحت التحصيل"

                        Case 1441 To 2880
                            img.ImageUrl = "~/Content/Images/48-times.png"
                            img.AlternateText = "تحت التحصيل"

                        Case > 2880
                            img.ImageUrl = "~/Content/Images/72-times.png"
                            img.AlternateText = "تحت التحصيل"
                    End Select
                End If

            Case "Cancel"
                img.ImageUrl = "~/Content/Images/DeleteRed.png"
                img.AlternateText = "إلغاء"

            Case "Credit"
                img.ImageUrl = "~/Content/Images/DebitNote.png"
                img.AlternateText = "على الحساب"

            Case "Payed"
                img.ImageUrl = "~/Content/Images/Approved.png"
                img.AlternateText = "تم التحصيل"

            Case "Agent"
                img.ImageUrl = "~/Content/Images/Approved.png"
                img.AlternateText = "لحساب المكتب/الوكيل"
            Case "PartialPayment"
                img.ImageUrl = "~/Content/Images/PartialPayment.png"
                img.AlternateText = "سداد جزئي"
            Case Else
                img.ImageUrl = ""
                img.AlternateText = ""
        End Select
    End Sub

    '    Protected Sub imgStatus_Init(sender As Object, e As EventArgs)
    '        Dim img As ASPxImage = CType(sender, ASPxImage)

    '        Dim container As GridViewDataItemTemplateContainer = CType(img.NamingContainer, GridViewDataItemTemplateContainer)
    '        Dim statusValue As String = DataBinder.Eval(container.DataItem, "Payment").ToString()
    '        Dim IssuanceDate As String = DataBinder.Eval(container.DataItem, "IssuDate").ToString()
    '        Dim TimeElapsed As Long = If(statusValue = "Payed", 0, DataBinder.Eval(container.DataItem, "IssMinutes"))
    '        'Dim TimeElapsed As Int16 = DateDiff(DateInterval.Minute, IssuanceTime, Date.Now)

    '        Select Case statusValue
    '            Case "Pending"
    '                If IssuanceDate = "" Then
    '                    img.ImageUrl = ""
    '                Else
    '                    Select Case TimeElapsed
    '                        Case <= 1440
    '                            img.ImageUrl = "~/Content/Images/24-times.png"
    '                            img.AlternateText = "تحت التحصيل"

    '                        Case 1441 To 2880
    '                            img.ImageUrl = "~/Content/Images/48-times.png"
    '                            img.AlternateText = "تحت التحصيل"
    '                             ' Send user notifications over 48Hr
    '                        Case > 2880
    '                            img.ImageUrl = "~/Content/Images/72-times.png"
    '                            img.AlternateText = "تحت التحصيل"
    '                            ' Send Finance alert 72Hr

    '                            Dim SysUsers As New DataSet
    '                            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
    '                                If con.State = ConnectionState.Open Then
    '                                    con.Close()
    '                                Else
    '                                End If
    '                                con.Open()
    '                                Dim dbadapter = New SqlDataAdapter(";WITH FilteredPolicy AS (  SELECT
    '        PolNo,
    '        IssuDate,
    '        IssueUser,
    '        SubIns,
    '        Branch,
    '        stop,
    '        PayType,
    '        AccountNo,
    '        inbox,
    '        TOTPRM
    '    FROM PolicyFile
    '    WHERE
    '        right(Branch, 3) = '000'
    '        and stop = 0
    '        and PayType = 1
    '        and AccountNo = '0'
    '        and inbox <> TOTPRM
    '        and IssuDate is not null
    ')
    'SELECT
    '    Count(P.PolNo) as CNT,
    '    P.SubIns,
    '    dbo.SysName(P.SubIns) As Type,
    '    Sum(P.Totprm) As TOT,
    '    P.IssueUser As AccountNo,
    '    A.AccountName,
    '    Max(P.IssuDate) As dFrom,
    '    Min(P.issudate) As DTO
    'FROM
    '    FilteredPolicy P
    '    LEFT JOIN AccountFile A ON P.IssueUser = A.AccountNo
    'GROUP BY
    '    P.IssueUser, A.AccountName, P.SubIns
    'ORDER BY
    '    P.IssueUser", con)
    '                                'or AccountFile.Branch = '" & RTrim(Request("Br")) & "
    '                                dbadapter.SelectCommand.CommandTimeout = 120
    '                                dbadapter.Fill(SysUsers)
    '                                If SysUsers.Tables(0).Rows.Count <> 0 Then
    '                                    For i As Integer = 1 To SysUsers.Tables(0).Rows.Count
    '                                        Dim RequestsHist As New DataSet
    '                                        Dim dbadapter1 = New SqlDataAdapter("select * From Notifications Where Type=3 and DATEADD(dd,3,cast(getdate() as Date))>Timestamp and UserId=" & SysUsers.Tables(0).Rows(i - 1)("AccountNo") & " and Message='تذكير بتسوية عدد " & SysUsers.Tables(0).Rows(i - 1)("CNT") & " وثيقة " & SysUsers.Tables(0).Rows(i - 1)("Type").ToString.TrimEnd & " وبقيمة إجمالية " & SysUsers.Tables(0).Rows(i - 1)("TOT") & " صادرة في الفترة من " & SysUsers.Tables(0).Rows(i - 1)("dFrom") & "  إلى " & SysUsers.Tables(0).Rows(i - 1)("DTO") & " يرجى مراجعة الإدارة المالية'", con)
    '                                        dbadapter1.SelectCommand.CommandTimeout = 120
    '                                        dbadapter1.Fill(RequestsHist)
    '                                        If RequestsHist.Tables(0).Rows.Count = 0 Then
    '                                            ExecConn("Insert Into Notifications (Action,IsRead,Message,Type,UserId,GeneratedBy,GeneratedByID) " _
    '                                                 & " VALUES ('/', 0,'تذكير بتسوية عدد " & SysUsers.Tables(0).Rows(i - 1)("CNT") & " وثيقة " & SysUsers.Tables(0).Rows(i - 1)("Type").ToString.TrimEnd & " وبقيمة إجمالية " & SysUsers.Tables(0).Rows(i - 1)("TOT") & " صادرة في الفترة من " & SysUsers.Tables(0).Rows(i - 1)("dFrom") & "  إلى " & SysUsers.Tables(0).Rows(i - 1)("dTO") & " يرجى مراجعة الإدارة المالية' " _
    '                                                 & ",3," & SysUsers.Tables(0).Rows(i - 1)("AccountNo") & ",'SYSTEM'," & SysUsers.Tables(0).Rows(i - 1)("AccountNo") & ")", con)
    '                                        Else

    '                                        End If
    '                                    Next
    '                                End If

    '                                con.Close()
    '                            End Using
    '                    End Select
    '                End If
    '            Case "Cancel"
    '                img.ImageUrl = "~/Content/Images/DeleteRed.png"
    '                img.AlternateText = "إلغاء"
    '            Case "Credit"
    '                img.ImageUrl = "~/Content/Images/DebitNote.png"
    '                img.AlternateText = "على الحساب"
    '            Case "Payed"
    '                img.ImageUrl = "~/Content/Images/Approved.png"
    '                img.AlternateText = "تم التحصيل"
    '            Case "Agent"
    '                img.ImageUrl = "~/Content/Images/Approved.png"
    '                img.AlternateText = "لحساب المكتب/الوكيل"
    '            Case Else
    '                Exit Select
    '        End Select

    '    End Sub

End Class