Imports System.Data.SqlClient
Imports System.Data.SqlClient.SqlParameter
Imports DevExpress.Web

Public Class Receipt
    Inherits Page
    Private ReadOnly Policy As New DataSet
    Private RNo As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Lo = Session("LogInfo")
        'ExecConn("update policyfile set polno=REPLACE(polno, char(9), '')", Conn)
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))
        'GridData.Columns.Clear()
        If Request("PolNo") = "أخـرى" Then
            ASPxLabel1.Text = "مقبوضات أخرى"
            If Not IsPostBack Then
                TOTPRM.Text = 0
                TOTPRM.Visible = False
                'Customer.Text = ""
                GridData.Visible = False
                CheckCust.Visible = False
            Else

            End If
        Else
            If Not IsPostBack Then
                If Not CheckCust.Checked Then
                    Dim dbadapter = New SqlDataAdapter("select PolNo,PolicyFile.CustNo,Currency,SubIns,EndNo,LoadNo,TOTPRM-InBox As TOTPRM,IssuDate,CustName,ExtraInfo.TpName, " _
                                                       & " Case when Commision<>0 and Broker<>0 then CAST(Commision AS NVARCHAR(100)) + iif(CommisionType=1,' '+EXTRAINFO.TPName,' %') +'-' + BrokersInfo.TPName else '0' END As Commissioned " _
                                                       & " FROM PolicyFile inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo LEFT OUTER JOIN EXTRAINFO As BrokersInfo on BrokersInfo.TP='Broker' and PolicyFile.Broker=BrokersInfo.TPNo " _
                                                       & " WHERE PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") & " and loadNo=" & Request("LoadNo"), Conn)
                    dbadapter.Fill(Policy)
                    'Policy = RecSet("select PolNo,PolicyFile.CustNo,Currency,EndNo,LoadNo,TOTPRM,IssuDate,CustName,TpName from PolicyFile inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo where PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") & " and loadNo=" & Request("LoadNo"), Conn)
                    TOTPRM.Text = Format(Policy.Tables(0).Rows(0)("TOTPRM"), "###,#0.000")
                    Payed.Text = Format(Policy.Tables(0).Rows(0)("TOTPRM"), "###,#0.000")
                    Customer.Text = Policy.Tables(0).Rows(0)("CustName")
                    If Month(Policy.Tables(0).Rows(0)("IssuDate")) = Month(Today.Date) Then
                        MoveDate.ClientEnabled = False
                    Else
                        MoveDate.Value = LastDayOfMonth(Policy.Tables(0).Rows(0)("IssuDate"))
                        MoveDate.ClientEnabled = False
                    End If
                    'MoveDate.Value = CDate(Policy.Tables(0).Rows(0)("IssuDate"))
                    'MoveDate.ClientEnabled = False
                    GridData.DataSource = Policy
                    GridData.DataBind()
                    Session.Add("SubIns", Policy.Tables(0).Rows(0)("SubIns"))
                    Session.Add("IssDate", Policy.Tables(0).Rows(0)("IssuDate"))
                End If
            Else
                If CheckCust.Checked Then
                    Dim dbadapter = New SqlDataAdapter("select PolNo,PolicyFile.CustNo,Currency,SubIns,EndNo,LoadNo,TOTPRM-Inbox As TOTPRM,IssuDate,CustName,TpName from " _
                                    & "PolicyFile inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo " _
                                    & " where PolicyFile.SubIns='" & Session("SubIns") & "'" _
                                    & " AND inbox<TOTPRM", Conn)
                    dbadapter.Fill(Policy)
                    'Policy = RecSet("select PolNo,PolicyFile.CustNo,Currency,EndNo,LoadNo,TOTPRM,IssuDate,CustName,TpName from PolicyFile inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo where PolicyFile.CustNo=" & Policy.Tables(0).Rows(0)("CustNo") _
                    '& " and IssuDate=CONVERT(DATETIME, '" & Format(Policy.Tables(0).Rows(0)("IssuDate"), "yyyy-MM-dd") & " 00:00:00', 102)", Conn)
                    Dim PolSum1 As Double
                    Dim PolRef As String
                    PolRef = " للوثائق من " & Policy.Tables(0).Rows(0)("PolNo") & " إلى" & Policy.Tables(0).Rows(Policy.Tables(0).Rows.Count - 1)("PolNo")

                    For i As Integer = 0 To Policy.Tables(0).Rows.Count - 1
                        PolSum1 += Policy.Tables(0).Rows(i)("TOTPRM")
                    Next
                    TOTPRM.Text = Format(PolSum1, "###,#0.000")
                    Payed.Text = Format(PolSum1, "###,#0.000")
                    Payed.ClientEnabled = False
                    GridData.DataSource = Policy
                    GridData.DataBind()
                Else
                    Dim dbadapter = New SqlDataAdapter("select PolNo,PolicyFile.CustNo,Currency,EndNo,LoadNo,TOTPRM-InBox As TOTPRM,IssuDate,CustName, ExtraInfo.TpName, " _
                                                       & " Case when Commision<>0 and Broker<>0 then CAST(Commision AS NVARCHAR(100)) + iif(CommisionType=1,' '+EXTRAINFO.TPName,' %') +'-' + BrokersInfo.TPName else '0' END As Commissioned " _
                                                       & " FROM PolicyFile inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo LEFT OUTER JOIN EXTRAINFO As BrokersInfo on BrokersInfo.TP='Broker' and PolicyFile.Broker=BrokersInfo.TPNo " _
                                                       & " WHERE PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") & " and loadNo=" & Request("LoadNo"), Conn)
                    dbadapter.Fill(Policy)
                    'Policy = RecSet("select PolNo,PolicyFile.CustNo,Currency,EndNo,LoadNo,TOTPRM,IssuDate,CustName,TpName from PolicyFile inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo where PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") & " and loadNo=" & Request("LoadNo"), Conn)
                    TOTPRM.Text = Format(Policy.Tables(0).Rows(0)("TOTPRM"), "###,#0.000")
                    Payed.ClientEnabled = True
                    'MoveDate.Date = Policy.Tables(0).Rows(0)("IssuDate")
                    'Payed.Text = Format(Policy.Tables(0).Rows(0)("TOTPRM"), "###,#0.000")
                    Customer.Text = Policy.Tables(0).Rows(0)("CustName")
                    GridData.DataSource = Policy
                    GridData.DataBind()
                    Session.Add("Cust", Policy.Tables(0).Rows(0)("CustNo"))
                    Session.Add("IssDate", Policy.Tables(0).Rows(0)("IssuDate"))
                End If

            End If

        End If

        ' change accounts data source
        'If IsCallback Then
        '    Select Case PayTyp.Value
        '        Case 3
        '            AccName.DataSource = "Accounts"
        '            AccName.Caption = "الحساب الجار رقم"
        '        Case 4
        '            AccName.DataSource = "الحساب المصرفي رقم"
        '        Case Else
        '            Exit Select
        '    End Select
        '    AccName.DataBind()
        'End If
        If Request("PolNo") = "أخـرى" Then
            Pay.SelectCommand = "select TPName,TPNo from EXTRAINFO where TP='Payment' AND TPNo<>4 order by TPNo"
            Payed.ReadOnly = False
        Else
            Pay.SelectCommand = "select TPName,TPNo from EXTRAINFO where TP='Payment'  order by TPNo"
            Payed.ReadOnly = True
        End If
    End Sub

    Protected Sub Unnamed1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckCust.CheckedChanged

        If CheckCust.Checked Then
        Else
            'Dim dbadapter = New Data.SqlClient.SqlDataAdapter("select PolNo,PolicyFile.CustNo,Currency,EndNo,LoadNo,TOTPRM,IssuDate,CustName,TpName from " _
            '                & "PolicyFile inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo " _
            '                & "where PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") & " and loadNo=" & Request("LoadNo"), Conn)
            'dbadapter.Fill(Policy)
            ''Policy = RecSet("select PolNo,PolicyFile.CustNo,Currency,EndNo,LoadNo,TOTPRM,IssuDate,CustName,TpName from PolicyFile inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo where PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") & " and loadNo=" & Request("LoadNo"), Conn)
            'Dim PolSum As Double
            'For i As Integer = 0 To Policy.Tables(0).Rows.Count - 1
            '    PolSum += Policy.Tables(0).Rows(i)("TOTPRM")
            'Next
            'TOTPRM.Text = Format(PolSum, "###,#0.000")
            'GridData.DataSource = Policy
            'GridData.DataBind()
        End If

    End Sub

    Protected Sub MoveDate_Init(sender As Object, e As EventArgs)
        MoveDate.Date = New DateTime(Today.Date().Year, Today.Date().Month, Today.Date().Day)
    End Sub

    'Protected Sub ASPxButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ASPxButton1.Click
    '    ' Dim ind As Integer
    '    'Dim Policy As New DataSet
    '    'If CDbl(Payed.Value) <= CDbl(TOTPRM.Value) Then

    '    '    If Request("PolNo") <> "أخـرى" Then
    '    '        If CheckCust.Checked Then
    '    '            Dim dbadapter = New SqlDataAdapter("select PolNo,SubIns,PolicyFile.Branch,BranchName,SubSysName,PolicyFile.CustNo,Currency,EndNo,LoadNo,TOTPRM-Inbox As TOTPRM,IssuDate,CustName,TpName from PolicyFile" _
    '    '                        & " inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo " _
    '    '                        & " inner join BranchInfo on PolicyFile.Branch=BranchNo " _
    '    '                        & " inner join SubSystems on SubIns=SubSysNo and SubSystems.Branch=PolicyFile.Branch" _
    '    '                        & " where PolicyFile.CustNo=" & Session("Cust") & " AND inbox<TOTPRM", Conn)
    '    '            dbadapter.Fill(Policy)
    '    '        Else
    '    '            Dim dbadapter = New SqlDataAdapter("select PolNo,SubIns,PolicyFile.Branch,BranchName,SubSysName,PolicyFile.CustNo,Currency,EndNo,LoadNo,TOTPRM,IssuDate,CustName,TpName from PolicyFile" _
    '    '                        & " inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo" _
    '    '                        & " inner join BranchInfo on PolicyFile.Branch=BranchNo " _
    '    '                        & " inner join SubSystems on SubIns=SubSysNo and SubSystems.Branch=PolicyFile.Branch" _
    '    '                        & " where PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") & " and loadNo=" & Request("LoadNo"), Conn)
    '    '            dbadapter.Fill(Policy)
    '    '        End If
    '    '        For i As Integer = 0 To Policy.Tables(0).Rows.Count - 1
    '    '            Parm = Array.CreateInstance(GetType(SqlParameter), 1)
    '    '            SetPm("@BR", DbType.String, Session("Branch"), Parm, 0)
    '    '            Dim RNo As String = CallSP("RecetNo", Conn, Parm)
    '    '            If CheckCust.Checked Then

    '    '            Else

    '    '            End If
    '    '            ExecConn("Insert into ACCMOVE(DocNo,DocDat,CustName,PayMent,Amount,ForW,EndNo,LoadNo,Tp,Branch,AccNo,Bank,Cur,Node,PayTp,Note) Values('" _
    '    '        & RNo & "'," _
    '    '        & "CONVERT(DATETIME, '" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00', 102),'" _
    '    '        & Trim(Policy.Tables(0).Rows(i)("CustName")) & "'," _
    '    '        & IIf(CheckCust.Checked, Policy.Tables(0).Rows(i)("TOTPRM"), CDbl(Payed.Value)) & "," _
    '    '        & IIf(CheckCust.Checked, Policy.Tables(0).Rows(i)("TOTPRM"), CDbl(Payed.Value)) & ",'" _
    '    '        & Policy.Tables(0).Rows(i)("PolNo") & "'," _
    '    '        & Policy.Tables(0).Rows(i)("EndNo") & "," _
    '    '        & Policy.Tables(0).Rows(i)("LoadNo") & ",'" _
    '    '        & Trim(Policy.Tables(0).Rows(i)("SubSysName")) & "','" _
    '    '        & Session("Branch") & "','" _
    '    '        & AccNo.Text & "','" _
    '    '        & Bank.Text & "','" _
    '    '        & Policy.Tables(0).Rows(i)("TpName") & "','" _
    '    '        & "" & "'," _
    '    '        & Currency.Value & ",'" _
    '    '        & Note.Text & "')", Conn)
    '    '            If CheckCust.Checked Then
    '    '                ExecConn("Update policyFile Set Inbox = Inbox + " & IIf(CheckCust.Checked, Policy.Tables(0).Rows(i)("TOTPRM"), CDbl(Payed.Value)) & " where " _
    '    '                         & " PolNo='" + Policy.Tables(0).Rows(i)("PolNo") + "' and EndNo=" & Policy.Tables(0).Rows(i)("EndNo") & " and LoadNo=" & Policy.Tables(0).Rows(i)("LoadNo"), Conn)
    '    '            Else
    '    '                ExecConn("Update policyFile Set Inbox = Inbox + " & IIf(CheckCust.Checked, Policy.Tables(0).Rows(i)("TOTPRM"), CDbl(Payed.Value)) & " where " _
    '    '         & "PolicyFile.IssuDate = Convert(DateTime, '" + Format(Policy.Tables(0).Rows(i)("IssuDate"), "yyyy/MM/dd") + " 00:00:00', 102) " _
    '    '         & " and PolNo='" + Policy.Tables(0).Rows(i)("PolNo") + "' and EndNo=" & Policy.Tables(0).Rows(i)("EndNo") & " and LoadNo=" & Policy.Tables(0).Rows(i)("LoadNo"), Conn)
    '    '            End If
    '    '            Dim Report = ReportsPath & "Reciept"
    '    '            'Dim Report = ReportsPath & PolRep(Request("Sys"))
    '    '            Dim p As ReportParameter() = New ReportParameter(0) {}
    '    '            p.SetValue(New ReportParameter("ReciptNo", RNo, False), 0)
    '    '            'Response.RedirectLocation("~/Reporting/PreviewPDF.aspx?Report=" & Report & "")
    '    '            'If (Page.IsCallback) Then
    '    '            ASPxWebControl.RedirectOnCallback("~/Reporting/PreviewPDF.aspx?Report=" & Report & "")
    '    '        Next

    '    '    Else

    '    '    End If
    '    'Else
    '    '    If Request("PolNo") = "أخـرى" Then
    '    '        Parm = Array.CreateInstance(GetType(SqlParameter), 1)
    '    '        SetPm("@BR", DbType.String, Session("Branch"), Parm, 0)
    '    '        Dim RNo As String = CallSP("RecetNo", Conn, Parm)
    '    '        ExecConn("Insert into ACCMOVE(DocNo,DocDat,CustNAme,PayMent,Amount,ForW,EndNo,LoadNo,Tp,Branch,AccNo,Bank,Cur,Node,PayTp,Note) Values('" _
    '    '    & RNo & "'," _
    '    '    & "CONVERT(DATETIME, '" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00', 102),'" _
    '    '    & Trim(Customer.Text) & "'," _
    '    '    & CDbl(Payed.Text) & "," _
    '    '    & CDbl(Payed.Text) & ",'" _
    '    '    & Request("PolNo") & "'," _
    '    '    & 0 & "," _
    '    '    & 0 & ",'" _
    '    '    & Request("PolNo") & "','" _
    '    '    & Session("Branch") & "','" _
    '    '    & AccNo.Text & "','" _
    '    '    & Bank.Text & "','" _
    '    '    & "دينار ليبي" & "','" _
    '    '    & "" & "'," _
    '    '    & Currency.Value & ",'" _
    '    '    & Note.Text & "')", Conn)

    '    '        Dim Report = ReportsPath & "Reciept"
    '    '        'Dim Report = ReportsPath & PolRep(Request("Sys"))
    '    '        Dim p As ReportParameter() = New ReportParameter(0) {}
    '    '        p.SetValue(New ReportParameter("ReciptNo", RNo, False), 0)
    '    '        'Response.RedirectLocation("~/Reporting/PreviewPDF.aspx?Report=" & Report & "")
    '    '        'If (Page.IsCallback) Then
    '    '        ASPxWebControl.RedirectOnCallback("~/Reporting/PreviewPDF.aspx?Report=" & Report & "")
    '    '    Else

    '    '    End If

    '    'End If
    '    'Response.Write("<script type='text/javascript'> " + " ReturnToParentPage(); " + "</script>")

    'End Sub

    Protected Sub ASPxButton2_Click(sender As Object, e As EventArgs) Handles ASPxButton2.Click
        Response.Write("<script type='text/javascript'> " + "window.close(); " + "</script>")
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Dim callbackPanel As ASPxCallbackPanel = TryCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)
        If isValid Then
            Select Case e.Parameter
                Case "Sadad"
                    Dim Policy1 As New DataSet
                    If CDbl(Payed.Value) <= CDbl(TOTPRM.Value) Then
                        If Request("PolNo") <> "أخـرى" Then

                            Dim dbadapter = New SqlDataAdapter("select PolNo,SubIns,PolicyFile.OrderNo,PolicyFile.Branch,PolicyFile.Commision,BranchName, SubSysName,PolicyFile.CustNo,Currency,EndNo,LoadNo,TOTPRM,IssuDate,CustName,TpName from PolicyFile" _
                                            & " inner join ExtraInfo on TP='Cur' and TpNo=Currency inner join CustomerFile on CustomerFile.CustNo=PolicyFile.CustNo" _
                                            & " inner join BranchInfo on PolicyFile.Branch=BranchNo " _
                                            & " inner join SubSystems on SubIns=SubSysNo and SubSystems.Branch=PolicyFile.Branch" _
                                            & " where PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo"), Conn)
                            dbadapter.Fill(Policy1)
                            'End If
                            For i As Integer = 0 To Policy1.Tables(0).Rows.Count - 1

                                If CDbl(Payed.Value) = 0 Then
                                    Exit Sub
                                End If

                                If (PayTyp.Value = 4 Or PayTyp.Value = 6) And IsNothing(AccName.Value) Then
                                    Exit Sub
                                Else

                                End If
                                If (PayTyp.Value = 4 Or PayTyp.Value = 6) And Len(AccName.Value) <> 0 Then
                                    ExecConn("Update policyFile Set AccountNo = '" & RTrim(AccName.Value) & "',PayType=2,Financed=1 where " _
                                             & " PolNo='" & Policy1.Tables(0).Rows(i)("PolNo").ToString.Trim & "' and EndNo=" & Policy1.Tables(0).Rows(i)("EndNo") & " and LoadNo=" & Policy1.Tables(0).Rows(i)("LoadNo"), Conn)
                                Else
                                    If CDbl(Payed.Value) > 0 Then
                                        Parm = Array.CreateInstance(GetType(SqlParameter), 1)
                                        SetPm("@BR", DbType.String, Session("Branch"), Parm, 0)
                                        RNo = CallSP("RecetNo", Conn, Parm)

                                        ExecConn("Insert into ACCMOVE(DocNo,DocDat,CustName,PayMent,Amount,ForW, " _
                                & "EndNo,LoadNo,Tp,Branch,AccNo,Bank,Cur,Node,PayTp,UserName,Note) Values('" _
                                & RNo & "'," _
                                & "CONVERT(DATETIME, '" & Format(MoveDate.Value, "yyyy-MM-dd") & " 00:00:00', 102),'" _
                                & Trim(Policy1.Tables(0).Rows(i)("CustName")) & "'," _
                                & IIf(CheckCust.Checked, Policy1.Tables(0).Rows(i)("TOTPRM"), CDbl(Payed.Value)) & "," _
                                & Policy1.Tables(0).Rows(i)("TOTPRM") & ",'" _
                                & Policy1.Tables(0).Rows(i)("PolNo") & "'," _
                                & Policy1.Tables(0).Rows(i)("EndNo") & "," _
                                & Policy1.Tables(0).Rows(i)("LoadNo") & ",'" _
                                & Policy1.Tables(0).Rows(i)("SubSysName").ToString & "','" _
                                & Session("Branch") & "','" _
                                & AccNo.Text & "','" _
                                & Bank.Text & "','" _
                                & Policy1.Tables(0).Rows(i)("TpName") & "','" _
                                & "/" & "'," _
                                & PayTyp.Value & ",'" _
                                & Session("User") & "','" _
                                & Note.Text & "')", Conn)
                                    Else

                                    End If

                                End If

                                'If CheckCust.Checked Then
                                '    ExecConn("Update policyFile Set Inbox = Inbox + " & IIf(CheckCust.Checked, Policy1.Tables(0).Rows(i)("TOTPRM"), CDbl(Payed.Value)) & " where " _
                                '             & " PolNo='" & Policy1.Tables(0).Rows(i)("PolNo") & "' and EndNo=" & Policy1.Tables(0).Rows(i)("EndNo") & " and LoadNo=" & Policy1.Tables(0).Rows(i)("LoadNo"), Conn)
                                'Else
                                If (PayTyp.Value = 4 Or PayTyp.Value = 6) And Len(AccName.Value) <> 0 Then
                                Else
                                    ExecConn("Update policyFile Set Inbox = Inbox + " & IIf(CheckCust.Checked, Policy1.Tables(0).Rows(i)("TOTPRM"), CDbl(Payed.Value)) & ",Financed=1 where " _
                                    & "PolicyFile.IssuDate = Convert(DateTime, '" & Format(Policy1.Tables(0).Rows(i)("IssuDate"), "yyyy/MM/dd") & " 00:00:00', 102) " _
                                    & " and PolNo='" & Policy1.Tables(0).Rows(i)("PolNo") & "' and EndNo=" & Policy1.Tables(0).Rows(i)("EndNo") & " and LoadNo=" & Policy1.Tables(0).Rows(i)("LoadNo"), Conn)
                                End If
                                If Request("AccNo") = "0" Then
                                    If (PayTyp.Value = 4 Or PayTyp.Value = 6) Then
                                        InsetJournal(Policy1.Tables(0).Rows(i)("PolNo"), Policy1.Tables(0).Rows(i)("EndNo"), Policy1.Tables(0).Rows(i)("LoadNo"), Session("User"), Policy1.Tables(0).Rows(i)("OrderNo"), "/", Session("Branch"))
                                        Dim tempTP
                                        If Left(AccName.Value.ToString.TrimEnd, 7) = "1.1.10." Then
                                            tempTP = "(تحت التحصيل)"
                                        Else
                                            tempTP = "مديونية"
                                        End If
                                        ExecConn("Update MainJournal Set DAILYDTE = '" & Format(Policy1.Tables(0).Rows(i)("IssuDate"), "yyyy/MM/dd") & "',Comment=replace(Comment,'مديونية','" & tempTP & "') where MoveRef='" & Policy1.Tables(0).Rows(i)("OrderNo").ToString & "'", Conn)
                                        Dim dlyno As New DataSet
                                        Dim dladabter = New SqlDataAdapter("select DAILYNUM from MainJournal where MoveRef='" & Policy1.Tables(0).Rows(i)("OrderNo").ToString & "'", Conn)
                                        dladabter.Fill(dlyno)
                                        ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "&Sys=1")
                                        GoTo ss
                                    Else

                                        InsetJournal(Request("PolNo").ToString, Policy1.Tables(0).Rows(i)("EndNo"), Policy1.Tables(0).Rows(i)("LoadNo"), Session("User"), Policy1.Tables(0).Rows(i)("OrderNo"), RNo, Session("Branch"))
                                        ExecConn("Update MainJournal Set DAILYDTE = '" & Format(MoveDate.Value, "yyyy/MM/dd") & "' where MoveRef='" & Policy1.Tables(0).Rows(i)("OrderNo").ToString & "' And RecNo='" & RNo & "'", Conn)

                                    End If
                                Else
                                    'Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                                    'SetPm("@TP", DbType.String, "1", Parm, 0)
                                    'SetPm("@Year", DbType.String, Right(Year(MoveDate.Value).ToString, 2), Parm, 1)

                                    Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                                    SetPm("@TP", DbType.String, "1", Parm, 0)
                                    SetPm("@Year", DbType.String, Right(Year(MoveDate.Value).ToString, 2), Parm, 1)
                                    SetPm("@Br", DbType.String, Session("Branch"), Parm, 2)

                                    Dim Dly As String = CallSP("LastDailyNo", Conn, Parm)

                                    Dim Str As String = " قيد إثبات سداد مديونية / " & Request("PolNo").ToString & "/" & Policy1.Tables(0).Rows(i)("EndNo") & "/" & Policy1.Tables(0).Rows(i)("SubSysName")

                                    ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] ,[Comment] " _
                                            & ",[Currency] ,[Exchange] ,[CurUser] ,[MoveRef],[RecNo],[DailyChk],[Branch])  " _
                                            & " VALUES ('" & Dly & "','" & Format(MoveDate.Value, "yyyy/MM/dd") & "', " _
                                            & 1 & ",'" & "A" & "','" & Str & "', " & 1 & "," & 1 & "," _
                                            & "'" & Session("User") & "','" & Str & "','/', " & 1 & ",'" & Session("Branch") & "')", Conn)
                                    ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch]) VALUES ('" & Dly.ToString & "',1, dbo.GetAccountNo('TOT', '" & Policy1.Tables(0).Rows(i)("SubIns") & "', 0 , '" & Session("Branch") & "')," & CDbl(Payed.Value) & ",0,'" & Session("User") & "','" & Session("Branche") & "')", Conn)
                                    ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch]) VALUES ('" & Dly.ToString & "',1, '" & Request("AccNo").ToString & "',0," & CDbl(Payed.Value) * -1 & ",'" & Session("User") & "','" & Session("Branch") & "')", Conn)

                                    '                                For Each row As DataRow In dt.Rows
                                    '                ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM] , [TP], [ACCNTNUM], [Dr], [Cr], [CurUser]) " _
                                    '& " VALUES ('" & Dly & "'," & 1 & ", '" & Request("AccNo") & "', " & CDbl(0) & "," & CDbl(Payed.Value) & ",'" & User & "')", Conn)
                                    '                                Next row
                                End If
                                If CDbl(Payed.Value) > 0 Then
                                    Dim DBtTp As New DataSet
                                    Dim dlyno, dlynoCr As New DataSet
                                    If (PayTyp.Value = 4 Or PayTyp.Value = 6) Then
                                        Dim dladabtercr = New SqlDataAdapter("select DAILYNUM,DailyTyp from MainJournal where MoveRef='" & Policy1.Tables(0).Rows(i)("OrderNo") & "'", Conn)
                                        dladabtercr.Fill(dlynoCr)
                                        ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" + dlynoCr.Tables(0).Rows(0)("DAILYNUM") + "&Sys=1")
                                        Exit Sub
                                    End If
                                    Dim dladabter = New SqlDataAdapter("select DAILYNUM,DailyTyp from MainJournal where MoveRef='" & Policy1.Tables(0).Rows(i)("OrderNo") & "' and RecNo='" & RNo & "'", Conn)
                                    dladabter.Fill(dlyno)
                                    Using con1 As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                                        Dim dbadapter5 = New SqlDataAdapter(String.Format("select PayTp from AccMove where DocNo='{0}' ", RNo), con1)
                                        dbadapter5.Fill(DBtTp)
                                        If DBtTp.Tables(0).Rows.Count = 0 Then
                                            Exit Sub
                                        Else
                                            Select Case DBtTp.Tables(0).Rows(0)("PayTp")
                                                Case 1
                                                    ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and DailyTyp=1 and ANALSNUM='A'", con1)
                                                    ExecConn("Update Journal set AccountNo ='" & GetBoxAccount(Session("Branch")) & "' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                                Case 2
                                                    ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and DailyTyp=1 and ANALSNUM='A' ", con1)
                                                    ExecConn("Update Journal set AccountNo ='" & GetcheqAccount(Session("Branch")) & "'  where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                                Case 3
                                                    ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and DailyTyp=1 and ANALSNUM='A' ", con1)
                                                    ExecConn("Update Journal set AccountNo ='" & AccName.Value & "' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                                Case 5
                                                    ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and DailyTyp=1 and ANALSNUM='A'", con1)
                                                    ExecConn("Update Journal set AccountNo ='" & AccName.Value & "' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                                Case Else
                                                    Exit Select
                                            End Select
                                        End If
                                        ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" + dlyno.Tables(0).Rows(0)("DAILYNUM") + "&Sys=1")
                                    End Using
                                Else

                                End If

                            Next
                        Else

                        End If
                    Else
                        If Request("PolNo") = "أخـرى" Then
                            Parm = Array.CreateInstance(GetType(SqlParameter), 1)
                            SetPm("@BR", DbType.String, Session("Branch"), Parm, 0)
                            RNo = CallSP("RecetNo", Conn, Parm)

                            ExecConn("Insert into ACCMOVE(DocNo,DocDat,CustNAme,PayMent,Amount,ForW,EndNo,LoadNo,Tp,Branch,AccNo,Bank,Cur,Node,PayTp,UserName,Note) Values('" _
                        & RNo & "'," _
                        & "CONVERT(DATETIME, '" & Format(MoveDate.Value, "yyyy-MM-dd") & " 00:00:00', 102),'" _
                        & Trim(Customer.Text) & "'," _
                        & CDbl(Payed.Text) & "," _
                        & CDbl(Payed.Text) & ",'" _
                        & Request("PolNo") & "'," _
                        & 0 & "," _
                        & 0 & ",'" _
                        & Request("PolNo") & "','" _
                        & Session("Branch") & "','" _
                        & AccNo.Text & "','" _
                        & Bank.Text & "','" _
                        & "دينار ليبي" & "','" _
                        & "" & "'," _
                        & PayTyp.Value & ",'" _
                        & Session("User") & "','" _
                        & Note.Text & "')", Conn)

                            'Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                            'SetPm("@TP", DbType.String, "1", Parm, 0)
                            'SetPm("@Year", DbType.String, Right(Year(MoveDate.Value).ToString, 2), Parm, 1)

                            Parm = Array.CreateInstance(GetType(SqlParameter), 3)
                            SetPm("@TP", DbType.String, "1", Parm, 0)
                            SetPm("@Year", DbType.String, Right(Year(MoveDate.Value).ToString, 2), Parm, 1)
                            SetPm("@Br", DbType.String, Session("Branch"), Parm, 2)

                            Dim Dly As String = CallSP("LastDailyNo", Conn, Parm)
                            Dim accnt As String = ""
                            Dim Str As String = Note.Value.ToString
                            Select Case PayTyp.Value
                                Case 1
                                    accnt = GetBoxAccount(Session("Branch"))
                                Case 2
                                    accnt = GetcheqAccount(Session("Branch"))
                                Case 3, 5
                                    accnt = AccName.Value
                                Case Else
                                    Exit Select
                            End Select
                            ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] ,[Comment] " _
                                            & ",[Currency] ,[Exchange] ,[CurUser] ,[RecNo],[DailyChk],[Branch])  " _
                                            & " VALUES ('" & Dly & "','" & Format(MoveDate.Value, "yyyy/MM/dd") & "', " _
                                            & 1 & ",'" & "P" & "','" & Str & " / RecNo.# " & RNo & "', " & 1 & "," & 1 & "," _
                                            & "'" & Session("User") & "','" & RNo & "'," & 1 & ",'" & Session("Branch") & "')", Conn)

                            ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch]) VALUES ('" & Dly.ToString & "',1, '" & accnt & "'," & CDbl(Payed.Value) & ",0,'" & Session("User") & "','" & Session("Branch") & "')", Conn)
                            ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" + Dly.ToString + "&Sys=1")
                            'ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [ACCNTNUM], [Dr], [Cr], [CurUser], [Branch]) VALUES ('" & Dly.ToString & "',1, '" & Request("AccNo").ToString & "',0," & CDbl(Payed.Value) * -1 & ",'" & Session("User") & "','" & Session("Branche") & "')", Conn)

                            '        Dim Report = ReportsPath & "Reciept"
                            '        'Dim Report = ReportsPath & PolRep(Request("Sys"))
                            '        Dim P As New List(Of ReportParameter) From {
                            '    New ReportParameter("ReciptNo", RNo, False)
                            '}
                            '        Session.Add("Parms", P)
                            '        ASPxWebControl.RedirectOnCallback("~/Reporting/Previewer.aspx?Report=" & Report & "")
                        Else

                        End If

                    End If
                Case "PaymentChanged"
                    Select Case PayTyp.Value
                        Case 1
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientVisible = False
                        Case 2
                            AccNo.ClientVisible = True
                            Bank.ClientVisible = True
                            AccName.ClientVisible = False
                        Case 3
                            AccNo.ClientVisible = True
                            Bank.ClientVisible = True
                            AccName.ClientVisible = True

                            AccName.DataSourceID = "BankAccounts"
                            AccName.Caption = "الحساب المصرفي رقم"
                        Case 4
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientVisible = True

                            AccName.DataSourceID = "Accounts"
                            AccName.Caption = "حساب المدينون رقم"
                            'Accounts.DataBind()
                        Case 5
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientVisible = True

                            AccName.DataSourceID = "BankAccounts"
                            AccName.Caption = "الحساب المصرفي رقم"
                        Case 6
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientVisible = True

                            AccName.DataSourceID = "AccountsNotPayed"
                            AccName.Caption = "حساب تحت التحصيل رقم"
                            'Accounts.DataBind()
                        Case Else
                            Exit Select
                    End Select
                    AccName.DataBind()
                Case Else
                    Exit Select
            End Select
        Else
            Select Case e.Parameter
                Case "PaymentChanged"
                    Select Case PayTyp.Value
                        Case 1
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientVisible = False
                        Case 2
                            AccNo.ClientVisible = True
                            Bank.ClientVisible = True
                            AccName.ClientVisible = False
                        Case 3
                            AccNo.ClientVisible = True
                            Bank.ClientVisible = True
                            AccName.ClientVisible = True

                            AccName.DataSourceID = "BankAccounts"
                            AccName.Caption = "الحساب المصرفي رقم"
                        Case 4
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientVisible = True

                            AccName.DataSourceID = "Accounts"
                            AccName.Caption = "حساب المدينون رقم"
                            'Accounts.DataBind()
                        Case 5
                            AccNo.ClientVisible = False
                            Bank.ClientVisible = False
                            AccName.ClientVisible = True

                            AccName.DataSourceID = "BankAccounts"
                            AccName.Caption = "الحساب المصرفي رقم"
                            'BankAccounts.DataBind()
                        Case Else
                            Exit Select
                    End Select
                    AccName.DataBind()
                Case Else
                    Exit Select
            End Select
        End If
ss:
    End Sub

    Protected Sub PayTyp_ValueChanged(sender As Object, e As EventArgs)

    End Sub

End Class