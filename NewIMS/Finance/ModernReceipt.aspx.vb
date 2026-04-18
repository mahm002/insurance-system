Imports System.Data.SqlClient
Imports System.Data.SqlClient.SqlParameter
Imports DevExpress.Web

Public Class ModernReceipt
    Inherits Page
    Private ReadOnly Policy As New DataSet
    Private RNo As String = ""

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExecConn("update policyfile set polno=REPLACE(polno, char(9), '')", Conn)
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If Request("PolNo") = "أخـرى" Then
            lblTitle.Text = "مقبوضات أخرى"
            If Not IsPostBack Then
                TOTPRM.Text = "0"
                TOTPRM.Visible = False
                GridData.Visible = False
                CheckCust.Visible = False
            End If
        Else
            If Not IsPostBack Then
                If Not CheckCust.Checked Then
                    LoadSinglePolicy()
                End If
            End If
        End If

        ' Configure payment data source
        If Request("PolNo") = "أخـرى" Then
            Pay.SelectCommand = "select TPName,TPNo from EXTRAINFO where TP='Payment' AND TPNo<>4 order by TPNo"
            Payed.ReadOnly = False
        Else
            Pay.SelectCommand = "select TPName,TPNo from EXTRAINFO where TP='Payment' order by TPNo"
            Payed.ReadOnly = True
        End If
    End Sub

    Private Sub LoadSinglePolicy()
        Dim query As String = "SELECT PolNo, PolicyFile.CustNo, Currency, SubIns, EndNo, LoadNo, TOTPRM-InBox As TOTPRM, " &
                              "IssuDate, CustName, ExtraInfo.TpName, " &
                              "CASE WHEN Commision <> 0 AND Broker <> 0 " &
                              "THEN CAST(Commision AS NVARCHAR(100)) + " &
                              "CASE WHEN CommisionType = 1 THEN ' ' + EXTRAINFO.TPName ELSE ' %' END + '-' + BrokersInfo.TPName " &
                              "ELSE '0' END As Commissioned " &
                              "FROM PolicyFile " &
                              "INNER JOIN ExtraInfo ON TP='Cur' AND TpNo=Currency " &
                              "INNER JOIN CustomerFile ON CustomerFile.CustNo=PolicyFile.CustNo " &
                              "LEFT OUTER JOIN EXTRAINFO As BrokersInfo ON BrokersInfo.TP='Broker' AND PolicyFile.Broker=BrokersInfo.TPNo " &
                              "WHERE PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & " AND LoadNo=" & Request("LoadNo")

        Using dbadapter As New SqlDataAdapter(query, Conn)
            dbadapter.Fill(Policy)
        End Using

        If Policy.Tables(0).Rows.Count > 0 Then
            TOTPRM.Text = Format(Policy.Tables(0).Rows(0)("TOTPRM"), "###,#0.000")
            Payed.Text = Format(Policy.Tables(0).Rows(0)("TOTPRM"), "###,#0.000")
            Customer.Text = Policy.Tables(0).Rows(0)("CustName")

            ' Set date
            Dim issueDate As Date = Convert.ToDateTime(Policy.Tables(0).Rows(0)("IssuDate"))
            If issueDate.Month = Today.Month Then
                MoveDate.ClientEnabled = False
            Else
                MoveDate.Value = LastDayOfMonth(issueDate)
                MoveDate.ClientEnabled = False
            End If

            GridData.DataSource = Policy
            GridData.DataBind()

            Session("SubIns") = Policy.Tables(0).Rows(0)("SubIns")
            Session("IssDate") = Policy.Tables(0).Rows(0)("IssuDate")
        End If
    End Sub

    Protected Sub CheckCust_CheckedChanged(sender As Object, e As EventArgs) Handles CheckCust.CheckedChanged
        If CheckCust.Checked Then
            LoadAllPolicies()
        Else
            LoadSinglePolicy()
        End If
    End Sub

    Private Sub LoadAllPolicies()
        Dim query As String = "SELECT PolNo, PolicyFile.CustNo, Currency, SubIns, EndNo, LoadNo, TOTPRM-Inbox As TOTPRM, " &
                              "IssuDate, CustName, TpName FROM " &
                              "PolicyFile INNER JOIN ExtraInfo ON TP='Cur' AND TpNo=Currency " &
                              "INNER JOIN CustomerFile ON CustomerFile.CustNo=PolicyFile.CustNo " &
                              "WHERE PolicyFile.SubIns='" & Session("SubIns") & "' AND inbox < TOTPRM"

        Using dbadapter As New SqlDataAdapter(query, Conn)
            Policy.Clear()
            dbadapter.Fill(Policy)
        End Using

        If Policy.Tables(0).Rows.Count > 0 Then
            Dim totalAmount As Double = 0
            For Each row As DataRow In Policy.Tables(0).Rows
                totalAmount += Convert.ToDouble(row("TOTPRM"))
            Next

            TOTPRM.Text = Format(totalAmount, "###,#0.000")
            Payed.Text = Format(totalAmount, "###,#0.000")
            Payed.ClientEnabled = False
            GridData.DataSource = Policy
            GridData.DataBind()
        End If
    End Sub

    Protected Sub MoveDate_Init(sender As Object, e As EventArgs)
        MoveDate.Date = Today.Date
    End Sub

    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Dim callbackPanel As ASPxCallbackPanel = TryCast(sender, ASPxCallbackPanel)
        Dim isValid As Boolean = ASPxEdit.ValidateEditorsInContainer(callbackPanel)

        If isValid Then
            Select Case e.Parameter
                Case "Sadad"
                    ProcessPayment()
                Case "PaymentChanged"
                    UpdatePaymentFields()
                Case Else
                    ' Do nothing
            End Select
        Else
            UpdatePaymentFields()
        End If
    End Sub

    Private Sub ProcessPayment()
        If CDbl(Payed.Value) <= CDbl(TOTPRM.Value) Then
            If Request("PolNo") <> "أخـرى" Then
                ProcessPolicyPayment()
            Else
                ProcessOtherPayment()
            End If
        Else
            If Request("PolNo") = "أخـرى" Then
                ProcessOtherPayment()
            End If
        End If
    End Sub

    Private Sub ProcessPolicyPayment()
        Dim query As String = "SELECT PolNo, SubIns, PolicyFile.OrderNo, PolicyFile.Branch, PolicyFile.Commision, " &
                              "BranchName, SubSysName, PolicyFile.CustNo, Currency, EndNo, LoadNo, TOTPRM, " &
                              "IssuDate, CustName, ExtraInfo.TpName " &
                              "FROM PolicyFile " &
                              "INNER JOIN ExtraInfo ON TP='Cur' AND TpNo=Currency " &
                              "INNER JOIN CustomerFile ON CustomerFile.CustNo=PolicyFile.CustNo " &
                              "INNER JOIN BranchInfo ON PolicyFile.Branch=BranchNo " &
                              "INNER JOIN SubSystems ON SubIns=SubSysNo AND SubSystems.Branch=PolicyFile.Branch " &
                              "WHERE PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & " AND LoadNo=" & Request("LoadNo")

        Using dbadapter As New SqlDataAdapter(query, Conn)
            Dim policyData As New DataSet()
            dbadapter.Fill(policyData)

            For i As Integer = 0 To policyData.Tables(0).Rows.Count - 1
                Dim row As DataRow = policyData.Tables(0).Rows(i)

                If CDbl(Payed.Value) = 0 Then
                    Exit Sub
                End If

                If (PayTyp.Value = "4" Or PayTyp.Value = "6") And IsNothing(AccName.Value) Then
                    Exit Sub
                End If

                If (PayTyp.Value = "4" Or PayTyp.Value = "6") And Len(AccName.Value) <> 0 Then
                    ExecConn("Update policyFile Set AccountNo = '" & RTrim(AccName.Value) & "', PayType=2, Financed=1 where " &
                             " PolNo='" & row("PolNo").ToString.Trim & "' and EndNo=" & row("EndNo") & " and LoadNo=" & row("LoadNo"), Conn)
                Else
                    If CDbl(Payed.Value) > 0 Then
                        Dim parameters As SqlParameter() = Array.CreateInstance(GetType(SqlParameter), 1)
                        SetPm("@BR", DbType.String, Session("Branch"), parameters, 0)
                        RNo = CallSP("RecetNo", Conn, parameters)

                        ExecConn("Insert into ACCMOVE(DocNo, DocDat, CustName, PayMent, Amount, ForW, " &
                                "EndNo, LoadNo, Tp, Branch, AccNo, Bank, Cur, Node, PayTp, UserName, Note) Values('" &
                                RNo & "', " &
                                "CONVERT(DATETIME, '" & Format(MoveDate.Value, "yyyy-MM-dd") & " 00:00:00', 102),'" &
                                Trim(row("CustName")) & "'," &
                                IIf(CheckCust.Checked, row("TOTPRM"), CDbl(Payed.Value)) & "," &
                                row("TOTPRM") & ",'" &
                                row("PolNo") & "'," &
                                row("EndNo") & "," &
                                row("LoadNo") & ",'" &
                                row("SubSysName").ToString & "','" &
                                Session("Branch") & "','" &
                                AccNo.Text & "','" &
                                Bank.Text & "','" &
                                row("TpName") & "','" &
                                "/" & "'," &
                                PayTyp.Value & ",'" &
                                Session("User") & "','" &
                                Note.Text & "')", Conn)
                    End If
                End If

                If (PayTyp.Value = "4" Or PayTyp.Value = "6") And Len(AccName.Value) <> 0 Then
                    ' Do nothing
                Else
                    ExecConn("Update policyFile Set Inbox = Inbox + " & IIf(CheckCust.Checked, row("TOTPRM"), CDbl(Payed.Value)) & ",Financed=1 where " &
                             "PolicyFile.IssuDate = Convert(DateTime, '" & Format(row("IssuDate"), "yyyy/MM/dd") & " 00:00:00', 102) " &
                             " and PolNo='" & row("PolNo") & "' and EndNo=" & row("EndNo") & " and LoadNo=" & row("LoadNo"), Conn)
                End If

                If Request("AccNo") = "0" Then
                    If (PayTyp.Value = "4" Or PayTyp.Value = "6") Then
                        InsetJournal(row("PolNo"), row("EndNo"), row("LoadNo"), Session("User"), row("OrderNo"), "/", Session("Branch"))
                        Dim tempTP As String
                        If Left(AccName.Value.ToString.TrimEnd, 7) = "1.1.10." Then
                            tempTP = "(تحت التحصيل)"
                        Else
                            tempTP = "مديونية"
                        End If
                        ExecConn("Update MainJournal Set DAILYDTE = '" & Format(row("IssuDate"), "yyyy/MM/dd") & "',Comment=replace(Comment,'مديونية','" & tempTP & "') where MoveRef='" & row("OrderNo").ToString & "'", Conn)
                        Dim dlyno As New DataSet
                        Using dladabter As New SqlDataAdapter("select DAILYNUM from MainJournal where MoveRef='" & row("OrderNo").ToString & "'", Conn)
                            dladabter.Fill(dlyno)
                        End Using
                        ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "&Sys=1")
                        Exit Sub
                    Else
                        InsetJournal(Request("PolNo"), row("EndNo"), row("LoadNo"), Session("User"), row("OrderNo"), RNo, Session("Branch"))
                        ExecConn("Update MainJournal Set DAILYDTE = '" & Format(MoveDate.Value, "yyyy/MM/dd") & "' where MoveRef='" & row("OrderNo").ToString & "' And RecNo='" & RNo & "'", Conn)
                    End If
                End If

                If CDbl(Payed.Value) > 0 Then
                    Dim DBtTp As New DataSet
                    Dim dlyno As New DataSet

                    If (PayTyp.Value = "4" Or PayTyp.Value = "6") Then
                        Dim dladabtercr As New SqlDataAdapter("select DAILYNUM,DailyTyp from MainJournal where MoveRef='" & row("OrderNo") & "'", Conn)
                        dladabtercr.Fill(dlyno)
                        ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "&Sys=1")
                        Exit Sub
                    End If

                    Using dladabter As New SqlDataAdapter("select DAILYNUM,DailyTyp from MainJournal where MoveRef='" & row("OrderNo") & "' and RecNo='" & RNo & "'", Conn)
                        dladabter.Fill(dlyno)
                    End Using

                    Using con1 As New SqlConnection(ConfigurationManager.ConnectionStrings.Item("IMSDBConnectionString").ConnectionString)
                        Using dbadapter5 As New SqlDataAdapter(String.Format("select PayTp from AccMove where DocNo='{0}' ", RNo), con1)
                            dbadapter5.Fill(DBtTp)
                            If DBtTp.Tables(0).Rows.Count = 0 Then
                                Exit Sub
                            Else
                                Select Case DBtTp.Tables(0).Rows(0)("PayTp")
                                    Case "1"
                                        ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and DailyTyp=1 and ANALSNUM='A'", con1)
                                        ExecConn("Update Journal set AccountNo ='" & GetBoxAccount(Session("Branch")) & "' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                    Case "2"
                                        ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and DailyTyp=1 and ANALSNUM='A' ", con1)
                                        ExecConn("Update Journal set AccountNo ='" & GetcheqAccount(Session("Branch")) & "'  where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                    Case "3"
                                        ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and DailyTyp=1 and ANALSNUM='A' ", con1)
                                        ExecConn("Update Journal set AccountNo ='" & AccName.Value & "' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                    Case "5"
                                        ExecConn("Update MainJournal set Comment=Comment + '(" & PayTyp.Text.Trim & ")' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and DailyTyp=1 and ANALSNUM='A'", con1)
                                        ExecConn("Update Journal set AccountNo ='" & AccName.Value & "' where DAILYNUM='" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "' and TP=1 and Dr>0 and Left(AccountNo,2)<>'5.'", con1)
                                End Select
                            End If
                            ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" & dlyno.Tables(0).Rows(0)("DAILYNUM") & "&Sys=1")
                        End Using
                    End Using
                End If
            Next
        End Using
    End Sub

    Private Sub ProcessOtherPayment()
        Dim parameters As SqlParameter() = Array.CreateInstance(GetType(SqlParameter), 1)
        SetPm("@BR", DbType.String, Session("Branch"), parameters, 0)
        RNo = CallSP("RecetNo", Conn, parameters)

        ExecConn("Insert into ACCMOVE(DocNo, DocDat, CustNAme, PayMent, Amount, ForW, EndNo, LoadNo, Tp, Branch, AccNo, Bank, Cur, Node, PayTp, UserName, Note) Values('" &
                RNo & "'," &
                "CONVERT(DATETIME, '" & Format(MoveDate.Value, "yyyy-MM-dd") & " 00:00:00', 102),'" &
                Trim(Customer.Text) & "'," &
                CDbl(Payed.Text) & "," &
                CDbl(Payed.Text) & ",'" &
                Request("PolNo") & "'," &
                "0," &
                "0,'" &
                Request("PolNo") & "','" &
                Session("Branch") & "','" &
                AccNo.Text & "','" &
                Bank.Text & "','" &
                "دينار ليبي" & "','" &
                "" & "'," &
                PayTyp.Value & ",'" &
                Session("User") & "','" &
                Note.Text & "')", Conn)

        parameters = Array.CreateInstance(GetType(SqlParameter), 3)
        SetPm("@TP", DbType.String, "1", parameters, 0)
        SetPm("@Year", DbType.String, Right(Year(MoveDate.Value).ToString, 2), parameters, 1)
        SetPm("@Br", DbType.String, Session("Branch"), parameters, 2)

        Dim Dly As String = CallSP("LastDailyNo", Conn, parameters)
        Dim accnt As String = ""
        Dim Str As String = Note.Value.ToString

        Select Case PayTyp.Value
            Case "1"
                accnt = GetBoxAccount(Session("Branch"))
            Case "2"
                accnt = GetcheqAccount(Session("Branch"))
            Case "3", "5"
                accnt = AccName.Value
        End Select

        ExecConn("INSERT INTO MainJournal ([DAILYNUM],[DAILYDTE] ,[DailyTyp] ,[ANALSNUM] ,[Comment] " &
                ",[Currency] ,[Exchange] ,[CurUser] ,[RecNo],[DailyChk],[Branch])  " &
                " VALUES ('" & Dly & "','" & Format(MoveDate.Value, "yyyy/MM/dd") & "', " &
                "1,'" & "P" & "','" & Str & " / RecNo.# " & RNo & "', " & 1 & "," & 1 & "," &
                "'" & Session("User") & "','" & RNo & "'," & 1 & ",'" & Session("Branch") & "')", Conn)

        ExecConn("INSERT INTO [dbo].[Journal]([DAILYNUM], [TP], [AccountNo], [Dr], [Cr], [CurUser], [Branch]) VALUES ('" & Dly.ToString & "',1, '" & accnt & "'," & CDbl(Payed.Value) & ",0,'" & Session("User") & "','" & Session("Branch") & "')", Conn)
        ASPxWebControl.RedirectOnCallback("~/Finance/DailySarf.aspx?daily=" & Dly.ToString & "&Sys=1")
    End Sub

    Private Sub UpdatePaymentFields()
        Select Case PayTyp.Value
            Case "1"
                AccNo.ClientVisible = False
                Bank.ClientVisible = False
                AccName.ClientVisible = False
            Case "2"
                AccNo.ClientVisible = True
                Bank.ClientVisible = True
                AccName.ClientVisible = False
            Case "3"
                AccNo.ClientVisible = True
                Bank.ClientVisible = True
                AccName.ClientVisible = True
                AccName.DataSourceID = "BankAccounts"
                AccName.Caption = "الحساب المصرفي رقم"
            Case "4"
                AccNo.ClientVisible = False
                Bank.ClientVisible = False
                AccName.ClientVisible = True
                AccName.DataSourceID = "Accounts"
                AccName.Caption = "حساب المدينون رقم"
            Case "5"
                AccNo.ClientVisible = False
                Bank.ClientVisible = False
                AccName.ClientVisible = True
                AccName.DataSourceID = "BankAccounts"
                AccName.Caption = "الحساب المصرفي رقم"
            Case "6"
                AccNo.ClientVisible = False
                Bank.ClientVisible = False
                AccName.ClientVisible = True
                AccName.DataSourceID = "AccountsNotPayed"
                AccName.Caption = "حساب تحت التحصيل رقم"
        End Select
        AccName.DataBind()
    End Sub

    Protected Sub PayTyp_ValueChanged(sender As Object, e As EventArgs)
        ' Server-side handler - logic is in Callback
    End Sub
End Class