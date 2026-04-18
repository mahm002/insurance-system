Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports DevExpress.Web
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class SpecialProcedures
    Inherits Page
    'Dim Lo() As String
    'Dim myList = CType(Session("UserInfo"), List(Of String))

    'Wakala.Text = Getwakalavalue(Request("Sys"))
    Dim PolicyRec As New DataSet

    Dim PolicyRec1 As New DataSet
    Dim ReqRec As New DataSet
    Private Action = ""
    Private RequestUser = 0

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsStoped(GetPolNo(Request("Sys"), Request("OrderNo"), Request("EndNo"), Request("LoadNo")), Request("EndNo"), Request("LoadNo")) Then
            Cancel.Visible = False
            Refund.Visible = False
            Exit Sub
        End If
        If Not IsPostBack Then
            Dim myList = DirectCast(Session("UserInfo"), List(Of String))
            Dim TT As Integer = CType(Mid(myList(1), InStr(1, myList(1), Request("sys")) + 3, 1), Short)
            Wakala.Text = Getwakalavalue(Request("Sys"))

            If Request("EndNo") = 0 Then
                Select Case Request("sys")
                    Case "MC", "MB", "MA" : Dim dbadapter = New SqlDataAdapter("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(EXTPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                        Dim unused = dbadapter.Fill(PolicyRec)
                        'PolicyRec = RecSet("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                    Case "OC" : Dim dbadapter1 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)
                        Dim unused1 = dbadapter1.Fill(PolicyRec)
                        'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)
                    Case Else : Dim dbadapter2 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                        Dim unused2 = dbadapter2.Fill(PolicyRec)
                        'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                End Select
            Else
                Dim dbadapter3 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                Dim unused3 = dbadapter3.Fill(PolicyRec)
                'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
            End If
            PolNo.Text = PolicyRec.Tables(0).Rows(0)("PolNo")
            NETPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(1), "###0.000")
            TAXPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(2), "###0.000")
            CONPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(3), "###0.000")
            STMPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(4), "###0.000")
            ISSPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(5), "###0.000")
            TOTPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(7), "###0.000")
            lastNet.Text = Format(PolicyRec.Tables(0).Rows(0)(8), "###0.000")

            Dim dbadapter4 = New SqlDataAdapter("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                  & " and LoadNo=" & Request("LoadNo"), Conn)
            Dim unused4 = dbadapter4.Fill(PolicyRec1)
            'PolicyRec = RecSet("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
            '& " and LoadNo=" & Request("LoadNo"), Conn)
            EndNo.Text = PolicyRec1.Tables(0).Rows(0)("EndNo")
            LoadNo.Text = PolicyRec1.Tables(0).Rows(0)("LoadNo")
            CoverTo.Value = Format(PolicyRec1.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
            CoverFrom.Value = Format(PolicyRec1.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
            CustName.Text = PolicyRec1.Tables(0).Rows(0)("custname")
            CustNo.Text = PolicyRec1.Tables(0).Rows(0)("CustNo")
            PayType.Text = PolicyRec1.Tables(0).Rows(0)("PayType")
            Currency.Value = PolicyRec1.Tables(0).Rows(0)("Currency")
            ExcRate.Text = PolicyRec1.Tables(0).Rows(0)("ExcRate")
            IssuDate.Text = Format(PolicyRec1.Tables(0).Rows(0)("IssuDate"), "yyyy/MM/dd")
            AccountNo.Text = PolicyRec1.Tables(0).Rows(0)("AccountNo")
            SerialNo.Text = IIf(PolicyRec1.Tables(0).Rows(0)("SerialNo") Is Nothing, 0, PolicyRec1.Tables(0).Rows(0)("SerialNo"))
            Br.Text = PolicyRec1.Tables(0).Rows(0)("Branch")
            Commision.Text = PolicyRec1.Tables(0).Rows(0)("Commision")
            CommisionType.Text = PolicyRec1.Tables(0).Rows(0)("CommisionType")
            If IsBranch(PolicyRec1.Tables(0).Rows(0)("Branch")) Then
                If IsSameMonth(Request("OrderNo"), Request("EndNo"), Request("LoadNo"), Request("Sys")) Then
                    If IsFinanced(Request("OrderNo"), Request("EndNo"), Request("LoadNo"), Request("Sys")) Then
                        If Commision.Text = 0 Then
                            GoTo 1
                        Else
                            MsgBob(Me, " هذه الوثيقة عليها عمولة إصدار ولا يمكن إلغائها إلا بعد الرجوع للإدارة المالية ")
                            If PolicyRec1.Tables(0).Rows(0)("Broker") = 69 Then GoTo 1
                            Cancel.Enabled = False
                            Refund.Visible = False
                        End If
                    Else

                    End If
                Else
                    If IsFinanced(Request("OrderNo"), Request("EndNo"), Request("LoadNo"), Request("Sys")) Then
                        If Commision.Text = 0 Then
                            GoTo 1
                        Else
                            MsgBob(Me, " هذه الوثيقة عليها عمولة إصدار ولا يمكن إلغائها إلا بعد الرجوع للإدارة المالية ")
                            Cancel.Enabled = False
                            Refund.Visible = False
                        End If
                    Else
                        MsgBob(Me, "  يجب أن يتم إثبات قيدها في المالية لعدم صدورها في نفس الشهر / يرجى مراجعة الإدارة المالية")
                        Cancel.Enabled = False
                        Refund.Visible = False
                        Exit Sub
                    End If

                End If
            Else

            End If
1:          If Request("Sys") = "OR" And PolicyRec1.Tables(0).Rows(0)("CoverFrom") > Now() Then
                'Response.Write("<script>alert(' الوثيقة المطلوب التعديل عليها تجاوزت 30 دقيقة من على إصدارها لا يمكن االاسترجاع أو الالغاء ');</script>")

                'If CType(Mid(myList(1), InStr(1, myList(1), Request("Sys")) + 3, 1), Short) >= 5 Then
                '    GoTo 10
                'Else
                '    MsgBob(Me, " ONE DAY LIMIT EXEEDED ")
                '    Cancel.Visible = False
                '    Refund.Visible = False
                '    Exit Sub
                'End If
            Else
10:             If (Month(PolicyRec1.Tables(0).Rows(0)("IssuDate")) = Month(Today.Date) And Year(PolicyRec1.Tables(0).Rows(0)("IssuDate")) = Year(Today.Date)) Or IsHeadQuarter(Session("Branch")) Then
                    If IsHeadQuarter(Session("Branch")) Or Session("Branch") = Trim(Br.Text) Then
                    Else
                        If GetMainBranch(Trim(Br.Text)) <> Session("Branch") Then
                            Cancel.Enabled = IIf(GetOfficeManager(Trim(Br.Text)) = Session("UserId"), True, False)
                            Refund.Visible = IIf(GetOfficeManager(Trim(Br.Text)) = Session("UserId"), True, False)
                            Exit Sub
                        Else

                        End If
                    End If
                Else
                    If IsHeadQuarter(Session("Branch")) Then
                    Else
                        Cancel.Enabled = False
                        Refund.Visible = False
                        Exit Sub
                    End If
                End If
            End If
        Else
            Dim dbadapter4 = New SqlDataAdapter("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                  & " and LoadNo=" & Request("LoadNo"), Conn)
            Dim unused4 = dbadapter4.Fill(PolicyRec1)
            'PolicyRec = RecSet("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
            '& " and LoadNo=" & Request("LoadNo"), Conn)
            EndNo.Text = PolicyRec1.Tables(0).Rows(0)("EndNo")
            LoadNo.Text = PolicyRec1.Tables(0).Rows(0)("LoadNo")
            CoverTo.Value = Format(PolicyRec1.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
            CoverFrom.Value = Format(PolicyRec1.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
            CustName.Text = PolicyRec1.Tables(0).Rows(0)("custname")
            CustNo.Text = PolicyRec1.Tables(0).Rows(0)("CustNo")
            PayType.Text = PolicyRec1.Tables(0).Rows(0)("PayType")
            Currency.Value = PolicyRec1.Tables(0).Rows(0)("Currency")
            ExcRate.Text = PolicyRec1.Tables(0).Rows(0)("ExcRate")
            IssuDate.Text = Format(PolicyRec1.Tables(0).Rows(0)("IssuDate"), "yyyy/MM/dd")
            AccountNo.Text = PolicyRec1.Tables(0).Rows(0)("AccountNo")
            SerialNo.Text = IIf(PolicyRec1.Tables(0).Rows(0)("SerialNo") Is Nothing, 0, PolicyRec1.Tables(0).Rows(0)("SerialNo"))
            Br.Text = PolicyRec1.Tables(0).Rows(0)("Branch")
            Select Case Session("RefTP")
                Case "Refund"
                    ASPxFormLayout1.ClientVisible = True
                    Refund.Visible = False
                    Cancel.Visible = False
                    'And Session("Refund")
                    If Request("sys") = "MC" Or Request("sys") = "MB" Or Request("sys") = "MA" Or Request("sys") = "OC" Or Request("sys") = "ER" Or Request("sys") = "CR" Then
                    Else
                        RefundFrom.MinDate = CDate(CoverFrom.Value)
                        RefundFrom.MaxDate = CDate(CoverTo.Value)
                    End If
                Case "Cancel100"
                    OKRefund.Visible = False
                    Refund.Visible = False
                    Cancel.Visible = False
                Case Else
                    Exit Select
            End Select

        End If

    End Sub
    Public Function CancelPolicy(polOcNo As String) As String
        Dim Apiurl As String = ConfigurationManager.AppSettings("ApiBaseUrl")
        Dim User As String = ConfigurationManager.AppSettings("ApiUsername")
        Dim Pass As String = ConfigurationManager.AppSettings("ApiPassword")

        ' Validate inputs
        If String.IsNullOrEmpty(Apiurl) Then Throw New ArgumentException("API URL is not configured")
        If String.IsNullOrEmpty(User) Then Throw New ArgumentException("Username is not configured")
        If String.IsNullOrEmpty(Pass) Then Throw New ArgumentException("Password is not configured")
        If String.IsNullOrEmpty(polOcNo) Then Throw New ArgumentException("Policy number is required")

        Dim EndPoint As String = "api/insurance/policy/cancel"
        Dim fullUrl As String = IIf(Apiurl.EndsWith("/"), Apiurl & EndPoint, Apiurl & "/" & EndPoint)

        ' Ensure TLS 1.2 for HTTPS
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Using webClient As New WebClient()
            webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
            webClient.Headers(HttpRequestHeader.Accept) = "application/json"
            webClient.Encoding = Encoding.UTF8

            ' Prepare JSON data with proper escaping
            Dim jsonData As String = JsonConvert.SerializeObject(New With {
            .user_name = User,
            .pass_word = Pass,
            .POL_OC_NO = polOcNo
        })

            Try
                Dim response As String = webClient.UploadString(fullUrl, "POST", jsonData)
                Return response
            Catch ex As WebException
                If ex.Response IsNot Nothing Then
                    Using reader As New StreamReader(ex.Response.GetResponseStream())
                        Dim errorResponse As String = reader.ReadToEnd()
                        Throw New Exception($"API Error ({CType(ex.Response, HttpWebResponse).StatusCode}): {errorResponse}")
                    End Using
                Else
                    Throw New Exception($"Network Error: {ex.Message}")
                End If
            End Try
        End Using
    End Function
    Protected Sub Callback_Callback(sender As Object, e As CallbackEventArgsBase) Handles Callback.Callback
        Dim userList = DirectCast(Session("UserInfo"), List(Of String))
        Dim myList = DirectCast(Session("UserInfo"), List(Of String))

        If Request("Request") <> "" Then
            Dim dbadapter5 = New SqlDataAdapter("Select top(1) Action,GeneratedByID from Notifications WHERE Message='" & Request("Request") & "'", Conn)
            dbadapter5.Fill(ReqRec)
            Action = ReqRec.Tables(0).Rows(0)("Action")
            RequestUser = ReqRec.Tables(0).Rows(0)("GeneratedByID")
        End If
        Dim splited = e.Parameter.Split("|")
        Dim Re As New DataSet
        Dim Financed As New DataSet
        Dim DayDiff As Double
        Select Case splited(0)

            Case "Cancel100"
                Session("RefTP") = "Cancel100"
                NETPRM.Text = Format(NETPRM.Text * -1, "###,0.000")
                NETPRM.ClientEnabled = False
                TAXPRM.Text = Format(TAXPRM.Text * -1, "###,0.000")
                TAXPRM.ClientEnabled = False
                CONPRM.Text = Format(CONPRM.Text * -1, "###,0.000")
                CONPRM.ClientEnabled = False
                STMPRM.Text = Format(STMPRM.Text * -1, "###,0.000")
                STMPRM.ClientEnabled = False
                ISSPRM.Text = Format(ISSPRM.Text * -1, "###,0.000")
                ISSPRM.ClientEnabled = False
                TOTPRM.Text = Format(TOTPRM.Text * -1, "###,0.000")
                TOTPRM.ClientEnabled = False

                MainCalcDx(Me, NETPRM.Text, Request("sys"), EndNo.Text, GetSpCase(Val(CustNo.Text)), True)
                OK.Enabled = True
                OK.Focus()
                Refund.Visible = False
                Cancel.Visible = False
                OKRefund.Visible = False
            Case "ConfirmCancel"
                If Request("Sys") = "OR" Then
                    'ASPxWebControl.RedirectOnCallback(GetEditForm(Request("Sys")) & "/Insurance/CancelPolicy?policyNumber=" & Request("OrderNo") & "&Refund=false&UserName=" & Session("UserLogin") & "&Branch=" & GetBranchbyOrderNo(Request("OrderNo")))
                    'CancelPolicy(Request("OrderNo"))
                    Try
                        ' Get the JSON response
                        Dim jsonResponse As String = CancelPolicy(Request("OrderNo"))

                        ' Parse it
                        Dim response As JObject = JObject.Parse(jsonResponse)

                        ' Access values
                        Dim status As Boolean = response("status")
                        Dim message As String = response("message").ToString()

                        If status Then
                            'Dim referenceNo As String = response("data")("referenceNumber").ToString()
                            ExecConn("update policyFile set Stop=1 where OrderNo='" & Request("OrderNo") & "'", Conn)

                            ExecConn("INSERT INTO PolicyFile " _
                                & " (PolNo, OrderNo, EndNo, LoadNo,IssuDate,IssuTime, EntryDate, CustNo, AgentNo, OwnNo, Broker, SubIns, Currency " _
                                & ",ExcRate, PayType, AccountNo, CoverType, Measure, Interval, CoverFrom, CoverTo, LASTNET, NETPRM, TAXPRM, CONPRM, STMPRM " _
                                & ",ISSPRM, EXTPRM, TOTPRM, ExtraRate, Stat, Inbox, ForLoss,Stop, Printed, financed, Discount, Branch, UserName,IssueUser, Commision) " _
                                & " Select '" & Request("OrderNo") & "','" & Request("OrderNo") & "-1" & "',1,0,CONVERT(DATETIME,getdate(),111) ,CONVERT(DATETIME,getdate(),111) ,CONVERT(DATETIME,getdate(),111) ,CustNo,AgentNo,OwnNo," & 0 & ",'" & Request("Sys") & "',Currency" _
                                & ",1,1, 0, CoverType,Measure,Interval,CoverFrom,CoverTo,LASTNET*-1,NETPRM*-1,TAXPRM*-1,CONPRM*-1,STMPRM*-1,ISSPRM*-1,EXTPRM*-1,TOTPRM*-1, ExtraRate, Stat," & 0 & ",ForLoss,1,Printed,financed,Discount,'" & Br.Text.Trim & "','" & GetUserByOrderNo(Request("OrderNo")) & "','" & Session("UserId") & "',0 " _
                                & "from policyfile where polno='" & Request("OrderNo") & "' and endno=0 and loadno=0", Conn)

                            ExecConn("INSERT INTO [dbo].[MOTORFILE] ([OrderNo] ,[EndNo],[LoadNo],[SubIns],[BudyNo],[TableNo],[CarType],[CarColor],[MadeYear],[AreaCover],[PassNo],[PermZone],[Power],[Premium],[PermType]) " _
                                        & " SELECT '" & Request("OrderNo") & "-1" & "',1,0 ,'OR',BudyNo,TableNo ,CarType ,'/',MadeYear ,AreaCover, 4 ,PermZone,0,Premium*-1,PermType From MotorFile where OrderNo='" & Request("OrderNo") & "'", Conn)

                            If Request("Request") <> "" Then
                                ExecConn("Insert Into Notifications (Action,IsRead,Message,Type,UserId,GeneratedBy,GeneratedByID) values('---', 0, 'تمت عملية الإلغاء بنجاح', " _
                                    & "1, " & RequestUser & ",'" & Session("User") & "'," & Session("UserId") & ")", Conn)
                                ExecConn("UPDATE Notifications Set IsTreated=1, TreatingDate='" & Now.ToString("yyyy/MM/dd HH:mm:ss").ToString & "', TreatedBy='" & Session("User") & "' WHERE Action='" & Action & "'", Conn)
                            End If
                        Else
                            If Request("Request") <> "" Then
                                ExecConn("Insert Into Notifications (Action,IsRead,Message,Type,UserId,GeneratedBy,GeneratedByID) values('---', 0, 'لم تتم عملية الإلغاء، يرجى مراجعة الإدار العامة/' & '" & message & "' , " _
                                    & "1, " & RequestUser & ",'" & Session("User") & "'," & Session("UserId") & ")", Conn)
                                ExecConn("UPDATE Notifications Set IsTreated=1, TreatingDate='" & Now.ToString("yyyy/MM/dd HH:mm:ss").ToString & "', TreatedBy='" & Session("User") & "' WHERE Action='" & Action & "'", Conn)
                            End If
                            Exit Sub
                        End If

                        ' Or deserialize to dynamic
                        Dim dynamicResponse = JsonConvert.DeserializeObject(Of Object)(jsonResponse)

                    Catch ex As Exception
                        'MessageBox.Show(ex.Message)
                    End Try

                    If Val(PayType.Text) = 2 And CDbl(NETPRM.Value) < 0 Then
                        'InsetJournal(PolNo.Text, 1, LoadNo.Text, myList(8), OrderNo.Text, "/", Br.Text.Trim)
                        Exit Sub
                    Else
                        Dim dbadapter = New SqlDataAdapter("Select * from MainJournal where MoveRef like '%" & Request("OrderNo") & "%'", Conn)
                        dbadapter.Fill(Re)

                        If Re.Tables(0).Rows.Count = 0 Then
                            If IsSameMonth(Request("OrderNo"), Request("EndNo"), Request("LoadNo"), Request("sys")) Then
                            Else
                                If IsBranch(Br.Text.Trim) Then
                                    InsetJournal(PolNo.Value, 1, LoadNo.Value, userList(8), OrderNo.Value, "/", Br.Text.Trim)
                                Else

                                End If
                            End If
                        Else
                            If IsBranch(Br.Text.Trim) Then
                                InsetJournal(PolNo.Value, 1, LoadNo.Value, userList(8), OrderNo.Value, "/", Br.Text.Trim)
                            Else

                            End If
                        End If
                    End If
                    Exit Sub
                End If

                Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                'SetPm("@BranchNo", DbType.String, IIf(Left(Mid(PolNo.Text, 10, 5), 2) = Left(myList(7), 2) And Right(PolNo.Text, 3) = Right(myList(7), 3), myList(7), Mid(PolNo.Text, 10, 5)), Parm, 1)
                SetPm("@BranchNo", DbType.String, GetBranchbyOrderNo(Request("OrderNo")), Parm, 1)
                'SetPm("@BranchNo", DbType.String, Br.Text, Parm, 1)
                OrderNo.Text = CallSP("LastOrderNo", Conn, Parm)

                ExecConn("Delete PolicyFile Where PolNo='" & PolNo.Text & "' and IssuDate Is Null", Conn)
                EndNo.Text = GetLastEnd(PolNo.Text) + 1

                Select Case Request("Sys")

                    Case "01", "02", "03", "04", "MC", "MB", "MA", "OC", "ER", "CR", "07", "08", "27", "MM", "MS", "PH", "CL", "HL", "FG", "FR", "FB", "EL"
                        ExecConn("insert into policyFile(PolNo,OldPolicy,OrderNo,EndNo,LoadNo,issuDate,issuTime,EntryDate,CustNo,AccountNo,AgentNo,OwnNo,SubIns,CoverType,Measure,Interval,CoverFrom,CoverTo,Currency,PayType,ExcRate,Stop,LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,Branch,UserName,IssueUser,SerialNo,Commision,CommisionType) values('" _
           & PolNo.Text & "','" _
           & OldPolicy.Text & "','" _
           & OrderNo.Text & "'," _
           & EndNo.Text & "," _
           & LoadNo.Text & "," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & "convert(varchar, '" & Format(Now(), "yyyy-MM-dd HH:mm:ss") & "', 120)," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & CustNo.Text & ",'" _
           & Trim(AccountNo.Text) & "'," _
           & 0 & "," _
           & 0 & ",'" _
           & Request("sys") & "'," _
           & 1 & "," _
           & 5 & "," _
           & 1 & "," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverFrom.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
           & Currency.Value & "," _
           & Val(PayType.Text) & "," _
           & ExcRate.Text & "," _
           & 1 & "," _
           & CDbl(lastNet.Text) & "," _
           & CDbl(NETPRM.Value) & "," _
           & CDbl(TAXPRM.Value) & "," _
           & CDbl(CONPRM.Value) & "," _
           & CDbl(STMPRM.Value) & "," _
           & CDbl(ISSPRM.Value) & "," _
           & CDbl(TOTPRM.Value) & ",'" & Br.Text.Trim & "','" & GetUserByOrderNo(Request("OrderNo")) & "','" & Session("UserID") & "','" & SerialNo.Value & "'," & Commision.Value & "," & CommisionType.Value & "" _
           & ")", Conn)

                        ExecConn("update policyFile set Stop=1 where PolNo='" & PolNo.Text & "' and endNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo"), Conn)
                        Cancel.Enabled = False
                        Parm = Array.CreateInstance(GetType(SqlParameter), 6)
                        SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)
                        ExecConn(CallSP("BuildInsertReissue", Conn, Parm), Conn)

                        If Val(PayType.Text) = 2 And CDbl(NETPRM.Value) < 0 Then
                            InsetJournal(PolNo.Text, EndNo.Text, LoadNo.Text, myList(8), OrderNo.Text, "/", Br.Text.Trim)
                        Else
                            Dim dbadapter = New SqlDataAdapter("Select * from MainJournal where MoveRef like '%" & Request("OrderNo") & "%'", Conn)
                            dbadapter.Fill(Re)

                            If Re.Tables(0).Rows.Count = 0 Then
                                If IsSameMonth(Request("OrderNo"), Request("EndNo"), Request("LoadNo"), Request("sys")) Then
                                Else
                                    If IsBranch(Br.Text.Trim) Then
                                        InsetJournal(PolNo.Value, EndNo.Value, LoadNo.Value, myList(8), OrderNo.Value, "/", Br.Text.Trim)
                                    Else

                                    End If
                                End If
                            Else
                                If IsBranch(Br.Text.Trim) Then
                                    InsetJournal(PolNo.Value, EndNo.Value, LoadNo.Value, myList(8), OrderNo.Value, "/", Br.Text.Trim)
                                Else

                                End If
                            End If
                        End If
                    Case Else
                        ExecConn("insert into policyFile(PolNo,OldPolicy,OrderNo,EndNo,LoadNo,EntryDate,CustNo,AccountNo,AgentNo,OwnNo,SubIns,CoverType,Measure,Interval,CoverFrom,CoverTo,Currency,PayType,ExcRate,Stop,LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,Branch,UserName,IssueUser,SerialNo,Commision,CommisionType) values('" _
           & PolNo.Text & "','" _
           & OldPolicy.Text & "','" _
           & OrderNo.Text & "'," _
           & EndNo.Text & "," _
           & LoadNo.Text & "," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
            & CustNo.Text & ",'" _
           & Trim(AccountNo.Text) & "'," _
           & 0 & "," _
           & 0 & ",'" _
           & Request("sys") & "'," _
           & 1 & "," _
           & 5 & "," _
           & 1 & "," _
           & "CONVERT(DATETIME,'" & Format(splited(1), "yyyy-MM-dd") & " 00:00:00',102)," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Text), "yyyy-MM-dd") & " 00:00:00',102)," _
           & Currency.Value & "," _
           & Val(PayType.Text) & "," _
           & ExcRate.Value & "," _
           & 1 & "," _
           & CDbl(lastNet.Text) & "," _
           & CDbl(NETPRM.Value) & "," _
           & CDbl(TAXPRM.Value) & "," _
           & CDbl(CONPRM.Value) & "," _
           & CDbl(STMPRM.Value) & "," _
           & CDbl(ISSPRM.Value) & "," _
           & CDbl(TOTPRM.Value) & ",'" & Br.Text.Trim & "','" & GetUserByOrderNo(Request("OrderNo")) & "','" & Session("UserID") & "','" & SerialNo.Value & "'," & Commision.Value & "," & CommisionType.Value & "" _
           & ")", Conn)

                        ExecConn("update policyFile set Stop=1 where PolNo='" & PolNo.Text & "' and endNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo"), Conn)
                        Cancel.Enabled = False

                        Parm = Array.CreateInstance(GetType(SqlParameter), 6)
                        SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)
                        ExecConn(CallSP("BuildInsertReissue", Conn, Parm), Conn)

                        If Val(PayType.Text) = 2 And CDbl(NETPRM.Value) < 0 Then
                            InsetJournal(PolNo.Text, EndNo.Text, LoadNo.Text, myList(8), OrderNo.Text, "/", Br.Text.Trim)
                        Else
                            Dim dbadapter = New SqlDataAdapter("Select * from MainJournal where MoveRef like '%" & Request("OrderNo") & "%'", Conn)

                            dbadapter.Fill(Re)
                            If Re.Tables(0).Rows.Count = 0 Then
                            Else
                                InsetJournal(PolNo.Text, EndNo.Text, LoadNo.Text, myList(8), OrderNo.Text, "/", Br.Text.Trim)
                            End If
                        End If
                End Select
                ClientScript.RegisterStartupScript([GetType](), "OpenWindow", "<script> window.open('../Policy/Default.aspx?sys=" & Request("sys") & "','_self'); </script>")

                If Request("Sys") = "01" Or Request("Sys") = "02" Or Request("Sys") = "03" Or Request("Sys") = "27" Or Request("Sys") = "OR" Then
                Else
                    ASPxWebControl.RedirectOnCallback("../Reins/DistPolicy.aspx?OrderNo=" + OrderNo.Text.Trim + "&EndNo=" + EndNo.Text + "&LoadNo=" + LoadNo.Text + "&Branch=" + GetBranchbyOrderNo(Request("OrderNo")) + "&Sys=" + Request("sys") + "&PolNo=" + PolNo.Text + "")
                End If

                Session("RefTP") = ""

                If Request("Request") <> "" Then
                    ExecConn("Insert Into Notifications (Action,IsRead,Message,Type,UserId,GeneratedBy,GeneratedByID) values('---', 0, 'تمت الموافقة على طلب الإلغاء', " _
                            & "1, " & RequestUser & ",'" & Session("User") & "'," & Session("UserID") & ")", Conn)
                    ExecConn("UPDATE Notifications Set IsTreated=1, TreatingDate='" & Now.ToString("yyyy/MM/dd HH:mm:ss").ToString & "', TreatedBy='" & Session("User") & "' WHERE Action='" & Action & "'", Conn)
                End If
            Case "Refund" 'ملحق مرتد

                EndNo.Text = Val(GetLastEnd(PolNo.Text, LoadNo.Text)) + 1
                ASPxFormLayout1.ClientVisible = True

                If Request("sys") = "MC" Or Request("sys") = "MB" Or Request("sys") = "MA" Or Request("sys") = "OC" Or Request("sys") = "ER" Or Request("sys") = "CR" Then
                Else
                    RefundFrom.MinDate = CDate(CoverFrom.Value)
                    RefundFrom.MaxDate = CDate(CoverTo.Value)
                End If
                Session("RefTP") = "Refund"
                RefundFrom.Value = Today().Date
                Refund.Visible = False
                Cancel.Visible = False

            Case "CalcRefund"
                If IssuPrmCalc.Value Then
                    Session.Add("ClcIss", 1)
                Else
                    Session.Add("ClcIss", 0)
                End If

                Dim PolicyRec As New DataSet
                If Request("EndNo") = 0 Then
                    Select Case Request("sys")
                        Case "MC", "MB", "MA" : Dim dbadapter = New SqlDataAdapter("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(EXTPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                            dbadapter.Fill(PolicyRec)
                             'PolicyRec = RecSet("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                        Case "OC" : Dim dbadapter1 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)
                            dbadapter1.Fill(PolicyRec)
                            'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)

                        Case Else : Dim dbadapter2 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                            dbadapter2.Fill(PolicyRec)
                            'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                    End Select
                Else
                    Dim dbadapter3 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                    dbadapter3.Fill(PolicyRec)
                    'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                End If
                PolNo.Text = PolicyRec.Tables(0).Rows(0)("PolNo")
                NETPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(1), "###0.000")
                TAXPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(2), "###0.000")
                CONPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(3), "###0.000")
                STMPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(4), "###0.000")
                ISSPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(5), "###0.000")
                TOTPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(7), "###0.000")
                lastNet.Text = Format(PolicyRec.Tables(0).Rows(0)(8), "###0.000")
                Dim PolicyRec1 As New DataSet
                Dim dbadapter4 = New SqlDataAdapter("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                  & " and LoadNo=" & Request("LoadNo"), Conn)
                dbadapter4.Fill(PolicyRec1)
                'PolicyRec = RecSet("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                '& " and LoadNo=" & Request("LoadNo"), Conn)
                'EndNo.Text = PolicyRec1.Tables(0).Rows(0)("EndNo")
                'LoadNo.Text = PolicyRec1.Tables(0).Rows(0)("LoadNo")
                CoverTo.Value = Format(PolicyRec1.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
                CoverFrom.Value = Format(PolicyRec1.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
                CustName.Text = PolicyRec1.Tables(0).Rows(0)("custname")
                CustNo.Text = PolicyRec1.Tables(0).Rows(0)("CustNo")
                PayType.Text = PolicyRec1.Tables(0).Rows(0)("PayType")
                Currency.Value = PolicyRec1.Tables(0).Rows(0)("Currency")
                ExcRate.Text = PolicyRec1.Tables(0).Rows(0)("ExcRate")
                IssuDate.Text = Format(PolicyRec1.Tables(0).Rows(0)("IssuDate"), "yyyy/MM/dd")
                Commision.Text = PolicyRec1.Tables(0).Rows(0)("Commision")
                CommisionType.Text = PolicyRec1.Tables(0).Rows(0)("CommisionType")
                'SerialNo.Text = IIf(PolicyRec1.Tables(0).Rows(0)("SerialNo") Is Nothing, 0, PolicyRec1.Tables(0).Rows(0)("SerialNo"))

                If RefundRatio.Value <= 0 Then
                    If Request("sys") = "MC" Or Request("sys") = "MB" Or Request("sys") = "MA" Or Request("sys") = "OC" Or Request("sys") = "CR" Or Request("sys") = "ER" Or Request("sys") = "OR" Then
                        DayDiff = 100
                    Else
                        DayDiff = DateDiff(DateInterval.Day, RefundFrom.Value, CDate(CoverTo.Text)) / GetPolDays(PolicyRec.Tables(0).Rows(0)("PolNo")) * 100
                    End If
                Else
                    DayDiff = CDbl(RefundRatio.Value)
                End If

                If Request("sys") = "MC" Or Request("sys") = "MB" Or Request("sys") = "MA" Or Request("sys") = "OC" Or Request("sys") = "CR" Or Request("sys") = "ER" Or Request("sys") = "OR" Or Today.Date < CoverFrom.Value Then
                    NETPRM.Text = Format(NETPRM.Text * (DayDiff / 100) * -1, "###0.000")
                    MainCalcDx(Me, NETPRM.Text, Request("sys"), EndNo.Value + 1, GetSpCase(Val(CustNo.Text)))
                Else
                    NETPRM.Text = Format(IIf(CDbl(NETPRM.Text) = 0, CDbl(lastNet.Text), CDbl(NETPRM.Text)) * (DayDiff / 100) * -1, "###,0.00\0")
                    MainCalcDx(Me, NETPRM.Text, Request("sys"), EndNo.Value + 1, GetSpCase(Val(CustNo.Text)))
                End If
                'Table1.Visible = True
                'me.Page.Server.
                OKRefund.Enabled = True

            Case "ConfirmRefund"
                If IssuPrmCalc.Value Then
                    Session.Add("ClcIss", 1)
                Else
                    Session.Add("ClcIss", 0)
                End If

                If Request("Sys") = "OR" Then
                    'ASPxWebControl.RedirectOnCallback(GetEditForm(Request("Sys")) & "/Insurance/CancelPolicy?policyNumber=" & Request("OrderNo") & "&Refund=false&UserName=" & Session("UserLogin") & "&Branch=" & GetBranchbyOrderNo(Request("OrderNo")))
                    'CancelPolicy(Request("OrderNo"))
                    Try
                        ' Get the JSON response
                        Dim jsonResponse As String = CancelPolicy(Request("OrderNo"))

                        ' Parse it
                        Dim response As JObject = JObject.Parse(jsonResponse)

                        ' Access values
                        Dim status As Boolean = response("status")
                        Dim message As String = response("message").ToString()

                        If status Then
                            'Dim referenceNo As String = response("data")("referenceNumber").ToString()
                            ExecConn("update policyFile set Stop=1 where OrderNo='" & Request("OrderNo") & "'", Conn)

                            ExecConn("INSERT INTO PolicyFile " _
                                & " (PolNo, OrderNo, EndNo, LoadNo,IssuDate,IssuTime, EntryDate, CustNo, AgentNo, OwnNo, Broker, SubIns, Currency " _
                                & ",ExcRate, PayType, AccountNo, CoverType, Measure, Interval, CoverFrom, CoverTo, LASTNET, NETPRM, TAXPRM, CONPRM, STMPRM " _
                                & ",ISSPRM, EXTPRM, TOTPRM, ExtraRate, Stat, Inbox, ForLoss,Stop, Printed, financed, Discount, Branch, UserName,IssueUser, Commision) " _
                                & " Select '" & Request("OrderNo") & "','" & Request("OrderNo") & "-1" & "',1,0,CONVERT(DATETIME,getdate(),111) ,CONVERT(DATETIME,getdate(),111) ,CONVERT(DATETIME,getdate(),111) ,CustNo,AgentNo,OwnNo," & 0 & ",'" & Request("Sys") & "',Currency" _
                                & ",1,1, 0, CoverType,Measure,Interval,CoverFrom,CoverTo,LASTNET*-1,NETPRM*-1,TAXPRM*-1,CONPRM*-1,STMPRM*-1,ISSPRM,EXTPRM*-1,(NETPRM*-1)+(TAXPRM*-1)+(CONPRM*-1)+STMPRM+ISSPRM, ExtraRate, Stat," & 0 & ",ForLoss,1,Printed,financed,Discount,'" & Br.Text.Trim & "','" & GetUserByOrderNo(Request("OrderNo")) & "','" & Session("UserId") & "',0 " _
                                & "from policyfile where polno='" & Request("OrderNo") & "' and endno=0 and loadno=0", Conn)

                            ExecConn("INSERT INTO [dbo].[MOTORFILE] ([OrderNo] ,[EndNo],[LoadNo],[SubIns],[BudyNo],[TableNo],[CarType],[CarColor],[MadeYear],[AreaCover],[PassNo],[PermZone],[Power],[Premium],[PermType]) " _
                                        & " SELECT '" & Request("OrderNo") & "-1" & "',1,0 ,'OR',BudyNo,TableNo ,CarType ,'/',MadeYear ,AreaCover, 4 ,PermZone,0,Premium*-1,PermType From MotorFile where OrderNo='" & Request("OrderNo") & "'", Conn)

                            If Request("Request") <> "" Then
                                ExecConn("Insert Into Notifications (Action,IsRead,Message,Type,UserId,GeneratedBy,GeneratedByID) values('---', 0, ' تمت عملية الإلغاء بنجاح. وتم احتساب رسوم الاًصدار على حسابكم', " _
                                    & "1, " & RequestUser & ",'" & Session("User") & "'," & Session("UserId") & ")", Conn)
                                ExecConn("UPDATE Notifications Set IsTreated=1, TreatingDate='" & Now.ToString("yyyy/MM/dd HH:mm:ss").ToString & "', TreatedBy='" & Session("User") & "' WHERE Action='" & Action & "'", Conn)
                            End If
                        Else
                            If Request("Request") <> "" Then
                                ExecConn("Insert Into Notifications (Action,IsRead,Message,Type,UserId,GeneratedBy,GeneratedByID) values('---', 0, 'لم تتم عملية الإلغاء، يرجى مراجعة الإدارة العامة/' & '" & message & "' , " _
                                    & "1, " & RequestUser & ",'" & Session("User") & "'," & Session("UserId") & ")", Conn)
                                ExecConn("UPDATE Notifications Set IsTreated=1, TreatingDate='" & Now.ToString("yyyy/MM/dd HH:mm:ss").ToString & "', TreatedBy='" & Session("User") & "' WHERE Action='" & Action & "'", Conn)
                            End If
                            Exit Sub
                        End If

                        ' Or deserialize to dynamic
                        Dim dynamicResponse = JsonConvert.DeserializeObject(Of Object)(jsonResponse)

                    Catch ex As Exception
                        'MessageBox.Show(ex.Message)
                    End Try

                    If Val(PayType.Text) = 2 And CDbl(NETPRM.Value) < 0 Then
                        InsetJournal(PolNo.Text, 1, LoadNo.Text, myList(8), OrderNo.Text, "/", Br.Text.Trim)
                        Exit Sub
                    Else
                        Dim dbadapter = New SqlDataAdapter("Select * from MainJournal where MoveRef like '%" & Request("OrderNo") & "%'", Conn)
                        dbadapter.Fill(Re)

                        If Re.Tables(0).Rows.Count = 0 Then
                            If IsSameMonth(Request("OrderNo"), Request("EndNo"), Request("LoadNo"), Request("sys")) Then
                            Else
                                If IsBranch(Br.Text.Trim) Then
                                    InsetJournal(PolNo.Value, 1, LoadNo.Value, myList(8), OrderNo.Value, "/", Br.Text.Trim)
                                Else

                                End If
                            End If
                        Else
                            If IsBranch(Br.Text.Trim) Then
                                InsetJournal(PolNo.Value, 1, LoadNo.Value, myList(8), OrderNo.Value, "/", Br.Text.Trim)
                            Else

                            End If
                        End If
                    End If
                    Exit Sub
                End If

                Dim PolicyRec As New DataSet
                If Request("EndNo") = 0 Then
                    Select Case Request("sys")
                        Case "MC", "MB", "MA" : Dim dbadapter = New SqlDataAdapter("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(EXTPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                            Dim unused1 = dbadapter.Fill(PolicyRec)
                             'PolicyRec = RecSet("Select PolNo,sum(NETPRM),sum(TAXPRM),sum(CONPRM),sum(STMPRM),sum(ISSPRM),sum(TOTPRM),sum(lastnet) from PolicyFile Group By PolNo Having PolNo='" & Request("PolNo") & "'", Conn)
                        Case "OC" : Dim dbadapter1 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)
                            Dim unused = dbadapter1.Fill(PolicyRec)
                            'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' and loadNo=" & Request("LoadNo"), Conn)

                        Case Else : Dim dbadapter2 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                            Dim unused2 = dbadapter2.Fill(PolicyRec)
                            'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "'", Conn)
                    End Select
                Else
                    Dim dbadapter3 = New SqlDataAdapter("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,EXTPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                    Dim unused3 = dbadapter3.Fill(PolicyRec)
                    'PolicyRec = RecSet("Select PolNo,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,lastnet from PolicyFile where PolNo='" & Request("PolNo") & "' AND EndNo=" & Request("EndNo") & "", Conn)
                End If
                PolNo.Text = PolicyRec.Tables(0).Rows(0)("PolNo")
                NETPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(1), "###0.000")
                TAXPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(2), "###0.000")
                CONPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(3), "###0.000")
                STMPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(4), "###0.000")
                ISSPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(5), "###0.000")
                TOTPRM.Text = Format(PolicyRec.Tables(0).Rows(0)(7), "###0.000")
                lastNet.Text = Format(PolicyRec.Tables(0).Rows(0)(8), "###0.000")
                Dim PolicyRec1 As New DataSet
                Dim dbadapter4 = New SqlDataAdapter("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                  & " and LoadNo=" & Request("LoadNo"), Conn)
                dbadapter4.Fill(PolicyRec1)
                'PolicyRec = RecSet("Select * from PolicyFile,CustomerFile where PolicyFile.CustNo=CustomerFile.CustNo and PolNo='" & Request("PolNo") & "' and EndNo=" & Request("EndNo") _
                '& " and LoadNo=" & Request("LoadNo"), Conn)
                'EndNo.Text = PolicyRec1.Tables(0).Rows(0)("EndNo")
                'LoadNo.Text = PolicyRec1.Tables(0).Rows(0)("LoadNo")
                CoverTo.Value = Format(PolicyRec1.Tables(0).Rows(0)("CoverTo"), "yyyy/MM/dd")
                CoverFrom.Value = Format(PolicyRec1.Tables(0).Rows(0)("CoverFrom"), "yyyy/MM/dd")
                CustName.Text = PolicyRec1.Tables(0).Rows(0)("custname")
                CustNo.Text = PolicyRec1.Tables(0).Rows(0)("CustNo")
                PayType.Text = PolicyRec1.Tables(0).Rows(0)("PayType")
                Currency.Value = PolicyRec1.Tables(0).Rows(0)("Currency")
                ExcRate.Text = PolicyRec1.Tables(0).Rows(0)("ExcRate")
                IssuDate.Text = Format(PolicyRec1.Tables(0).Rows(0)("IssuDate"), "yyyy/MM/dd")
                Commision.Text = PolicyRec1.Tables(0).Rows(0)("Commision")
                CommisionType.Text = PolicyRec1.Tables(0).Rows(0)("CommisionType")
                'SerialNo.Text = IIf(PolicyRec1.Tables(0).Rows(0)("SerialNo") Is Nothing, 0, PolicyRec1.Tables(0).Rows(0)("SerialNo"))
                'Dim DayDiff = DateDiff(DateInterval.Day, CDate(CoverTo.Text), CDate(splited(1))) / DateDiff(DateInterval.Day, CDate(CoverFrom.Text), CDate(CoverTo.Text)) / 12
                If RefundRatio.Value <= 0 Then
                    If Request("sys") = "MC" Or Request("sys") = "MB" Or Request("sys") = "MA" Or Request("sys") = "OC" Or Request("sys") = "CR" Or Request("sys") = "ER" Or Request("sys") = "OR" Then
                        DayDiff = 100
                    Else
                        DayDiff = DateDiff(DateInterval.Day, RefundFrom.Value, CDate(CoverTo.Text)) / GetPolDays(PolicyRec.Tables(0).Rows(0)("PolNo")) * 100
                    End If
                Else
                    DayDiff = CDbl(RefundRatio.Value)
                End If

                If Request("sys") = "MC" Or Request("sys") = "MB" Or Request("sys") = "MA" Or Request("sys") = "OC" Or Request("sys") = "CR" Or Request("sys") = "ER" Or Request("sys") = "OR" Or Today.Date < CoverFrom.Value Then
                    NETPRM.Text = Format(NETPRM.Text * (DayDiff / 100) * -1, "###0.000")
                    MainCalcDx(Me, NETPRM.Text, Request("sys"), EndNo.Text, GetSpCase(Val(CustNo.Text)))
                Else
                    NETPRM.Text = Format(IIf(CDbl(NETPRM.Text) = 0, CDbl(lastNet.Text), CDbl(NETPRM.Text)) * (DayDiff / 100) * -1, "###,0.00\0")
                    MainCalcDx(Me, NETPRM.Text, Request("sys"), EndNo.Text, GetSpCase(Val(CustNo.Text)))
                End If
                'Table1.Visible = True
                'me.Page.Server.
                OKRefund.Enabled = True
                'Dim myList = DirectCast(Session("UserInfo"), List(Of String))
                Parm = Array.CreateInstance(GetType(SqlParameter), 2)
                SetPm("@TP", DbType.String, Request("sys"), Parm, 0)
                'SetPm("@BranchNo", DbType.String, IIf(Left(PolNo.Text, 2) = Left(myList(7), 2) And Right(PolNo.Text, 2) = Right(myList(7), 2), myList(7), Left(PolNo.Text, 4)), Parm, 1)
                SetPm("@BranchNo", DbType.String, GetBranchbyOrderNo(Request("OrderNo")), Parm, 1)
                OrderNo.Text = CallSP("LastOrderNo", Conn, Parm)
                ExecConn("Delete PolicyFile Where PolNo='" & PolNo.Text & "' and IssuDate Is Null", Conn)
                If EndNo.Text = Request("EndNo") Then
                    EndNo.Text = GetLastEnd(PolNo.Text) + 1
                End If

                Select Case Request("Sys")

                    Case "01", "02", "03", "04", "MC", "MB", "MA", "OC", "ER", "CR", "07", "08", "27", "MM", "MS", "PH", "CL", "HL", "FG", "FR", "FB", "EL"
                        ExecConn("insert into policyFile(PolNo,OldPolicy,OrderNo,EndNo,LoadNo,issuDate,issuTime,EntryDate,CustNo,AccountNo,AgentNo,OwnNo,SubIns,CoverType,Measure,Interval,CoverFrom,CoverTo,Currency,PayType,ExcRate,Stop,LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,Branch,UserName,IssueUser) values('" _
           & PolNo.Text & "','" _
           & OldPolicy.Text & "','" _
           & OrderNo.Text & "'," _
           & EndNo.Text & "," _
           & LoadNo.Text & "," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & "convert(varchar, '" & Format(Now(), "yyyy-MM-dd HH:mm:ss") & "', 120)," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & CustNo.Text & ",'" _
           & Trim(AccountNo.Text) & "'," _
           & 0 & "," _
           & 0 & ",'" _
           & Request("sys") & "'," _
           & 1 & "," _
           & 5 & "," _
           & 1 & "," _
           & "CONVERT(DATETIME,'" & Format(CDate(tmpdate.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
           & Currency.Value & "," _
           & Val(PayType.Text) & "," _
           & ExcRate.Text & "," _
           & 1 & "," _
           & CDbl(lastNet.Text) & "," _
           & CDbl(NETPRM.Value) & "," _
           & CDbl(TAXPRM.Value) & "," _
           & CDbl(CONPRM.Value) & "," _
           & CDbl(STMPRM.Value) & "," _
           & CDbl(ISSPRM.Value) & "," _
           & CDbl(TOTPRM.Value) & ",'" & Br.Text.Trim & "','" & GetUserByOrderNo(Request("OrderNo")) & "','" & Session("UserID") & "')", Conn)

                        ExecConn("update policyFile set Stop=1 where PolNo='" & PolNo.Text & "' and endNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo") & "", Conn)
                        Cancel.Enabled = False

                        'Parm = Array.CreateInstance(GetType(SqlParameter), 7)
                        'SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        'SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        'SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        'SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        'SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        'SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)
                        'SetPm("@Ratio", DbType.Double, DayDiff, Parm, 6)
                        'ExecSql(CallSP("BuildInsertReissueRatio", Conn, Parm))

                        Parm = Array.CreateInstance(GetType(SqlParameter), 6)
                        SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)

                        ExecConn(CallSP("BuildInsertReissue", Conn, Parm), Conn)

                        If Val(PayType.Text) = 2 Or CDbl(NETPRM.Value) < 0 Then
                            InsetJournal(PolNo.Text, EndNo.Text, LoadNo.Text, myList(8), OrderNo.Text, "/", Br.Text.Trim)
                        Else
                            Dim dbadapter = New SqlDataAdapter("Select * from MainJournal where MoveRef like '%" & Request("OrderNo") & "%'", Conn)

                            dbadapter.Fill(Re)
                            If Re.Tables(0).Rows.Count = 0 Then
                            Else
                                InsetJournal(PolNo.Text, EndNo.Text, LoadNo.Text, myList(8), OrderNo.Text, "/", Br.Text.Trim)
                            End If
                        End If
                    Case Else
                        ExecConn("insert into policyFile(PolNo,OldPolicy,OrderNo,EndNo,LoadNo,EntryDate,CustNo,AccountNo,AgentNo,OwnNo,SubIns,CoverType,Measure,Interval,CoverFrom,CoverTo,Currency,PayType,ExcRate,Stop,LASTNET,NETPRM,TAXPRM,CONPRM,STMPRM,ISSPRM,TOTPRM,Branch,UserName,IssueUser) values('" _
           & PolNo.Text & "','" _
           & OldPolicy.Text & "','" _
           & OrderNo.Text & "'," _
           & EndNo.Text & "," _
           & LoadNo.Text & "," _
           & "CONVERT(DATETIME,'" & Format(Today.Date, "yyyy-MM-dd") & " 00:00:00',102)," _
           & CustNo.Text & ",'" _
           & Trim(AccountNo.Text) & "'," _
           & 0 & "," _
           & 0 & ",'" _
           & Request("sys") & "'," _
           & 1 & "," _
           & 5 & "," _
           & 1 & "," _
           & "CONVERT(DATETIME,'" & Format(CDate(tmpdate.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
           & "CONVERT(DATETIME,'" & Format(CDate(CoverTo.Value), "yyyy-MM-dd") & " 00:00:00',102)," _
           & Currency.Value & "," _
           & Val(PayType.Text) & "," _
           & ExcRate.Text & "," _
           & 1 & "," _
           & CDbl(lastNet.Text) & "," _
           & CDbl(NETPRM.Value) & "," _
           & CDbl(TAXPRM.Value) & "," _
           & CDbl(CONPRM.Value) & "," _
           & CDbl(STMPRM.Value) & "," _
           & CDbl(ISSPRM.Value) & "," _
           & CDbl(TOTPRM.Value) & ",'" & Br.Text.Trim & "','" & GetUserByOrderNo(Request("OrderNo")) & "','" & Session("UserID") & "' )", Conn)

                        ExecConn("update policyFile set Stop=1 where PolNo='" & PolNo.Text & "' and endNo=" & Request("EndNo") & " and LoadNo=" & Request("LoadNo") & "", Conn)
                        Cancel.Enabled = False

                        'Parm = Array.CreateInstance(GetType(SqlParameter), 7)
                        'SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        'SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        'SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        'SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        'SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        'SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)
                        'SetPm("@Ratio", DbType.Double, DayDiff, Parm, 6)
                        'ExecSql(CallSP("BuildInsertReissueRatio", Conn, Parm))

                        Parm = Array.CreateInstance(GetType(SqlParameter), 6)
                        SetPm("@table", DbType.String, GetGroupFile(Request("Sys")), Parm, 0)
                        SetPm("@OldOrder", DbType.String, Request("OrderNo"), Parm, 1)
                        SetPm("@NewOrder", DbType.String, OrderNo.Text, Parm, 2)
                        SetPm("@OldEnd", DbType.String, Request("EndNo"), Parm, 3)
                        SetPm("@NewEnd", DbType.String, EndNo.Text, Parm, 4)
                        SetPm("@Sys", DbType.String, Request("sys"), Parm, 5)

                        ExecConn(CallSP("BuildInsertReissue", Conn, Parm), Conn)

                        If Val(PayType.Text) = 2 Or CDbl(NETPRM.Value) < 0 Then
                            InsetJournal(PolNo.Text, EndNo.Text, LoadNo.Text, myList(8), OrderNo.Text, "/", Br.Text.Trim)
                        Else
                            Dim dbadapter = New SqlDataAdapter("Select * from MainJournal where MoveRef like '%" & Request("OrderNo") & "%'", Conn)

                            dbadapter.Fill(Re)
                            If Re.Tables(0).Rows.Count = 0 Then
                            Else
                                InsetJournal(PolNo.Text, EndNo.Text, LoadNo.Text, myList(8), OrderNo.Text, "/", Br.Text.Trim)
                            End If
                        End If

                End Select
                ClientScript.RegisterStartupScript([GetType](), "OpenWindow", "<script> window.open('../Policy/Default.aspx?sys=" & Request("sys") & "','_self'); </script>")
                If Request("Sys") = "01" Or Request("Sys") = "02" Or Request("Sys") = "03" Or Request("Sys") = "27" Or Request("Sys") = "OR" Or Request("Sys") = "08" Then
                Else
                    ASPxWebControl.RedirectOnCallback("../Reins/DistPolicy.aspx?OrderNo=" + OrderNo.Text.Trim + "&EndNo=" + EndNo.Text + "&LoadNo=" + LoadNo.Text + "&Branch=" + GetBranchbyOrderNo(Request("OrderNo")) + "&Sys=" + Request("sys") + "&PolNo=" + PolNo.Text + "")
                End If
                Session("RefTP") = ""
            Case Else
                Exit Select
        End Select
    End Sub

End Class